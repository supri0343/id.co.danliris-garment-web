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
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries
{
	public class GarmentExpenditureGoodQueryHandler : IQueryHandler<GetMonitoringExpenditureGoodQuery, GarmentMonitoringExpenditureGoodListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
		private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;

		public GarmentExpenditureGoodQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
			garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}
 
		class monitoringView
		{
			public string expenditureGoodNo { get; internal set; }
			public string expenditureGoodType { get; internal set; }
			public DateTimeOffset expenditureDate { get; internal set; }
			public string roNo { get; internal set; }
			public string buyerArticle { get; internal set; }
			public string colour { get; internal set; }
			public string name { get; internal set; }
			public double qty { get; internal set; }
			public string invoice { get; internal set; }
		}
		public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
		{
			CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
			var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/{listRO}";
			var httpResponse = await _http.GetAsync(costCalculationUri, token);

			var freeRO = new List<string>();

			if (httpResponse.IsSuccessStatusCode)
			{
				var contentString = await httpResponse.Content.ReadAsStringAsync();
				Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
				var dataString = content.GetValueOrDefault("data").ToString();
				var listData = JsonConvert.DeserializeObject<List<CostCalViewModel>>(dataString);

				foreach (var item in ro)
				{
					var data = listData.SingleOrDefault(s => s.ro == item);
					if (data != null)
					{
						costCalculationGarmentDataProductionReport.data.Add(data);
					}
					else
					{
						freeRO.Add(item);
					}
				}
			}

			HOrderDataProductionReport hOrderDataProductionReport = await GetDataHOrder(freeRO, token);

			Dictionary<string, string> comodities = new Dictionary<string, string>();
			if (hOrderDataProductionReport.data.Count > 0)
			{
				var comodityCodes = hOrderDataProductionReport.data.Select(s => s.Kode).Distinct().ToList();
				var filter = "{\"(" + string.Join(" || ", comodityCodes.Select(s => "Code==" + "\\\"" + s + "\\\"")) + ")\" : \"true\"}";

				var masterGarmentComodityUri = MasterDataSettings.Endpoint + $"master/garment-comodities?filter=" + filter;
				var garmentComodityResponse = _http.GetAsync(masterGarmentComodityUri).Result;
				var garmentComodityResult = new GarmentComodityResult();
				if (garmentComodityResponse.IsSuccessStatusCode)
				{
					garmentComodityResult = JsonConvert.DeserializeObject<GarmentComodityResult>(garmentComodityResponse.Content.ReadAsStringAsync().Result);
					//comodities = garmentComodityResult.data.ToDictionary(d => d.Code, d => d.Name);
					foreach (var comodity in garmentComodityResult.data)
					{
						comodities[comodity.Code] = comodity.Name;
					}
				}
			}

			foreach (var hOrder in hOrderDataProductionReport.data)
			{
				costCalculationGarmentDataProductionReport.data.Add(new CostCalViewModel
				{
					ro = hOrder.No,
					buyerCode = hOrder.Codeby,
					comodityName = comodities.GetValueOrDefault(hOrder.Kode),
					hours = (double)hOrder.Sh_Cut,
					qtyOrder = (double)hOrder.Qty
				});
			}

			return costCalculationGarmentDataProductionReport;
		}
		async Task<HOrderDataProductionReport> GetDataHOrder(List<string> ro, string token)
		{
			HOrderDataProductionReport hOrderDataProductionReport = new HOrderDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
			var costCalculationUri = SalesDataSettings.Endpoint + $"local-merchandiser/horders/data-production-report-by-no/{listRO}";
			var httpResponse = await _http.GetAsync(costCalculationUri, token);

			if (httpResponse.IsSuccessStatusCode)
			{
				var contentString = await httpResponse.Content.ReadAsStringAsync();
				Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
				var dataString = content.GetValueOrDefault("data").ToString();
				var listData = JsonConvert.DeserializeObject<List<HOrderViewModel>>(dataString);

				foreach (var item in ro)
				{
					var data = listData.SingleOrDefault(s => s.No == item);
					if (data != null)
					{
						hOrderDataProductionReport.data.Add(data);
					}
				}
			}

			return hOrderDataProductionReport;
		}
		public async Task<GarmentMonitoringExpenditureGoodListViewModel> Handle(GetMonitoringExpenditureGoodQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));


			var QueryRo = (from a in garmentExpenditureGoodRepository.Query
									where a.UnitId == request.unit && a.ExpenditureDate  >= dateFrom && a.ExpenditureDate <= dateTo
									select a.RONo).Distinct();
		 
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
			GarmentMonitoringExpenditureGoodListViewModel listViewModel = new GarmentMonitoringExpenditureGoodListViewModel();
			List<GarmentMonitoringExpenditureGoodDto> monitoringDtos = new List<GarmentMonitoringExpenditureGoodDto>();
			var Query= from a in garmentExpenditureGoodRepository.Query
								 join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
								 where a.UnitId == request.unit && a.ExpenditureDate >= dateFrom && a.ExpenditureDate <= dateTo
					   select new monitoringView { buyerArticle = a.BuyerCode +" "+ a.Article, roNo = a.RONo, expenditureDate= a.ExpenditureDate,expenditureGoodNo= a.ExpenditureGoodNo,expenditureGoodType=a.ExpenditureType,invoice= a.Invoice,colour=a.Description,qty= b.Quantity, name = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };

		
			 
			var querySum = Query.ToList().GroupBy(x => new { x.buyerArticle, x.roNo, x.expenditureDate, x.expenditureGoodNo, x.expenditureGoodType,x.invoice,x.colour,x.name }, (key, group) => new
			{
				ros = key.roNo,
				buyer = key.buyerArticle,
				expenditureDates = key.expenditureDate,
				qty = group.Sum(s => s.qty),
				expendituregoodNo = key.expenditureGoodNo,
				expendituregoodTypes = key.expenditureGoodType,
				color= key.colour,
				names= key.name,
				invoices = key.invoice
			 
			}).OrderBy(s => s.expendituregoodNo);
			foreach (var item in querySum)
			{
				GarmentMonitoringExpenditureGoodDto dto = new GarmentMonitoringExpenditureGoodDto
				{
					roNo  = item.ros,
					buyerArticle = item.buyer ,
					expenditureGoodType = item.expendituregoodTypes,
					expenditureGoodNo = item.expendituregoodNo,
					expenditureDate= item.expenditureDates,
					qty = item.qty,
					colour = item.color,
					name = item.names,
					invoice = item.invoices
					 
				};
				monitoringDtos.Add(dto);
			}
			listViewModel.garmentMonitorings = monitoringDtos;
			return listViewModel;
		}
	}
}
