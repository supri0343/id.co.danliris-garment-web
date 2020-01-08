using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;

namespace Manufactures.Application.GarmentFinishingOuts.Queries
{
	public class GetMonitoringFinishingQueryHandler : IQueryHandler<GetMonitoringFinishingQuery, GarmentMonitoringFinishingListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;

		public GetMonitoringFinishingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();

			garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}

		public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
		{
			List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel>();
			CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();
			foreach (var item in ro)
			{
				var garmentUnitExpenditureNoteUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/{item}";
				var httpResponse = _http.GetAsync(garmentUnitExpenditureNoteUri, token).Result;

				if (httpResponse.IsSuccessStatusCode)
				{
					var a = await httpResponse.Content.ReadAsStringAsync();
					Dictionary<string, object> keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);
					var data = JsonConvert.DeserializeObject<CostCalViewModel>(keyValues.GetValueOrDefault("data").ToString());
					CostCalViewModel expenditureROViewModel = new CostCalViewModel
					{
						ro = data.ro,
						buyerCode = data.buyerCode,
						hours = data.hours,
						qtyOrder = data.qtyOrder,
						comodityName = data.comodityName
					};
					costCalViewModels.Add(expenditureROViewModel);
				}
				else
				{
					await GetDataCostCal(ro, token);
				}
			}
			costCalculationGarmentDataProductionReport.data = costCalViewModels;
			return costCalculationGarmentDataProductionReport;
		}
		class monitoringView
		{
			public string roJob { get; internal set; }
			public string article { get; internal set; }
			public double qtyOrder { get; internal set; }
			public double stock { get; internal set; }
			public string style { get; internal set; }
			public double sewingQtyPcs { get; internal set; }
			public double finishingQtyPcs { get; internal set; }
			public string uomUnit { get; internal set; }
			public double remainQty { get; internal set; }
		}

		public async Task<GarmentMonitoringFinishingListViewModel> Handle(GetMonitoringFinishingQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));


			var QueryRoFinishing = (from a in garmentFinishingOutRepository.Query
									join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									where a.UnitId == request.unit && a.FinishingOutDate <= dateTo
									select a.RONo).Distinct();
			var QueryRoSewingOut = (from a in garmentSewingOutRepository.Query
									join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
									where a.UnitId == request.unit && a.SewingOutDate <= dateTo
									select a.RONo).Distinct();
			var QueryRo = QueryRoSewingOut.Union(QueryRoFinishing).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
			GarmentMonitoringFinishingListViewModel listViewModel = new GarmentMonitoringFinishingListViewModel();
			List<GarmentMonitoringFinishingDto> monitoringDtos = new List<GarmentMonitoringFinishingDto>();
			var QueryFinishing = from a in garmentFinishingOutRepository.Query
								join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
								where a.UnitId == request.unit && a.FinishingOutDate <= dateTo
								select new monitoringView { finishingQtyPcs = a.FinishingOutDate >= dateFrom ? b.Quantity : 0, sewingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.FinishingOutDate < dateFrom ?- b.Quantity : 0, roJob = a.RONo, article = a.Article, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };

			var QuerySewingOut = from a in garmentSewingOutRepository.Query
									join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
									where a.UnitId == request.unit && a.SewingOutDate <= dateTo
								  select new monitoringView { finishingQtyPcs = 0, sewingQtyPcs = a.SewingOutDate >= dateFrom ? b.Quantity :0, uomUnit = "PCS", remainQty = 0, stock = a.SewingOutDate  < dateFrom ? b.Quantity : 0, roJob = a.RONo, article = a.Article, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };

			var queryNow = QuerySewingOut.Union(QueryFinishing);
			var querySum = queryNow.ToList().GroupBy(x => new { x.qtyOrder, x.roJob, x.article, x.uomUnit, x.style }, (key, group) => new
			{
				QtyOrder = key.qtyOrder,
				RoJob = key.roJob,
				Style = key.style,
				Stock = group.Sum(s => s.stock),
				UomUnit = key.uomUnit,
				Article = key.article,
				SewingQtyPcs = group.Sum(s => s.sewingQtyPcs),
				Finishing = group.Sum(s => s.finishingQtyPcs)
			}).OrderBy(s => s.RoJob);
			foreach (var item in querySum)
			{
				GarmentMonitoringFinishingDto dto = new GarmentMonitoringFinishingDto
				{
					roJob = item.RoJob,
					article = item.Article,
					uomUnit = item.UomUnit,
					qtyOrder = item.QtyOrder,
					sewingOutQtyPcs = item.SewingQtyPcs,
					finishingOutQtyPcs = item.Finishing,
					stock = item.Stock,
					style = item.Style,
					remainQty = item.Stock + item.SewingQtyPcs  - item.Finishing
				};
				monitoringDtos.Add(dto);
			}
			listViewModel.garmentMonitorings = monitoringDtos;
			return listViewModel;
		}
	}
}
