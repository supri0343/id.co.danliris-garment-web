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
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentSewingDOs.Repositories;

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
		private readonly IGarmentSewingInRepository garmentSewingInRepository;
		private readonly IGarmentSewingInItemRepository garmentSewingInItemRepository;
		private readonly IGarmentAvalComponentRepository garmentAvalComponentRepository;
		private readonly IGarmentAvalComponentItemRepository garmentAvalComponentItemRepository;
		private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
		private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentSewingOutDetailRepository garmentSewingOutDetailRepository;
		private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentFinishingOutDetailRepository garmentFinishingOutDetailRepository;
		private readonly IGarmentFinishingInRepository garmentFinishingInRepository;
		private readonly IGarmentFinishingInItemRepository garmentFinishingInItemRepository;
		private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
		private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
		private readonly IGarmentExpenditureGoodReturnRepository garmentExpenditureGoodReturnRepository;
		private readonly IGarmentExpenditureGoodReturnItemRepository garmentExpenditureGoodReturnItemRepository;
		private readonly IGarmentSewingDORepository garmentSewingDORepository;
		private readonly IGarmentSewingDOItemRepository garmentSewingDOItemRepository;
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
			garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
			garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
			garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
			garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
			garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
			garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
			garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
			garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
			garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
			garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
			garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
			garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
			garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
			garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
			garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
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
			public double BeginingBalanceLoadingQty { get; internal set; }
			public double QtyLoadingIn { get; internal set; }
			public double QtyLoading { get; internal set; }
			public double QtyLoadingAdjs { get; internal set; }
			public double EndBalanceLoadingQty { get; internal set; }
			public double BeginingBalanceSewingQty { get; internal set; }
			public double QtySewingIn { get; internal set; }
			public double QtySewingOut { get; internal set; }
			public double QtySewingInTransfer { get; internal set; }
			public double WipSewingOut { get; internal set; }
			public double WipFinishingOut { get; internal set; }
			public double QtySewingRetur { get; internal set; }
			public double QtySewingAdj { get; internal set; }
			public double EndBalanceSewingQty { get; internal set; }
			public double BeginingBalanceFinishingQty { get; internal set; }
			public double FinishingInQty { get; internal set; }
			public double BeginingBalanceSubconQty { get; internal set; }
			public double SubconInQty { get; internal set; }
			public double SubconOutQty { get; internal set; }
			public double EndBalanceSubconQty { get; internal set; }
			public double FinishingOutQty { get; internal set; }
			public double FinishingInTransferQty { get; internal set; }
			public double FinishingAdjQty { get; internal set; }
			public double FinishingReturQty { get; internal set; }
			public double EndBalanceFinishingQty { get; internal set; }
			public double BeginingBalanceExpenditureGood { get; internal set; }
			public double FinishingTransferExpenditure { get; internal set; }
			public double ExpenditureGoodRetur{ get; internal set; }
			public double ExportQty { get; internal set; }
			public double OtherQty { get; internal set; }
			public double SampleQty { get; internal set; }
			public double ExpenditureGoodRemainingQty { get; internal set; }
			public double ExpenditureGoodAdj { get; internal set; }
			public double EndBalanceExpenditureGood { get; internal set; }

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

			var QueryCuttingOut = (from a in garmentCuttingOutRepository.Query
								   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								   //join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
								   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SEWING" && a.UnitId == a.UnitFromId
								   select new monitoringView { BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0, BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0, QtyCuttingsubkon = 0, QtyCuttingTransfer = 0, AvalCutting = 0, AvalSewing = 0 });
			var QueryCuttingOutSubkon = (from a in garmentCuttingOutRepository.Query
										 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
										 //join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
										 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SUBKON"
										 select new monitoringView { BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0, BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = 0, QtyCuttingsubkon = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0, QtyCuttingTransfer = 0, AvalCutting = 0, AvalSewing = 0 });
			var QueryCuttingOutTransfer = (from a in garmentCuttingOutRepository.Query
										   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
										   //join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
										   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SEWING" && a.UnitId != a.UnitFromId
										   select new monitoringView { BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0, BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = 0, QtyCuttingsubkon = 0, QtyCuttingTransfer = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0, AvalCutting = 0, AvalSewing = 0 });
			var QueryCuttingIn = (from a in garmentCuttingInRepository.Query
								  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
								  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
								  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.CuttingInDate <= dateTo
								  select new monitoringView { BeginingBalanceCuttingQty = a.CuttingInDate < dateFrom ? c.CuttingInQuantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = a.CuttingInDate >= dateFrom ? c.CuttingInQuantity : 0, QtyCuttingOut = 0 });

			var QueryAvalCompSewing = from a in garmentAvalComponentRepository.Query
									  join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
									  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.Date <= dateTo && a.AvalComponentType == "SEWING"
									  select new monitoringView { BeginingBalanceCuttingQty = a.Date < dateFrom ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = 0, AvalCutting = 0, AvalSewing = a.Date >= dateFrom ? b.Quantity : 0 };
			var QueryAvalCompCutting = from a in garmentAvalComponentRepository.Query
									   join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
									   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.Date <= dateTo && a.AvalComponentType == "CUTTING"
									   select new monitoringView { BeginingBalanceCuttingQty = a.Date < dateFrom ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = 0, AvalCutting = a.Date >= dateFrom ? b.Quantity : 0, AvalSewing = 0 };
			var QuerySewingDO = (from a in garmentSewingDORepository.Query
								 join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.SewingDODate <= dateTo
								 select new monitoringView { QtyLoadingIn = a.SewingDODate >= dateFrom ? b.Quantity : 0, BeginingBalanceLoadingQty = (a.SewingDODate < dateFrom) ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() });

			var QueryLoading = from a in garmentLoadingRepository.Query
							   join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
							   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.LoadingDate >= dateFrom && a.LoadingDate <= dateTo
							   select new monitoringView { BeginingBalanceSewingQty = a.LoadingDate < dateFrom ? b.Quantity : 0, BeginingBalanceLoadingQty = a.LoadingDate < dateFrom ? -b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = 0, AvalCutting = 0, AvalSewing = 0, QtyLoading = a.LoadingDate >= dateFrom ? b.Quantity : 0 };
			var QueryLoadingAdj = from a in garmentAdjustmentRepository.Query
								  join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
								  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.AdjustmentDate <= dateTo && a.AdjustmentType == "LOADING"
								  select new monitoringView { BeginingBalanceLoadingQty = a.AdjustmentDate < dateFrom ? -b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCuttingIn = 0, QtyCuttingOut = 0, AvalCutting = 0, AvalSewing = 0, QtyLoading = 0, QtyLoadingAdjs = a.AdjustmentDate >= dateFrom ? b.Quantity : 0 };
			var QuerySewingIn = (from a in garmentSewingInRepository.Query
								 join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.SewingInDate <= dateTo
								 select new monitoringView { BeginingBalanceSewingQty = (a.SewingInDate < dateFrom && a.SewingFrom == "FINISHING") ? -b.Quantity : 0, QtySewingIn = (a.SewingInDate >= dateFrom) ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() });
			var QuerySewingOut = (from a in garmentSewingOutRepository.Query
								  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
								  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.SewingOutDate <= dateTo
								  select new monitoringView { FinishingTransferExpenditure = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId) ? b.Quantity : 0, FinishingInTransferQty = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId) ? b.Quantity : 0, BeginingBalanceSewingQty = (a.SewingOutDate < dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId) ? b.Quantity : 0, QtySewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId) ? b.Quantity : 0, QtySewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId) ? b.Quantity : 0, WipSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId) ? b.Quantity : 0, WipFinishingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId) ? b.Quantity : 0, QtySewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId) ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() });

			var QuerySewingAdj = from a in garmentAdjustmentRepository.Query
								 join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.AdjustmentDate <= dateTo && a.AdjustmentType == "SEWING"
								 select new monitoringView { BeginingBalanceSewingQty = a.AdjustmentDate < dateFrom ? -b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewingAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0 };

			var QueryFinishingIn = (from a in garmentFinishingInRepository.Query
									join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
									where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.FinishingInDate <= dateTo
									select new monitoringView { BeginingBalanceSubconQty = (a.FinishingInDate < dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0, FinishingInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0, SubconInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() });
			var QueryFinishingOut = (from a in garmentFinishingOutRepository.Query
									 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									 join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
									 join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
									 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.FinishingOutDate <= dateTo && a.FinishingTo == "GUDANG JADI"
									 select new monitoringView { FinishingOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0, BeginingBalanceExpenditureGood = (a.FinishingOutDate < dateFrom) ? b.Quantity : 0, EndBalanceSubconQty = (a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? -b.Quantity : 0, SubconOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() });

			var QueryFinishingAdj = from a in garmentAdjustmentRepository.Query
									join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
									where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.AdjustmentDate <= dateTo && a.AdjustmentType == "FINISHING"
									select new monitoringView { FinishingAdjQty = a.AdjustmentDate >= dateFrom ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() };

			var QueryFinishingRetur = (from a in garmentFinishingOutRepository.Query
									   join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									   join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
									   join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
									   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.FinishingOutDate <= dateTo && a.FinishingTo == "SEWING"
									   select new monitoringView { FinishingReturQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitId == a.UnitToId) ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() });
			var QueryExpenditureGoods = (from a in garmentExpenditureGoodRepository.Query
										 join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
										 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.ExpenditureDate <= dateTo
										 select new monitoringView { BeginingBalanceExpenditureGood = a.ExpenditureDate < dateFrom ? -b.Quantity : 0, ExportQty = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Quantity : 0, SampleQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SAMPLE" || (a.ExpenditureType == "LAIN-LAIN"))) ? b.Quantity : 0, OtherQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() });
			var QueryExpenditureGoodsAdj = from a in garmentAdjustmentRepository.Query
										   join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
										   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.AdjustmentDate <= dateTo && a.AdjustmentType == "BARANG JADI"
										   select new monitoringView { BeginingBalanceExpenditureGood = a.AdjustmentDate < dateFrom ? b.Quantity : 0, ExpenditureGoodAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() };
			var QueryExpenditureGoodRetur = from a in garmentExpenditureGoodReturnRepository.Query
											join b in garmentExpenditureGoodReturnItemRepository.Query on a.Identity equals b.ReturId
											where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.ReturDate <= dateTo
											select new monitoringView { BeginingBalanceExpenditureGood = a.ReturDate < dateFrom ? b.Quantity : 0, ExpenditureGoodRetur = a.ReturDate >= dateFrom ? b.Quantity : 0, Ro = a.RONo, Article = a.Article, Comodity = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault() };
			

			var queryNow = QueryCuttingIn
				.Union(QueryCuttingOut)
				.Union(QueryCuttingOutSubkon)
				.Union(QueryCuttingOutTransfer)
				.Union(QueryAvalCompCutting)
				.Union(QueryAvalCompSewing)
				.Union(QuerySewingDO)
				.Union(QueryLoading)
				.Union(QueryLoadingAdj)
				.Union(QuerySewingIn)
				.Union(QuerySewingOut)
				.Union(QuerySewingAdj)
				.Union(QueryFinishingIn)
				.Union(QueryFinishingOut)
				.Union(QueryFinishingAdj)
				.Union(QueryFinishingRetur)
				.Union(QueryExpenditureGoods)
				.Union(QueryExpenditureGoodsAdj)
				.Union(QueryExpenditureGoodRetur)
				.AsEnumerable();

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
				qtyavalcut=group.Sum(s=>s.AvalCutting),
				beginingloading = group.Sum(s => s.BeginingBalanceLoadingQty),
				qtyLoadingIn=group.Sum(s=>s.QtyLoadingIn),
				qtyloading =group.Sum(s=>s.QtyLoading),
				qtyLoadingAdj=group.Sum(s=>s.QtyLoadingAdjs),
				beginingSewing=group.Sum(s=>s.BeginingBalanceSewingQty),
				sewingIn=group.Sum(s=>s.QtySewingIn),
				sewingintransfer = group.Sum(s => s.QtySewingInTransfer),
				sewingout = group.Sum(s => s.QtySewingOut),
				sewingretur= group.Sum(s => s.QtySewingRetur),
				wipsewing = group.Sum(s => s.WipSewingOut),
				wipfinishing = group.Sum(s => s.WipFinishingOut),
				sewingadj=group.Sum(s=>s.QtySewingAdj),
				finishingin=group.Sum(s=>s.FinishingInQty),
				finishingintransfer=group.Sum(s=>s.FinishingInTransferQty),
				finishingadj=group.Sum(s=>s.FinishingAdjQty),
				finishingout=group.Sum(s=>s.FinishingOutQty),
				finishinigretur=group.Sum(s=>s.FinishingReturQty),
				beginingbalanceFinishing=group.Sum(s=>s.BeginingBalanceFinishingQty),
				beginingbalancesubcon=group.Sum(s=>s.BeginingBalanceSubconQty),
				subconIn=group.Sum(s=>s.SubconInQty),
				subconout=group.Sum(s=>s.SubconOutQty),
				exportQty=group.Sum(s => s.ExportQty),
				otherqty= group.Sum(s => s.OtherQty),
				sampleQty=group.Sum(s=>s.SampleQty),
				expendAdj=group.Sum(s=>s.ExpenditureGoodAdj),
				expendRetur=group.Sum(s=>s.ExpenditureGoodRetur),
				finishinginqty=group.Sum(s=>s.FinishingInQty),
				beginingBalanceExpenditureGood= group.Sum(s=>s.BeginingBalanceExpenditureGood)



			}) ;

			GarmentMonitoringProductionStockFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringProductionStockFlowListViewModel();
			List<GarmentMonitoringProductionStockFlowDto> monitoringDtos = new List<GarmentMonitoringProductionStockFlowDto>();

			foreach (var item in querySum)
			{
				GarmentMonitoringProductionStockFlowDto garmentMonitoringDto = new GarmentMonitoringProductionStockFlowDto()
				{
					Article = item.article,
					Ro = item.ro,
					QtyOrder = item.qtyOrder,
					BeginingBalanceCuttingQty = item.begining,
					QtyCuttingTransfer = item.qtyCuttingTransfer,
					QtyCuttingsubkon = item.qtCuttingSubkon,
					QtyCuttingIn = item.qtyCuttingIn,
					QtyCuttingOut = item.qtycutting,
					Comodity = item.comodity,
					AvalCutting = item.qtyavalcut,
					AvalSewing = item.qtyavalsew,
					EndBalancCuttingeQty = item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon - item.qtyavalcut - item.qtyavalsew,
					BeginingBalanceLoadingQty = item.beginingloading,
					QtyLoadingIn = item.qtyLoadingIn,
					QtyLoading = item.qtyloading,
					QtyLoadingAdjs = item.qtyLoadingAdj,
					EndBalanceLoadingQty = item.beginingloading + item.qtyLoadingIn - item.qtyloading - item.qtyLoadingAdj,
					BeginingBalanceSewingQty = item.beginingSewing,
					QtySewingIn = item.sewingIn,
					QtySewingOut = item.sewingout,
					QtySewingInTransfer = item.sewingintransfer,
					QtySewingRetur = item.sewingretur,
					WipSewingOut = item.wipsewing,
					WipFinishingOut = item.wipfinishing,
					QtySewingAdj = item.sewingadj,
					EndBalanceSewingQty = item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj,
					BeginingBalanceFinishingQty = item.beginingbalanceFinishing,
					FinishingInExpenditure = item.finishingout + item.subconout,
					FinishingInQty = item.finishinginqty,
					FinishingOutQty = item.finishingout,
					BeginingBalanceSubconQty = item.beginingbalancesubcon,
					SubconInQty = item.subconIn,
					SubconOutQty = item.subconout,
					EndBalanceSubconQty = item.beginingbalancesubcon + item.subconIn - item.subconout,
					FinishingInTransferQty = item.finishingintransfer,
					FinishingReturQty = item.finishinigretur,
					FinishingAdjQty = item.finishingadj,
					BeginingBalanceExpenditureGood = item.beginingBalanceExpenditureGood,

					EndBalanceFinishingQty = item.beginingbalanceFinishing + item.finishingin - item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur,
					ExportQty = item.exportQty,
					SampleQty = item.sampleQty,
					OtherQty = item.otherqty,
					ExpenditureGoodAdj = item.expendAdj,
					EndBalanceExpenditureGood = item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur - item.finishingintransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj


				};
				monitoringDtos.Add(garmentMonitoringDto);
			}
			 
			garmentMonitoringProductionFlow.garmentMonitorings = monitoringDtos;

			return garmentMonitoringProductionFlow;
		}
	}
}
