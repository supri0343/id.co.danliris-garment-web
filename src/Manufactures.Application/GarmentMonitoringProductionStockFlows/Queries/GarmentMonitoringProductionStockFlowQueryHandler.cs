using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Linq;
using Manufactures.Domain.MonitoringProductionFlow;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalComponents.Repositories;

namespace Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries
{
	public class GarmentMonitoringProductionStockFlowQueryHandler : IQueryHandler<GetMonitoringProductionStockFlowQuery, GarmentMonitoringProductionStockFlowListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
		private readonly IGarmentAvalComponentRepository garmentAvalComponentRepository;
		private readonly IGarmentAvalComponentItemRepository garmentAvalComponentItemRepository;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentSewingOutDetailRepository garmentSewingOutDetailRepository;
		private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentFinishingOutDetailRepository garmentFinishingOutDetailRepository;

		public GarmentMonitoringProductionStockFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
			garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
			//garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			//garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			//garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
			//garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
			//garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
			//garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}

		class monitoringView
		{
			public string Ro { get; internal set; }
			public string BuyerCode { get; internal set; }
			public string Article { get; internal set; }
			public string Comodity { get; internal set; }
			public double QtyOrder { get; internal set; }
			public double BeginingBalanceCuttingQty { get; internal set; }
			public double QtyCuttingIn { get; internal set; }
			public double QtyCuttingOut { get; internal set; }
			public double QtyCuttingTransfer { get; internal set; }
			public double QtyCuttingsubkon { get; internal set; }
			public double AvalCutting { get; internal set; }
			public double AvalSewing { get; internal set; }
			public double EndBalancCuttingeQty { get; internal set; }

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

			HOrderDataProductionReport hOrderDataProductionReport = await GetDataHOrder(ro, token);

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


		public async Task<GarmentMonitoringProductionStockFlowListViewModel> Handle(GetMonitoringProductionStockFlowQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));

			var QueryRo = (from a in garmentCuttingInRepository.Query
						   join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
						   where a.UnitId == request.unit && a.CuttingInDate <= dateTo
						   select a.RONo).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
			var QueryBeginingCuttingOut = (from a in garmentCuttingOutRepository.Query
								   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								   //join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
								   where a.UnitFromId == request.unit && a.CuttingOutDate < dateTo && a.UnitId == a.UnitFromId
								   select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), BeginingBalanceCuttingQty = -b.TotalCuttingOut, QtyCuttingIn = 0, QtyCuttingOut = 0, QtyCuttingsubkon = 0, QtyCuttingTransfer = 0, AvalCutting = 0, AvalSewing = 0, });
			var QueryBEginingCuttingIn = (from a in garmentCuttingInRepository.Query
								  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
								  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
								  where a.UnitId == request.unit &&  a.CuttingInDate < dateTo
								  select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(),BeginingBalanceCuttingQty=c.CuttingInQuantity, QtyCuttingIn = c.CuttingInQuantity, QtyCuttingOut = 0 });
			var QueryBeginingAvalComp = from a in garmentAvalComponentRepository.Query
									   join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
									   where a.UnitId == request.unit  && a.Date < dateTo 
									   select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(),BeginingBalanceCuttingQty= -b.Quantity, QtyCuttingIn = 0, QtyCuttingOut = 0, AvalCutting = 0, AvalSewing = 0 };

			
			var QueryCuttingOut = (from a in garmentCuttingOutRepository.Query
								   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								   //join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
								   where a.UnitFromId == request.unit && a.CuttingOutDate >= dateFrom && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SEWING" && a.UnitId == a.UnitFromId
								   select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), BeginingBalanceCuttingQty = 0, QtyCuttingIn = 0, QtyCuttingOut = b.TotalCuttingOut, QtyCuttingsubkon = 0, QtyCuttingTransfer = 0, AvalCutting = 0, AvalSewing = 0 });
			var QueryCuttingOutSubkon = (from a in garmentCuttingOutRepository.Query
										 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
										 //join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
										 where a.UnitFromId == request.unit && a.CuttingOutDate >= dateFrom && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SUBKON"
										 select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), BeginingBalanceCuttingQty = 0, QtyCuttingIn = 0, QtyCuttingOut = 0, QtyCuttingsubkon = b.TotalCuttingOut, QtyCuttingTransfer = 0, AvalCutting = 0, AvalSewing = 0 });
			var QueryCuttingOutTransfer = (from a in garmentCuttingOutRepository.Query
										   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
										   //join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
										   where a.UnitFromId == request.unit && a.CuttingOutDate >= dateFrom && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SEWING" && a.UnitId != a.UnitFromId
										   select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), BeginingBalanceCuttingQty = 0, QtyCuttingIn = 0, QtyCuttingOut = 0, QtyCuttingsubkon = 0, QtyCuttingTransfer = b.TotalCuttingOut, AvalCutting = 0, AvalSewing = 0 });
			var QueryCuttingIn = (from a in garmentCuttingInRepository.Query
								  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
								  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
								  where a.UnitId == request.unit && a.CuttingInDate >= dateFrom && a.CuttingInDate <= dateTo
								  select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = c.CuttingInQuantity, QtyCuttingOut = 0 });

			var QueryAvalCompSewing = from a in garmentAvalComponentRepository.Query
								join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
								where a.UnitId == request.unit && a.Date >=dateFrom && a.Date <= dateTo && a.AvalComponentType =="SEWING"
							   select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = 0 ,AvalCutting=0, AvalSewing=b.Quantity};
			var QueryAvalCompCutting = from a in garmentAvalComponentRepository.Query
									  join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
									  where a.UnitId == request.unit && a.Date >= dateFrom && a.Date <= dateTo && a.AvalComponentType =="CUTTING"
									   select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = 0, AvalCutting = b.Quantity, AvalSewing = 0 };
			//var QuerySewingOutIsDifSize = from a in garmentSewingOutRepository.Query
			//							  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
			//							  join c in garmentSewingOutDetailRepository.Query on b.Identity equals c.SewingOutItemId
			//							  where a.SewingTo == "FINISHING" && a.UnitId == request.unit && a.SewingOutDate <= date
			//							  select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewing = c.Quantity, Size = c.SizeName };
			//var QuerySewingOut = from a in garmentSewingOutRepository.Query
			//					 join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
			//					 where a.SewingTo == "FINISHING" && a.UnitId == request.unit && a.SewingOutDate <= date && a.IsDifferentSize == false
			//					 select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewing = b.Quantity, Size = b.SizeName };
			//var QueryFinishingOutisDifSize = from a in garmentFinishingOutRepository.Query
			//								 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
			//								 join c in garmentFinishingOutDetailRepository.Query on b.Identity equals c.FinishingOutItemId
			//								 where a.FinishingTo == "GUDANG JADI" && a.UnitId == request.unit && a.FinishingOutDate <= date
			//								 select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyFinishing = c.Quantity, Size = c.SizeName };
			//var QueryFinishingOut = from a in garmentFinishingOutRepository.Query
			//						join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
			//						where a.FinishingTo == "GUDANG JADI" && a.UnitId == request.unit && a.FinishingOutDate <= date && a.IsDifferentSize == false
			//						select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyFinishing = b.Quantity, Size = b.SizeName };


			var queryNow = QueryBeginingCuttingOut.Union(QueryBEginingCuttingIn).Union(QueryBeginingAvalComp).Union(QueryCuttingOut).Union(QueryCuttingOutSubkon).Union(QueryCuttingOutTransfer).Union(QueryAvalCompCutting).Union(QueryAvalCompSewing).AsEnumerable();

			var querySum = queryNow.GroupBy(x => new { x.Ro, x.Article, x.Comodity, x.QtyOrder }, (key, group) => new
			{
				ro = key.Ro,
				article = key.Article,
				comodity = key.Comodity,
				qtyOrder = key.QtyOrder,
				qtycutting = group.Sum(s => s.QtyCuttingOut),
				qtCuttingSubkon = group.Sum(s => s.QtyCuttingsubkon),
				qtyCuttingTransfer = group.Sum(s => s.QtyCuttingTransfer),
				qtyCuttingIn = group.Sum(s => s.QtyCuttingIn),
				begining=group.Sum(s=>s.BeginingBalanceCuttingQty),
				qtyavalsew=group.Sum(s=>s.AvalSewing),
				qtyavalcut=group.Sum(s=>s.AvalCutting)

			});


			//var query = querySum.Union(querySumTotal).OrderBy(s => s.ro);
			GarmentMonitoringProductionStockFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringProductionStockFlowListViewModel();
			List<GarmentMonitoringProductionStockFlowDto> monitoringDtos = new List<GarmentMonitoringProductionStockFlowDto>();
			if (request.ro == null)
			{
				foreach (var item in querySum)
				{
					GarmentMonitoringProductionStockFlowDto garmentMonitoringDto = new GarmentMonitoringProductionStockFlowDto()
					{
						Article = item.article,
						Ro = item.ro,
						QtyOrder = item.qtyOrder,
						BeginingBalanceCuttingQty=item.begining,
						QtyCuttingTransfer=item.qtyCuttingTransfer,
						QtyCuttingsubkon=item.qtCuttingSubkon,
						QtyCuttingIn=item.qtyCuttingIn,
						Comodity=item.comodity,
						AvalCutting=item.qtyavalcut,
						AvalSewing=item.qtyavalsew,
						EndBalancCuttingeQty= item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer-item.qtCuttingSubkon- item.qtyavalcut-item.qtyavalsew
					};
					monitoringDtos.Add(garmentMonitoringDto);
				}
			}
			else
			{
				foreach (var item in querySum.Where(s => s.ro == request.ro))
				{
					GarmentMonitoringProductionStockFlowDto garmentMonitoringDto = new GarmentMonitoringProductionStockFlowDto()
					{
						Article = item.article,
						Ro = item.ro,
						QtyOrder = item.qtyOrder,
						QtyCuttingTransfer = item.qtyCuttingTransfer,
						QtyCuttingsubkon = item.qtCuttingSubkon,
						QtyCuttingIn = item.qtyCuttingIn,
						Comodity = item.comodity
					};
					monitoringDtos.Add(garmentMonitoringDto);
				}

			}
			garmentMonitoringProductionFlow.garmentMonitorings = monitoringDtos;

			return garmentMonitoringProductionFlow;
		}
	}
}
