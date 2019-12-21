using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Manufactures.Domain.GarmentLoadings.Repositories;

namespace Manufactures.Application.GarmentLoadings.Queries
{
	public class GetMonitoringLoadingQueryHandler : IQueryHandler<GetMonitoringLoadingQuery, GarmentMonitoringLoadingListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;

		public GetMonitoringLoadingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
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
			public double cuttingQtyPcs { get; internal set; }
			public double loadingQtyPcs { get; internal set; }
			public string uomUnit { get; internal set; }
			public double remainQty { get; internal set; }
		}

		public async Task<GarmentMonitoringLoadingListViewModel> Handle(GetMonitoringLoadingQuery request, CancellationToken cancellationToken)
		{
			DateTime dateFrom = request.dateFrom.ToUniversalTime();
			DateTime dateTo = request.dateTo.AddDays(1).ToUniversalTime();


			var QueryRoCuttingOut = (from a in garmentCuttingOutRepository.Query
									 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
									 where a.UnitId == request.unit && a.CuttingOutDate <= dateTo
									 select a.RONo).Distinct();
			var QueryRoLoading = (from a in garmentLoadingRepository.Query
									 join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
									 where a.UnitId == request.unit && a.LoadingDate <= dateTo
									 select a.RONo).Distinct();
			var QueryRo = QueryRoCuttingOut.Union(QueryRoLoading).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
			var QueryCuttingOut = from a in garmentCuttingOutRepository.Query
								  join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								  where a.UnitId == request.unit && a.CuttingOutDate <= dateTo
								  select new monitoringView { loadingQtyPcs = 0,uomUnit="PCS",remainQty = 0, stock = a.CuttingOutDate < dateFrom ? b.TotalCuttingOut : 0, cuttingQtyPcs = b.TotalCuttingOut, roJob = a.RONo, article = a.Article,  qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };
			var QueryLoading = from a in garmentLoadingRepository.Query
								  join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
								  where a.UnitId == request.unit && a.LoadingDate <= dateTo
								  select new monitoringView { loadingQtyPcs = a.LoadingDate >= dateFrom ? b.Quantity : 0, cuttingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.LoadingDate < dateFrom ? - b.Quantity : 0, roJob = a.RONo, article = a.Article, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };
			var queryNow = QueryCuttingOut.Union(QueryLoading);
			var querySum = queryNow.ToList().GroupBy(x => new { x.qtyOrder, x.roJob, x.article,x.uomUnit,x.style }, (key, group) => new
			{
				QtyOrder = key.qtyOrder,
				RoJob = key.roJob,
				Style= key.style,
				Stock = group.Sum(s => s.stock),
				UomUnit = key.uomUnit,
				Article = key.article,
				CuttingQtyPcs = group.Sum(s => s.cuttingQtyPcs),
				Loading = group.Sum(s => s.loadingQtyPcs)
			}).OrderBy(s => s.RoJob);
			GarmentMonitoringLoadingListViewModel listViewModel = new GarmentMonitoringLoadingListViewModel();
			List<GarmentMonitoringLoadingDto> monitoringDtos = new List<GarmentMonitoringLoadingDto>();
			foreach (var item in querySum)
			{
				GarmentMonitoringLoadingDto dto = new GarmentMonitoringLoadingDto
				{
					roJob = item.RoJob,
					article = item.Article,
					uomUnit=item.UomUnit,
					qtyOrder = item.QtyOrder,
					cuttingQtyPcs = item.CuttingQtyPcs,
					loadingQtyPcs = item.Loading,
					stock = item.Stock,
					style=item.Style,
					remainQty = item.Stock + item.CuttingQtyPcs - item.Loading
				};
				monitoringDtos.Add(dto);
			}
			listViewModel.garmentMonitorings = monitoringDtos;
			return listViewModel;
		}
	}
}
