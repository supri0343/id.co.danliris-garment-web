using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.MonitoringProductionStockFlow;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;

namespace Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries
{
    public class GetXlsMonitoringProductionStockFlowMIIQueryHandler : IQueryHandler<GetXlsMonitoringProductionStockFlowMIIQuery, MemoryStream>
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
        private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentFinishingInRepository garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository garmentFinishingInItemRepository;
        private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
        private readonly IGarmentExpenditureGoodReturnRepository garmentExpenditureGoodReturnRepository;
        private readonly IGarmentExpenditureGoodReturnItemRepository garmentExpenditureGoodReturnItemRepository;
        private readonly IGarmentSewingDORepository garmentSewingDORepository;
        private readonly IGarmentSewingDOItemRepository garmentSewingDOItemRepository;
        private readonly IGarmentPreparingRepository garmentPreparingRepository;
        private readonly IGarmentBalanceMonitoringProductionStockFlowRepository garmentBalanceProductionStockRepository;
        private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
        private string unitName;
        public GetXlsMonitoringProductionStockFlowMIIQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentBalanceProductionStockRepository = storage.GetRepository<IGarmentBalanceMonitoringProductionStockFlowRepository>();
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
            garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
            garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
            garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
            garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
           
            garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
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
            public double QtyLoadingInTransfer { get; internal set; } 
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
            public double SubconInPrice { get; internal set; }
            public double SubconOutQty { get; internal set; }
            public double EndBalanceSubconQty { get; internal set; }
            public double FinishingOutQty { get; internal set; }
            public double FinishingInTransferQty { get; internal set; }
            public double FinishingAdjQty { get; internal set; }
            public double FinishingReturQty { get; internal set; }
            public double EndBalanceFinishingQty { get; internal set; }
            public double BeginingBalanceExpenditureGood { get; internal set; }
            public double FinishingTransferExpenditure { get; internal set; }
            public double ExpenditureGoodRetur { get; internal set; }
            public double ExportQty { get; internal set; }
            public double OtherQty { get; internal set; }
            public double SampleQty { get; internal set; }
            public double ExpenditureGoodRemainingQty { get; internal set; }
            public double ExpenditureGoodAdj { get; internal set; }
            public double ExpenditureGoodAdjPrice { get; internal set; }
            public double EndBalanceExpenditureGood { get; internal set; }
            public double ExpenditureGoodInTransfer { get; internal set; }

        }

        class monitoringUnionView
        {
            public string ro { get; internal set; }
            public string article { get; internal set; }
            public string comodity { get; internal set; }
            public double fc { get; internal set; }
            public decimal fare { get; internal set; }
            public decimal farenew { get; internal set; }
            public decimal basicprice { get; internal set; }
            public double qtycutting { get; internal set; }
            public double priceCuttingOut { get; internal set; }
            public double qtCuttingSubkon { get; internal set; }
            public double priceCuttingSubkon { get; internal set; }
            public double qtyCuttingTransfer { get; internal set; }
            public double priceCuttingTransfer { get; internal set; }
            public double qtyCuttingIn { get; internal set; }
            public double priceCuttingIn { get; internal set; }
            public double begining { get; internal set; }
            public double beginingcuttingPrice { get; internal set; }
            public double qtyavalsew { get; internal set; }
            public double priceavalsew { get; internal set; }
            public double qtyavalcut { get; internal set; }
            public double priceavalcut { get; internal set; }
            public double beginingloading { get; internal set; }
            public double beginingloadingPrice { get; internal set; }
            public double qtyLoadingIn { get; internal set; }
            public double priceLoadingIn { get; internal set; }
            public double qtyloading { get; internal set; }
            public double priceloading { get; internal set; }
            public double qtyLoadingAdj { get; internal set; }
            public double priceLoadingAdj { get; internal set; }
            public double beginingSewing { get; internal set; }
            public double beginingSewingPrice { get; internal set; }
            public double sewingIn { get; internal set; }
            public double sewingInPrice { get; internal set; }
            public double sewingintransfer { get; internal set; }
            public double sewingintransferPrice { get; internal set; }
            public double sewingout { get; internal set; }
            public double sewingoutPrice { get; internal set; }
            public double sewingretur { get; internal set; }
            public double sewingreturPrice { get; internal set; }
            public double wipsewing { get; internal set; }
            public double wipsewingPrice { get; internal set; }
            public double wipfinishing { get; internal set; }
            public double wipfinishingPrice { get; internal set; }
            public double sewingadj { get; internal set; }
            public double sewingadjPrice { get; internal set; }
            public double finishingin { get; internal set; }
            public double finishinginPrice { get; internal set; }
            public double finishingintransfer { get; internal set; }
            public double finishingintransferPrice { get; internal set; }
            public double finishingadj { get; internal set; }
            public double finishingadjPrice { get; internal set; }
            public double finishingout { get; internal set; }
            public double finishingoutPrice { get; internal set; }
            public double finishinigretur { get; internal set; }
            public double finishinigreturPrice { get; internal set; }
            public double beginingbalanceFinishing { get; internal set; }
            public double beginingbalanceFinishingPrice { get; internal set; }
            public double beginingbalancesubcon { get; internal set; }
            public double beginingbalancesubconPrice { get; internal set; }
            public double subconIn { get; internal set; }
            public double subconInPrice { get; internal set; }
            public double subconout { get; internal set; }
            public double subconoutPrice { get; internal set; }
            public double exportQty { get; internal set; }
            public double exportPrice { get; internal set; }
            public double otherqty { get; internal set; }
            public double otherprice { get; internal set; }
            public double sampleQty { get; internal set; }
            public double samplePrice { get; internal set; }
            public double expendAdj { get; internal set; }
            public double expendAdjPrice { get; internal set; }
            public double expendRetur { get; internal set; }
            public double expendReturPrice { get; internal set; }
            //finishinginqty =group.Sum(s=>s.FinishingInQty)
            public double beginingBalanceExpenditureGood { get; internal set; }
            public double beginingBalanceExpenditureGoodPrice { get; internal set; }
            public double expenditureInTransfer { get; internal set; }
            public double expenditureInTransferPrice { get; internal set; }
            public double qtyloadingInTransfer { get; internal set; }
            public double priceloadingInTransfer { get; internal set; }

        }

        public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
        {
            CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

            var listRO = string.Join(",", ro.Distinct());
            var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/";

            var httpContent = new StringContent(JsonConvert.SerializeObject(listRO), Encoding.UTF8, "application/json");

            var httpResponse = await _http.SendAsync(HttpMethod.Get, costCalculationUri, token, httpContent);

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
            else
            {
                var err = await httpResponse.Content.ReadAsStringAsync();

            }

            return costCalculationGarmentDataProductionReport;
        }

        class ViewBasicPrices
        {
            public string RO { get; internal set; }
            public decimal BasicPrice { get; internal set; }
            public int Count { get; internal set; }
        }
        class ViewFC
        {
            public string RO { get; internal set; }
            public double FC { get; internal set; }
            public double Count { get; internal set; }
        }
        public async Task<MemoryStream> Handle(GetXlsMonitoringProductionStockFlowMIIQuery request, CancellationToken cancellationToken)
        {
            //DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
            //DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            DateTimeOffset dateBalance = (from a in garmentBalanceProductionStockRepository.Query.OrderByDescending(s => s.CreatedDate)
                                          select a.CreatedDate).FirstOrDefault();
            DateTimeOffset dateFareNew = dateTo.AddDays(1);


           
            var queryGroup = (from a in (from aa in garmentCuttingOutRepository.Query
                                         where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro))
                                         select new { aa.RONo, aa.ComodityId, aa.ComodityName, aa.Article })
                              select new
                              {
                                       Ro = a.RONo,
                                  Article = a.Article,
                                  Comodity = a.ComodityName
                               }).Distinct();
            unitName = (from aa in garmentCuttingOutRepository.Query
                        where aa.UnitId == request.unit
                        select aa.UnitName).FirstOrDefault().ToString();

            var queryBalance = from a in
                                      (from aa in garmentBalanceProductionStockRepository.Query
                                       where aa.CreatedDate.AddHours(7) < dateFrom && (request.ro == null || (request.ro != null && request.ro != "" && aa.Ro == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.UnitId == aa.UnitId
                                       select aa)

                               select new monitoringView
                               {
                                   QtyCuttingIn = 0,
                                    
                                   QtySewingIn = 0,
                                   QtyCuttingTransfer = 0,
                                   QtyCuttingsubkon = 0,
                                   AvalCutting = 0,
                                   AvalSewing = 0,
                                   QtyLoading = 0,
                                   QtyLoadingAdjs = 0,
                                   QtySewingOut = 0,
                                   QtySewingAdj = 0,
                                   WipSewingOut = 0,
                                   WipFinishingOut = 0,
                                   QtySewingRetur = 0,
                                   QtySewingInTransfer = 0,
                                   FinishingInQty = 0,
                                   SubconInQty = 0,
                                   SubconInPrice = 0,
                                   FinishingAdjQty = 0,
                                   FinishingTransferExpenditure = 0,
                                   FinishingInTransferQty = 0,
                                   FinishingOutQty = 0,
                                   FinishingReturQty = 0,
                                   SubconOutQty = 0,
                                   //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
                                   //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
                                   BeginingBalanceLoadingQty = a.BeginingBalanceLoadingQty,
                                   BeginingBalanceCuttingQty = a.BeginingBalanceCuttingQty,
                                   BeginingBalanceFinishingQty = a.BeginingBalanceFinishingQty,
                                   BeginingBalanceSewingQty = a.BeginingBalanceSewingQty,
                                   BeginingBalanceExpenditureGood = a.BeginingBalanceExpenditureGood,
                                   BeginingBalanceSubconQty = a.BeginingBalanceSubconQty,
                                   Ro = a.Ro,
                                   ExpenditureGoodRetur = 0,
                                   QtyCuttingOut = 0,
                                   ExportQty = 0,
                                   SampleQty = 0,
                                   OtherQty = 0,
                                   QtyLoadingInTransfer = 0,
                                   ExpenditureGoodInTransfer = 0

                               };


            var QueryCuttingOut = (from a in (from aa in garmentCuttingOutRepository.Query
                                              where aa.CuttingOutDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.CuttingOutDate.AddHours(7) <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId == aa.UnitFromId
                                              select new { aa.RONo, aa.Identity, aa.CuttingOutDate, aa.CuttingOutType })
                                   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId

                                   select new
                                   {
                                       BeginingBalanceCuttingQty = a.CuttingOutDate.AddHours(7) < dateFrom && a.CuttingOutDate.AddHours(7) > dateBalance ? -c.CuttingOutQuantity : 0,
                                       BeginingBalanceCuttingPrice = a.CuttingOutDate.AddHours(7) < dateFrom && a.CuttingOutDate.AddHours(7) > dateBalance ? -c.Price : 0,
                                       Ro = a.RONo,
                                       QtyCuttingOut = a.CuttingOutDate.AddHours(7) >= dateFrom ? c.CuttingOutQuantity : 0,
                                       PriceCuttingOut = a.CuttingOutDate.AddHours(7) >= dateFrom ? c.Price : 0,
                                   }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                   {
                                       QtyCuttingIn = 0,
                                      
                                       QtySewingIn = 0,
                                       
                                       QtyCuttingTransfer = 0,
                                       
                                       QtyCuttingsubkon = 0,
                                       
                                       AvalCutting = 0,
                                       
                                       AvalSewing = 0,
                                       
                                       QtyLoading = 0,
                                       
                                       QtyLoadingAdjs = 0,
                                       
                                       QtySewingOut = 0,
                                       
                                       QtySewingAdj = 0,
                                       
                                       WipSewingOut = 0,
                                       
                                       WipFinishingOut = 0,
                                       
                                       QtySewingRetur = 0,
                                       
                                       QtySewingInTransfer = 0,
                                       
                                       FinishingInQty = 0,
                                       
                                       SubconInQty = 0,
                                       SubconInPrice = 0,
                                       FinishingAdjQty = 0,
                                       
                                       FinishingTransferExpenditure = 0,
                                       
                                       FinishingInTransferQty = 0,
                                       
                                       FinishingOutQty = 0,
                                       
                                       FinishingReturQty = 0,
                                       
                                       SubconOutQty = 0,
                                       
                                       BeginingBalanceLoadingQty = 0,
                                        
                                       BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
                                      
                                       Ro = key,
                                       ExpenditureGoodRetur = 0,
                                       
                                       QtyCuttingOut = group.Sum(x => x.QtyCuttingOut),
                                       
                                       ExportQty = 0,
                                       
                                       SampleQty = 0,
                                       
                                       OtherQty = 0,
                                       
                                       QtyLoadingInTransfer = 0,
                                      
                                       ExpenditureGoodInTransfer = 0,
                                       
                                       BeginingBalanceFinishingQty = 0 

                                   });
            var QueryCuttingOutSubkon = (from a in (from aa in garmentCuttingOutRepository.Query
                                                    where aa.CuttingOutDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == (request.unit == 0 ? aa.UnitFromId : request.unit) && aa.CuttingOutDate.AddHours(7) <= dateTo && aa.CuttingOutType == "SUBKON"
                                                    select new { aa.RONo, aa.Identity, aa.CuttingOutDate, aa.CuttingOutType })
                                         join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                         join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
                                         select new
                                         {

                                             BeginingBalanceCuttingQty = a.CuttingOutDate.AddHours(7) < dateFrom && a.CuttingOutDate.AddHours(7) > dateBalance ? -c.CuttingOutQuantity : 0,
                                             Ro = a.RONo,
                                             BeginingBalanceCuttingPrice = a.CuttingOutDate.AddHours(7) < dateFrom && a.CuttingOutDate.AddHours(7) > dateBalance ? -c.Price : 0,
                                             QtyCuttingsubkon = a.CuttingOutDate.AddHours(7) >= dateFrom ? c.CuttingOutQuantity : 0,
                                             PriceCuttingsubkon = a.CuttingOutDate.AddHours(7) >= dateFrom ? c.Price : 0,
                                         }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                         {
                                             QtyCuttingIn = 0,
                                            
                                             QtySewingIn = 0,
                                             
                                             QtyCuttingOut = 0,
                                             
                                             QtyCuttingTransfer = 0,
                                             
                                             AvalCutting = 0,
                                             
                                             AvalSewing = 0,
                                             
                                             QtyLoading = 0,
                                             
                                             QtyLoadingAdjs = 0,
                                             
                                             QtySewingOut = 0,
                                             
                                             QtySewingAdj = 0,
                                             
                                             WipSewingOut = 0,
                                             
                                             WipFinishingOut = 0,
                                             
                                             QtySewingRetur = 0,
                                             
                                             QtySewingInTransfer = 0,
                                             
                                             FinishingInQty = 0,
                                             
                                             SubconInQty = 0,
                                             SubconInPrice = 0,
                                             FinishingAdjQty = 0,
                                             
                                             FinishingTransferExpenditure = 0,
                                             
                                             FinishingInTransferQty = 0,
                                             
                                             FinishingOutQty = 0,
                                             
                                             FinishingReturQty = 0,
                                             
                                             SubconOutQty = 0,
                                             
                                             BeginingBalanceLoadingQty = 0,
                                              
                                             BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
                                             Ro = key,
                                            
                                            
                                             QtyCuttingsubkon = group.Sum(x => x.QtyCuttingsubkon),
                                            
                                             ExpenditureGoodRetur = 0,
                                             
                                             ExportQty = 0,
                                             
                                             SampleQty = 0,
                                             
                                             OtherQty = 0,
                                             
                                             QtyLoadingInTransfer = 0,
                                            
                                             ExpenditureGoodInTransfer = 0,
                                             
                                             BeginingBalanceFinishingQty = 0 

                                         });
            var QueryCuttingOutTransfer = (from a in (from aa in garmentCuttingOutRepository.Query
                                                      where aa.CuttingOutDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == (request.unit == 0 ? aa.UnitFromId : request.unit) && aa.CuttingOutDate.AddHours(7) <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId != aa.UnitFromId
                                                      select new { aa.RONo, aa.Identity, aa.CuttingOutType, aa.CuttingOutDate })
                                           join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                           join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
                                           select new
                                           {
                                               BeginingBalanceCuttingQty = a.CuttingOutDate.AddHours(7) < dateFrom && a.CuttingOutDate.AddHours(7) > dateBalance ? -c.CuttingOutQuantity : 0,
                                               BeginingBalanceCuttingPrice = a.CuttingOutDate.AddHours(7) < dateFrom && a.CuttingOutDate.AddHours(7) > dateBalance ? -c.Price : 0,
                                               Ro = a.RONo,
                                               QtyCuttingTransfer = a.CuttingOutDate.AddHours(7) >= dateFrom ? c.CuttingOutQuantity : 0,
                                               PriceCuttingTransfer = a.CuttingOutDate.AddHours(7) >= dateFrom ? c.Price : 0,
                                           }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                           {
                                               QtyCuttingIn = 0,
                                              
                                               QtySewingIn = 0,
                                               
                                               QtyCuttingOut = 0,
                                               
                                               QtyCuttingsubkon = 0,
                                               
                                               AvalCutting = 0,
                                               
                                               AvalSewing = 0,
                                               
                                               QtyLoading = 0,
                                               
                                               QtyLoadingAdjs = 0,
                                               
                                               QtySewingOut = 0,
                                               
                                               QtySewingAdj = 0,
                                               
                                               WipSewingOut = 0,
                                               
                                               WipFinishingOut = 0,
                                               
                                               QtySewingRetur = 0,
                                               
                                               QtySewingInTransfer = 0,
                                               
                                               FinishingInQty = 0,
                                               
                                               SubconInQty = 0,
                                               SubconInPrice = 0,
                                               FinishingAdjQty = 0,
                                               
                                               FinishingTransferExpenditure = 0,
                                               
                                               FinishingInTransferQty = 0,
                                               
                                               FinishingOutQty = 0,
                                               
                                               FinishingReturQty = 0,
                                               
                                               SubconOutQty = 0,
                                               
                                               //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
                                               //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
                                               BeginingBalanceLoadingQty = 0,
                                               
                                               BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
                                              
                                               Ro = key,
                                               QtyCuttingTransfer = group.Sum(x => x.QtyCuttingTransfer),
                                               
                                               ExpenditureGoodRetur = 0,
                                               
                                               ExportQty = 0,
                                               
                                               SampleQty = 0,
                                               
                                               OtherQty = 0,
                                               
                                               QtyLoadingInTransfer = 0,
                                              
                                               ExpenditureGoodInTransfer = 0,
                                               
                                               BeginingBalanceFinishingQty = 0
                                               

                                           });
            var QueryCuttingIn = (from a in (from aa in garmentCuttingInRepository.Query
                                             where aa.CuttingInDate.AddHours(7) >= dateBalance && aa.CuttingType != "Non Main Fabric" && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.CuttingInDate.AddHours(7) <= dateTo
                                             select new { aa.RONo, aa.Identity, aa.CuttingInDate })
                                  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                  select new
                                  {

                                      BeginingBalanceCuttingQty = a.CuttingInDate.AddHours(7) < dateFrom && a.CuttingInDate.AddHours(7) > dateBalance ? c.CuttingInQuantity : 0,
                                      BeginingBalanceCuttingPrice = a.CuttingInDate.AddHours(7) < dateFrom && a.CuttingInDate.AddHours(7) > dateBalance ? c.Price : 0,
                                      Ro = a.RONo,
                                      QtyCuttingIn = a.CuttingInDate.AddHours(7) >= dateFrom ? c.CuttingInQuantity : 0,
                                      PriceCuttingIn = a.CuttingInDate.AddHours(7) >= dateFrom ? c.Price : 0,

                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                  {
                                      QtySewingIn = 0,
                                      
                                      QtyCuttingOut = 0,
                                      
                                      QtyCuttingTransfer = 0,
                                      
                                      QtyCuttingsubkon = 0,
                                      
                                      AvalCutting = 0,
                                      
                                      AvalSewing = 0,
                                      
                                      QtyLoading = 0,
                                      
                                      QtyLoadingAdjs = 0,
                                      
                                      QtySewingOut = 0,
                                      
                                      QtySewingAdj = 0,
                                      
                                      WipSewingOut = 0,
                                      
                                      WipFinishingOut = 0,
                                      
                                      QtySewingRetur = 0,
                                      
                                      QtySewingInTransfer = 0,
                                      
                                      FinishingInQty = 0,
                                      
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      
                                      FinishingTransferExpenditure = 0,
                                      
                                      FinishingInTransferQty = 0,
                                      
                                      FinishingOutQty = 0,
                                      
                                      FinishingReturQty = 0,
                                      
                                      SubconOutQty = 0,
                                      
                                      BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
                                     
                                      Ro = key,
                                      QtyCuttingIn = group.Sum(x => x.QtyCuttingIn),
                                       
                                      ExpenditureGoodRetur = 0,
                                      
                                      ExportQty = 0,
                                      
                                      SampleQty = 0,
                                      
                                      OtherQty = 0,
                                      
                                      QtyLoadingInTransfer = 0,
                                     
                                      ExpenditureGoodInTransfer = 0,
                                      
                                      BeginingBalanceLoadingQty = 0,
                                       
                                      BeginingBalanceFinishingQty = 0
                                      
                                  });


            var QueryAvalCompSewing = (from a in (from aa in garmentAvalComponentRepository.Query
                                                  where aa.Date.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.Date.AddHours(7) <= dateTo && aa.AvalComponentType == "SEWING"
                                                  select new { aa.RONo, aa.Identity, aa.Date, aa.AvalComponentType })
                                       join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                                       select new
                                       {
                                           Ro = a.RONo,
                                           AvalSewing = a.Date.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                           AvalSewingPrice = a.Date.AddHours(7) >= dateFrom ? Convert.ToDouble(b.Price) : 0,
                                           BeginingBalanceCuttingQty = a.Date.AddHours(7) < dateFrom && a.Date.AddHours(7) > dateBalance ? -b.Quantity : 0,

                                       }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                       {
                                           QtySewingIn = 0,
                                           
                                           QtyCuttingOut = 0,
                                           
                                           QtyCuttingTransfer = 0,
                                           
                                           QtyCuttingsubkon = 0,
                                           
                                           AvalCutting = 0,
                                           
                                           QtyLoading = 0,
                                           
                                           QtyLoadingAdjs = 0,
                                           
                                           QtySewingOut = 0,
                                           
                                           QtySewingAdj = 0,
                                           
                                           WipSewingOut = 0,
                                           
                                           WipFinishingOut = 0,
                                           
                                           QtySewingRetur = 0,
                                           
                                           QtySewingInTransfer = 0,
                                           
                                           FinishingInQty = 0,
                                           
                                           SubconInQty = 0,
                                           SubconInPrice = 0,
                                           FinishingAdjQty = 0,
                                           
                                           FinishingTransferExpenditure = 0,
                                           
                                           FinishingInTransferQty = 0,
                                           
                                           FinishingOutQty = 0,
                                           
                                           FinishingReturQty = 0,
                                           
                                           SubconOutQty = 0,
                                           
                                           BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty), //0,a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
                                           //a.Date < dateFrom && a.Date > dateBalance  ? -Convert.ToDouble(b.Price) : 0,
                                           Ro = key,
                                           QtyCuttingIn = 0,
                                          
                                           AvalSewing = group.Sum(x => x.AvalSewing),
                                           
                                           ExpenditureGoodRetur = 0,
                                           
                                           ExportQty = 0,
                                           
                                           SampleQty = 0,
                                           
                                           OtherQty = 0,
                                           
                                           QtyLoadingInTransfer = 0,
                                          
                                           ExpenditureGoodInTransfer = 0,
                                           
                                           BeginingBalanceLoadingQty = 0,
                                            
                                           BeginingBalanceFinishingQty = 0
                                           
                                       });
            var QueryAvalCompCutting = (from a in (from aa in garmentAvalComponentRepository.Query
                                                   where aa.Date.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.Date.AddHours(7) <= dateTo && aa.AvalComponentType == "CUTTING"
                                                   select new { aa.RONo, aa.Identity, aa.Date, aa.AvalComponentType })
                                        join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                                        select new
                                        {
                                            Ro = a.RONo,
                                            AvalCutting = a.Date.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                            AvalCuttingPrice = a.Date.AddHours(7) >= dateFrom ? Convert.ToDouble(b.Price) : 0,
                                            BeginingBalanceCuttingQty = a.Date.AddHours(7) < dateFrom && a.Date.AddHours(7) > dateBalance ? -b.Quantity : 0,
                                            //BeginingBalanceCuttingPrice = a.Date < dateFrom && a.Date > dateBalance ? -Convert.ToDouble(b.Price) : 0
                                        }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                        {
                                            QtyCuttingIn = 0,
                                           
                                            QtySewingIn = 0,
                                            
                                            QtyCuttingOut = 0,
                                            
                                            QtyCuttingTransfer = 0,
                                            
                                            QtyCuttingsubkon = 0,
                                            
                                            AvalSewing = 0,
                                            
                                            QtyLoading = 0,
                                            
                                            QtyLoadingAdjs = 0,
                                            
                                            QtySewingOut = 0,
                                            
                                            QtySewingAdj = 0,
                                            
                                            WipSewingOut = 0,
                                            
                                            WipFinishingOut = 0,
                                            
                                            QtySewingRetur = 0,
                                            
                                            QtySewingInTransfer = 0,
                                            
                                            FinishingInQty = 0,
                                            
                                            SubconInQty = 0,
                                            SubconInPrice = 0,
                                            FinishingAdjQty = 0,
                                            
                                            FinishingTransferExpenditure = 0,
                                            
                                            FinishingInTransferQty = 0,
                                            
                                            FinishingOutQty = 0,
                                            
                                            FinishingReturQty = 0,
                                            
                                            SubconOutQty = 0,
                                            
                                            BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty), //0,a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
                                            // a.Date < dateFrom && a.Date > dateBalance ? -Convert.ToDouble(b.Price) : 0,
                                            Ro = key,
                                            AvalCutting = group.Sum(x => x.AvalCutting),
                                            
                                            ExpenditureGoodRetur = 0,
                                            
                                            ExportQty = 0,
                                            
                                            SampleQty = 0,
                                            
                                            OtherQty = 0,
                                            
                                            QtyLoadingInTransfer = 0,
                                           
                                            ExpenditureGoodInTransfer = 0,
                                            
                                            BeginingBalanceLoadingQty = 0,
                                             
                                            BeginingBalanceFinishingQty = 0
                                            
                                        });
            var QuerySewingDO = (from a in (from aa in garmentSewingDORepository.Query
                                            where aa.SewingDODate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.UnitFromId == aa.UnitId && aa.SewingDODate.AddHours(7) <= dateTo
                                            select new { aa.RONo, aa.Identity, aa.SewingDODate })
                                 join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
                                 select new
                                 {

                                     QtyLoadingIn = a.SewingDODate.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                     PriceLoadingIn = a.SewingDODate.AddHours(7) >= dateFrom ? b.Price : 0,
                                     BeginingBalanceLoadingQty = (a.SewingDODate.AddHours(7) < dateFrom && a.SewingDODate.AddHours(7) > dateBalance) ? b.Quantity : 0,
                                     BeginingBalanceLoadingPrice = (a.SewingDODate.AddHours(7) < dateFrom && a.SewingDODate.AddHours(7) > dateBalance) ? b.Price : 0,
                                     Ro = a.RONo,

                                 }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                 {
                                     QtyCuttingIn = 0,
                                    
                                     QtySewingIn = 0,
                                     
                                     QtyCuttingOut = 0,
                                     
                                     QtyCuttingTransfer = 0,
                                     
                                     QtyCuttingsubkon = 0,
                                     
                                     AvalCutting = 0,
                                     
                                     AvalSewing = 0,
                                     
                                     QtyLoading = 0,
                                     
                                     QtyLoadingAdjs = 0,
                                     
                                     QtySewingOut = 0,
                                     
                                     QtySewingAdj = 0,
                                     
                                     WipSewingOut = 0,
                                     
                                     WipFinishingOut = 0,
                                     
                                     QtySewingRetur = 0,
                                     
                                     QtySewingInTransfer = 0,
                                     
                                     FinishingInQty = 0,
                                     
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     
                                     FinishingTransferExpenditure = 0,
                                     
                                     FinishingInTransferQty = 0,
                                     
                                     FinishingOutQty = 0,
                                     
                                     FinishingReturQty = 0,
                                     
                                     SubconOutQty = 0,
                                     
                                     QtyLoadingIn = group.Sum(x => x.QtyLoadingIn),
                                     
                                     BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty),
                                      
                                     Ro = key,
                                     ExpenditureGoodRetur = 0,
                                     
                                     ExportQty = 0,
                                     
                                     SampleQty = 0,
                                     
                                     OtherQty = 0,
                                     
                                     QtyLoadingInTransfer = 0,
                                    
                                     ExpenditureGoodInTransfer = 0,
                                     
                                     BeginingBalanceCuttingQty = 0,
                                     
                                     BeginingBalanceFinishingQty = 0
                                     
                                 });

            var QueryLoadingInTransfer = (from a in (from aa in garmentSewingDORepository.Query
                                                     where aa.SewingDODate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.UnitFromId != aa.UnitId && aa.SewingDODate.AddHours(7) <= dateTo
                                                     select new { aa.RONo, aa.Identity, aa.SewingDODate })
                                          join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
                                          select new
                                          {

                                              QtyLoadingInTransfer = a.SewingDODate.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                              PriceLoadingInTransfer = a.SewingDODate.AddHours(7) >= dateFrom ? b.Price : 0,
                                              BeginingBalanceLoadingQty = (a.SewingDODate.AddHours(7) < dateFrom && a.SewingDODate.AddHours(7) > dateBalance) ? b.Quantity : 0,
                                              BeginingBalanceLoadingPrice = (a.SewingDODate.AddHours(7) < dateFrom && a.SewingDODate.AddHours(7) > dateBalance) ? b.Price : 0,
                                              Ro = a.RONo,

                                          }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                          {
                                              QtyCuttingIn = 0,
                                             
                                              QtySewingIn = 0,
                                              
                                              QtyCuttingOut = 0,
                                              
                                              QtyCuttingTransfer = 0,
                                              
                                              QtyCuttingsubkon = 0,
                                              
                                              AvalCutting = 0,
                                              
                                              AvalSewing = 0,
                                              
                                              QtyLoading = 0,
                                              
                                              QtyLoadingAdjs = 0,
                                              
                                              QtySewingOut = 0,
                                              
                                              QtySewingAdj = 0,
                                              
                                              WipSewingOut = 0,
                                              
                                              WipFinishingOut = 0,
                                              
                                              QtySewingRetur = 0,
                                              
                                              QtySewingInTransfer = 0,
                                              
                                              FinishingInQty = 0,
                                              
                                              SubconInQty = 0,
                                              SubconInPrice = 0,
                                              FinishingAdjQty = 0,
                                              
                                              FinishingTransferExpenditure = 0,
                                              
                                              FinishingInTransferQty = 0,
                                              
                                              FinishingOutQty = 0,
                                              
                                              FinishingReturQty = 0,
                                              
                                              SubconOutQty = 0,
                                              
                                              QtyLoadingInTransfer = group.Sum(x => x.QtyLoadingInTransfer),
                                               
                                              BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty), 
                                              Ro = key,
                                              ExpenditureGoodRetur = 0,
                                              
                                              ExportQty = 0,
                                              
                                              SampleQty = 0,
                                              
                                              OtherQty = 0,
                                              
                                              ExpenditureGoodInTransfer = 0,
                                              
                                              BeginingBalanceCuttingQty = 0,
                                              
                                              BeginingBalanceFinishingQty = 0
                                              
                                          });
            var QueryLoading = (from a in (from aa in garmentLoadingRepository.Query
                                           where aa.LoadingDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.LoadingDate.AddHours(7) <= dateTo
                                           select new { aa.RONo, aa.Identity, aa.LoadingDate, aa.UnitId, aa.UnitFromId })
                                join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
                                select new
                                {
                                    BeginingBalanceLoadingQty = a.LoadingDate.AddHours(7) < dateFrom && a.UnitId == a.UnitFromId && a.LoadingDate.AddHours(7) > dateBalance ? -b.Quantity : 0,
                                    BeginingBalanceLoadingPrice = a.LoadingDate.AddHours(7) < dateFrom && a.UnitId == a.UnitFromId && a.LoadingDate.AddHours(7) > dateBalance ? -b.Price : 0,
                                    Ro = a.RONo,
                                    QtyLoading = a.LoadingDate.AddHours(7) >= dateFrom && a.UnitId == a.UnitFromId ? b.Quantity : 0,
                                    PriceLoading = a.LoadingDate.AddHours(7) >= dateFrom && a.UnitId == a.UnitFromId ? b.Price : 0,

                                }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                {
                                    QtyCuttingIn = 0,
                                   
                                    QtySewingIn = 0,
                                    
                                    QtyCuttingOut = 0,
                                    
                                    QtyCuttingTransfer = 0,
                                    
                                    QtyCuttingsubkon = 0,
                                    
                                    AvalCutting = 0,
                                    
                                    AvalSewing = 0,
                                    
                                    QtyLoadingAdjs = 0,
                                    
                                    QtySewingOut = 0,
                                    
                                    QtySewingAdj = 0,
                                    
                                    WipSewingOut = 0,
                                    
                                    WipFinishingOut = 0,
                                    
                                    QtySewingRetur = 0,
                                    
                                    QtySewingInTransfer = 0,
                                    
                                    FinishingInQty = 0,
                                    
                                    SubconInQty = 0,
                                    SubconInPrice = 0,
                                    FinishingAdjQty = 0,
                                    
                                    FinishingTransferExpenditure = 0,
                                    
                                    FinishingInTransferQty = 0,
                                    
                                    FinishingOutQty = 0,
                                    
                                    FinishingReturQty = 0,
                                    
                                    SubconOutQty = 0,
                                    
                                    QtyLoadingInTransfer = 0,
                                   
                                    // BeginingBalanceSewingQty = a.LoadingDate < dateFrom ? b.Quantity : 0,
                                    //BeginingBalanceSewingPrice = a.LoadingDate < dateFrom ? b.Price : 0,
                                    BeginingBalanceSewingQty = 0,
                                     
                                    BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty), 
                                    Ro = key,
                                    QtyLoading = group.Sum(x => x.QtyLoading), 
                                    ExpenditureGoodRetur = 0,
                                    
                                    ExportQty = 0,
                                    
                                    SampleQty = 0,
                                    
                                    OtherQty = 0,
                                    
                                    ExpenditureGoodInTransfer = 0,
                                    
                                    BeginingBalanceCuttingQty = 0,
                                    
                                    BeginingBalanceFinishingQty = 0
                                    
                                }); 
            var QueryLoadingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
                                              where aa.AdjustmentDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate.AddHours(7) <= dateTo && aa.AdjustmentType == "LOADING"
                                              select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                   join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                   select new
                                   {

                                       BeginingBalanceLoadingQty = a.AdjustmentDate.AddHours(7) < dateFrom && a.AdjustmentDate.AddHours(7) > dateBalance ? -b.Quantity : 0,
                                       BeginingBalanceLoadingPrice = a.AdjustmentDate.AddHours(7) < dateFrom && a.AdjustmentDate.AddHours(7) > dateBalance ? -b.Price : 0,
                                       Ro = a.RONo,
                                       QtyLoadingAdjs = a.AdjustmentDate.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                       PriceLoadingAdjs = a.AdjustmentDate.AddHours(7) >= dateFrom ? b.Price : 0,

                                   }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                   {
                                       QtyCuttingIn = 0,
                                      
                                       QtySewingIn = 0,
                                       
                                       QtyCuttingOut = 0,
                                       
                                       QtyCuttingTransfer = 0,
                                       
                                       QtyCuttingsubkon = 0,
                                       
                                       AvalCutting = 0,
                                       
                                       AvalSewing = 0,
                                       
                                       QtyLoading = 0,
                                       
                                       QtySewingOut = 0,
                                       
                                       QtySewingAdj = 0,
                                       
                                       WipSewingOut = 0,
                                       
                                       WipFinishingOut = 0,
                                       
                                       QtySewingRetur = 0,
                                       
                                       QtySewingInTransfer = 0,
                                       
                                       FinishingInQty = 0,
                                       
                                       SubconInQty = 0,
                                       SubconInPrice = 0,
                                       FinishingAdjQty = 0,
                                       
                                       FinishingTransferExpenditure = 0,
                                       
                                       FinishingInTransferQty = 0,
                                       
                                       FinishingOutQty = 0,
                                       
                                       FinishingReturQty = 0,
                                       
                                       SubconOutQty = 0,
                                       
                                       BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty),
                                        
                                       Ro = key,
                                       QtyLoadingAdjs = group.Sum(x => x.QtyLoadingAdjs),
                                      
                                       ExpenditureGoodRetur = 0,
                                       
                                       ExportQty = 0,
                                       
                                       SampleQty = 0,
                                       
                                       OtherQty = 0,
                                       
                                       QtyLoadingInTransfer = 0,
                                      
                                       ExpenditureGoodInTransfer = 0,
                                       
                                       BeginingBalanceCuttingQty = 0,
                                       
                                       BeginingBalanceFinishingQty = 0
                                       
                                   });
            var QuerySewingIn = (from a in (from aa in garmentSewingInRepository.Query
                                            where aa.SewingInDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.SewingInDate.AddHours(7) <= dateTo
                                            select new { aa.RONo, aa.Identity, aa.SewingInDate, aa.SewingFrom })
                                 join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
                                 select new
                                 {
                                     BeginingBalanceSewingQty = (a.SewingInDate.AddHours(7) < dateFrom && a.SewingInDate.AddHours(7) > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Quantity : 0,
                                     BeginingBalanceSewingPrice = (a.SewingInDate.AddHours(7) < dateFrom && a.SewingInDate.AddHours(7) > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Price : 0,
                                     QtySewingIn = (a.SewingInDate.AddHours(7) >= dateFrom) && a.SewingFrom != "SEWING" ? b.Quantity : 0,
                                     PriceSewingIn = (a.SewingInDate.AddHours(7) >= dateFrom) && a.SewingFrom != "SEWING" ? b.Price : 0,
                                     Ro = a.RONo

                                 }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                 {
                                     QtyCuttingIn = 0,
                                    
                                     QtyCuttingOut = 0,
                                     
                                     QtyCuttingTransfer = 0,
                                     
                                     QtyCuttingsubkon = 0,
                                     
                                     AvalCutting = 0,
                                     
                                     AvalSewing = 0,
                                     
                                     QtyLoading = 0,
                                     
                                     QtyLoadingAdjs = 0,
                                     
                                     QtySewingOut = 0,
                                     
                                     QtySewingAdj = 0,
                                     
                                     WipSewingOut = 0,
                                     
                                     WipFinishingOut = 0,
                                     
                                     QtySewingRetur = 0,
                                     
                                     QtySewingInTransfer = 0,
                                     
                                     FinishingInQty = 0,
                                     
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     
                                     FinishingTransferExpenditure = 0,
                                     
                                     FinishingInTransferQty = 0,
                                     
                                     FinishingOutQty = 0,
                                     
                                     FinishingReturQty = 0,
                                     
                                     SubconOutQty = 0,
                                     
                                     BeginingBalanceSewingQty = group.Sum(x => x.BeginingBalanceSewingQty),
                                     QtySewingIn = group.Sum(x => x.QtySewingIn),
                                   
                                     Ro = key,
                                     ExpenditureGoodRetur = 0,
                                     
                                     ExportQty = 0,
                                     
                                     SampleQty = 0,
                                     
                                     OtherQty = 0,
                                     
                                     QtyLoadingInTransfer = 0,
                                    
                                     ExpenditureGoodInTransfer = 0,
                                     
                                     BeginingBalanceCuttingQty = 0,
                                     
                                     BeginingBalanceLoadingQty = 0,
                                     
                                     BeginingBalanceFinishingQty = 0
                                     
                                 });


            var QuerySewingOut = (from a in (from aa in garmentSewingOutRepository.Query
                                             where aa.SewingOutDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.SewingOutDate.AddHours(7) <= dateTo
                                             select new { aa.RONo, aa.Identity, aa.SewingOutDate, aa.SewingTo, aa.UnitToId, aa.UnitId })
                                  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId

                                  select new
                                  {

                                      FinishingTransferExpenditure = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      FinishingTransferExpenditurePrice = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      FinishingInTransferQty = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      FinishingInTransferPrice = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      BeginingBalanceFinishingQty = (a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      BeginingBalanceFinishingPrice = (a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      BeginingBalanceSewingQty = (a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Quantity : 0 - ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Quantity : 0) + ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0),
                                      BeginingBalanceSewingPrice = (a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Price : 0 - ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Price : 0) + ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate.AddHours(7) < dateFrom && a.SewingOutDate.AddHours(7) > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0),

                                      QtySewingRetur = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      PriceSewingRetur = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      QtySewingInTransfer = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      PriceSewingInTransfer = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      WipSewingOut = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      WipSewingOutPrice = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      WipFinishingOut = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      WipFinishingOutPrice = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      QtySewingOut = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      PriceSewingOut = (a.SewingOutDate.AddHours(7) >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,

                                      Ro = a.RONo,


                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                  {
                                      QtyCuttingIn = 0,
                                     
                                      QtySewingIn = 0,
                                      
                                      QtyCuttingOut = 0,
                                      
                                      QtyCuttingTransfer = 0,
                                      
                                      AvalCutting = 0,
                                      
                                      AvalSewing = 0,
                                      
                                      QtyLoading = 0,
                                      
                                      QtyLoadingAdjs = 0,
                                      
                                      QtySewingAdj = 0,
                                      
                                      FinishingInQty = 0,
                                      
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      
                                      FinishingOutQty = 0,
                                      
                                      FinishingReturQty = 0,
                                      
                                      SubconOutQty = 0,
                                      
                                      FinishingTransferExpenditure = group.Sum(x => x.FinishingTransferExpenditure),
                                     
                                      FinishingInTransferQty = group.Sum(x => x.FinishingInTransferQty),
                                      
                                      BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                      
                                      BeginingBalanceSewingQty = group.Sum(x => x.BeginingBalanceSewingQty),

                                      QtySewingRetur = group.Sum(x => x.QtySewingRetur),
                                      
                                      QtySewingInTransfer = group.Sum(x => x.QtySewingInTransfer),
                                      
                                      WipSewingOut = group.Sum(x => x.WipSewingOut),
                                      
                                      WipFinishingOut = group.Sum(x => x.WipFinishingOut),
                                      
                                      QtySewingOut = group.Sum(x => x.QtySewingOut),
                                      BeginingBalanceExpenditureGood = 0,
                                      
                                      Ro = key,
                                      ExpenditureGoodRetur = 0,
                                      
                                      QtyLoadingInTransfer = 0,
                                     
                                      ExportQty = 0,
                                      
                                      SampleQty = 0,
                                      
                                      OtherQty = 0,
                                      
                                      ExpenditureGoodInTransfer = 0,
                                      
                                      BeginingBalanceCuttingQty = 0,
                                      
                                      BeginingBalanceLoadingQty = 0
                                      
                                  });
             
            var QuerySewingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
                                             where aa.AdjustmentDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate.AddHours(7) <= dateTo && aa.AdjustmentType == "SEWING"
                                             select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                  join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                  select new
                                  {

                                      BeginingBalanceSewingQty = a.AdjustmentDate.AddHours(7) < dateFrom && a.AdjustmentDate.AddHours(7) > dateBalance ? -b.Quantity : 0,
                                      BeginingBalanceSewingPrice = a.AdjustmentDate.AddHours(7) < dateFrom && a.AdjustmentDate.AddHours(7) > dateBalance ? -b.Price : 0,
                                      Ro = a.RONo,
                                      QtySewingAdj = a.AdjustmentDate.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                      PriceSewingAdj = a.AdjustmentDate.AddHours(7) >= dateFrom ? b.Price : 0
                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                  {
                                      QtyCuttingIn = 0,
                                     
                                      QtySewingIn = 0,
                                      
                                      QtyCuttingOut = 0,
                                      
                                      QtyCuttingTransfer = 0,
                                      
                                      QtyCuttingsubkon = 0,
                                      
                                      AvalCutting = 0,
                                      
                                      AvalSewing = 0,
                                      
                                      QtyLoading = 0,
                                      
                                      QtyLoadingAdjs = 0,
                                      
                                      QtySewingOut = 0,
                                      
                                      WipSewingOut = 0,
                                      
                                      WipFinishingOut = 0,
                                      
                                      QtySewingRetur = 0,
                                      
                                      QtySewingInTransfer = 0,
                                      
                                      FinishingInQty = 0,
                                      
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      
                                      FinishingTransferExpenditure = 0,
                                      
                                      FinishingInTransferQty = 0,
                                      
                                      FinishingOutQty = 0,
                                      
                                      FinishingReturQty = 0,
                                      
                                      SubconOutQty = 0,
                                      
                                      BeginingBalanceSewingQty = group.Sum(x => x.BeginingBalanceSewingQty),
                                       
                                      Ro = key,
                                      QtySewingAdj = group.Sum(x => x.QtySewingAdj), 
                                      ExpenditureGoodRetur = 0,
                                      
                                      ExportQty = 0,
                                      
                                      SampleQty = 0,
                                      
                                      OtherQty = 0,
                                      
                                      QtyLoadingInTransfer = 0,
                                     
                                      ExpenditureGoodInTransfer = 0,
                                      
                                      BeginingBalanceCuttingQty = 0,
                                      
                                      BeginingBalanceLoadingQty = 0,
                                       
                                      BeginingBalanceFinishingQty = 0
                                      
                                  });
             

            var QueryFinishingIn = (from a in (from aa in garmentFinishingInRepository.Query
                                               where aa.FinishingInDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingInDate.AddHours(7) <= dateTo
                                               select new { aa.RONo, aa.Identity, aa.FinishingInDate, aa.FinishingInType })
                                    join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                    select new
                                    {

                                        //BeginingBalanceSubconQty = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                        //BeginingBalanceSubconPrice = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                        BeginingBalanceFinishingQty = (a.FinishingInDate.AddHours(7) < dateFrom && a.FinishingInDate.AddHours(7) > dateBalance && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                        BeginingBalanceFinishingPrice = (a.FinishingInDate.AddHours(7) < dateFrom && a.FinishingInDate.AddHours(7) > dateBalance && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                        FinishingInQty = (a.FinishingInDate.AddHours(7) >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                        FinishingInPrice = (a.FinishingInDate.AddHours(7) >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                        //SubconInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                        //SubconInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                        Ro = a.RONo,

                                    }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                    {
                                        QtyCuttingIn = 0,
                                       
                                        QtySewingIn = 0,
                                        
                                        QtyCuttingOut = 0,
                                        
                                        QtyCuttingTransfer = 0,
                                        
                                        QtyCuttingsubkon = 0,
                                        
                                        AvalCutting = 0,
                                        
                                        AvalSewing = 0,
                                        
                                        QtyLoading = 0,
                                        
                                        QtyLoadingAdjs = 0,
                                        
                                        QtySewingOut = 0,
                                        
                                        QtySewingAdj = 0,
                                        
                                        WipSewingOut = 0,
                                        
                                        WipFinishingOut = 0,
                                        
                                        QtySewingRetur = 0,
                                        
                                        QtySewingInTransfer = 0,
                                        
                                        FinishingAdjQty = 0,
                                        
                                        FinishingTransferExpenditure = 0,
                                        
                                        FinishingInTransferQty = 0,
                                        
                                        FinishingOutQty = 0,
                                        
                                        FinishingReturQty = 0,
                                        
                                        SubconOutQty = 0,
                                        
                                        QtyLoadingInTransfer = 0,
                                       
                                        BeginingBalanceSubconQty = 0,
                                        
                                        BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                        
                                        FinishingInQty = group.Sum(x => x.FinishingInQty),
                                         
                                        SubconInQty = 0,
                                        SubconInPrice = 0,
                                        Ro = key,
                                        ExpenditureGoodRetur = 0,
                                        
                                        ExportQty = 0,
                                        
                                        SampleQty = 0,
                                        
                                        OtherQty = 0,
                                        
                                        ExpenditureGoodInTransfer = 0,
                                        
                                        BeginingBalanceCuttingQty = 0,
                                        
                                        BeginingBalanceLoadingQty = 0,
                                        
                                    });
            var QuerySubconIn = (from a in (from aa in garmentFinishingInRepository.Query
                                            where aa.FinishingInDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingInDate.AddHours(7) <= dateTo
                                            select new { aa.RONo, aa.Identity, aa.FinishingInDate, aa.FinishingInType })
                                 join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                 select new
                                 {

                                     BeginingBalanceSubconQty = (a.FinishingInDate.AddHours(7) < dateFrom && a.FinishingInDate.AddHours(7) > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                     BeginingBalanceSubconPrice = (a.FinishingInDate.AddHours(7) < dateFrom && a.FinishingInDate.AddHours(7) > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                     //BeginingBalanceFinishingQty = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                     //BeginingBalanceFinishingPrice = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                     //FinishingInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                     //FinishingInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                     SubconInQty = (a.FinishingInDate.AddHours(7) >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                     SubconInPrice = (a.FinishingInDate.AddHours(7) >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                     Ro = a.RONo,

                                 }).GroupBy(x => x.Ro, (key, group) => new monitoringUnionView
                                 {
                                     ro = key,
                                     article = null,
                                     comodity = null,
                                    
                                     fare = 0,
                                     farenew = 0,
                                     basicprice = 0,
                                     qtycutting = 0,
                                     
                                     qtCuttingSubkon = 0,
                                     
                                     qtyCuttingTransfer = 0,
                                     
                                     qtyCuttingIn = 0,
                                    
                                     begining = 0,
                                     beginingcuttingPrice = 0,
                                     qtyavalsew = 0,
                                     priceavalsew = 0,
                                     qtyavalcut = 0,
                                     priceavalcut = 0,
                                     beginingloading = 0,
                                     beginingloadingPrice = 0,
                                     qtyLoadingIn = 0,
                                     priceLoadingIn = 0,
                                     qtyloading = 0,
                                     
                                     qtyLoadingAdj = 0,
                                     priceLoadingAdj = 0,
                                     beginingSewing = 0,
                                     beginingSewingPrice = 0,
                                     sewingIn = 0,
                                     sewingInPrice = 0,
                                     sewingintransfer = 0,
                                     sewingintransferPrice = 0,
                                     sewingout = 0,
                                     sewingoutPrice = 0,
                                     sewingretur = 0,
                                     sewingreturPrice = 0,
                                     wipsewing = 0,
                                     wipsewingPrice = 0,
                                     wipfinishing = 0,
                                     wipfinishingPrice = 0,
                                     sewingadj = 0,
                                     sewingadjPrice = 0,
                                     finishingin = 0,
                                     
                                     finishingintransfer = 0,
                                     
                                     finishingadj = 0,
                                     
                                     finishingout = 0,
                                     
                                     finishinigretur = 0,
                                     finishinigreturPrice = 0,
                                     beginingbalanceFinishing = 0,
                                      
                                     beginingbalancesubcon = group.Sum(s => s.BeginingBalanceSubconQty),
                                     beginingbalancesubconPrice = group.Sum(s => s.BeginingBalanceSubconPrice),
                                     subconIn = group.Sum(s => s.SubconInQty),
                                     subconInPrice = group.Sum(s => s.SubconInPrice),
                                     subconout = 0,
                                     
                                     exportQty = 0,
                                     
                                     otherqty = 0,
                                     
                                     sampleQty = 0,
                                     
                                     expendAdj = 0,
                                     expendAdjPrice = 0,
                                     expendRetur = 0,
                                     expendReturPrice = 0,
                                     //finishinginqty =group.Sum(s=>s.FinishingInQty)
                                     beginingBalanceExpenditureGood = 0,
                                     beginingBalanceExpenditureGoodPrice = 0,
                                     expenditureInTransfer = 0,
                                     expenditureInTransferPrice = 0,
                                     qtyloadingInTransfer = 0,
                                     priceloadingInTransfer = 0
                                 });
             

            var QueryFinishingOut = (from a in (from aa in garmentFinishingOutRepository.Query
                                                where aa.FinishingOutDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate.AddHours(7) <= dateTo && aa.FinishingTo == "GUDANG JADI"
                                                select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
                                     join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                     join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                     join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                     select new
                                     {

                                         BeginingBalanceFinishingQty = (a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance && d.FinishingInType != "PEMBELIAN") ? -b.Quantity : 0,
                                         BeginingBalanceFinishingPrice = (a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance && d.FinishingInType != "PEMBELIAN") ? -b.Price : 0,
                                         BeginingBalanceExpenditureGood = ((a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0) + ((a.FinishingOutDate.AddHours(7) < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0),
                                         BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Price : 0 + ((a.FinishingOutDate.AddHours(7) < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0),
                                         //BeginingBalanceSubconQty = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Quantity : 0,
                                         //BeginingBalanceSubconPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Price : 0,

                                         FinishingOutQty = (a.FinishingOutDate.AddHours(7) >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                         FinishingOutPrice = (a.FinishingOutDate.AddHours(7) >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                         //SubconOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                         //SubconOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                         Ro = a.RONo,


                                     }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                     {
                                         QtyCuttingIn = 0,
                                        
                                         QtySewingIn = 0,
                                         
                                         QtyCuttingOut = 0,
                                         
                                         QtyCuttingTransfer = 0,
                                         
                                         QtyCuttingsubkon = 0,
                                         
                                         AvalCutting = 0,
                                         
                                         AvalSewing = 0,
                                         
                                         QtyLoading = 0,
                                         
                                         QtyLoadingAdjs = 0,
                                         
                                         QtySewingOut = 0,
                                         
                                         QtySewingAdj = 0,
                                         
                                         WipSewingOut = 0,
                                         
                                         WipFinishingOut = 0,
                                         
                                         QtySewingRetur = 0,
                                         
                                         QtySewingInTransfer = 0,
                                         
                                         FinishingInQty = 0,
                                         
                                         SubconInQty = 0,
                                         SubconInPrice = 0,
                                         FinishingAdjQty = 0,
                                         
                                         FinishingTransferExpenditure = 0,
                                         
                                         FinishingInTransferQty = 0,
                                         
                                         FinishingReturQty = 0,
                                         
                                         QtyLoadingInTransfer = 0,
                                        
                                         BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                         
                                         BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                         
                                         //BeginingBalanceSubconQty = group.Sum(x => x.BeginingBalanceSubconQty),
                                         //BeginingBalanceSubconPrice = group.Sum(x => x.BeginingBalanceSubconPrice),
                                         BeginingBalanceSubconQty = 0,
                                         

                                         FinishingOutQty = group.Sum(x => x.FinishingOutQty),
                                          
                                         //SubconOutQty = group.Sum(x => x.SubconOutQty),
                                         //SubconOutPrice = group.Sum(x => x.SubconOutPrice),
                                         SubconOutQty = 0,
                                         
                                         Ro = key,
                                         ExpenditureGoodRetur = 0,
                                         
                                         ExportQty = 0,
                                         
                                         SampleQty = 0,
                                         
                                         OtherQty = 0,
                                         
                                         ExpenditureGoodInTransfer = 0,
                                         
                                         BeginingBalanceCuttingQty = 0,
                                         
                                         BeginingBalanceLoadingQty = 0,
                                         
                                     });
            var QuerySubconOut = (from a in (from aa in garmentFinishingOutRepository.Query
                                             where aa.FinishingOutDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate.AddHours(7) <= dateTo && aa.FinishingTo == "GUDANG JADI"
                                             select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
                                  join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                  join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                  join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                  select new
                                  {

                                      //BeginingBalanceFinishingQty = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? -b.Quantity : 0,
                                      //BeginingBalanceFinishingPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? -b.Price : 0,
                                      //BeginingBalanceExpenditureGood = ((a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0) + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0),
                                      //BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Price : 0 + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0),
                                      BeginingBalanceSubconQty = (a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Quantity : 0,
                                      BeginingBalanceSubconPrice = (a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Price : 0,

                                      //FinishingOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                      //FinishingOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                      SubconOutQty = (a.FinishingOutDate.AddHours(7) >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                      SubconOutPrice = (a.FinishingOutDate.AddHours(7) >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                      Ro = a.RONo,


                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringUnionView
                                  {
                                      ro = key,
                                      article = null,
                                      comodity = null,
                                     
                                      fare = 0,
                                      farenew = 0,
                                      basicprice = 0,
                                      qtycutting = 0,
                                      
                                      qtCuttingSubkon = 0,
                                      
                                      qtyCuttingTransfer = 0,
                                      
                                      qtyCuttingIn = 0,
                                     
                                      begining = 0,
                                      beginingcuttingPrice = 0,
                                      qtyavalsew = 0,
                                      priceavalsew = 0,
                                      qtyavalcut = 0,
                                      priceavalcut = 0,
                                      beginingloading = 0,
                                      beginingloadingPrice = 0,
                                      qtyLoadingIn = 0,
                                      priceLoadingIn = 0,
                                      qtyloading = 0,
                                      
                                      qtyLoadingAdj = 0,
                                      priceLoadingAdj = 0,
                                      beginingSewing = 0,
                                      beginingSewingPrice = 0,
                                      sewingIn = 0,
                                      sewingInPrice = 0,
                                      sewingintransfer = 0,
                                      sewingintransferPrice = 0,
                                      sewingout = 0,
                                      sewingoutPrice = 0,
                                      sewingretur = 0,
                                      sewingreturPrice = 0,
                                      wipsewing = 0,
                                      wipsewingPrice = 0,
                                      wipfinishing = 0,
                                      wipfinishingPrice = 0,
                                      sewingadj = 0,
                                      sewingadjPrice = 0,
                                      finishingin = 0,
                                      
                                      finishingintransfer = 0,
                                      
                                      finishingadj = 0,
                                      
                                      finishingout = 0,
                                      
                                      finishinigretur = 0,
                                      finishinigreturPrice = 0,
                                      beginingbalanceFinishing = 0,
                                      
                                      beginingbalancesubcon = group.Sum(s => s.BeginingBalanceSubconQty),
                                      beginingbalancesubconPrice = group.Sum(s => s.BeginingBalanceSubconPrice),
                                      subconIn = 0,
                                      subconInPrice = 0,
                                      subconout = group.Sum(s => s.SubconOutQty),
                                      subconoutPrice = group.Sum(s => s.SubconOutPrice),
                                      exportQty = 0,
                                      
                                      otherqty = 0,
                                      
                                      sampleQty = 0,
                                      
                                      expendAdj = 0,
                                      expendAdjPrice = 0,
                                      expendRetur = 0,
                                      expendReturPrice = 0,
                                      //finishinginqty =group.Sum(s=>s.FinishingInQty)
                                      beginingBalanceExpenditureGood = 0,
                                      beginingBalanceExpenditureGoodPrice = 0,
                                      expenditureInTransfer = 0,
                                      expenditureInTransferPrice = 0,
                                      qtyloadingInTransfer = 0,
                                      priceloadingInTransfer = 0
                                  });
             

            var QueryExpenditureGoodInTransfer = (from a in (from aa in garmentFinishingOutRepository.Query
                                                             where aa.FinishingOutDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId != aa.UnitToId && aa.FinishingOutDate.AddHours(7) <= dateTo && aa.FinishingTo == "GUDANG JADI" && aa.UnitToId == (request.unit == 0 ? aa.UnitToId : request.unit)
                                                             select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
                                                  join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                                  join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                                  join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                                  select new
                                                  {

                                                      Ro = a.RONo,
                                                      ExpenditureGoodInTransfer = (a.FinishingOutDate.AddHours(7) >= dateFrom) ? b.Quantity : 0,
                                                      ExpenditureGoodInTransferPrice = (a.FinishingOutDate.AddHours(7) >= dateFrom) ? b.Price : 0,
                                                      BeginingBalanceExpenditureGood = (a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance) ? b.Quantity : 0,
                                                      BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance) ? b.Price : 0,

                                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                                  {
                                                      QtyCuttingIn = 0,
                                                     
                                                      QtySewingIn = 0,
                                                      
                                                      QtyCuttingOut = 0,
                                                      
                                                      QtyCuttingTransfer = 0,
                                                      
                                                      QtyCuttingsubkon = 0,
                                                      
                                                      AvalCutting = 0,
                                                      
                                                      AvalSewing = 0,
                                                      
                                                      QtyLoading = 0,
                                                      
                                                      QtyLoadingAdjs = 0,
                                                      
                                                      QtySewingOut = 0,
                                                      
                                                      QtySewingAdj = 0,
                                                      
                                                      WipSewingOut = 0,
                                                      
                                                      WipFinishingOut = 0,
                                                      
                                                      QtySewingRetur = 0,
                                                      
                                                      QtySewingInTransfer = 0,
                                                      
                                                      FinishingInQty = 0,
                                                      
                                                      SubconInQty = 0,
                                                      SubconInPrice = 0,
                                                      FinishingAdjQty = 0,
                                                      
                                                      FinishingTransferExpenditure = 0,
                                                      
                                                      FinishingInTransferQty = 0,
                                                      
                                                      FinishingReturQty = 0,
                                                      
                                                      BeginingBalanceFinishingQty = 0
                                                      ,
                                                      FinishingOutQty = 0,
                                                      
                                                      SubconOutQty = 0,
                                                      
                                                      Ro = key,
                                                      ExpenditureGoodRetur = 0,
                                                      
                                                      ExportQty = 0,
                                                      
                                                      SampleQty = 0,
                                                      
                                                      OtherQty = 0,
                                                      
                                                      QtyLoadingInTransfer = 0,
                                                     
                                                      ExpenditureGoodInTransfer = group.Sum(x => x.ExpenditureGoodInTransfer),
                                                       
                                                      BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                                      
                                                      BeginingBalanceCuttingQty = 0,
                                                      
                                                      BeginingBalanceLoadingQty = 0,
                                                      
                                                  });
             

            var QueryFinishingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
                                                where aa.AdjustmentDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate.AddHours(7) <= dateTo && aa.AdjustmentType == "FINISHING"
                                                select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                     join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                     select new
                                     {
                                         BeginingBalanceFinishingQty = a.AdjustmentDate.AddHours(7) < dateFrom && a.AdjustmentDate.AddHours(7) > dateBalance ? -b.Quantity : 0,
                                         BeginingBalanceFinishingPrice = a.AdjustmentDate.AddHours(7) < dateFrom && a.AdjustmentDate.AddHours(7) > dateBalance ? -b.Price : 0,
                                         FinishingAdjQty = a.AdjustmentDate.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                         FinishingAdjPrice = a.AdjustmentDate.AddHours(7) >= dateFrom ? b.Price : 0,
                                         Ro = a.RONo
                                     }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                     {
                                         QtyCuttingIn = 0,
                                        
                                         QtySewingIn = 0,
                                         
                                         QtyCuttingOut = 0,
                                         
                                         QtyCuttingTransfer = 0,
                                         
                                         QtyCuttingsubkon = 0,
                                         
                                         AvalCutting = 0,
                                         
                                         AvalSewing = 0,
                                         
                                         QtyLoading = 0,
                                         
                                         QtyLoadingAdjs = 0,
                                         
                                         QtySewingOut = 0,
                                         
                                         QtySewingAdj = 0,
                                         
                                         WipSewingOut = 0,
                                         
                                         WipFinishingOut = 0,
                                         
                                         QtySewingRetur = 0,
                                         
                                         QtySewingInTransfer = 0,
                                         
                                         FinishingInQty = 0,
                                         
                                         SubconInQty = 0,
                                         SubconInPrice = 0,
                                         FinishingTransferExpenditure = 0,
                                         
                                         FinishingInTransferQty = 0,
                                         
                                         QtyLoadingInTransfer = 0,
                                        
                                         BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                         
                                         FinishingAdjQty = group.Sum(x => x.FinishingAdjQty),
                                       
                                         FinishingOutQty = 0,
                                         
                                         FinishingReturQty = 0,
                                         
                                         SubconOutQty = 0,
                                         
                                         Ro = key,
                                         ExpenditureGoodRetur = 0,
                                         
                                         ExportQty = 0,
                                         
                                         SampleQty = 0,
                                         
                                         OtherQty = 0,
                                         
                                         ExpenditureGoodInTransfer = 0,
                                         
                                         BeginingBalanceCuttingQty = 0,
                                         
                                         BeginingBalanceLoadingQty = 0,
                                         
                                     });
             

            var QueryFinishingRetur = (from a in (from aa in garmentFinishingOutRepository.Query
                                                  where aa.FinishingOutDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate.AddHours(7) <= dateTo && aa.FinishingTo == "SEWING"
                                                  select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo, aa.UnitId, aa.UnitToId })
                                       join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                       join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                       join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                       select new
                                       {

                                           BeginingBalanceFinishingQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance && a.UnitId == a.UnitToId) ? -b.Quantity : 0,
                                           BeginingBalanceFinishingPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate.AddHours(7) < dateFrom && a.FinishingOutDate.AddHours(7) > dateBalance && a.UnitId == a.UnitToId) ? -b.Price : 0,
                                           FinishingReturQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate.AddHours(7) >= dateFrom && a.UnitToId == a.UnitToId) ? b.Quantity : 0,
                                           FinishingReturPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate.AddHours(7) >= dateFrom && a.UnitToId == a.UnitToId) ? b.Price : 0,
                                           Ro = a.RONo,

                                       }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                       {
                                           QtyCuttingIn = 0,
                                          
                                           QtySewingIn = 0,
                                           
                                           QtyCuttingOut = 0,
                                           
                                           QtyCuttingTransfer = 0,
                                           
                                           QtyCuttingsubkon = 0,
                                           
                                           AvalCutting = 0,
                                           
                                           AvalSewing = 0,
                                           
                                           QtyLoading = 0,
                                           
                                           QtyLoadingAdjs = 0,
                                           
                                           QtySewingOut = 0,
                                           
                                           QtySewingAdj = 0,
                                           
                                           WipSewingOut = 0,
                                           
                                           WipFinishingOut = 0,
                                           
                                           QtySewingRetur = 0,
                                           
                                           QtySewingInTransfer = 0,
                                           
                                           FinishingInQty = 0,
                                           
                                           SubconInQty = 0,
                                           SubconInPrice = 0,
                                           FinishingAdjQty = 0,
                                           
                                           FinishingTransferExpenditure = 0,
                                           
                                           FinishingInTransferQty = 0,
                                           
                                           FinishingOutQty = 0,
                                           
                                           SubconOutQty = 0,
                                           QtyLoadingInTransfer = 0,
                                          
                                           
                                           BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                           
                                           FinishingReturQty = group.Sum(x => x.FinishingReturQty),
                                            
                                           Ro = key,
                                           ExpenditureGoodRetur = 0,
                                           
                                           ExportQty = 0,
                                           
                                           SampleQty = 0,
                                           
                                           OtherQty = 0,
                                           
                                           ExpenditureGoodInTransfer = 0,
                                           
                                           BeginingBalanceCuttingQty = 0,
                                           
                                           BeginingBalanceLoadingQty = 0,
                                           
                                       }); 
            var QueryExpenditureGoods = (from a in (from aa in garmentExpenditureGoodRepository.Query
                                                    where aa.ExpenditureDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.ExpenditureDate.AddHours(7) <= dateTo
                                                    select new { aa.RONo, aa.Identity, aa.ExpenditureDate, aa.ExpenditureType })
                                         join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                                         select new
                                         {

                                             BeginingBalanceExpenditureGood = a.ExpenditureDate.AddHours(7) < dateFrom && a.ExpenditureDate.AddHours(7) > dateBalance ? -b.Quantity : 0,
                                             BeginingBalanceExpenditureGoodPrice = a.ExpenditureDate.AddHours(7) < dateFrom && a.ExpenditureDate.AddHours(7) > dateBalance ? -b.Price : 0,
                                             ExportQty = (a.ExpenditureDate.AddHours(7) >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Quantity : 0,
                                             ExportPrice = (a.ExpenditureDate.AddHours(7) >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Price : 0,
                                             SampleQty = (a.ExpenditureDate.AddHours(7) >= dateFrom && (a.ExpenditureType == "LAIN-LAIN")) ? b.Quantity : 0,
                                             SamplePrice = (a.ExpenditureDate.AddHours(7) >= dateFrom & (a.ExpenditureType == "LAIN-LAIN")) ? b.Price : 0,
                                             OtherQty = (a.ExpenditureDate.AddHours(7) >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Quantity : 0,
                                             OtherPrice = (a.ExpenditureDate.AddHours(7) >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Price : 0,
                                             Ro = a.RONo,

                                         }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                         {
                                             QtyCuttingIn = 0,
                                            
                                             QtySewingIn = 0,
                                             
                                             QtyCuttingOut = 0,
                                             
                                             QtyCuttingTransfer = 0,
                                             
                                             QtyCuttingsubkon = 0,
                                             
                                             QtyLoadingInTransfer = 0,
                                            
                                             AvalCutting = 0,
                                             
                                             AvalSewing = 0,
                                             
                                             QtyLoading = 0,
                                             
                                             QtyLoadingAdjs = 0,
                                             
                                             QtySewingOut = 0,
                                             
                                             QtySewingAdj = 0,
                                             
                                             WipSewingOut = 0,
                                             
                                             WipFinishingOut = 0,
                                             
                                             QtySewingRetur = 0,
                                             
                                             QtySewingInTransfer = 0,
                                             
                                             FinishingInQty = 0,
                                             
                                             SubconInQty = 0,
                                             SubconInPrice = 0,
                                             FinishingAdjQty = 0,
                                             
                                             FinishingTransferExpenditure = 0,
                                             
                                             FinishingInTransferQty = 0,
                                             
                                             FinishingOutQty = 0,
                                             
                                             FinishingReturQty = 0,
                                             
                                             SubconOutQty = 0,
                                             
                                             BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                             
                                             ExportQty = group.Sum(x => x.ExportQty),
                                             
                                             SampleQty = group.Sum(x => x.SampleQty),
                                              
                                             OtherQty = group.Sum(x => x.OtherQty),
                                              
                                             Ro = key,
                                             ExpenditureGoodRetur = 0,
                                             
                                             ExpenditureGoodInTransfer = 0,
                                             
                                             BeginingBalanceCuttingQty = 0,
                                             
                                             BeginingBalanceLoadingQty = 0,
                                             
                                             BeginingBalanceFinishingQty = 0
                                             
                                         });
             

            var QueryExpenditureGoodsAdj = (from a in (from aa in garmentAdjustmentRepository.Query
                                                       where aa.AdjustmentDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate.AddHours(7) <= dateTo && aa.AdjustmentType == "BARANG JADI"
                                                       select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                            join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                            select new
                                            {

                                                BeginingBalanceExpenditureGood = a.AdjustmentDate.AddHours(7) < dateFrom && a.AdjustmentDate.AddHours(7) > dateBalance ? -b.Quantity : 0,
                                                BeginingBalanceExpenditureGoodPrice = a.AdjustmentDate.AddHours(7) < dateFrom && a.AdjustmentDate.AddHours(7) > dateBalance ? -b.Price : 0,
                                                ExpenditureGoodAdj = a.AdjustmentDate.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                                ExpenditureGoodAdjPrice = a.AdjustmentDate.AddHours(7) >= dateFrom ? b.Price : 0,
                                                Ro = a.RONo,

                                            }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                            {
                                                QtyCuttingIn = 0,
                                               
                                                QtySewingIn = 0,
                                                
                                                QtyCuttingOut = 0,
                                                
                                                QtyCuttingTransfer = 0,
                                                
                                                QtyCuttingsubkon = 0,
                                                
                                                AvalCutting = 0,
                                                
                                                AvalSewing = 0,
                                                
                                                QtyLoading = 0,
                                                
                                                QtyLoadingAdjs = 0,
                                                
                                                QtyLoadingInTransfer = 0,
                                               
                                                QtySewingOut = 0,
                                                
                                                QtySewingAdj = 0,
                                                
                                                WipSewingOut = 0,
                                                
                                                WipFinishingOut = 0,
                                                
                                                QtySewingRetur = 0,
                                                
                                                QtySewingInTransfer = 0,
                                                
                                                FinishingInQty = 0,
                                                
                                                SubconInQty = 0,
                                                SubconInPrice = 0,
                                                FinishingAdjQty = 0,
                                                
                                                FinishingTransferExpenditure = 0,
                                                
                                                FinishingInTransferQty = 0,
                                                
                                                FinishingOutQty = 0,
                                                
                                                FinishingReturQty = 0,
                                                
                                                SubconOutQty = 0,
                                                
                                                BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                                
                                                ExpenditureGoodAdj = group.Sum(x => x.ExpenditureGoodAdj),
                                                ExpenditureGoodAdjPrice = group.Sum(x => x.ExpenditureGoodAdjPrice),
                                                Ro = key,
                                                ExpenditureGoodRetur = 0,
                                                
                                                ExportQty = 0,
                                                
                                                SampleQty = 0,
                                                
                                                OtherQty = 0,
                                                
                                                ExpenditureGoodInTransfer = 0,
                                                
                                                BeginingBalanceCuttingQty = 0,
                                                
                                                BeginingBalanceLoadingQty = 0,
                                                
                                                BeginingBalanceFinishingQty = 0
                                                
                                            });
             
            var QueryExpenditureGoodRetur = (from a in (from aa in garmentExpenditureGoodReturnRepository.Query
                                                        where aa.ReturDate.AddHours(7) >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.ReturDate.AddHours(7) <= dateTo
                                                        select new { aa.RONo, aa.Identity, aa.ReturDate })
                                             join b in garmentExpenditureGoodReturnItemRepository.Query on a.Identity equals b.ReturId
                                             select new monitoringView
                                             {

                                                 BeginingBalanceExpenditureGood = a.ReturDate.AddHours(7) < dateFrom && a.ReturDate.AddHours(7) > dateBalance ? b.Quantity : 0,
                                                  
                                                 ExpenditureGoodRetur = a.ReturDate.AddHours(7) >= dateFrom ? b.Quantity : 0,
                                                  
                                                 Ro = a.RONo,

                                             }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                             {
                                                 QtyCuttingIn = 0,
                                                
                                                 QtySewingIn = 0,
                                                 
                                                 QtyCuttingOut = 0,
                                                 
                                                 QtyCuttingTransfer = 0,
                                                 
                                                 QtyLoadingInTransfer = 0,
                                                
                                                 QtyCuttingsubkon = 0,
                                                 
                                                 AvalCutting = 0,
                                                 
                                                 AvalSewing = 0,
                                                 
                                                 QtyLoading = 0,
                                                 
                                                 QtyLoadingAdjs = 0,
                                                 
                                                 QtySewingOut = 0,
                                                 
                                                 QtySewingAdj = 0,
                                                 
                                                 WipSewingOut = 0,
                                                 
                                                 WipFinishingOut = 0,
                                                 
                                                 QtySewingRetur = 0,
                                                 
                                                 QtySewingInTransfer = 0,
                                                 
                                                 FinishingInQty = 0,
                                                 
                                                 SubconInQty = 0,
                                                 SubconInPrice = 0,
                                                 FinishingAdjQty = 0,
                                                 
                                                 FinishingTransferExpenditure = 0,
                                                 
                                                 FinishingInTransferQty = 0,
                                                 
                                                 FinishingOutQty = 0,
                                                 
                                                 FinishingReturQty = 0,
                                                 
                                                 SubconOutQty = 0,
                                                 
                                                 BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                                 
                                                 ExpenditureGoodRetur = group.Sum(x => x.ExpenditureGoodRetur),
                                                  
                                                 Ro = key,
                                                 ExportQty = 0,
                                                 
                                                 SampleQty = 0,
                                                 
                                                 OtherQty = 0,
                                                 
                                                 ExpenditureGoodInTransfer = 0,
                                                 
                                                 BeginingBalanceCuttingQty = 0,
                                                 
                                                 BeginingBalanceLoadingQty = 0,
                                                 
                                                 BeginingBalanceFinishingQty = 0
                                                 
                                             });


            var queryNow = QueryCuttingIn
                .Union(queryBalance)
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
                .Union(QueryExpenditureGoodInTransfer)
                .Union(QueryLoadingInTransfer)
                .AsEnumerable();

            //queryNow = queryNow.Where(x => Convert.ToInt32(x.Ro.Substring(0, 2)) > 19).AsEnumerable();

            var querySumAwal = (from a in queryNow
                                    //join b in queryGroup on a.Ro equals b.Ro
                                join cutt in queryGroup on a.Ro equals cutt.Ro into res
                                from b in res.DefaultIfEmpty()
                                select new
                                {
                                    Article = b != null ? b.Article : "",
                                    Comodity = b != null ? b.Comodity : "",
                                    
                                    a.Ro,
                                    a.BeginingBalanceCuttingQty,
                                    
                                    a.QtyCuttingIn,
 
                                    a.QtyCuttingOut,
                                    
                                    a.QtyCuttingTransfer,
                                    
                                    a.QtyCuttingsubkon, 
                                    a.AvalCutting,
                                    
                                    a.AvalSewing,
                                   
                                    a.BeginingBalanceLoadingQty,
                                    
                                    a.QtyLoadingIn,
                                    
                                    a.QtyLoading,
                                    
                                    a.QtyLoadingAdjs,
                                     
                                    a.BeginingBalanceSewingQty,
                                    
                                    a.QtySewingIn,
                                    
                                    a.QtySewingOut,
                                    
                                    a.QtySewingInTransfer,
                                    
                                    a.WipSewingOut,
                                     
                                    a.WipFinishingOut, 
                                    a.QtySewingRetur,
                                   
                                    a.QtySewingAdj,
                                    
                                    a.BeginingBalanceFinishingQty,
                                    
                                    a.FinishingInQty,
                                    
                                    a.BeginingBalanceSubconQty,
                                    
                                    a.SubconInQty,
                                    a.SubconInPrice,
                                    a.SubconOutQty,
                                   
                                    a.FinishingOutQty,
                                    
                                    a.FinishingInTransferQty,
                                     
                                    a.FinishingAdjQty,
                                    
                                    a.FinishingReturQty,
                                    
                                    a.BeginingBalanceExpenditureGood, 
                                    a.ExpenditureGoodRetur, 
                                    a.ExportQty, 
                                    a.OtherQty, 
                                    a.SampleQty, 
                                    a.ExpenditureGoodAdj, 
                                    a.ExpenditureGoodInTransfer, 
                                    a.QtyLoadingInTransfer
                                })
                .GroupBy(x => new {  x.Ro, x.Article, x.Comodity }, (key, group) => new monitoringUnionView
                {
                    ro = key.Ro,
                    article = key.Article,
                    comodity = key.Comodity,
                    
                    qtycutting = group.Sum(s => s.QtyCuttingOut),
                    qtCuttingSubkon = group.Sum(s => s.QtyCuttingsubkon),
                    qtyCuttingTransfer = group.Sum(s => s.QtyCuttingTransfer),
                    qtyCuttingIn = group.Sum(s => s.QtyCuttingIn),
                    begining = group.Sum(s => s.BeginingBalanceCuttingQty),
                    qtyavalsew = group.Sum(s => s.AvalSewing),
                    qtyavalcut = group.Sum(s => s.AvalCutting),
                    beginingloading = group.Sum(s => s.BeginingBalanceLoadingQty),
                    qtyLoadingIn = group.Sum(s => s.QtyLoadingIn),
                    qtyloading = group.Sum(s => s.QtyLoading),
                    qtyLoadingAdj = group.Sum(s => s.QtyLoadingAdjs),
                    beginingSewing = group.Sum(s => s.BeginingBalanceSewingQty),
                    sewingIn = group.Sum(s => s.QtySewingIn),
                    sewingintransfer = group.Sum(s => s.QtySewingInTransfer),
                    sewingout = group.Sum(s => s.QtySewingOut),
                    sewingretur = group.Sum(s => s.QtySewingRetur),
                    wipsewing = group.Sum(s => s.WipSewingOut),
                    wipfinishing = group.Sum(s => s.WipFinishingOut), 
                    sewingadj = group.Sum(s => s.QtySewingAdj), 
                    finishingin = group.Sum(s => s.FinishingInQty),
                    finishingintransfer = group.Sum(s => s.FinishingInTransferQty),
                    finishingadj = group.Sum(s => s.FinishingAdjQty),
                    finishingout = group.Sum(s => s.FinishingOutQty),
                    finishinigretur = group.Sum(s => s.FinishingReturQty),
                    beginingbalanceFinishing = group.Sum(s => s.BeginingBalanceFinishingQty),
                    beginingbalancesubcon = group.Sum(s => s.BeginingBalanceSubconQty),
                    subconIn = group.Sum(s => s.SubconInQty),
                    subconInPrice = group.Sum(s => s.SubconInPrice),
                    subconout = group.Sum(s => s.SubconOutQty),
                    exportQty = group.Sum(s => s.ExportQty),
                    otherqty = group.Sum(s => s.OtherQty),
                    sampleQty = group.Sum(s => s.SampleQty),
                    expendAdj = group.Sum(s => s.ExpenditureGoodAdj),
                    expendRetur = group.Sum(s => s.ExpenditureGoodRetur),
                    //finishinginqty =group.Sum(s=>s.FinishingInQty)
                    beginingBalanceExpenditureGood = group.Sum(s => s.BeginingBalanceExpenditureGood),
                    expenditureInTransfer = group.Sum(s => s.ExpenditureGoodInTransfer),
                    qtyloadingInTransfer = group.Sum(s => s.QtyLoadingInTransfer)



                }).AsEnumerable();


            var unionSubcon = QuerySubconIn.Union(QuerySubconOut).AsEnumerable();

            //var BasicPrices = (from a in sumbasicPrice
            //                   join b in sumFCs on a.RO equals b.RO into sumFCes
            //                   from bb in sumFCes.DefaultIfEmpty()
            //                   join c in queryBalance on a.RO equals c.Ro into queryBalances
            //                   from cc in queryBalances.DefaultIfEmpty()
            //                   where ros.Contains(a.RO)
            //                   select new

            var QuerySubcon = (from a in unionSubcon
                               join b in queryGroup on a.ro equals b.Ro into querResu
                               from bb in querResu.DefaultIfEmpty()
                               select new
                               {
                                   Article = bb == null ? "" : bb.Article,
                                   Comodity = bb == null ? "" : bb.Comodity,
                                 
                                   a.ro,
                                   a.beginingbalancesubcon,
                                   a.beginingbalancesubconPrice,
                                   a.subconIn,
                                   a.subconInPrice,
                                   a.subconout,
                                   a.subconoutPrice,
                               })
                .GroupBy(x => new {  x.ro, x.Article, x.Comodity }, (key, group) => new monitoringUnionView
                {
                    ro = key.ro,
                    article = key.Article,
                    comodity = key.Comodity,
                     
                    qtycutting = 0,
                    
                    qtCuttingSubkon = 0,
                    
                    qtyCuttingTransfer = 0,
                    
                    qtyCuttingIn = 0,
                   
                    begining = 0,
                    beginingcuttingPrice = 0,
                    qtyavalsew = 0,
                    priceavalsew = 0,
                    qtyavalcut = 0,
                    priceavalcut = 0,
                    beginingloading = 0,
                    beginingloadingPrice = 0,
                    qtyLoadingIn = 0,
                    priceLoadingIn = 0,
                    qtyloading = 0,
                    
                    qtyLoadingAdj = 0,
                    priceLoadingAdj = 0,
                    beginingSewing = 0,
                    beginingSewingPrice = 0,
                    sewingIn = 0,
                    sewingInPrice = 0,
                    sewingintransfer = 0,
                    sewingintransferPrice = 0,
                    sewingout = 0,
                    sewingoutPrice = 0,
                    sewingretur = 0,
                    sewingreturPrice = 0,
                    wipsewing = 0,
                    wipsewingPrice = 0,
                    wipfinishing = 0,
                    wipfinishingPrice = 0,
                    sewingadj = 0,
                    sewingadjPrice = 0,
                    finishingin = 0,
                    
                    finishingintransfer = 0,
                    
                    finishingadj = 0,
                    
                    finishingout = 0,
                    
                    finishinigretur = 0, 
                    beginingbalanceFinishing = 0,
                     
                    beginingbalancesubcon = group.Sum(s => s.beginingbalancesubcon), 
                    subconIn = group.Sum(s => s.subconIn), 
                    subconout = group.Sum(s => s.subconout), 
                    exportQty = 0,
                    
                    otherqty = 0,
                    
                    sampleQty = 0,
                    
                    expendAdj = 0,
                    expendAdjPrice = 0,
                    expendRetur = 0,
                    expendReturPrice = 0,
                    //finishinginqty =group.Sum(s=>s.FinishingInQty)
                    beginingBalanceExpenditureGood = 0,
                    beginingBalanceExpenditureGoodPrice = 0,
                    expenditureInTransfer = 0,
                    expenditureInTransferPrice = 0,
                    qtyloadingInTransfer = 0,
                    priceloadingInTransfer = 0



                }).AsEnumerable();

            var queryUnion = querySumAwal.Union(QuerySubcon).AsEnumerable();

            var querySum = queryUnion.GroupBy(x => new { x.farenew, x.fare, x.basicprice, x.fc, x.ro, x.article, x.comodity }, (key, group) => new monitoringUnionView
            {
                ro = key.ro,
                article = key.article,
                comodity = key.comodity,
                
                qtycutting = group.Sum(s => s.qtycutting),
                qtCuttingSubkon = group.Sum(s => s.qtCuttingSubkon),
                qtyCuttingTransfer = group.Sum(s => s.qtyCuttingTransfer),
                priceCuttingTransfer = group.Sum(s => s.priceCuttingTransfer),
                qtyCuttingIn = group.Sum(s => s.qtyCuttingIn),
                begining = group.Sum(s => s.begining),
                qtyavalsew = group.Sum(s => s.qtyavalsew),
                qtyavalcut = group.Sum(s => s.qtyavalcut),
                beginingloading = group.Sum(s => s.beginingloading),
                qtyLoadingIn = group.Sum(s => s.qtyLoadingIn),
                qtyloading = group.Sum(s => s.qtyloading),
                qtyLoadingAdj = group.Sum(s => s.qtyLoadingAdj),
                beginingSewing = group.Sum(s => s.beginingSewing),
                sewingIn = group.Sum(s => s.sewingIn),
                sewingintransfer = group.Sum(s => s.sewingintransfer),
                sewingout = group.Sum(s => s.sewingout),
                sewingretur = group.Sum(s => s.sewingretur),
                wipsewing = group.Sum(s => s.wipsewing),
                wipfinishing = group.Sum(s => s.wipfinishing),
                sewingadj = group.Sum(s => s.sewingadj),
                finishingin = group.Sum(s => s.finishingin),
                finishingadj = group.Sum(s => s.finishingadj),
                finishingout = group.Sum(s => s.finishingout),
                finishinigretur = group.Sum(s => s.finishinigretur),
                beginingbalanceFinishing = group.Sum(s => s.beginingbalanceFinishing),
                beginingbalancesubcon = group.Sum(s => s.beginingbalancesubcon),
                subconIn = group.Sum(s => s.subconIn),
                subconout = group.Sum(s => s.subconout),
                exportQty = group.Sum(s => s.exportQty),
                otherqty = group.Sum(s => s.otherqty),
                sampleQty = group.Sum(s => s.sampleQty),
                expendAdj = group.Sum(s => s.expendAdj),
                expendRetur = group.Sum(s => s.expendRetur),
                //finishinginqty =group.Sum(s=>s.FinishingInQty)
                beginingBalanceExpenditureGood = group.Sum(s => s.beginingBalanceExpenditureGood),
                expenditureInTransfer = group.Sum(s => s.expenditureInTransfer),
                qtyloadingInTransfer = group.Sum(s => s.qtyloadingInTransfer)



            }).ToList();

            var getComoditiExpe = (from a in garmentExpenditureGoodRepository.Query
                                   where a.ExpenditureDate >= dateBalance
                                   select new { a.ComodityName, a.Article, a.RONo }).Distinct();

            foreach (var a in querySum)
            {
                if (string.IsNullOrWhiteSpace(a.comodity))
                {
                    var getComodity = getComoditiExpe.Where(x => x.RONo == a.ro).FirstOrDefault();

                    a.comodity = getComodity != null ? getComodity.ComodityName : "";
                    a.article = getComodity != null ? getComodity.Article : "";
                }
            }


            GarmentMonitoringProductionStockFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringProductionStockFlowListViewModel();
            List<GarmentMonitoringProductionStockFlowDto> monitoringDtos = new List<GarmentMonitoringProductionStockFlowDto>();

            var ros = querySum.Select(x => x.ro).Distinct().ToArray();

           

            var dtos = (from a in querySum
                        
                        select new GarmentMonitoringProductionStockFlowDto
                        {
                            Article = a.article,
                            Ro = a.ro,
                            

                            BeginingBalanceCuttingQty = a.begining < 0 ? 0 : a.begining,
                            QtyCuttingTransfer = Math.Round(a.qtyCuttingTransfer, 2),
                            QtyCuttingsubkon = Math.Round(a.qtCuttingSubkon, 2),
                            QtyCuttingIn = Math.Round(a.qtyCuttingIn, 2),
                            QtyCuttingOut = Math.Round(a.qtycutting, 2),
                            Comodity = a.comodity,
                            AvalCutting = Math.Round(a.qtyavalcut, 2),
                            AvalSewing = Math.Round(a.qtyavalsew, 2),
                            EndBalancCuttingeQty = Math.Round(a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew, 2) < 0 ? 0 : Math.Round(a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew, 2),
                            BeginingBalanceLoadingQty = Math.Round(a.beginingloading, 2) < 0 ? 0 : Math.Round(a.beginingloading, 2),
                            QtyLoadingIn = Math.Round(a.qtyLoadingIn, 2),
                            QtyLoadingInTransfer = Math.Round(a.qtyloadingInTransfer, 2),
                            QtyLoading = Math.Round(a.qtyloading, 2),
                            QtyLoadingAdjs = Math.Round(a.qtyLoadingAdj, 2),
                            EndBalanceLoadingQty = (Math.Round(a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj, 2)) < 0 ? 0 : (Math.Round(a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj, 2)),
                            BeginingBalanceSewingQty = Math.Round(a.beginingSewing, 2),
                            QtySewingIn = Math.Round(a.sewingIn, 2),
                            QtySewingOut = Math.Round(a.sewingout, 2),
                            QtySewingInTransfer = Math.Round(a.sewingintransfer, 2),
                            QtySewingRetur = Math.Round(a.sewingretur, 2),
                            WipSewingOut = Math.Round(a.wipsewing, 2),
                            WipFinishingOut = Math.Round(a.wipfinishing, 2),
                            QtySewingAdj = Math.Round(a.sewingadj, 2),
                            EndBalanceSewingQty = Math.Round(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.wipsewing - a.wipfinishing - a.sewingretur - a.sewingadj, 2),
                            BeginingBalanceFinishingQty = Math.Round(a.beginingbalanceFinishing, 2),
                            FinishingInExpenditure = Math.Round(a.finishingout + a.subconout, 2),
                            FinishingInQty = Math.Round(a.finishingin, 2),
                            FinishingOutQty = Math.Round(a.finishingout, 2),
                            BeginingBalanceSubconQty = Math.Round(a.beginingbalancesubcon, 2),
                            SubconInQty = Math.Round(a.subconIn, 2),
                            SubconOutQty = Math.Round(a.subconout, 2),
                            EndBalanceSubconQty = Math.Round(a.beginingbalancesubcon + a.subconIn - a.subconout, 2),
                            FinishingInTransferQty = Math.Round(a.finishingintransfer, 2),
                            FinishingReturQty = Math.Round(a.finishinigretur, 2),
                            FinishingAdjQty = Math.Round(a.finishingadj, 2),
                            BeginingBalanceExpenditureGood = Math.Round(a.beginingBalanceExpenditureGood, 2),
                            EndBalanceFinishingQty = Math.Round(a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur, 2),
                            ExportQty = Math.Round(a.exportQty, 2),
                            SampleQty = Math.Round(a.sampleQty, 2),
                            OtherQty = Math.Round(a.otherqty, 2),
                            ExpenditureGoodAdj = Math.Round(a.expendAdj, 2),
                            ExpenditureGoodRetur = Math.Round(a.expendRetur, 2),
                            ExpenditureGoodInTransfer = Math.Round(a.expenditureInTransfer, 2),
                            EndBalanceExpenditureGood = Math.Round(a.beginingBalanceExpenditureGood + a.finishingout + a.subconout + a.expendRetur + a.expenditureInTransfer - a.exportQty - a.otherqty - a.sampleQty - a.expendAdj, 2),
                            FareNew = a.farenew,
                            CuttingNew = Math.Round(a.farenew * Convert.ToDecimal(a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew), 2),
                            LoadingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingloading + a.qtyLoadingIn - a.qtyloading - a.qtyLoadingAdj), 2),
                            SewingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.wipsewing - a.wipfinishing - a.sewingretur - a.sewingadj), 2),
                            FinishingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur), 2),
                            ExpenditureNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingBalanceExpenditureGood + a.finishingout + a.subconout + a.expendRetur + a.expenditureInTransfer - a.exportQty - a.otherqty - a.sampleQty - a.expendAdj), 2),
                            SubconNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingbalancesubcon + a.subconIn - a.subconout), 2)
                        }).ToList();

            var data = from a in dtos
                       where a.BeginingBalanceCuttingQty > 0 || a.QtyCuttingIn > 0 || a.QtyCuttingOut > 0 || a.QtyCuttingsubkon > 0 || a.QtyCuttingTransfer > 0 || a.EndBalancCuttingeQty > 0 ||
                        a.BeginingBalanceLoadingQty > 0 || a.QtyLoading > 0 || a.QtyLoadingAdjs > 0 || a.QtyLoadingIn > 0 || a.QtyLoadingInTransfer > 0 || a.EndBalanceLoadingQty > 0 ||
                        a.BeginingBalanceSewingQty > 0 || a.QtySewingAdj > 0 || a.QtySewingIn > 0 || a.QtySewingInTransfer > 0 || a.QtySewingOut > 0 || a.QtySewingRetur > 0 || a.WipSewingOut > 0 || a.WipFinishingOut > 0 || a.EndBalanceSewingQty > 0 ||
                        a.BeginingBalanceSubconQty > 0 || a.EndBalanceSubconQty > 0 || a.SubconInQty > 0 || a.SubconOutQty > 0 || a.AvalCutting > 0 || a.AvalSewing > 0 ||
                        a.BeginingBalanceFinishingQty > 0 || a.FinishingAdjQty > 0 || a.FinishingInExpenditure > 0 || a.FinishingInQty > 0 || a.FinishingInTransferQty > 0 || a.FinishingOutQty > 0 || a.FinishingReturQty > 0 ||
                        a.BeginingBalanceExpenditureGood > 0 || a.ExpenditureGoodAdj > 0 || a.ExpenditureGoodInTransfer > 0 || a.ExpenditureGoodRemainingQty > 0 || a.ExpenditureGoodRetur > 0 || a.EndBalanceExpenditureGood > 0
                       select a;

            //var data2 = data.Count();

            var roList = (from a in data
                          select a.Ro).Distinct().ToList();
            var roBalance = from a in garmentBalanceProductionStockRepository.Query
                            select new CostCalViewModel { comodityName = a.Comodity, buyerCode = a.BuyerCode, hours = a.Hours, qtyOrder = a.QtyOrder, ro = a.Ro };

            CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(roList, request.token);

            foreach (var item in roBalance)
            {
                costCalculation.data.Add(item);
            }

            var costcalgroup = costCalculation.data.GroupBy(x => new { x.ro, }, (key, group) => new CostCalViewModel
            {
                buyerCode = group.FirstOrDefault().buyerCode,
                comodityName = group.FirstOrDefault().comodityName,
                hours = group.FirstOrDefault().hours,
                qtyOrder = group.FirstOrDefault().qtyOrder,
                ro = key.ro
            }).ToList();

            //var costcal2 = costCalculation.data.Distinct().Count();

            //var aaaa = data.Where(x => x.Ro == "2250850");

            var dataend = (from item in data
                           join b in costcalgroup on item.Ro equals b.ro
                           select new GarmentMonitoringProductionStockFlowDto
                           {
                               Article = item.Article,
                               Ro = item.Ro,
                               
                               BuyerCode = item.BuyerCode == null ? b.buyerCode : item.BuyerCode,
                               Comodity = item.Comodity == null ? b.comodityName : item.Comodity,
                               QtyOrder = b.qtyOrder,
                              
                               BeginingBalanceCuttingQty = item.BeginingBalanceCuttingQty,
                               QtyCuttingTransfer = item.QtyCuttingTransfer,
                               QtyCuttingsubkon = item.QtyCuttingsubkon,
                               QtyCuttingIn = item.QtyCuttingIn,
                               QtyCuttingOut = item.QtyCuttingOut,
                               AvalCutting = item.AvalCutting,
                               AvalSewing = item.AvalSewing,
                               EndBalancCuttingeQty = item.EndBalancCuttingeQty,
                               BeginingBalanceLoadingQty = item.BeginingBalanceLoadingQty,
                               QtyLoadingIn = item.QtyLoadingIn,
                               QtyLoadingInTransfer = item.QtyLoadingInTransfer,
                               QtyLoading = item.QtyLoading,
                               QtyLoadingAdjs = item.QtyLoadingAdjs,
                               EndBalanceLoadingQty = item.EndBalanceLoadingQty,
                               BeginingBalanceSewingQty = item.BeginingBalanceSewingQty,
                               QtySewingIn = item.QtySewingIn,
                               QtySewingOut = item.QtySewingOut,
                               QtySewingInTransfer = item.QtySewingInTransfer,
                               QtySewingRetur = item.QtySewingRetur,
                               WipSewingOut = item.WipSewingOut,
                               WipFinishingOut = item.WipFinishingOut,
                               WipFinishingOutPrice = item.WipFinishingOutPrice,
                               QtySewingAdj = item.QtySewingAdj,
                               EndBalanceSewingQty = item.EndBalanceSewingQty,
                               BeginingBalanceFinishingQty = item.BeginingBalanceFinishingQty,
                               FinishingInExpenditure = item.FinishingInExpenditure,
                               FinishingInQty = item.FinishingInQty,
                               FinishingOutQty = item.FinishingOutQty,
                               BeginingBalanceSubconQty = item.BeginingBalanceSubconQty,
                               SubconInQty = item.SubconInQty,
                               SubconOutQty = item.SubconOutQty,
                               EndBalanceSubconQty = item.EndBalanceSubconQty,
                               FinishingInTransferQty = item.FinishingInTransferQty,
                               FinishingReturQty = item.FinishingReturQty,
                               FinishingAdjQty = item.FinishingAdjQty,
                               BeginingBalanceExpenditureGood = item.BeginingBalanceExpenditureGood,
                               EndBalanceFinishingQty = item.EndBalanceFinishingQty,
                               ExportQty = item.ExportQty,
                               SampleQty = item.SampleQty,
                               OtherQty = item.OtherQty,
                               ExpenditureGoodAdj = item.ExpenditureGoodAdj,
                               ExpenditureGoodRetur = item.ExpenditureGoodRetur,
                               ExpenditureGoodInTransfer = item.ExpenditureGoodInTransfer,
                               EndBalanceExpenditureGood = item.EndBalanceExpenditureGood,
                               FareNew = item.FareNew,
                               CuttingNew = item.CuttingNew,
                               LoadingNew = item.LoadingNew,
                               SewingNew = item.SewingNew,
                               FinishingNew = item.FinishingNew,
                               ExpenditureNew = item.ExpenditureNew,
                               SubconNew = item.SubconNew,
                               ExpenditureGoodRemainingQty = item.ExpenditureGoodRemainingQty,
                               FinishingTransferExpenditure = item.FinishingTransferExpenditure,
                               MaterialUsage = item.FinishingInExpenditure * item.BasicPrice,
                               PriceUsage = item.FinishingInExpenditure * Convert.ToDouble(item.Fare),
                               SubconSewingInQty = 0,
                               SubconSewingOutQty = 0,
                               SubconExpenditureGoodInQty = 0,
                               SubconExpenditureGoodQty = 0,
                           }).OrderBy(x => x.Ro).ToList();


            garmentMonitoringProductionFlow.garmentMonitorings = dataend.ToList();
            garmentMonitoringProductionFlow.count = dataend.Count();
          

            //}
            monitoringDtos = dataend.ToList();
            GarmentMonitoringProductionStockFlowDto total = new GarmentMonitoringProductionStockFlowDto()
            {
               
                BeginingBalanceCuttingQty = dataend.Sum(x => x.BeginingBalanceCuttingQty),
                QtyCuttingTransfer = dataend.Sum(x => x.QtyCuttingTransfer),
                QtyCuttingsubkon = dataend.Sum(x => x.QtyCuttingsubkon),
                QtyCuttingIn = dataend.Sum(x => x.QtyCuttingIn),
                QtyCuttingOut = dataend.Sum(x => x.QtyCuttingOut),
                AvalCutting = dataend.Sum(x => x.AvalCutting),
                AvalSewing = dataend.Sum(x => x.AvalSewing),
                EndBalancCuttingeQty = dataend.Sum(x => x.EndBalancCuttingeQty),
                BeginingBalanceLoadingQty = dataend.Sum(x => x.BeginingBalanceLoadingQty),
                QtyLoadingIn = dataend.Sum(x => x.QtyLoadingIn),
                QtyLoadingInTransfer = dataend.Sum(x => x.QtyLoadingInTransfer),
                QtyLoading = dataend.Sum(x => x.QtyLoading),
                QtyLoadingAdjs = dataend.Sum(x => x.QtyLoadingAdjs),
                EndBalanceLoadingQty = dataend.Sum(x => x.EndBalanceLoadingQty),
                BeginingBalanceSewingQty = dataend.Sum(x => x.BeginingBalanceSewingQty),
                QtySewingIn = dataend.Sum(x => x.QtySewingIn),
                QtySewingOut = dataend.Sum(x => x.QtySewingOut),
                QtySewingInTransfer = dataend.Sum(x => x.QtySewingInTransfer),
                QtySewingRetur = dataend.Sum(x => x.QtySewingRetur),
                WipSewingOut = dataend.Sum(x => x.WipSewingOut),
                WipFinishingOut = dataend.Sum(x => x.WipFinishingOut),
                QtySewingAdj = dataend.Sum(x => x.QtySewingAdj),
                EndBalanceSewingQty = dataend.Sum(x => x.EndBalanceSewingQty),
                BeginingBalanceFinishingQty = dataend.Sum(x => x.BeginingBalanceFinishingQty),
                FinishingInExpenditure = dataend.Sum(x => x.FinishingInExpenditure),
                FinishingInQty = dataend.Sum(x => x.FinishingInQty),
                FinishingOutQty = dataend.Sum(x => x.FinishingOutQty),
                BeginingBalanceSubconQty = dataend.Sum(x => x.BeginingBalanceSubconQty),
                SubconInQty = dataend.Sum(x => x.SubconInQty),
                SubconOutQty = dataend.Sum(x => x.SubconOutQty),
                EndBalanceSubconQty = dataend.Sum(x => x.EndBalanceSubconQty),
                FinishingInTransferQty = dataend.Sum(x => x.FinishingInTransferQty),
                FinishingReturQty = dataend.Sum(x => x.FinishingReturQty),
                FinishingAdjQty = dataend.Sum(x => x.FinishingAdjQty),
                BeginingBalanceExpenditureGood = dataend.Sum(x => x.BeginingBalanceExpenditureGood),
                EndBalanceFinishingQty = dataend.Sum(x => x.EndBalanceFinishingQty),
                ExportQty = dataend.Sum(x => x.ExportQty),
                SampleQty = dataend.Sum(x => x.SampleQty),
                OtherQty = dataend.Sum(x => x.OtherQty),
                ExpenditureGoodAdj = dataend.Sum(x => x.ExpenditureGoodAdj),
                ExpenditureGoodRetur = dataend.Sum(x => x.ExpenditureGoodRetur),
                ExpenditureGoodInTransfer = dataend.Sum(x => x.ExpenditureGoodInTransfer),
                EndBalanceExpenditureGood = dataend.Sum(x => x.EndBalanceExpenditureGood),

                CuttingNew = dataend.Sum(x => x.CuttingNew),
                LoadingNew = dataend.Sum(x => x.LoadingNew),
                SewingNew = dataend.Sum(x => x.SewingNew),
                FinishingNew = dataend.Sum(x => x.FinishingNew),
                ExpenditureNew = dataend.Sum(x => x.ExpenditureNew),
                SubconNew = dataend.Sum(x => x.SubconNew)
            };
            monitoringDtos.Add(total);

            garmentMonitoringProductionFlow.garmentMonitorings = monitoringDtos;
            var reportDataTable = new DataTable();

            if (request.type != "bookkeeping")
            {

                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING2", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING3", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI2", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI3", DataType = typeof(string) });

                reportDataTable.Rows.Add("","", "", "", "",
                "Saldo Awal WIP Cutting",  "Saldo Akhir WIP Cutting","Selisih",
                "Saldo Awal Loading", "Saldo Akhir Loading","Selisih",
                "Saldo Awal WIP Sewing","Saldo Akhir WIP Sewing","Selisih",
                "Saldo Awal WIP Finishing", "Saldo Akhir WIP Finishing","Selisih",
                "Saldo Awal Barang jadi","Saldo Akhir Barang Jadi","Selisih"
                );
            }
            
            int counter = 6;

            foreach (var report in garmentMonitoringProductionFlow.garmentMonitorings)
            {
                if (request.type != "bookkeeping")
                {
                    reportDataTable.Rows.Add(unitName, report.Ro, report.Article, report.Comodity, report.QtyOrder,
                    report.BeginingBalanceCuttingQty,  report.EndBalancCuttingeQty, report.BeginingBalanceCuttingQty - report.EndBalancCuttingeQty,
                    report.BeginingBalanceLoadingQty, report.EndBalanceLoadingQty, report.BeginingBalanceLoadingQty- report.EndBalanceLoadingQty,
                    report.BeginingBalanceSewingQty,  report.EndBalanceSewingQty, report.BeginingBalanceSewingQty- report.EndBalanceSewingQty,
                    report.BeginingBalanceFinishingQty, report.EndBalanceFinishingQty, report.BeginingBalanceFinishingQty- report.EndBalanceFinishingQty,
                    report.BeginingBalanceExpenditureGood,  report.EndBalanceExpenditureGood, report.BeginingBalanceExpenditureGood- report.EndBalanceExpenditureGood);
                    counter++;
                }
                
            }
            var _unitName = (from a in garmentFinishingOutRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                if (request.type != "bookkeeping")
                {
                    worksheet.Cells["A1"].Value = "Report Produksi"; worksheet.Cells["A" + 1 + ":AU" + 1 + ""].Merge = true;
                    worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                    worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
                    worksheet.Cells["F" + 5 + ":H" + 5 + ""].Merge = true;
                    worksheet.Cells["I" + 5 + ":K" + 5 + ""].Merge = true;
                    worksheet.Cells["L" + 5 + ":N" + 5 + ""].Merge = true;
                    worksheet.Cells["O" + 5 + ":Q" + 5 + ""].Merge = true;
                    worksheet.Cells["R" + 5 + ":T" + 5 + ""].Merge = true;
                  
                    worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);

                    worksheet.Cells["E" + 5 + ":T" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A" + 5 + ":T" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + 5 + ":T" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + 5 + ":T" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + 5 + ":T" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + counter + ":S" + counter + ""].Style.Font.Bold = true;
                    foreach (var cell in worksheet.Cells["E" +  7+ ":T" + (counter + 1) + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                }
              
                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;
            }


        }


    }
}