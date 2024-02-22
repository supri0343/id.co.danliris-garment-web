using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Linq;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Http;
using System.Text;
using Manufactures.Domain.MonitoringProductionStockFlow;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;

namespace Manufactures.Application.GarmentMonitoringProductionSubconStockFlows.Queries
{
	public class GetXlsMonitoringProductionStockFlowQueryHandler : IQueryHandler<GetXlsMonitoringProductionStockFlowQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentSubconCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentSubconCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentSubconCuttingOutDetailRepository garmentCuttingOutDetailRepository;

		private readonly IGarmentSubconCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentSubconCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentSubconCuttingInDetailRepository garmentCuttingInDetailRepository;

		private readonly IGarmentSubconLoadingInRepository garmentLoadingInRepository;
		private readonly IGarmentSubconLoadingInItemRepository garmentLoadingInItemRepository;

        private readonly IGarmentSubconLoadingOutRepository garmentLoadingOutRepository;
        private readonly IGarmentSubconLoadingOutItemRepository garmentLoadingOutItemRepository;

        private readonly IGarmentSubconSewingInRepository garmentSewingInRepository;
		private readonly IGarmentSubconSewingInItemRepository garmentSewingInItemRepository;

		//private readonly IGarmentAvalComponentRepository garmentAvalComponentRepository;
		//private readonly IGarmentAvalComponentItemRepository garmentAvalComponentItemRepository;
		//private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
		//private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;

		private readonly IGarmentSubconSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSubconSewingOutItemRepository garmentSewingOutItemRepository;

		private readonly IGarmentSubconFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentSubconFinishingOutItemRepository garmentFinishingOutItemRepository;

		private readonly IGarmentSubconFinishingInRepository garmentFinishingInRepository;
		private readonly IGarmentSubconFinishingInItemRepository garmentFinishingInItemRepository;

		private readonly IGarmentSubconPackingInRepository garmentSubconPackingInRepository;
		private readonly IGarmentSubconPackingInItemRepository garmentSubconPackingInItemRepository;

        private readonly IGarmentSubconPackingOutRepository garmentSubconPackingOutRepository;
        private readonly IGarmentSubconPackingOutItemRepository garmentSubconPackingOutItemRepository;
        //private readonly IGarmentExpenditureGoodReturnRepository garmentExpenditureGoodReturnRepository;
        //private readonly IGarmentExpenditureGoodReturnItemRepository garmentExpenditureGoodReturnItemRepository;
        //private readonly IGarmentSewingDORepository garmentSewingDORepository;
        //private readonly IGarmentSewingDOItemRepository garmentSewingDOItemRepository;
        private readonly IGarmentComodityPriceRepository garmentComodityPriceRepository;
		private readonly IGarmentSubconPreparingRepository garmentPreparingRepository;
        private readonly IGarmentSubconPreparingItemRepository garmentPreparingItemRepository;
        //private readonly IGarmentBalanceMonitoringProductionStockFlowRepository garmentBalanceProductionStockRepository;

        public GetXlsMonitoringProductionStockFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
            //garmentBalanceProductionStockRepository = storage.GetRepository<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();

			garmentCuttingInRepository = storage.GetRepository<IGarmentSubconCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentSubconCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSubconCuttingInDetailRepository>();

			garmentLoadingInRepository = storage.GetRepository<IGarmentSubconLoadingInRepository>();
			garmentLoadingInItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();

            garmentLoadingOutRepository = storage.GetRepository<IGarmentSubconLoadingOutRepository>();
            garmentLoadingOutItemRepository = storage.GetRepository<IGarmentSubconLoadingOutItemRepository>();

            garmentSewingInRepository = storage.GetRepository<IGarmentSubconSewingInRepository>();
			garmentSewingInItemRepository = storage.GetRepository<IGarmentSubconSewingInItemRepository>();

            garmentSewingOutRepository = storage.GetRepository<IGarmentSubconSewingOutRepository>();
            garmentSewingOutItemRepository = storage.GetRepository<IGarmentSubconSewingOutItemRepository>();

            garmentFinishingInRepository = storage.GetRepository<IGarmentSubconFinishingInRepository>();
            garmentFinishingInItemRepository = storage.GetRepository<IGarmentSubconFinishingInItemRepository>();

            garmentFinishingOutRepository = storage.GetRepository<IGarmentSubconFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSubconFinishingOutItemRepository>();


            garmentSubconPackingInRepository = storage.GetRepository<IGarmentSubconPackingInRepository>();
            garmentSubconPackingInItemRepository = storage.GetRepository<IGarmentSubconPackingInItemRepository>();

            garmentSubconPackingOutRepository = storage.GetRepository<IGarmentSubconPackingOutRepository>();
            garmentSubconPackingOutItemRepository = storage.GetRepository<IGarmentSubconPackingOutItemRepository>();

            garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentSubconPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentSubconPreparingItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}


		class monitoringView
		{
			public string Ro { get; internal set; }
			public string BuyerCode { get; internal set; }
			public string Article { get; internal set; }
			public string Comodity { get; internal set; }
			public double QtyOrder { get; internal set; }
			public double BasicPrice { get; internal set; }
			public decimal Fare { get; internal set; }
			public double FC { get; internal set; }
			public double Hours { get; internal set; }
			public double BeginingBalanceCuttingQty { get; internal set; }
			public double BeginingBalanceCuttingPrice { get; internal set; }
			public double QtyCuttingIn { get; internal set; }
			public double PriceCuttingIn { get; internal set; }
			public double QtyCuttingOut { get; internal set; }
			public double PriceCuttingOut { get; internal set; }
			public double QtyCuttingToPacking { get; internal set; }
			public double PriceCuttingToPacking { get; internal set; }
			public double QtyCuttingsubkon { get; internal set; }
			public double PriceCuttingsubkon { get; internal set; }
			public double AvalCutting { get; internal set; }
			public double AvalCuttingPrice { get; internal set; }
			public double AvalSewing { get; internal set; }
			public double AvalSewingPrice { get; internal set; }
			public double EndBalancCuttingeQty { get; internal set; }
			public double EndBalancCuttingePrice { get; internal set; }
			public double BeginingBalanceLoadingQty { get; internal set; }
			public double BeginingBalanceLoadingPrice { get; internal set; }
			public double QtyLoadingIn { get; internal set; }
			public double PriceLoadingIn { get; internal set; }
			public double QtyLoading { get; internal set; }
			public double PriceLoading { get; internal set; }
			public double QtyLoadingInTransfer { get; internal set; }
			public double PriceLoadingInTransfer { get; internal set; }
			public double QtyLoadingAdjs { get; internal set; }
			public double PriceLoadingAdjs { get; internal set; }
			public double EndBalanceLoadingQty { get; internal set; }
			public double EndBalanceLoadingPrice { get; internal set; }
			public double BeginingBalanceSewingQty { get; internal set; }
			public double BeginingBalanceSewingPrice { get; internal set; }
			public double QtySewingIn { get; internal set; }
			public double PriceSewingIn { get; internal set; }
			public double QtySewingOut { get; internal set; }
			public double PriceSewingOut { get; internal set; }
			public double QtySewingInTransfer { get; internal set; }
			public double PriceSewingInTransfer { get; internal set; }
			public double WipSewingToPacking { get; internal set; }
			public double WipSewingToPackingPrice { get; internal set; }
			public double WipFinishingOut { get; internal set; }
			public double WipFinishingOutPrice { get; internal set; }
			public double QtySewingRetur { get; internal set; }
			public double PriceSewingRetur { get; internal set; }
			public double QtySewingAdj { get; internal set; }
			public double PriceSewingAdj { get; internal set; }
			public double EndBalanceSewingQty { get; internal set; }
			public double EndBalanceSewingPrice { get; internal set; }
			public double BeginingBalanceFinishingQty { get; internal set; }
			public double BeginingBalanceFinishingPrice { get; internal set; }
			public double FinishingInQty { get; internal set; }
			public double FinishingInPrice { get; internal set; }
			public double BeginingBalanceSubconQty { get; internal set; }
			public double BeginingBalanceSubconPrice { get; internal set; }
			public double SubconInQty { get; internal set; }
			public double SubconInPrice { get; internal set; }
			public double SubconOutQty { get; internal set; }
			public double SubconOutPrice { get; internal set; }
			public double EndBalanceSubconQty { get; internal set; }
			public double EndBalanceSubconPrice { get; internal set; }
			public double FinishingOutQty { get; internal set; }
			public double FinishingOutPrice { get; internal set; }
            public double FinishingOutAvalQty { get; internal set; }
            public double FinishingOutAvalPrice { get; internal set; }
            public double FinishingInTransferQty { get; internal set; }
			public double FinishingInTransferPrice { get; internal set; }
			public double FinishingAdjQty { get; internal set; }
			public double FinishingAdjPrice { get; internal set; }
			public double FinishingReturQty { get; internal set; }
			public double FinishingReturPrice { get; internal set; }
			public double EndBalanceFinishingQty { get; internal set; }
			public double EndBalanceFinishingPrice { get; internal set; }
			public double BeginingBalanceExpenditureGood { get; internal set; }
			public double BeginingBalanceExpenditureGoodPrice { get; internal set; }
			public double FinishingTransferExpenditure { get; internal set; }
			public double FinishingTransferExpenditurePrice { get; internal set; }
			public double ExpenditureGoodRetur { get; internal set; }
			public double ExpenditureGoodReturPrice { get; internal set; }
			public double PackingInQty { get; internal set; }
			public double PackingInPrice { get; internal set; }
            public double PackingOutQty { get; internal set; }
            public double PackingOutPrice { get; internal set; }
            public double OtherQty { get; internal set; }
			public double OtherPrice { get; internal set; }
			public double SampleQty { get; internal set; }
			public double SamplePrice { get; internal set; }
			public double ExpenditureGoodRemainingQty { get; internal set; }
			public double ExpenditureGoodRemainingPrice { get; internal set; }
			public double ExpenditureGoodAdj { get; internal set; }
			public double ExpenditureGoodAdjPrice { get; internal set; }
			public double EndBalanceExpenditureGood { get; internal set; }
			public double EndBalanceExpenditureGoodPrice { get; internal set; }
			public double ExpenditureGoodInTransfer { get; internal set; }
			public double ExpenditureGoodInTransferPrice { get; internal set; }

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
            public double QtyCuttingToPacking { get; internal set; }
            public double PriceCuttingToPacking { get; internal set; }
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
            public double WipSewingToPacking { get; internal set; }
            public double WipSewingToPackingPrice { get; internal set; }
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
            public double finishingoutaval { get; internal set; }
            public double finishingoutavalPrice { get; internal set; }
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
            public double packingInQty { get; internal set; }
            public double packingInPrice { get; internal set; }
            public double packingOutQty { get; internal set; }
            public double packingOutPrice { get; internal set; }
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
		public async Task<MemoryStream> Handle(GetXlsMonitoringProductionStockFlowQuery request, CancellationToken cancellationToken)
		{
            //DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
            //DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            //DateTimeOffset dateBalance = (from a in garmentBalanceProductionStockRepository.Query.OrderByDescending(s => s.CreatedDate)
            //                              select a.CreatedDate).FirstOrDefault();

            var dateBalance = DateTimeOffset.MinValue;
            DateTimeOffset dateFareNew = dateTo.AddDays(1);


            var sumbasicPrice = (from a in (from prep in garmentPreparingRepository.Query
                                            where (request.ro == null || (request.ro != null && request.ro != "" && prep.RONo == request.ro))
                                            select new { prep.RONo,prep.Identity})
                                            join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSubconPreparingId
                                 
                                 select new { a.RONo, b.BasicPrice })
                         .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                         {
                             RO = key.RONo,
                             BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
                             Count = group.Count()
                         });
            var sumFCs = (from a in garmentCuttingInRepository.Query
                          where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.CuttingType == "Main Fabric" &&
                          a.CuttingInDate.AddHours(7).Date <= dateTo
                          join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new { a.FC, a.RONo, FCs= Convert.ToDouble( c.CuttingInQuantity  * a.FC),c.CuttingInQuantity}) 
                         .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                         {
                             RO = key.RONo,
                             FC = group.Sum(s => (s.FCs)),
                             Count = group.Sum(s =>  s.CuttingInQuantity)
                         });

            var queryGroup = (from a in (from aa in garmentCuttingOutRepository.Query
                                         where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro))
                                         select new { aa.RONo, aa.ComodityId, aa.ComodityName, aa.Article })
                              select new { BasicPrice = (from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault(),
                                  FareNew = (from aa in garmentComodityPriceRepository.Query where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && a.ComodityId == aa.ComodityId && aa.Date == dateFareNew select aa.Price).FirstOrDefault(),
                                  Fare = (from aa in garmentComodityPriceRepository.Query where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && a.ComodityId == aa.ComodityId && aa.IsValid == true select aa.Price).FirstOrDefault(),
                                  Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName,
                                  FC = (from cost in sumFCs where cost.RO == a.RONo select cost.FC / cost.Count).FirstOrDefault() }).Distinct();

            //var queryBalance = from a in
            //                          (from aa in garmentBalanceProductionStockRepository.Query
            //                           where aa.CreatedDate.AddHours(7).Date < dateFrom && (request.ro == null || (request.ro != null && request.ro != "" && aa.Ro == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.UnitId == aa.UnitId
            //                           select aa)

            //                   select new monitoringView
            //                   {
            //                       QtyCuttingIn = 0,
            //                       PriceCuttingIn = 0,
            //                       QtySewingIn = 0,
            //                       PriceSewingIn = 0,
            //                       QtyCuttingToPacking = 0,
            //                       PriceCuttingToPacking = 0,
            //                       QtyCuttingsubkon = 0,
            //                       PriceCuttingsubkon = 0,
            //                       AvalCutting = 0,
            //                       AvalCuttingPrice = 0,
            //                       AvalSewing = 0,
            //                       AvalSewingPrice = 0,
            //                       QtyLoading = 0,
            //                       PriceLoading = 0,
            //                       QtyLoadingAdjs = 0,
            //                       PriceLoadingAdjs = 0,
            //                       QtySewingOut = 0,
            //                       PriceSewingOut = 0,
            //                       QtySewingAdj = 0,
            //                       PriceSewingAdj = 0,
            //                       WipSewingToPacking = 0,
            //                       WipSewingToPackingPrice = 0,
            //                       WipFinishingOut = 0,
            //                       WipFinishingOutPrice = 0,
            //                       QtySewingRetur = 0,
            //                       PriceSewingRetur = 0,
            //                       QtySewingInTransfer = 0,
            //                       PriceSewingInTransfer = 0,
            //                       FinishingInQty = 0,
            //                       FinishingInPrice = 0,
            //                       SubconInQty = 0,
            //                       SubconInPrice = 0,
            //                       FinishingAdjQty = 0,
            //                       FinishingAdjPrice = 0,
            //                       FinishingTransferExpenditure = 0,
            //                       FinishingTransferExpenditurePrice = 0,
            //                       FinishingInTransferQty = 0,
            //                       FinishingInTransferPrice = 0,
            //                       FinishingOutQty = 0,
            //                       FinishingOutPrice = 0,
            //                       FinishingReturQty = 0,
            //                       FinishingReturPrice = 0,
            //                       SubconOutQty = 0,
            //                       SubconOutPrice = 0,
            //                       BeginingBalanceLoadingQty = a.BeginingBalanceLoadingQty,
            //                       BeginingBalanceLoadingPrice = a.BeginingBalanceLoadingPrice,
            //                       BeginingBalanceCuttingQty = a.BeginingBalanceCuttingQty,
            //                       BeginingBalanceCuttingPrice = a.BeginingBalanceCuttingPrice,
            //                       BeginingBalanceFinishingQty = a.BeginingBalanceFinishingQty,
            //                       BeginingBalanceFinishingPrice = a.BeginingBalanceFinishingPrice,
            //                       BeginingBalanceSewingQty = a.BeginingBalanceSewingQty,
            //                       BeginingBalanceSewingPrice = a.BeginingBalanceSewingPrice,
            //                       BeginingBalanceExpenditureGood = a.BeginingBalanceExpenditureGood,
            //                       BeginingBalanceExpenditureGoodPrice = a.BeginingBalanceExpenditureGoodPrice,
            //                       BeginingBalanceSubconQty = a.BeginingBalanceSubconQty,
            //                       BeginingBalanceSubconPrice = a.BeginingBalanceSubconPrice,
            //                       Ro = a.Ro,
            //                       ExpenditureGoodRetur = 0,
            //                       ExpenditureGoodReturPrice = 0,
            //                       QtyCuttingOut = 0,
            //                       PriceCuttingOut = 0,
            //                       PackingInQty = 0,
            //                       PackingInPrice = 0,
            //                       SampleQty = 0,
            //                       SamplePrice = 0,
            //                       OtherQty = 0,
            //                       OtherPrice = 0,
            //                       QtyLoadingInTransfer = 0,
            //                       PriceLoadingInTransfer = 0,
            //                       ExpenditureGoodInTransfer = 0,
            //                       ExpenditureGoodInTransferPrice = 0

            //                   };

            var QueryCuttingIn = (from a in (from aa in garmentCuttingInRepository.Query
                                             where aa.CuttingInDate.AddHours(7).Date >= dateBalance && aa.CuttingType != "Non Main Fabric" && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.CuttingInDate.AddHours(7).Date <= dateTo
                                             select new { aa.RONo, aa.Identity, aa.CuttingInDate })
                                  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                  select new
                                  {

                                      BeginingBalanceCuttingQty = a.CuttingInDate.AddHours(7).Date < dateFrom && a.CuttingInDate.AddHours(7).Date > dateBalance ? c.CuttingInQuantity : 0,
                                      BeginingBalanceCuttingPrice = a.CuttingInDate.AddHours(7).Date < dateFrom && a.CuttingInDate.AddHours(7).Date > dateBalance ? c.Price : 0,
                                      Ro = a.RONo,
                                      QtyCuttingIn = a.CuttingInDate.AddHours(7).Date >= dateFrom ? c.CuttingInQuantity : 0,
                                      PriceCuttingIn = a.CuttingInDate.AddHours(7).Date >= dateFrom ? c.Price : 0,

                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                  {
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingToPacking = 0,
                                      PriceCuttingToPacking = 0,
                                      QtyCuttingsubkon = 0,
                                      PriceCuttingsubkon = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtyLoadingAdjs = 0,
                                      PriceLoadingAdjs = 0,
                                      QtySewingOut = 0,
                                      PriceSewingOut = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      WipSewingToPacking = 0,
                                      WipSewingToPackingPrice = 0,
                                      WipFinishingOut = 0,
                                      WipFinishingOutPrice = 0,
                                      QtySewingRetur = 0,
                                      PriceSewingRetur = 0,
                                      QtySewingInTransfer = 0,
                                      PriceSewingInTransfer = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingTransferExpenditure = 0,
                                      FinishingTransferExpenditurePrice = 0,
                                      FinishingInTransferQty = 0,
                                      FinishingInTransferPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
                                      BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
                                      Ro = key,
                                      QtyCuttingIn = group.Sum(x => x.QtyCuttingIn),
                                      PriceCuttingIn = group.Sum(x => x.PriceCuttingIn),
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      PackingInQty = 0,
                                      PackingInPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceLoadingQty = 0,
                                      BeginingBalanceLoadingPrice = 0,
                                      BeginingBalanceFinishingQty = 0,
                                      BeginingBalanceFinishingPrice = 0
                                  });

            var QueryCuttingOut = (from a in (from aa in garmentCuttingOutRepository.Query
                                              where aa.CuttingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.CuttingOutDate.AddHours(7).Date <= dateTo
                                             /* && aa.CuttingOutType == "SEWING"*/ && aa.UnitId == aa.UnitFromId
                                              select new { aa.RONo, aa.Identity, aa.CuttingOutDate, aa.CuttingOutType })
                                   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId

                                   select new
                                   {
                                       BeginingBalanceCuttingQty = a.CuttingOutDate.AddHours(7).Date < dateFrom && a.CuttingOutDate.AddHours(7).Date > dateBalance ? -c.CuttingOutQuantity : 0,
                                       BeginingBalanceCuttingPrice = a.CuttingOutDate.AddHours(7).Date < dateFrom && a.CuttingOutDate.AddHours(7).Date > dateBalance ? -c.Price : 0,
                                       Ro = a.RONo,
                                       QtyCuttingOut = a.CuttingOutDate.AddHours(7).Date >= dateFrom && a.CuttingOutType == "SEWING" ? c.CuttingOutQuantity : 0,
                                       PriceCuttingOut = a.CuttingOutDate.AddHours(7).Date >= dateFrom && a.CuttingOutType == "SEWING" ? c.Price : 0,
                                       QtyCuttingToPacking = a.CuttingOutDate.AddHours(7).Date >= dateFrom && a.CuttingOutType == "PACKING" ? c.CuttingOutQuantity : 0,
                                       PriceCuttingToPacking = a.CuttingOutDate.AddHours(7).Date >= dateFrom && a.CuttingOutType == "PACKING" ? c.Price : 0,
                                   }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                   {
                                       QtyCuttingIn = 0,
                                       PriceCuttingIn = 0,
                                       QtySewingIn = 0,
                                       PriceSewingIn = 0,
                                       QtyCuttingToPacking = group.Sum(x => x.QtyCuttingToPacking),
                                       PriceCuttingToPacking = group.Sum(x => x.PriceCuttingToPacking),
                                       QtyCuttingsubkon = 0,
                                       PriceCuttingsubkon = 0,
                                       AvalCutting = 0,
                                       AvalCuttingPrice = 0,
                                       AvalSewing = 0,
                                       AvalSewingPrice = 0,
                                       QtyLoading = 0,
                                       PriceLoading = 0,
                                       QtyLoadingAdjs = 0,
                                       PriceLoadingAdjs = 0,
                                       QtySewingOut = 0,
                                       PriceSewingOut = 0,
                                       QtySewingAdj = 0,
                                       PriceSewingAdj = 0,
                                       WipSewingToPacking = 0,
                                       WipSewingToPackingPrice = 0,
                                       WipFinishingOut = 0,
                                       WipFinishingOutPrice = 0,
                                       QtySewingRetur = 0,
                                       PriceSewingRetur = 0,
                                       QtySewingInTransfer = 0,
                                       PriceSewingInTransfer = 0,
                                       FinishingInQty = 0,
                                       FinishingInPrice = 0,
                                       SubconInQty = 0,
                                       SubconInPrice = 0,
                                       FinishingAdjQty = 0,
                                       FinishingAdjPrice = 0,
                                       FinishingTransferExpenditure = 0,
                                       FinishingTransferExpenditurePrice = 0,
                                       FinishingInTransferQty = 0,
                                       FinishingInTransferPrice = 0,
                                       FinishingOutQty = 0,
                                       FinishingOutPrice = 0,
                                       FinishingReturQty = 0,
                                       FinishingReturPrice = 0,
                                       SubconOutQty = 0,
                                       SubconOutPrice = 0,
                                       BeginingBalanceLoadingQty = 0,
                                       BeginingBalanceLoadingPrice = 0,
                                       BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
                                       BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
                                       Ro = key,
                                       ExpenditureGoodRetur = 0,
                                       ExpenditureGoodReturPrice = 0,
                                       QtyCuttingOut = group.Sum(x => x.QtyCuttingOut),
                                       PriceCuttingOut = group.Sum(x => x.PriceCuttingOut),
                                       PackingInQty = 0,
                                       PackingInPrice = 0,
                                       SampleQty = 0,
                                       SamplePrice = 0,
                                       OtherQty = 0,
                                       OtherPrice = 0,
                                       QtyLoadingInTransfer = 0,
                                       PriceLoadingInTransfer = 0,
                                       ExpenditureGoodInTransfer = 0,
                                       ExpenditureGoodInTransferPrice = 0,
                                       BeginingBalanceFinishingQty = 0,
                                       BeginingBalanceFinishingPrice = 0

                                   });
            //var QueryCuttingOutSubkon = (from a in (from aa in garmentCuttingOutRepository.Query
            //                                        where aa.CuttingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == request.unit && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SUBKON"
            //                                        select new { aa.RONo,aa.Identity,aa.CuttingOutDate,aa.CuttingOutType})
            //                             join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
            //                             join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
            //                             select new monitoringView
            //                             {
            //                                 QtyCuttingIn = 0,
            //                                 PriceCuttingIn = 0,
            //                                 QtySewingIn = 0,
            //                                 PriceSewingIn = 0,
            //                                 QtyCuttingOut = 0,
            //                                 PriceCuttingOut = 0,
            //                                 QtyCuttingToPacking = 0,
            //                                 PriceCuttingToPacking = 0,
            //                                 AvalCutting = 0,
            //                                 AvalCuttingPrice = 0,
            //                                 AvalSewing = 0,
            //                                 AvalSewingPrice = 0,
            //                                 QtyLoading = 0,
            //                                 PriceLoading = 0,
            //                                 QtyLoadingAdjs = 0,
            //                                 PriceLoadingAdjs = 0,
            //                                 QtySewingOut = 0,
            //                                 PriceSewingOut = 0,
            //                                 QtySewingAdj = 0,
            //                                 PriceSewingAdj = 0,
            //                                 WipSewingToPacking = 0,
            //                                 WipSewingToPackingPrice = 0,
            //                                 WipFinishingOut = 0,
            //                                 WipFinishingOutPrice = 0,
            //                                 QtySewingRetur = 0,
            //                                 PriceSewingRetur = 0,
            //                                 QtySewingInTransfer = 0,
            //                                 PriceSewingInTransfer = 0,
            //                                 FinishingInQty = 0,
            //                                 FinishingInPrice = 0,
            //                                 SubconInQty = 0,
            //                                 SubconInPrice = 0,
            //                                 FinishingAdjQty = 0,
            //                                 FinishingAdjPrice = 0,
            //                                 FinishingTransferExpenditure = 0,
            //                                 FinishingTransferExpenditurePrice = 0,
            //                                 FinishingInTransferQty = 0,
            //                                 FinishingInTransferPrice = 0,
            //                                 FinishingOutQty = 0,
            //                                 FinishingOutPrice = 0,
            //                                 FinishingReturQty = 0,
            //                                 FinishingReturPrice = 0,
            //                                 SubconOutQty = 0,
            //                                 SubconOutPrice = 0,
            //                                 //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? c.CuttingOutQuantity : 0,
            //                                 //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? c.Price : 0,
            //                                 BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.CuttingOutQuantity : 0,
            //                                 Ro = a.RONo,
            //                                 BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.Price : 0,
            //                                 // FC = (from cost in FC where cost.ro == a.RONo select cost.fc).FirstOrDefault(),
            //                                 QtyCuttingsubkon = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
            //                                 PriceCuttingsubkon = a.CuttingOutDate >= dateFrom ? c.Price : 0,
            //                                 ExpenditureGoodRetur = 0,
            //                                 ExpenditureGoodReturPrice = 0,
            //                                 PackingInQty = 0,
            //                                 PackingInPrice = 0,
            //                                 SampleQty = 0,
            //                                 SamplePrice = 0,
            //                                 OtherQty = 0,
            //                                 OtherPrice = 0,
            //                                 QtyLoadingInTransfer = 0,
            //                                 PriceLoadingInTransfer = 0,
            //                                 ExpenditureGoodInTransfer = 0,
            //                                 ExpenditureGoodInTransferPrice = 0,
            //                                 BeginingBalanceLoadingQty = 0,
            //                                 BeginingBalanceLoadingPrice = 0,
            //                                 BeginingBalanceFinishingQty = 0,
            //                                 BeginingBalanceFinishingPrice = 0
            //                             });
            //var QueryCuttingOutSubkon = (from a in (from aa in garmentCuttingOutRepository.Query
            //                                        where aa.CuttingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == (request.unit == 0 ? aa.UnitFromId : request.unit) && aa.CuttingOutDate.AddHours(7).Date <= dateTo && aa.CuttingOutType == "SUBKON"
            //                                        select new { aa.RONo, aa.Identity, aa.CuttingOutDate, aa.CuttingOutType })
            //                             join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
            //                             join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
            //                             select new
            //                             {

            //                                 BeginingBalanceCuttingQty = a.CuttingOutDate.AddHours(7).Date < dateFrom && a.CuttingOutDate.AddHours(7).Date > dateBalance ? -c.CuttingOutQuantity : 0,
            //                                 Ro = a.RONo,
            //                                 BeginingBalanceCuttingPrice = a.CuttingOutDate.AddHours(7).Date < dateFrom && a.CuttingOutDate.AddHours(7).Date > dateBalance ? -c.Price : 0,
            //                                 QtyCuttingsubkon = a.CuttingOutDate.AddHours(7).Date >= dateFrom ? c.CuttingOutQuantity : 0,
            //                                 PriceCuttingsubkon = a.CuttingOutDate.AddHours(7).Date >= dateFrom ? c.Price : 0,
            //                             }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                             {
            //                                 QtyCuttingIn = 0,
            //                                 PriceCuttingIn = 0,
            //                                 QtySewingIn = 0,
            //                                 PriceSewingIn = 0,
            //                                 QtyCuttingOut = 0,
            //                                 PriceCuttingOut = 0,
            //                                 QtyCuttingToPacking = 0,
            //                                 PriceCuttingToPacking = 0,
            //                                 AvalCutting = 0,
            //                                 AvalCuttingPrice = 0,
            //                                 AvalSewing = 0,
            //                                 AvalSewingPrice = 0,
            //                                 QtyLoading = 0,
            //                                 PriceLoading = 0,
            //                                 QtyLoadingAdjs = 0,
            //                                 PriceLoadingAdjs = 0,
            //                                 QtySewingOut = 0,
            //                                 PriceSewingOut = 0,
            //                                 QtySewingAdj = 0,
            //                                 PriceSewingAdj = 0,
            //                                 WipSewingToPacking = 0,
            //                                 WipSewingToPackingPrice = 0,
            //                                 WipFinishingOut = 0,
            //                                 WipFinishingOutPrice = 0,
            //                                 QtySewingRetur = 0,
            //                                 PriceSewingRetur = 0,
            //                                 QtySewingInTransfer = 0,
            //                                 PriceSewingInTransfer = 0,
            //                                 FinishingInQty = 0,
            //                                 FinishingInPrice = 0,
            //                                 SubconInQty = 0,
            //                                 SubconInPrice = 0,
            //                                 FinishingAdjQty = 0,
            //                                 FinishingAdjPrice = 0,
            //                                 FinishingTransferExpenditure = 0,
            //                                 FinishingTransferExpenditurePrice = 0,
            //                                 FinishingInTransferQty = 0,
            //                                 FinishingInTransferPrice = 0,
            //                                 FinishingOutQty = 0,
            //                                 FinishingOutPrice = 0,
            //                                 FinishingReturQty = 0,
            //                                 FinishingReturPrice = 0,
            //                                 SubconOutQty = 0,
            //                                 SubconOutPrice = 0,
            //                                 BeginingBalanceLoadingQty = 0,
            //                                 BeginingBalanceLoadingPrice = 0,
            //                                 BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
            //                                 Ro = key,
            //                                 BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
            //                                 FC = 0,
            //                                 QtyCuttingsubkon = group.Sum(x => x.QtyCuttingsubkon),
            //                                 PriceCuttingsubkon = group.Sum(x => x.PriceCuttingsubkon),
            //                                 ExpenditureGoodRetur = 0,
            //                                 ExpenditureGoodReturPrice = 0,
            //                                 PackingInQty = 0,
            //                                 PackingInPrice = 0,
            //                                 SampleQty = 0,
            //                                 SamplePrice = 0,
            //                                 OtherQty = 0,
            //                                 OtherPrice = 0,
            //                                 QtyLoadingInTransfer = 0,
            //                                 PriceLoadingInTransfer = 0,
            //                                 ExpenditureGoodInTransfer = 0,
            //                                 ExpenditureGoodInTransferPrice = 0,
            //                                 BeginingBalanceFinishingQty = 0,
            //                                 BeginingBalanceFinishingPrice = 0

            //                             });
            
            //var QueryCuttingOutTransfer = (from a in (from aa in garmentCuttingOutRepository.Query
            //                                          where aa.CuttingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == (request.unit == 0 ? aa.UnitFromId : request.unit) && aa.CuttingOutDate.AddHours(7).Date <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId != aa.UnitFromId
            //                                          select new { aa.RONo, aa.Identity, aa.CuttingOutType, aa.CuttingOutDate })
            //                               join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
            //                               join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
            //                               select new
            //                               {
            //                                   BeginingBalanceCuttingQty = a.CuttingOutDate.AddHours(7).Date < dateFrom && a.CuttingOutDate.AddHours(7).Date > dateBalance ? -c.CuttingOutQuantity : 0,
            //                                   BeginingBalanceCuttingPrice = a.CuttingOutDate.AddHours(7).Date < dateFrom && a.CuttingOutDate.AddHours(7).Date > dateBalance ? -c.Price : 0,
            //                                   Ro = a.RONo,
            //                                   QtyCuttingToPacking = a.CuttingOutDate.AddHours(7).Date >= dateFrom ? c.CuttingOutQuantity : 0,
            //                                   PriceCuttingToPacking = a.CuttingOutDate.AddHours(7).Date >= dateFrom ? c.Price : 0,
            //                               }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                               {
            //                                   QtyCuttingIn = 0,
            //                                   PriceCuttingIn = 0,
            //                                   QtySewingIn = 0,
            //                                   PriceSewingIn = 0,
            //                                   QtyCuttingOut = 0,
            //                                   PriceCuttingOut = 0,
            //                                   QtyCuttingsubkon = 0,
            //                                   PriceCuttingsubkon = 0,
            //                                   AvalCutting = 0,
            //                                   AvalCuttingPrice = 0,
            //                                   AvalSewing = 0,
            //                                   AvalSewingPrice = 0,
            //                                   QtyLoading = 0,
            //                                   PriceLoading = 0,
            //                                   QtyLoadingAdjs = 0,
            //                                   PriceLoadingAdjs = 0,
            //                                   QtySewingOut = 0,
            //                                   PriceSewingOut = 0,
            //                                   QtySewingAdj = 0,
            //                                   PriceSewingAdj = 0,
            //                                   WipSewingToPacking = 0,
            //                                   WipSewingToPackingPrice = 0,
            //                                   WipFinishingOut = 0,
            //                                   WipFinishingOutPrice = 0,
            //                                   QtySewingRetur = 0,
            //                                   PriceSewingRetur = 0,
            //                                   QtySewingInTransfer = 0,
            //                                   PriceSewingInTransfer = 0,
            //                                   FinishingInQty = 0,
            //                                   FinishingInPrice = 0,
            //                                   SubconInQty = 0,
            //                                   SubconInPrice = 0,
            //                                   FinishingAdjQty = 0,
            //                                   FinishingAdjPrice = 0,
            //                                   FinishingTransferExpenditure = 0,
            //                                   FinishingTransferExpenditurePrice = 0,
            //                                   FinishingInTransferQty = 0,
            //                                   FinishingInTransferPrice = 0,
            //                                   FinishingOutQty = 0,
            //                                   FinishingOutPrice = 0,
            //                                   FinishingReturQty = 0,
            //                                   FinishingReturPrice = 0,
            //                                   SubconOutQty = 0,
            //                                   SubconOutPrice = 0,
            //                                   //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
            //                                   //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
            //                                   BeginingBalanceLoadingQty = 0,
            //                                   BeginingBalanceLoadingPrice = 0,
            //                                   BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
            //                                   BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
            //                                   Ro = key,
            //                                   QtyCuttingToPacking = group.Sum(x => x.QtyCuttingToPacking),
            //                                   PriceCuttingToPacking = group.Sum(x => x.PriceCuttingToPacking),
            //                                   ExpenditureGoodRetur = 0,
            //                                   ExpenditureGoodReturPrice = 0,
            //                                   PackingInQty = 0,
            //                                   PackingInPrice = 0,
            //                                   SampleQty = 0,
            //                                   SamplePrice = 0,
            //                                   OtherQty = 0,
            //                                   OtherPrice = 0,
            //                                   QtyLoadingInTransfer = 0,
            //                                   PriceLoadingInTransfer = 0,
            //                                   ExpenditureGoodInTransfer = 0,
            //                                   ExpenditureGoodInTransferPrice = 0,
            //                                   BeginingBalanceFinishingQty = 0,
            //                                   BeginingBalanceFinishingPrice = 0

            //                               });
            
            

           
            //var QueryAvalCompSewing = (from a in (from aa in garmentAvalComponentRepository.Query
            //                                      where aa.Date.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.Date.AddHours(7).Date <= dateTo && aa.AvalComponentType == "SEWING"
            //                                      select new { aa.RONo, aa.Identity, aa.Date, aa.AvalComponentType })
            //                           join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
            //                           select new
            //                           {
            //                               Ro = a.RONo,
            //                               AvalSewing = a.Date.AddHours(7).Date >= dateFrom ? b.Quantity : 0,
            //                               AvalSewingPrice = a.Date.AddHours(7).Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
            //                               BeginingBalanceCuttingQty = a.Date.AddHours(7).Date < dateFrom && a.Date.AddHours(7).Date > dateBalance ? -b.Quantity : 0,

            //                           }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                           {
            //                               QtySewingIn = 0,
            //                               PriceSewingIn = 0,
            //                               QtyCuttingOut = 0,
            //                               PriceCuttingOut = 0,
            //                               QtyCuttingToPacking = 0,
            //                               PriceCuttingToPacking = 0,
            //                               QtyCuttingsubkon = 0,
            //                               PriceCuttingsubkon = 0,
            //                               AvalCutting = 0,
            //                               AvalCuttingPrice = 0,
            //                               QtyLoading = 0,
            //                               PriceLoading = 0,
            //                               QtyLoadingAdjs = 0,
            //                               PriceLoadingAdjs = 0,
            //                               QtySewingOut = 0,
            //                               PriceSewingOut = 0,
            //                               QtySewingAdj = 0,
            //                               PriceSewingAdj = 0,
            //                               WipSewingToPacking = 0,
            //                               WipSewingToPackingPrice = 0,
            //                               WipFinishingOut = 0,
            //                               WipFinishingOutPrice = 0,
            //                               QtySewingRetur = 0,
            //                               PriceSewingRetur = 0,
            //                               QtySewingInTransfer = 0,
            //                               PriceSewingInTransfer = 0,
            //                               FinishingInQty = 0,
            //                               FinishingInPrice = 0,
            //                               SubconInQty = 0,
            //                               SubconInPrice = 0,
            //                               FinishingAdjQty = 0,
            //                               FinishingAdjPrice = 0,
            //                               FinishingTransferExpenditure = 0,
            //                               FinishingTransferExpenditurePrice = 0,
            //                               FinishingInTransferQty = 0,
            //                               FinishingInTransferPrice = 0,
            //                               FinishingOutQty = 0,
            //                               FinishingOutPrice = 0,
            //                               FinishingReturQty = 0,
            //                               FinishingReturPrice = 0,
            //                               SubconOutQty = 0,
            //                               SubconOutPrice = 0,
            //                               BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty), //0,a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
            //                               BeginingBalanceCuttingPrice = 0,//a.Date < dateFrom && a.Date > dateBalance  ? -Convert.ToDouble(b.Price) : 0,
            //                               Ro = key,
            //                               QtyCuttingIn = 0,
            //                               PriceCuttingIn = 0,
            //                               AvalSewing = group.Sum(x => x.AvalSewing),
            //                               AvalSewingPrice = group.Sum(x => x.AvalSewingPrice),
            //                               ExpenditureGoodRetur = 0,
            //                               ExpenditureGoodReturPrice = 0,
            //                               PackingInQty = 0,
            //                               PackingInPrice = 0,
            //                               SampleQty = 0,
            //                               SamplePrice = 0,
            //                               OtherQty = 0,
            //                               OtherPrice = 0,
            //                               QtyLoadingInTransfer = 0,
            //                               PriceLoadingInTransfer = 0,
            //                               ExpenditureGoodInTransfer = 0,
            //                               ExpenditureGoodInTransferPrice = 0,
            //                               BeginingBalanceLoadingQty = 0,
            //                               BeginingBalanceLoadingPrice = 0,
            //                               BeginingBalanceFinishingQty = 0,
            //                               BeginingBalanceFinishingPrice = 0
            //                           });
            

            //var QueryAvalCompCutting = (from a in (from aa in garmentAvalComponentRepository.Query
            //                                       where aa.Date.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.Date.AddHours(7).Date <= dateTo && aa.AvalComponentType == "CUTTING"
            //                                       select new { aa.RONo, aa.Identity, aa.Date, aa.AvalComponentType })
            //                            join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
            //                            select new
            //                            {
            //                                Ro = a.RONo,
            //                                AvalCutting = a.Date.AddHours(7).Date >= dateFrom ? b.Quantity : 0,
            //                                AvalCuttingPrice = a.Date.AddHours(7).Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
            //                                BeginingBalanceCuttingQty = a.Date.AddHours(7).Date < dateFrom && a.Date.AddHours(7).Date > dateBalance ? -b.Quantity : 0,
            //                                //BeginingBalanceCuttingPrice = a.Date < dateFrom && a.Date > dateBalance ? -Convert.ToDouble(b.Price) : 0
            //                            }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                            {
            //                                QtyCuttingIn = 0,
            //                                PriceCuttingIn = 0,
            //                                QtySewingIn = 0,
            //                                PriceSewingIn = 0,
            //                                QtyCuttingOut = 0,
            //                                PriceCuttingOut = 0,
            //                                QtyCuttingToPacking = 0,
            //                                PriceCuttingToPacking = 0,
            //                                QtyCuttingsubkon = 0,
            //                                PriceCuttingsubkon = 0,
            //                                AvalSewing = 0,
            //                                AvalSewingPrice = 0,
            //                                QtyLoading = 0,
            //                                PriceLoading = 0,
            //                                QtyLoadingAdjs = 0,
            //                                PriceLoadingAdjs = 0,
            //                                QtySewingOut = 0,
            //                                PriceSewingOut = 0,
            //                                QtySewingAdj = 0,
            //                                PriceSewingAdj = 0,
            //                                WipSewingToPacking = 0,
            //                                WipSewingToPackingPrice = 0,
            //                                WipFinishingOut = 0,
            //                                WipFinishingOutPrice = 0,
            //                                QtySewingRetur = 0,
            //                                PriceSewingRetur = 0,
            //                                QtySewingInTransfer = 0,
            //                                PriceSewingInTransfer = 0,
            //                                FinishingInQty = 0,
            //                                FinishingInPrice = 0,
            //                                SubconInQty = 0,
            //                                SubconInPrice = 0,
            //                                FinishingAdjQty = 0,
            //                                FinishingAdjPrice = 0,
            //                                FinishingTransferExpenditure = 0,
            //                                FinishingTransferExpenditurePrice = 0,
            //                                FinishingInTransferQty = 0,
            //                                FinishingInTransferPrice = 0,
            //                                FinishingOutQty = 0,
            //                                FinishingOutPrice = 0,
            //                                FinishingReturQty = 0,
            //                                FinishingReturPrice = 0,
            //                                SubconOutQty = 0,
            //                                SubconOutPrice = 0,
            //                                BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty), //0,a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
            //                                BeginingBalanceCuttingPrice = 0,// a.Date < dateFrom && a.Date > dateBalance ? -Convert.ToDouble(b.Price) : 0,
            //                                Ro = key,
            //                                AvalCutting = group.Sum(x => x.AvalCutting),
            //                                AvalCuttingPrice = group.Sum(x => x.AvalCuttingPrice),
            //                                ExpenditureGoodRetur = 0,
            //                                ExpenditureGoodReturPrice = 0,
            //                                PackingInQty = 0,
            //                                PackingInPrice = 0,
            //                                SampleQty = 0,
            //                                SamplePrice = 0,
            //                                OtherQty = 0,
            //                                OtherPrice = 0,
            //                                QtyLoadingInTransfer = 0,
            //                                PriceLoadingInTransfer = 0,
            //                                ExpenditureGoodInTransfer = 0,
            //                                ExpenditureGoodInTransferPrice = 0,
            //                                BeginingBalanceLoadingQty = 0,
            //                                BeginingBalanceLoadingPrice = 0,
            //                                BeginingBalanceFinishingQty = 0,
            //                                BeginingBalanceFinishingPrice = 0
            //                            });
            
            
            //var QueryLoadingInTransfer = (from a in (from aa in garmentSewingDORepository.Query
            //                                         where aa.SewingDODate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.UnitFromId != aa.UnitId && aa.SewingDODate.AddHours(7).Date <= dateTo
            //                                         select new { aa.RONo, aa.Identity, aa.SewingDODate })
            //                              join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
            //                              select new
            //                              {

            //                                  QtyLoadingInTransfer = a.SewingDODate.AddHours(7).Date >= dateFrom ? b.Quantity : 0,
            //                                  PriceLoadingInTransfer = a.SewingDODate.AddHours(7).Date >= dateFrom ? b.Price : 0,
            //                                  BeginingBalanceLoadingQty = (a.SewingDODate.AddHours(7).Date < dateFrom && a.SewingDODate.AddHours(7).Date > dateBalance) ? b.Quantity : 0,
            //                                  BeginingBalanceLoadingPrice = (a.SewingDODate.AddHours(7).Date < dateFrom && a.SewingDODate.AddHours(7).Date > dateBalance) ? b.Price : 0,
            //                                  Ro = a.RONo,

            //                              }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                              {
            //                                  QtyCuttingIn = 0,
            //                                  PriceCuttingIn = 0,
            //                                  QtySewingIn = 0,
            //                                  PriceSewingIn = 0,
            //                                  QtyCuttingOut = 0,
            //                                  PriceCuttingOut = 0,
            //                                  QtyCuttingToPacking = 0,
            //                                  PriceCuttingToPacking = 0,
            //                                  QtyCuttingsubkon = 0,
            //                                  PriceCuttingsubkon = 0,
            //                                  AvalCutting = 0,
            //                                  AvalCuttingPrice = 0,
            //                                  AvalSewing = 0,
            //                                  AvalSewingPrice = 0,
            //                                  QtyLoading = 0,
            //                                  PriceLoading = 0,
            //                                  QtyLoadingAdjs = 0,
            //                                  PriceLoadingAdjs = 0,
            //                                  QtySewingOut = 0,
            //                                  PriceSewingOut = 0,
            //                                  QtySewingAdj = 0,
            //                                  PriceSewingAdj = 0,
            //                                  WipSewingToPacking = 0,
            //                                  WipSewingToPackingPrice = 0,
            //                                  WipFinishingOut = 0,
            //                                  WipFinishingOutPrice = 0,
            //                                  QtySewingRetur = 0,
            //                                  PriceSewingRetur = 0,
            //                                  QtySewingInTransfer = 0,
            //                                  PriceSewingInTransfer = 0,
            //                                  FinishingInQty = 0,
            //                                  FinishingInPrice = 0,
            //                                  SubconInQty = 0,
            //                                  SubconInPrice = 0,
            //                                  FinishingAdjQty = 0,
            //                                  FinishingAdjPrice = 0,
            //                                  FinishingTransferExpenditure = 0,
            //                                  FinishingTransferExpenditurePrice = 0,
            //                                  FinishingInTransferQty = 0,
            //                                  FinishingInTransferPrice = 0,
            //                                  FinishingOutQty = 0,
            //                                  FinishingOutPrice = 0,
            //                                  FinishingReturQty = 0,
            //                                  FinishingReturPrice = 0,
            //                                  SubconOutQty = 0,
            //                                  SubconOutPrice = 0,
            //                                  QtyLoadingInTransfer = group.Sum(x => x.QtyLoadingInTransfer),
            //                                  PriceLoadingInTransfer = group.Sum(x => x.PriceLoadingInTransfer),
            //                                  BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty),
            //                                  BeginingBalanceLoadingPrice = group.Sum(x => x.BeginingBalanceLoadingPrice),
            //                                  Ro = key,
            //                                  ExpenditureGoodRetur = 0,
            //                                  ExpenditureGoodReturPrice = 0,
            //                                  PackingInQty = 0,
            //                                  PackingInPrice = 0,
            //                                  SampleQty = 0,
            //                                  SamplePrice = 0,
            //                                  OtherQty = 0,
            //                                  OtherPrice = 0,
            //                                  ExpenditureGoodInTransfer = 0,
            //                                  ExpenditureGoodInTransferPrice = 0,
            //                                  BeginingBalanceCuttingQty = 0,
            //                                  BeginingBalanceCuttingPrice = 0,
            //                                  BeginingBalanceFinishingQty = 0,
            //                                  BeginingBalanceFinishingPrice = 0
            //                              });
            
            var QueryLoadingIn = (from a in (from aa in garmentLoadingInRepository.Query
                                           where aa.LoadingDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.LoadingDate.AddHours(7).Date <= dateTo
                                           select new { aa.RONo, aa.Identity, aa.LoadingDate, aa.UnitId, aa.UnitFromId })
                                join b in garmentLoadingInItemRepository.Query on a.Identity equals b.LoadingId
                                select new
                                {
                                    BeginingBalanceLoadingQty = a.LoadingDate.AddHours(7).Date < dateFrom && a.UnitId == a.UnitFromId && a.LoadingDate.AddHours(7).Date > dateBalance ? b.Quantity : 0,
                                    BeginingBalanceLoadingPrice = a.LoadingDate.AddHours(7).Date < dateFrom && a.UnitId == a.UnitFromId && a.LoadingDate.AddHours(7).Date > dateBalance ? b.Price : 0,
                                    Ro = a.RONo,
                                    QtyLoadingIn = a.LoadingDate.AddHours(7).Date >= dateFrom && a.UnitId == a.UnitFromId ? b.Quantity : 0,
                                    PriceLoadingIn = a.LoadingDate.AddHours(7).Date >= dateFrom && a.UnitId == a.UnitFromId ? b.Price : 0,

                                }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                {
                                    QtyCuttingIn = 0,
                                    PriceCuttingIn = 0,
                                    QtySewingIn = 0,
                                    PriceSewingIn = 0,
                                    QtyCuttingOut = 0,
                                    PriceCuttingOut = 0,
                                    QtyCuttingToPacking = 0,
                                    PriceCuttingToPacking = 0,
                                    QtyCuttingsubkon = 0,
                                    PriceCuttingsubkon = 0,
                                    AvalCutting = 0,
                                    AvalCuttingPrice = 0,
                                    AvalSewing = 0,
                                    AvalSewingPrice = 0,
                                    QtyLoadingAdjs = 0,
                                    PriceLoadingAdjs = 0,
                                    QtySewingOut = 0,
                                    PriceSewingOut = 0,
                                    QtySewingAdj = 0,
                                    PriceSewingAdj = 0,
                                    WipSewingToPacking = 0,
                                    WipSewingToPackingPrice = 0,
                                    WipFinishingOut = 0,
                                    WipFinishingOutPrice = 0,
                                    QtySewingRetur = 0,
                                    PriceSewingRetur = 0,
                                    QtySewingInTransfer = 0,
                                    PriceSewingInTransfer = 0,
                                    FinishingInQty = 0,
                                    FinishingInPrice = 0,
                                    SubconInQty = 0,
                                    SubconInPrice = 0,
                                    FinishingAdjQty = 0,
                                    FinishingAdjPrice = 0,
                                    FinishingTransferExpenditure = 0,
                                    FinishingTransferExpenditurePrice = 0,
                                    FinishingInTransferQty = 0,
                                    FinishingInTransferPrice = 0,
                                    FinishingOutQty = 0,
                                    FinishingOutPrice = 0,
                                    FinishingReturQty = 0,
                                    FinishingReturPrice = 0,
                                    SubconOutQty = 0,
                                    SubconOutPrice = 0,
                                    QtyLoadingInTransfer = 0,
                                    PriceLoadingInTransfer = 0,
                                    QtyLoadingIn = group.Sum(x => x.QtyLoadingIn),
                                    PriceLoadingIn = group.Sum(x => x.PriceLoadingIn),
                                    // BeginingBalanceSewingQty = a.LoadingDate < dateFrom ? b.Quantity : 0,
                                    //BeginingBalanceSewingPrice = a.LoadingDate < dateFrom ? b.Price : 0,
                                    BeginingBalanceSewingQty = 0,
                                    BeginingBalanceSewingPrice = 0,
                                    BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty),
                                    BeginingBalanceLoadingPrice = group.Sum(x => x.BeginingBalanceLoadingPrice),
                                    Ro = key,
                                    QtyLoading = 0,
                                    PriceLoading = 0,
                                    ExpenditureGoodRetur = 0,
                                    ExpenditureGoodReturPrice = 0,
                                    PackingInQty = 0,
                                    PackingInPrice = 0,
                                    SampleQty = 0,
                                    SamplePrice = 0,
                                    OtherQty = 0,
                                    OtherPrice = 0,
                                    ExpenditureGoodInTransfer = 0,
                                    ExpenditureGoodInTransferPrice = 0,
                                    BeginingBalanceCuttingQty = 0,
                                    BeginingBalanceCuttingPrice = 0,
                                    BeginingBalanceFinishingQty = 0,
                                    BeginingBalanceFinishingPrice = 0
                                });

            var QueryLoadingOut = (from a in (from aa in garmentLoadingOutRepository.Query
                                             where aa.LoadingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.LoadingOutDate.AddHours(7).Date <= dateTo
                                             select new { aa.RONo, aa.Identity, aa.LoadingOutDate, aa.UnitId, aa.UnitFromId })
                                  join b in garmentLoadingOutItemRepository.Query on a.Identity equals b.LoadingOutId
                                  select new
                                  {
                                      BeginingBalanceLoadingQty = a.LoadingOutDate.AddHours(7).Date < dateFrom && a.UnitId == a.UnitFromId && a.LoadingOutDate.AddHours(7).Date > dateBalance ? -b.Quantity : 0,
                                      BeginingBalanceLoadingPrice = a.LoadingOutDate.AddHours(7).Date < dateFrom && a.UnitId == a.UnitFromId && a.LoadingOutDate.AddHours(7).Date > dateBalance ? -b.Price : 0,
                                      Ro = a.RONo,
                                      QtyLoading = a.LoadingOutDate.AddHours(7).Date >= dateFrom && a.UnitId == a.UnitFromId ? b.Quantity : 0,
                                      PriceLoading = a.LoadingOutDate.AddHours(7).Date >= dateFrom && a.UnitId == a.UnitFromId ? b.Price : 0,

                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                  {
                                      QtyCuttingIn = 0,
                                      PriceCuttingIn = 0,
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingToPacking = 0,
                                      PriceCuttingToPacking = 0,
                                      QtyCuttingsubkon = 0,
                                      PriceCuttingsubkon = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoadingAdjs = 0,
                                      PriceLoadingAdjs = 0,
                                      QtySewingOut = 0,
                                      PriceSewingOut = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      WipSewingToPacking = 0,
                                      WipSewingToPackingPrice = 0,
                                      WipFinishingOut = 0,
                                      WipFinishingOutPrice = 0,
                                      QtySewingRetur = 0,
                                      PriceSewingRetur = 0,
                                      QtySewingInTransfer = 0,
                                      PriceSewingInTransfer = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingTransferExpenditure = 0,
                                      FinishingTransferExpenditurePrice = 0,
                                      FinishingInTransferQty = 0,
                                      FinishingInTransferPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      // BeginingBalanceSewingQty = a.LoadingDate < dateFrom ? b.Quantity : 0,
                                      //BeginingBalanceSewingPrice = a.LoadingDate < dateFrom ? b.Price : 0,
                                      BeginingBalanceSewingQty = 0,
                                      BeginingBalanceSewingPrice = 0,
                                      BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty),
                                      BeginingBalanceLoadingPrice = group.Sum(x => x.BeginingBalanceLoadingPrice),
                                      Ro = key,
                                      QtyLoading = group.Sum(x => x.QtyLoading),
                                      PriceLoading = group.Sum(x => x.PriceLoading),
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      PackingInQty = 0,
                                      PackingInPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceCuttingQty = 0,
                                      BeginingBalanceCuttingPrice = 0,
                                      BeginingBalanceFinishingQty = 0,
                                      BeginingBalanceFinishingPrice = 0
                                  });

            //var QueryLoadingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
            //                                  where aa.AdjustmentDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate.AddHours(7).Date <= dateTo && aa.AdjustmentType == "LOADING"
            //                                  select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
            //                       join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
            //                       select new
            //                       {

            //                           BeginingBalanceLoadingQty = a.AdjustmentDate.AddHours(7).Date < dateFrom && a.AdjustmentDate.AddHours(7).Date > dateBalance ? -b.Quantity : 0,
            //                           BeginingBalanceLoadingPrice = a.AdjustmentDate.AddHours(7).Date < dateFrom && a.AdjustmentDate.AddHours(7).Date > dateBalance ? -b.Price : 0,
            //                           Ro = a.RONo,
            //                           QtyLoadingAdjs = a.AdjustmentDate.AddHours(7).Date >= dateFrom ? b.Quantity : 0,
            //                           PriceLoadingAdjs = a.AdjustmentDate.AddHours(7).Date >= dateFrom ? b.Price : 0,

            //                       }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                       {
            //                           QtyCuttingIn = 0,
            //                           PriceCuttingIn = 0,
            //                           QtySewingIn = 0,
            //                           PriceSewingIn = 0,
            //                           QtyCuttingOut = 0,
            //                           PriceCuttingOut = 0,
            //                           QtyCuttingToPacking = 0,
            //                           PriceCuttingToPacking = 0,
            //                           QtyCuttingsubkon = 0,
            //                           PriceCuttingsubkon = 0,
            //                           AvalCutting = 0,
            //                           AvalCuttingPrice = 0,
            //                           AvalSewing = 0,
            //                           AvalSewingPrice = 0,
            //                           QtyLoading = 0,
            //                           PriceLoading = 0,
            //                           QtySewingOut = 0,
            //                           PriceSewingOut = 0,
            //                           QtySewingAdj = 0,
            //                           PriceSewingAdj = 0,
            //                           WipSewingToPacking = 0,
            //                           WipSewingToPackingPrice = 0,
            //                           WipFinishingOut = 0,
            //                           WipFinishingOutPrice = 0,
            //                           QtySewingRetur = 0,
            //                           PriceSewingRetur = 0,
            //                           QtySewingInTransfer = 0,
            //                           PriceSewingInTransfer = 0,
            //                           FinishingInQty = 0,
            //                           FinishingInPrice = 0,
            //                           SubconInQty = 0,
            //                           SubconInPrice = 0,
            //                           FinishingAdjQty = 0,
            //                           FinishingAdjPrice = 0,
            //                           FinishingTransferExpenditure = 0,
            //                           FinishingTransferExpenditurePrice = 0,
            //                           FinishingInTransferQty = 0,
            //                           FinishingInTransferPrice = 0,
            //                           FinishingOutQty = 0,
            //                           FinishingOutPrice = 0,
            //                           FinishingReturQty = 0,
            //                           FinishingReturPrice = 0,
            //                           SubconOutQty = 0,
            //                           SubconOutPrice = 0,
            //                           BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty),
            //                           BeginingBalanceLoadingPrice = group.Sum(x => x.BeginingBalanceLoadingPrice),
            //                           Ro = key,
            //                           QtyLoadingAdjs = group.Sum(x => x.QtyLoadingAdjs),
            //                           PriceLoadingAdjs = group.Sum(x => x.PriceLoadingAdjs),
            //                           ExpenditureGoodRetur = 0,
            //                           ExpenditureGoodReturPrice = 0,
            //                           PackingInQty = 0,
            //                           PackingInPrice = 0,
            //                           SampleQty = 0,
            //                           SamplePrice = 0,
            //                           OtherQty = 0,
            //                           OtherPrice = 0,
            //                           QtyLoadingInTransfer = 0,
            //                           PriceLoadingInTransfer = 0,
            //                           ExpenditureGoodInTransfer = 0,
            //                           ExpenditureGoodInTransferPrice = 0,
            //                           BeginingBalanceCuttingQty = 0,
            //                           BeginingBalanceCuttingPrice = 0,
            //                           BeginingBalanceFinishingQty = 0,
            //                           BeginingBalanceFinishingPrice = 0
            //                       });


            var QuerySewingIn = (from a in (from aa in garmentSewingInRepository.Query
                                            where aa.SewingInDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.SewingInDate.AddHours(7).Date <= dateTo
                                            select new { aa.RONo, aa.Identity, aa.SewingInDate, aa.SewingFrom })
                                 join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
                                 select new
                                 {
                                     BeginingBalanceSewingQty = (a.SewingInDate.AddHours(7).Date < dateFrom && a.SewingInDate.AddHours(7).Date > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Quantity : 0,
                                     BeginingBalanceSewingPrice = (a.SewingInDate.AddHours(7).Date < dateFrom && a.SewingInDate.AddHours(7).Date > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Price : 0,
                                     QtySewingIn = (a.SewingInDate.AddHours(7).Date >= dateFrom) && a.SewingFrom != "SEWING" ? b.Quantity : 0,
                                     PriceSewingIn = (a.SewingInDate.AddHours(7).Date >= dateFrom) && a.SewingFrom != "SEWING" ? b.Price : 0,
                                     Ro = a.RONo

                                 }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                 {
                                     QtyCuttingIn = 0,
                                     PriceCuttingIn = 0,
                                     QtyCuttingOut = 0,
                                     PriceCuttingOut = 0,
                                     QtyCuttingToPacking = 0,
                                     PriceCuttingToPacking = 0,
                                     QtyCuttingsubkon = 0,
                                     PriceCuttingsubkon = 0,
                                     AvalCutting = 0,
                                     AvalCuttingPrice = 0,
                                     AvalSewing = 0,
                                     AvalSewingPrice = 0,
                                     QtyLoading = 0,
                                     PriceLoading = 0,
                                     QtyLoadingAdjs = 0,
                                     PriceLoadingAdjs = 0,
                                     QtySewingOut = 0,
                                     PriceSewingOut = 0,
                                     QtySewingAdj = 0,
                                     PriceSewingAdj = 0,
                                     WipSewingToPacking = 0,
                                     WipSewingToPackingPrice = 0,
                                     WipFinishingOut = 0,
                                     WipFinishingOutPrice = 0,
                                     QtySewingRetur = 0,
                                     PriceSewingRetur = 0,
                                     QtySewingInTransfer = 0,
                                     PriceSewingInTransfer = 0,
                                     FinishingInQty = 0,
                                     FinishingInPrice = 0,
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     FinishingAdjPrice = 0,
                                     FinishingTransferExpenditure = 0,
                                     FinishingTransferExpenditurePrice = 0,
                                     FinishingInTransferQty = 0,
                                     FinishingInTransferPrice = 0,
                                     FinishingOutQty = 0,
                                     FinishingOutPrice = 0,
                                     FinishingReturQty = 0,
                                     FinishingReturPrice = 0,
                                     SubconOutQty = 0,
                                     SubconOutPrice = 0,
                                     BeginingBalanceSewingQty = group.Sum(x => x.BeginingBalanceSewingQty),
                                     BeginingBalanceSewingPrice = group.Sum(x => x.BeginingBalanceSewingPrice),
                                     QtySewingIn = group.Sum(x => x.QtySewingIn),
                                     PriceSewingIn = group.Sum(x => x.PriceSewingIn),
                                     Ro = key,
                                     ExpenditureGoodRetur = 0,
                                     ExpenditureGoodReturPrice = 0,
                                     PackingInQty = 0,
                                     PackingInPrice = 0,
                                     SampleQty = 0,
                                     SamplePrice = 0,
                                     OtherQty = 0,
                                     OtherPrice = 0,
                                     QtyLoadingInTransfer = 0,
                                     PriceLoadingInTransfer = 0,
                                     ExpenditureGoodInTransfer = 0,
                                     ExpenditureGoodInTransferPrice = 0,
                                     BeginingBalanceCuttingQty = 0,
                                     BeginingBalanceCuttingPrice = 0,
                                     BeginingBalanceLoadingQty = 0,
                                     BeginingBalanceLoadingPrice = 0,
                                     BeginingBalanceFinishingQty = 0,
                                     BeginingBalanceFinishingPrice = 0
                                 });


            //var QuerySewingOut = (from a in (from aa in garmentSewingOutRepository.Query
            //                                 where aa.SewingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.SewingOutDate <= dateTo
            //                                 select new { aa.RONo, aa.Identity, aa.SewingOutDate,aa.SewingTo,aa.UnitToId,aa.UnitId})
            //                      join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId

            //                      select new monitoringView
            //                      {
            //                          QtyCuttingIn = 0,
            //                          PriceCuttingIn = 0,
            //                          QtySewingIn = 0,
            //                          PriceSewingIn = 0,
            //                          QtyCuttingOut = 0,
            //                          PriceCuttingOut = 0,
            //                          QtyCuttingToPacking = 0,
            //                          PriceCuttingToPacking = 0,
            //                          AvalCutting = 0,
            //                          AvalCuttingPrice = 0,
            //                          AvalSewing = 0,
            //                          AvalSewingPrice = 0,
            //                          QtyLoading = 0,
            //                          PriceLoading = 0,
            //                          QtyLoadingAdjs = 0,
            //                          PriceLoadingAdjs = 0,
            //                          QtySewingAdj = 0,
            //                          PriceSewingAdj = 0,
            //                          FinishingInQty = 0,
            //                          FinishingInPrice = 0,
            //                          SubconInQty = 0,
            //                          SubconInPrice = 0,
            //                          FinishingAdjQty = 0,
            //                          FinishingAdjPrice = 0,
            //                          FinishingOutQty = 0,
            //                          FinishingOutPrice = 0,
            //                          FinishingReturQty = 0,
            //                          FinishingReturPrice = 0,
            //                          SubconOutQty = 0,
            //                          SubconOutPrice = 0,
            //                          FinishingTransferExpenditure = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0,
            //                          FinishingTransferExpenditurePrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == request.unit) ? b.Price : 0,
            //                          FinishingInTransferQty = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
            //                          FinishingInTransferPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
            //                          BeginingBalanceFinishingQty = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
            //                          BeginingBalanceFinishingPrice = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
            //                          BeginingBalanceSewingQty = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0 - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0) + ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0),
            //                          BeginingBalanceSewingPrice = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Price : 0 - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Price : 0) + ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0),

            //                          QtySewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0,
            //                          PriceSewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Price : 0,
            //                          QtySewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
            //                          PriceSewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
            //                          WipSewingToPacking = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
            //                          WipSewingToPackingPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
            //                          WipFinishingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
            //                          WipFinishingOutPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
            //                          QtySewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
            //                          PriceSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
            //                          //BeginingBalanceExpenditureGood = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Quantity : 0,
            //                          //BeginingBalanceExpenditureGoodPrice = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Price : 0,
            //                          BeginingBalanceExpenditureGood = 0,
            //                          BeginingBalanceExpenditureGoodPrice = 0,
            //                          Ro = a.RONo,
            //                          ExpenditureGoodRetur = 0,
            //                          ExpenditureGoodReturPrice = 0,
            //                          QtyLoadingInTransfer = 0,
            //                          PriceLoadingInTransfer = 0,
            //                          PackingInQty = 0,
            //                          PackingInPrice = 0,
            //                          SampleQty = 0,
            //                          SamplePrice = 0,
            //                          OtherQty = 0,
            //                          OtherPrice = 0,
            //                          ExpenditureGoodInTransfer = 0,
            //                          ExpenditureGoodInTransferPrice = 0,
            //                          BeginingBalanceCuttingQty = 0,
            //                          BeginingBalanceCuttingPrice = 0,
            //                          BeginingBalanceLoadingQty = 0,
            //                          BeginingBalanceLoadingPrice = 0

            //                      });

            var QuerySewingOut = (from a in (from aa in garmentSewingOutRepository.Query
                                             where aa.SewingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.SewingOutDate.AddHours(7).Date <= dateTo
                                             select new { aa.RONo, aa.Identity, aa.SewingOutDate, aa.SewingTo, aa.UnitToId, aa.UnitId })
                                  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId

                                  select new
                                  {

                                      //FinishingTransferExpenditure = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      //FinishingTransferExpenditurePrice = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      //FinishingInTransferQty = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      //FinishingInTransferPrice = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      //BeginingBalanceFinishingQty = (a.SewingOutDate.AddHours(7).Date < dateFrom && a.SewingOutDate.AddHours(7).Date > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      //BeginingBalanceFinishingPrice = (a.SewingOutDate.AddHours(7).Date < dateFrom && a.SewingOutDate.AddHours(7).Date > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      BeginingBalanceSewingQty = (a.SewingOutDate.AddHours(7).Date < dateFrom && a.SewingOutDate.AddHours(7).Date > dateBalance && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Quantity : 0, 
                                      BeginingBalanceSewingPrice = (a.SewingOutDate.AddHours(7).Date < dateFrom && a.SewingOutDate.AddHours(7).Date > dateBalance && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Price : 0,

                                      //QtySewingRetur = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      //PriceSewingRetur = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      //QtySewingInTransfer = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      //PriceSewingInTransfer = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      WipSewingToPacking = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "PACKING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      WipSewingToPackingPrice = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "PACKING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      //WipFinishingOut = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      //WipFinishingOutPrice = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      QtySewingOut = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      PriceSewingOut = (a.SewingOutDate.AddHours(7).Date >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,

                                      Ro = a.RONo,


                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                  {
                                      QtyCuttingIn = 0,
                                      PriceCuttingIn = 0,
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingToPacking = 0,
                                      PriceCuttingToPacking = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtyLoadingAdjs = 0,
                                      PriceLoadingAdjs = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      //FinishingTransferExpenditure = group.Sum(x => x.FinishingTransferExpenditure),
                                      //FinishingTransferExpenditurePrice = group.Sum(x => x.FinishingTransferExpenditurePrice),
                                      //FinishingInTransferQty = group.Sum(x => x.FinishingInTransferQty),
                                      //FinishingInTransferPrice = group.Sum(x => x.FinishingInTransferPrice),
                                      //BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                      //BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
                                      BeginingBalanceSewingQty = group.Sum(x => x.BeginingBalanceSewingQty),
                                      BeginingBalanceSewingPrice = group.Sum(x => x.BeginingBalanceSewingPrice),

                                      //QtySewingRetur = group.Sum(x => x.QtySewingRetur),
                                      //PriceSewingRetur = group.Sum(x => x.PriceSewingRetur),
                                      //QtySewingInTransfer = group.Sum(x => x.QtySewingInTransfer),
                                      //PriceSewingInTransfer = group.Sum(x => x.PriceSewingInTransfer),
                                      WipSewingToPacking = group.Sum(x => x.WipSewingToPacking),
                                      WipSewingToPackingPrice = group.Sum(x => x.WipSewingToPackingPrice),
                                      //WipFinishingOut = group.Sum(x => x.WipFinishingOut),
                                      //WipFinishingOutPrice = group.Sum(x => x.WipFinishingOutPrice),
                                      QtySewingOut = group.Sum(x => x.QtySewingOut),
                                      PriceSewingOut = group.Sum(x => x.PriceSewingOut),
                                      //BeginingBalanceExpenditureGood = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Quantity : 0,
                                      //BeginingBalanceExpenditureGoodPrice = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Price : 0,
                                      BeginingBalanceExpenditureGood = 0,
                                      BeginingBalanceExpenditureGoodPrice = 0,
                                      Ro = key,
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      PackingInQty = 0,
                                      PackingInPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceCuttingQty = 0,
                                      BeginingBalanceCuttingPrice = 0,
                                      BeginingBalanceLoadingQty = 0,
                                      BeginingBalanceLoadingPrice = 0
                                  });

          

            //var QuerySewingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
            //                                 where aa.AdjustmentDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate.AddHours(7).Date <= dateTo && aa.AdjustmentType == "SEWING"
            //                                 select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
            //                      join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
            //                      select new
            //                      {

            //                          BeginingBalanceSewingQty = a.AdjustmentDate.AddHours(7).Date < dateFrom && a.AdjustmentDate.AddHours(7).Date > dateBalance ? -b.Quantity : 0,
            //                          BeginingBalanceSewingPrice = a.AdjustmentDate.AddHours(7).Date < dateFrom && a.AdjustmentDate.AddHours(7).Date > dateBalance ? -b.Price : 0,
            //                          Ro = a.RONo,
            //                          QtySewingAdj = a.AdjustmentDate.AddHours(7).Date >= dateFrom ? b.Quantity : 0,
            //                          PriceSewingAdj = a.AdjustmentDate.AddHours(7).Date >= dateFrom ? b.Price : 0
            //                      }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                      {
            //                          QtyCuttingIn = 0,
            //                          PriceCuttingIn = 0,
            //                          QtySewingIn = 0,
            //                          PriceSewingIn = 0,
            //                          QtyCuttingOut = 0,
            //                          PriceCuttingOut = 0,
            //                          QtyCuttingToPacking = 0,
            //                          PriceCuttingToPacking = 0,
            //                          QtyCuttingsubkon = 0,
            //                          PriceCuttingsubkon = 0,
            //                          AvalCutting = 0,
            //                          AvalCuttingPrice = 0,
            //                          AvalSewing = 0,
            //                          AvalSewingPrice = 0,
            //                          QtyLoading = 0,
            //                          PriceLoading = 0,
            //                          QtyLoadingAdjs = 0,
            //                          PriceLoadingAdjs = 0,
            //                          QtySewingOut = 0,
            //                          PriceSewingOut = 0,
            //                          WipSewingToPacking = 0,
            //                          WipSewingToPackingPrice = 0,
            //                          WipFinishingOut = 0,
            //                          WipFinishingOutPrice = 0,
            //                          QtySewingRetur = 0,
            //                          PriceSewingRetur = 0,
            //                          QtySewingInTransfer = 0,
            //                          PriceSewingInTransfer = 0,
            //                          FinishingInQty = 0,
            //                          FinishingInPrice = 0,
            //                          SubconInQty = 0,
            //                          SubconInPrice = 0,
            //                          FinishingAdjQty = 0,
            //                          FinishingAdjPrice = 0,
            //                          FinishingTransferExpenditure = 0,
            //                          FinishingTransferExpenditurePrice = 0,
            //                          FinishingInTransferQty = 0,
            //                          FinishingInTransferPrice = 0,
            //                          FinishingOutQty = 0,
            //                          FinishingOutPrice = 0,
            //                          FinishingReturQty = 0,
            //                          FinishingReturPrice = 0,
            //                          SubconOutQty = 0,
            //                          SubconOutPrice = 0,
            //                          BeginingBalanceSewingQty = group.Sum(x => x.BeginingBalanceSewingQty),
            //                          BeginingBalanceSewingPrice = group.Sum(x => x.BeginingBalanceSewingPrice),
            //                          Ro = key,
            //                          QtySewingAdj = group.Sum(x => x.QtySewingAdj),
            //                          PriceSewingAdj = group.Sum(x => x.PriceSewingAdj),
            //                          ExpenditureGoodRetur = 0,
            //                          ExpenditureGoodReturPrice = 0,
            //                          PackingInQty = 0,
            //                          PackingInPrice = 0,
            //                          SampleQty = 0,
            //                          SamplePrice = 0,
            //                          OtherQty = 0,
            //                          OtherPrice = 0,
            //                          QtyLoadingInTransfer = 0,
            //                          PriceLoadingInTransfer = 0,
            //                          ExpenditureGoodInTransfer = 0,
            //                          ExpenditureGoodInTransferPrice = 0,
            //                          BeginingBalanceCuttingQty = 0,
            //                          BeginingBalanceCuttingPrice = 0,
            //                          BeginingBalanceLoadingQty = 0,
            //                          BeginingBalanceLoadingPrice = 0,
            //                          BeginingBalanceFinishingQty = 0,
            //                          BeginingBalanceFinishingPrice = 0
            //                      });

           
            var QueryFinishingIn = (from a in (from aa in garmentFinishingInRepository.Query
                                               where aa.FinishingInDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingInDate.AddHours(7).Date <= dateTo
                                               select new { aa.RONo, aa.Identity, aa.FinishingInDate, aa.FinishingInType })
                                    join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                    select new
                                    {

                                        //BeginingBalanceSubconQty = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                        //BeginingBalanceSubconPrice = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                        BeginingBalanceFinishingQty = (a.FinishingInDate.AddHours(7).Date < dateFrom && a.FinishingInDate.AddHours(7).Date > dateBalance) ? b.Quantity : 0,
                                        BeginingBalanceFinishingPrice = (a.FinishingInDate.AddHours(7).Date < dateFrom && a.FinishingInDate.AddHours(7).Date > dateBalance) ? b.Price : 0,
                                        FinishingInQty = (a.FinishingInDate.AddHours(7).Date >= dateFrom && a.FinishingInType == "SEWING") ? b.Quantity : 0,
                                        FinishingInPrice = (a.FinishingInDate.AddHours(7).Date >= dateFrom && a.FinishingInType == "SEWING") ? b.Price : 0,
                                        //SubconInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                        //SubconInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                        Ro = a.RONo,

                                    }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                    {
                                        QtyCuttingIn = 0,
                                        PriceCuttingIn = 0,
                                        QtySewingIn = 0,
                                        PriceSewingIn = 0,
                                        QtyCuttingOut = 0,
                                        PriceCuttingOut = 0,
                                        QtyCuttingToPacking = 0,
                                        PriceCuttingToPacking = 0,
                                        QtyCuttingsubkon = 0,
                                        PriceCuttingsubkon = 0,
                                        AvalCutting = 0,
                                        AvalCuttingPrice = 0,
                                        AvalSewing = 0,
                                        AvalSewingPrice = 0,
                                        QtyLoading = 0,
                                        PriceLoading = 0,
                                        QtyLoadingAdjs = 0,
                                        PriceLoadingAdjs = 0,
                                        QtySewingOut = 0,
                                        PriceSewingOut = 0,
                                        QtySewingAdj = 0,
                                        PriceSewingAdj = 0,
                                        WipSewingToPacking = 0,
                                        WipSewingToPackingPrice = 0,
                                        WipFinishingOut = 0,
                                        WipFinishingOutPrice = 0,
                                        QtySewingRetur = 0,
                                        PriceSewingRetur = 0,
                                        QtySewingInTransfer = 0,
                                        PriceSewingInTransfer = 0,
                                        FinishingAdjQty = 0,
                                        FinishingAdjPrice = 0,
                                        FinishingTransferExpenditure = 0,
                                        FinishingTransferExpenditurePrice = 0,
                                        FinishingInTransferQty = 0,
                                        FinishingInTransferPrice = 0,
                                        FinishingOutQty = 0,
                                        FinishingOutPrice = 0,
                                        FinishingReturQty = 0,
                                        FinishingReturPrice = 0,
                                        SubconOutQty = 0,
                                        SubconOutPrice = 0,
                                        QtyLoadingInTransfer = 0,
                                        PriceLoadingInTransfer = 0,
                                        BeginingBalanceSubconQty =0,
                                        BeginingBalanceSubconPrice = 0,
                                        BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                        BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
                                        FinishingInQty = group.Sum(x => x.FinishingInQty),
                                        FinishingInPrice = group.Sum(x => x.FinishingInPrice),
                                        SubconInQty = 0,
                                        SubconInPrice = 0,
                                        Ro = key,
                                        ExpenditureGoodRetur = 0,
                                        ExpenditureGoodReturPrice = 0,
                                        PackingInQty = 0,
                                        PackingInPrice = 0,
                                        SampleQty = 0,
                                        SamplePrice = 0,
                                        OtherQty = 0,
                                        OtherPrice = 0,
                                        ExpenditureGoodInTransfer = 0,
                                        ExpenditureGoodInTransferPrice = 0,
                                        BeginingBalanceCuttingQty = 0,
                                        BeginingBalanceCuttingPrice = 0,
                                        BeginingBalanceLoadingQty = 0,
                                        BeginingBalanceLoadingPrice = 0
                                    });
            
          
            var QueryFinishingOut = (from a in (from aa in garmentFinishingOutRepository.Query
                                                where aa.FinishingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate.AddHours(7).Date <= dateTo 
                                                select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
                                     join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                     join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                     join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                     select new
                                     {

                                         BeginingBalanceFinishingQty = (a.FinishingOutDate.AddHours(7).Date < dateFrom && a.FinishingOutDate.AddHours(7).Date > dateBalance ) ? -b.Quantity : 0,
                                         BeginingBalanceFinishingPrice = (a.FinishingOutDate.AddHours(7).Date < dateFrom && a.FinishingOutDate.AddHours(7).Date > dateBalance ) ? -b.Price : 0,
                                         //BeginingBalanceExpenditureGood = ((a.FinishingOutDate.AddHours(7).Date < dateFrom && a.FinishingOutDate.AddHours(7).Date > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0) + ((a.FinishingOutDate.AddHours(7).Date < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0),
                                         //BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate.AddHours(7).Date < dateFrom && a.FinishingOutDate.AddHours(7).Date > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Price : 0 + ((a.FinishingOutDate.AddHours(7).Date < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0),
                                         //BeginingBalanceSubconQty = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Quantity : 0,
                                         //BeginingBalanceSubconPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Price : 0,

                                         FinishingOutQty = (a.FinishingOutDate.AddHours(7).Date >= dateFrom && a.FinishingTo == "PACKING") ? b.Quantity : 0,
                                         FinishingOutPrice = (a.FinishingOutDate.AddHours(7).Date >= dateFrom && a.FinishingTo == "PACKING") ? b.Price : 0,
                                         //SubconOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                         //SubconOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                         Ro = a.RONo,
                                         FinishingOutAval = (a.FinishingOutDate.AddHours(7).Date >= dateFrom && a.FinishingTo == "AVAL") ? b.Quantity : 0,
                                         FinishingOutAvalPrice = (a.FinishingOutDate.AddHours(7).Date >= dateFrom && a.FinishingTo == "AVAL") ? b.Price : 0,
                                     }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                     {
                                         QtyCuttingIn = 0,
                                         PriceCuttingIn = 0,
                                         QtySewingIn = 0,
                                         PriceSewingIn = 0,
                                         QtyCuttingOut = 0,
                                         PriceCuttingOut = 0,
                                         QtyCuttingToPacking = 0,
                                         PriceCuttingToPacking = 0,
                                         QtyCuttingsubkon = 0,
                                         PriceCuttingsubkon = 0,
                                         AvalCutting = 0,
                                         AvalCuttingPrice = 0,
                                         AvalSewing = 0,
                                         AvalSewingPrice = 0,
                                         QtyLoading = 0,
                                         PriceLoading = 0,
                                         QtyLoadingAdjs = 0,
                                         PriceLoadingAdjs = 0,
                                         QtySewingOut = 0,
                                         PriceSewingOut = 0,
                                         QtySewingAdj = 0,
                                         PriceSewingAdj = 0,
                                         WipSewingToPacking = 0,
                                         WipSewingToPackingPrice = 0,
                                         WipFinishingOut = 0,
                                         WipFinishingOutPrice = 0,
                                         QtySewingRetur = 0,
                                         PriceSewingRetur = 0,
                                         QtySewingInTransfer = 0,
                                         PriceSewingInTransfer = 0,
                                         FinishingInQty = 0,
                                         FinishingInPrice = 0,
                                         SubconInQty = 0,
                                         SubconInPrice = 0,
                                         FinishingAdjQty = 0,
                                         FinishingAdjPrice = 0,
                                         FinishingTransferExpenditure = 0,
                                         FinishingTransferExpenditurePrice = 0,
                                         FinishingInTransferQty = 0,
                                         FinishingInTransferPrice = 0,
                                         FinishingReturQty = 0,
                                         FinishingReturPrice = 0,
                                         QtyLoadingInTransfer = 0,
                                         PriceLoadingInTransfer = 0,
                                         BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                         BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
                                         //BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                         //BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                                         BeginingBalanceSubconQty =0,
                                         BeginingBalanceSubconPrice = 0,
                                         FinishingOutQty = group.Sum(x => x.FinishingOutQty),
                                         FinishingOutPrice = group.Sum(x => x.FinishingOutPrice),
                                         FinishingOutAvalQty = group.Sum(x => x.FinishingOutAval),
                                         FinishingOutAvalPrice = group.Sum(x => x.FinishingOutAvalPrice),
                                         SubconOutQty = 0,
                                         SubconOutPrice = 0,
                                         Ro = key,
                                         ExpenditureGoodRetur = 0,
                                         ExpenditureGoodReturPrice = 0,
                                         PackingInQty = 0,
                                         PackingInPrice = 0,
                                         SampleQty = 0,
                                         SamplePrice = 0,
                                         OtherQty = 0,
                                         OtherPrice = 0,
                                         ExpenditureGoodInTransfer = 0,
                                         ExpenditureGoodInTransferPrice = 0,
                                         BeginingBalanceCuttingQty = 0,
                                         BeginingBalanceCuttingPrice = 0,
                                         BeginingBalanceLoadingQty = 0,
                                         BeginingBalanceLoadingPrice = 0
                                     });
           
            //var QueryExpenditureGoodInTransfer = (from a in (from aa in garmentFinishingOutRepository.Query
            //                                                 where aa.FinishingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId != aa.UnitToId && aa.FinishingOutDate.AddHours(7).Date <= dateTo && aa.FinishingTo == "GUDANG JADI" && aa.UnitToId == (request.unit == 0 ? aa.UnitToId : request.unit)
            //                                                 select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
            //                                      join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
            //                                      join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
            //                                      join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
            //                                      select new
            //                                      {

            //                                          Ro = a.RONo,
            //                                          ExpenditureGoodInTransfer = (a.FinishingOutDate.AddHours(7).Date >= dateFrom) ? b.Quantity : 0,
            //                                          ExpenditureGoodInTransferPrice = (a.FinishingOutDate.AddHours(7).Date >= dateFrom) ? b.Price : 0,
            //                                          BeginingBalanceExpenditureGood = (a.FinishingOutDate.AddHours(7).Date < dateFrom && a.FinishingOutDate.AddHours(7).Date > dateBalance) ? b.Quantity : 0,
            //                                          BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate.AddHours(7).Date < dateFrom && a.FinishingOutDate.AddHours(7).Date > dateBalance) ? b.Price : 0,

            //                                      }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                                      {
            //                                          QtyCuttingIn = 0,
            //                                          PriceCuttingIn = 0,
            //                                          QtySewingIn = 0,
            //                                          PriceSewingIn = 0,
            //                                          QtyCuttingOut = 0,
            //                                          PriceCuttingOut = 0,
            //                                          QtyCuttingToPacking = 0,
            //                                          PriceCuttingToPacking = 0,
            //                                          QtyCuttingsubkon = 0,
            //                                          PriceCuttingsubkon = 0,
            //                                          AvalCutting = 0,
            //                                          AvalCuttingPrice = 0,
            //                                          AvalSewing = 0,
            //                                          AvalSewingPrice = 0,
            //                                          QtyLoading = 0,
            //                                          PriceLoading = 0,
            //                                          QtyLoadingAdjs = 0,
            //                                          PriceLoadingAdjs = 0,
            //                                          QtySewingOut = 0,
            //                                          PriceSewingOut = 0,
            //                                          QtySewingAdj = 0,
            //                                          PriceSewingAdj = 0,
            //                                          WipSewingToPacking = 0,
            //                                          WipSewingToPackingPrice = 0,
            //                                          WipFinishingOut = 0,
            //                                          WipFinishingOutPrice = 0,
            //                                          QtySewingRetur = 0,
            //                                          PriceSewingRetur = 0,
            //                                          QtySewingInTransfer = 0,
            //                                          PriceSewingInTransfer = 0,
            //                                          FinishingInQty = 0,
            //                                          FinishingInPrice = 0,
            //                                          SubconInQty = 0,
            //                                          SubconInPrice = 0,
            //                                          FinishingAdjQty = 0,
            //                                          FinishingAdjPrice = 0,
            //                                          FinishingTransferExpenditure = 0,
            //                                          FinishingTransferExpenditurePrice = 0,
            //                                          FinishingInTransferQty = 0,
            //                                          FinishingInTransferPrice = 0,
            //                                          FinishingReturQty = 0,
            //                                          FinishingReturPrice = 0,
            //                                          BeginingBalanceFinishingQty = 0,
            //                                          BeginingBalanceFinishingPrice = 0,
            //                                          FinishingOutQty = 0,
            //                                          FinishingOutPrice = 0,
            //                                          SubconOutQty = 0,
            //                                          SubconOutPrice = 0,
            //                                          Ro = key,
            //                                          ExpenditureGoodRetur = 0,
            //                                          ExpenditureGoodReturPrice = 0,
            //                                          PackingInQty = 0,
            //                                          PackingInPrice = 0,
            //                                          SampleQty = 0,
            //                                          SamplePrice = 0,
            //                                          OtherQty = 0,
            //                                          OtherPrice = 0,
            //                                          QtyLoadingInTransfer = 0,
            //                                          PriceLoadingInTransfer = 0,
            //                                          ExpenditureGoodInTransfer = group.Sum(x => x.ExpenditureGoodInTransfer),
            //                                          ExpenditureGoodInTransferPrice = group.Sum(x => x.ExpenditureGoodInTransferPrice),
            //                                          BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
            //                                          BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
            //                                          BeginingBalanceCuttingQty = 0,
            //                                          BeginingBalanceCuttingPrice = 0,
            //                                          BeginingBalanceLoadingQty = 0,
            //                                          BeginingBalanceLoadingPrice = 0
            //                                      });

            //var QueryFinishingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
            //                                    where aa.AdjustmentDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate.AddHours(7).Date <= dateTo && aa.AdjustmentType == "FINISHING"
            //                                    select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
            //                         join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
            //                         select new
            //                         {
            //                             BeginingBalanceFinishingQty = a.AdjustmentDate.AddHours(7).Date < dateFrom && a.AdjustmentDate.AddHours(7).Date > dateBalance ? -b.Quantity : 0,
            //                             BeginingBalanceFinishingPrice = a.AdjustmentDate.AddHours(7).Date < dateFrom && a.AdjustmentDate.AddHours(7).Date > dateBalance ? -b.Price : 0,
            //                             FinishingAdjQty = a.AdjustmentDate.AddHours(7).Date >= dateFrom ? b.Quantity : 0,
            //                             FinishingAdjPrice = a.AdjustmentDate.AddHours(7).Date >= dateFrom ? b.Price : 0,
            //                             Ro = a.RONo
            //                         }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                         {
            //                             QtyCuttingIn = 0,
            //                             PriceCuttingIn = 0,
            //                             QtySewingIn = 0,
            //                             PriceSewingIn = 0,
            //                             QtyCuttingOut = 0,
            //                             PriceCuttingOut = 0,
            //                             QtyCuttingToPacking = 0,
            //                             PriceCuttingToPacking = 0,
            //                             QtyCuttingsubkon = 0,
            //                             PriceCuttingsubkon = 0,
            //                             AvalCutting = 0,
            //                             AvalCuttingPrice = 0,
            //                             AvalSewing = 0,
            //                             AvalSewingPrice = 0,
            //                             QtyLoading = 0,
            //                             PriceLoading = 0,
            //                             QtyLoadingAdjs = 0,
            //                             PriceLoadingAdjs = 0,
            //                             QtySewingOut = 0,
            //                             PriceSewingOut = 0,
            //                             QtySewingAdj = 0,
            //                             PriceSewingAdj = 0,
            //                             WipSewingToPacking = 0,
            //                             WipSewingToPackingPrice = 0,
            //                             WipFinishingOut = 0,
            //                             WipFinishingOutPrice = 0,
            //                             QtySewingRetur = 0,
            //                             PriceSewingRetur = 0,
            //                             QtySewingInTransfer = 0,
            //                             PriceSewingInTransfer = 0,
            //                             FinishingInQty = 0,
            //                             FinishingInPrice = 0,
            //                             SubconInQty = 0,
            //                             SubconInPrice = 0,
            //                             FinishingTransferExpenditure = 0,
            //                             FinishingTransferExpenditurePrice = 0,
            //                             FinishingInTransferQty = 0,
            //                             FinishingInTransferPrice = 0,
            //                             QtyLoadingInTransfer = 0,
            //                             PriceLoadingInTransfer = 0,
            //                             BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
            //                             BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
            //                             FinishingAdjQty = group.Sum(x => x.FinishingAdjQty),
            //                             FinishingAdjPrice = group.Sum(x => x.FinishingAdjPrice),
            //                             FinishingOutQty = 0,
            //                             FinishingOutPrice = 0,
            //                             FinishingReturQty = 0,
            //                             FinishingReturPrice = 0,
            //                             SubconOutQty = 0,
            //                             SubconOutPrice = 0,
            //                             Ro = key,
            //                             ExpenditureGoodRetur = 0,
            //                             ExpenditureGoodReturPrice = 0,
            //                             PackingInQty = 0,
            //                             PackingInPrice = 0,
            //                             SampleQty = 0,
            //                             SamplePrice = 0,
            //                             OtherQty = 0,
            //                             OtherPrice = 0,
            //                             ExpenditureGoodInTransfer = 0,
            //                             ExpenditureGoodInTransferPrice = 0,
            //                             BeginingBalanceCuttingQty = 0,
            //                             BeginingBalanceCuttingPrice = 0,
            //                             BeginingBalanceLoadingQty = 0,
            //                             BeginingBalanceLoadingPrice = 0
            //                         });


            //var QueryFinishingRetur = (from a in (from aa in garmentFinishingOutRepository.Query
            //                                      where aa.FinishingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate.AddHours(7).Date <= dateTo && aa.FinishingTo == "SEWING"
            //                                      select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo, aa.UnitId, aa.UnitToId })
            //                           join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
            //                           join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
            //                           join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
            //                           select new
            //                           {

            //                               BeginingBalanceFinishingQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate.AddHours(7).Date < dateFrom && a.FinishingOutDate.AddHours(7).Date > dateBalance && a.UnitId == a.UnitToId) ? -b.Quantity : 0,
            //                               BeginingBalanceFinishingPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate.AddHours(7).Date < dateFrom && a.FinishingOutDate.AddHours(7).Date > dateBalance && a.UnitId == a.UnitToId) ? -b.Price : 0,
            //                               FinishingReturQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate.AddHours(7).Date >= dateFrom && a.UnitToId == a.UnitToId) ? b.Quantity : 0,
            //                               FinishingReturPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate.AddHours(7).Date >= dateFrom && a.UnitToId == a.UnitToId) ? b.Price : 0,
            //                               Ro = a.RONo,

            //                           }).GroupBy(x => x.Ro, (key, group) => new monitoringView
            //                           {
            //                               QtyCuttingIn = 0,
            //                               PriceCuttingIn = 0,
            //                               QtySewingIn = 0,
            //                               PriceSewingIn = 0,
            //                               QtyCuttingOut = 0,
            //                               PriceCuttingOut = 0,
            //                               QtyCuttingToPacking = 0,
            //                               PriceCuttingToPacking = 0,
            //                               QtyCuttingsubkon = 0,
            //                               PriceCuttingsubkon = 0,
            //                               AvalCutting = 0,
            //                               AvalCuttingPrice = 0,
            //                               AvalSewing = 0,
            //                               AvalSewingPrice = 0,
            //                               QtyLoading = 0,
            //                               PriceLoading = 0,
            //                               QtyLoadingAdjs = 0,
            //                               PriceLoadingAdjs = 0,
            //                               QtySewingOut = 0,
            //                               PriceSewingOut = 0,
            //                               QtySewingAdj = 0,
            //                               PriceSewingAdj = 0,
            //                               WipSewingToPacking = 0,
            //                               WipSewingToPackingPrice = 0,
            //                               WipFinishingOut = 0,
            //                               WipFinishingOutPrice = 0,
            //                               QtySewingRetur = 0,
            //                               PriceSewingRetur = 0,
            //                               QtySewingInTransfer = 0,
            //                               PriceSewingInTransfer = 0,
            //                               FinishingInQty = 0,
            //                               FinishingInPrice = 0,
            //                               SubconInQty = 0,
            //                               SubconInPrice = 0,
            //                               FinishingAdjQty = 0,
            //                               FinishingAdjPrice = 0,
            //                               FinishingTransferExpenditure = 0,
            //                               FinishingTransferExpenditurePrice = 0,
            //                               FinishingInTransferQty = 0,
            //                               FinishingInTransferPrice = 0,
            //                               FinishingOutQty = 0,
            //                               FinishingOutPrice = 0,
            //                               SubconOutQty = 0,
            //                               QtyLoadingInTransfer = 0,
            //                               PriceLoadingInTransfer = 0,
            //                               SubconOutPrice = 0,
            //                               BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
            //                               BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
            //                               FinishingReturQty = group.Sum(x => x.FinishingReturQty),
            //                               FinishingReturPrice = group.Sum(x => x.FinishingReturPrice),
            //                               Ro = key,
            //                               ExpenditureGoodRetur = 0,
            //                               ExpenditureGoodReturPrice = 0,
            //                               PackingInQty = 0,
            //                               PackingInPrice = 0,
            //                               SampleQty = 0,
            //                               SamplePrice = 0,
            //                               OtherQty = 0,
            //                               OtherPrice = 0,
            //                               ExpenditureGoodInTransfer = 0,
            //                               ExpenditureGoodInTransferPrice = 0,
            //                               BeginingBalanceCuttingQty = 0,
            //                               BeginingBalanceCuttingPrice = 0,
            //                               BeginingBalanceLoadingQty = 0,
            //                               BeginingBalanceLoadingPrice = 0
            //                           });
           

            var QueryPackingIn = (from a in (from aa in garmentSubconPackingInRepository.Query
                                                    where aa.PackingInDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.PackingInDate.AddHours(7).Date <= dateTo
                                                    select new { aa.RONo, aa.Identity, aa.PackingInDate})
                                         join b in garmentSubconPackingInItemRepository.Query on a.Identity equals b.PackingInId
                                         select new
                                         {

                                             BeginingBalanceExpenditureGood = a.PackingInDate.AddHours(7).Date < dateFrom && a.PackingInDate.AddHours(7).Date > dateBalance ? -b.Quantity : 0,
                                             BeginingBalanceExpenditureGoodPrice = a.PackingInDate.AddHours(7).Date < dateFrom && a.PackingInDate.AddHours(7).Date > dateBalance ? -b.Price : 0,
                                             PackingInQty = (a.PackingInDate.AddHours(7).Date >= dateFrom ) ? b.Quantity : 0,
                                             PackingInPrice = (a.PackingInDate.AddHours(7).Date >= dateFrom ) ? b.Price : 0,
                                           
                                             Ro = a.RONo,

                                         }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                         {
                                             QtyCuttingIn = 0,
                                             PriceCuttingIn = 0,
                                             QtySewingIn = 0,
                                             PriceSewingIn = 0,
                                             QtyCuttingOut = 0,
                                             PriceCuttingOut = 0,
                                             QtyCuttingToPacking = 0,
                                             PriceCuttingToPacking = 0,
                                             QtyCuttingsubkon = 0,
                                             PriceCuttingsubkon = 0,
                                             QtyLoadingInTransfer = 0,
                                             PriceLoadingInTransfer = 0,
                                             AvalCutting = 0,
                                             AvalCuttingPrice = 0,
                                             AvalSewing = 0,
                                             AvalSewingPrice = 0,
                                             QtyLoading = 0,
                                             PriceLoading = 0,
                                             QtyLoadingAdjs = 0,
                                             PriceLoadingAdjs = 0,
                                             QtySewingOut = 0,
                                             PriceSewingOut = 0,
                                             QtySewingAdj = 0,
                                             PriceSewingAdj = 0,
                                             WipSewingToPacking = 0,
                                             WipSewingToPackingPrice = 0,
                                             WipFinishingOut = 0,
                                             WipFinishingOutPrice = 0,
                                             QtySewingRetur = 0,
                                             PriceSewingRetur = 0,
                                             QtySewingInTransfer = 0,
                                             PriceSewingInTransfer = 0,
                                             FinishingInQty = 0,
                                             FinishingInPrice = 0,
                                             SubconInQty = 0,
                                             SubconInPrice = 0,
                                             FinishingAdjQty = 0,
                                             FinishingAdjPrice = 0,
                                             FinishingTransferExpenditure = 0,
                                             FinishingTransferExpenditurePrice = 0,
                                             FinishingInTransferQty = 0,
                                             FinishingInTransferPrice = 0,
                                             FinishingOutQty = 0,
                                             FinishingOutPrice = 0,
                                             FinishingReturQty = 0,
                                             FinishingReturPrice = 0,
                                             SubconOutQty = 0,
                                             SubconOutPrice = 0,
                                             BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                             BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                                             PackingInQty = group.Sum(x => x.PackingInQty),
                                             PackingInPrice = group.Sum(x => x.PackingInPrice),
                                             //SampleQty = group.Sum(x => x.SampleQty),
                                             //SamplePrice = group.Sum(x => x.SamplePrice),
                                             //OtherQty = group.Sum(x => x.OtherQty),
                                             //OtherPrice = group.Sum(x => x.OtherPrice),
                                             Ro = key,
                                             ExpenditureGoodRetur = 0,
                                             ExpenditureGoodReturPrice = 0,
                                             ExpenditureGoodInTransfer = 0,
                                             ExpenditureGoodInTransferPrice = 0,
                                             BeginingBalanceCuttingQty = 0,
                                             BeginingBalanceCuttingPrice = 0,
                                             BeginingBalanceLoadingQty = 0,
                                             BeginingBalanceLoadingPrice = 0,
                                             BeginingBalanceFinishingQty = 0,
                                             BeginingBalanceFinishingPrice = 0
                                         });

            var QueryPackingOut = (from a in (from aa in garmentSubconPackingOutRepository.Query
                                             where aa.PackingOutDate.AddHours(7).Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.PackingOutDate.AddHours(7).Date <= dateTo
                                             select new { aa.RONo, aa.Identity, aa.PackingOutDate })
                                  join b in garmentSubconPackingOutItemRepository.Query on a.Identity equals b.PackingOutId
                                  select new
                                  {

                                      BeginingBalanceExpenditureGood = a.PackingOutDate.AddHours(7).Date < dateFrom && a.PackingOutDate.AddHours(7).Date > dateBalance ? -b.Quantity : 0,
                                      BeginingBalanceExpenditureGoodPrice = a.PackingOutDate.AddHours(7).Date < dateFrom && a.PackingOutDate.AddHours(7).Date > dateBalance ? -b.Price : 0,
                                      PackingOutQty = (a.PackingOutDate.AddHours(7).Date >= dateFrom) ? b.Quantity : 0,
                                      PackingOutPrice = (a.PackingOutDate.AddHours(7).Date >= dateFrom) ? b.Price : 0,

                                      Ro = a.RONo,

                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                  {
                                      QtyCuttingIn = 0,
                                      PriceCuttingIn = 0,
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingToPacking = 0,
                                      PriceCuttingToPacking = 0,
                                      QtyCuttingsubkon = 0,
                                      PriceCuttingsubkon = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtyLoadingAdjs = 0,
                                      PriceLoadingAdjs = 0,
                                      QtySewingOut = 0,
                                      PriceSewingOut = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      WipSewingToPacking = 0,
                                      WipSewingToPackingPrice = 0,
                                      WipFinishingOut = 0,
                                      WipFinishingOutPrice = 0,
                                      QtySewingRetur = 0,
                                      PriceSewingRetur = 0,
                                      QtySewingInTransfer = 0,
                                      PriceSewingInTransfer = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingTransferExpenditure = 0,
                                      FinishingTransferExpenditurePrice = 0,
                                      FinishingInTransferQty = 0,
                                      FinishingInTransferPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                      BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                                      PackingInQty = 0,
                                      PackingInPrice = 0,
                                      PackingOutQty = group.Sum(x => x.PackingOutQty),
                                      PackingOutPrice = group.Sum(x => x.PackingOutPrice),
                                      //SampleQty = group.Sum(x => x.SampleQty),
                                      //SamplePrice = group.Sum(x => x.SamplePrice),
                                      //OtherQty = group.Sum(x => x.OtherQty),
                                      //OtherPrice = group.Sum(x => x.OtherPrice),
                                      Ro = key,
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceCuttingQty = 0,
                                      BeginingBalanceCuttingPrice = 0,
                                      BeginingBalanceLoadingQty = 0,
                                      BeginingBalanceLoadingPrice = 0,
                                      BeginingBalanceFinishingQty = 0,
                                      BeginingBalanceFinishingPrice = 0
                                  });
           
           
            var queryNow = QueryCuttingIn
                //.Union(queryBalance)
                .Union(QueryCuttingOut)
                //.Union(QueryCuttingOutSubkon)
                //.Union(QueryCuttingOutTransfer)
                //.Union(QueryAvalCompCutting)
                //.Union(QueryAvalCompSewing)
                //.Union(QuerySewingDO)
                .Union(QueryLoadingIn)
                .Union(QueryLoadingOut)
                .Union(QuerySewingIn)
                .Union(QuerySewingOut)
                //.Union(QuerySewingAdj)
                .Union(QueryFinishingIn)
                .Union(QueryFinishingOut)
                  //.Union(QueryFinishingAdj)
                  //.Union(QueryFinishingRetur)
                  //.Union(QueryExpenditureGoods)
                  //.Union(QueryExpenditureGoodsAdj)
                  //.Union(QueryExpenditureGoodRetur)
                  //.Union(QueryExpenditureGoodInTransfer)
                  //.Union(QueryLoadingInTransfer)
                .Union(QueryPackingIn)
                .Union(QueryPackingOut)
                .AsEnumerable();

            var querySumAwal = (from a in queryNow
                                    //join b in queryGroup on a.Ro equals b.Ro
                                join cutt in queryGroup on a.Ro equals cutt.Ro into res
                                from b in res.DefaultIfEmpty()
                                select new
                                {
                                Article = b != null ? b.Article : "",
                                Comodity = b != null ? b.Comodity : "",
                                FC = b != null ? b.FC : 0,
                                BasicPrice = b != null ? b.BasicPrice : 0,
                                Fare = b != null ? b.Fare : 0,
                                FareNew = b != null ? b.FareNew : 0,
                                a.Ro,
                                a.BeginingBalanceCuttingQty,
                                a.BeginingBalanceCuttingPrice,
                                a.QtyCuttingIn,
                                a.PriceCuttingIn,
                                a.QtyCuttingOut,
                                a.PriceCuttingOut,
                                a.QtyCuttingToPacking,
                                a.PriceCuttingToPacking,
                                a.QtyCuttingsubkon,
                                a.PriceCuttingsubkon,
                                a.AvalCutting,
                                a.AvalCuttingPrice,
                                a.AvalSewing,
                                a.AvalSewingPrice,
                                a.BeginingBalanceLoadingQty,
                                a.BeginingBalanceLoadingPrice,
                                a.QtyLoadingIn,
                                a.PriceLoadingIn,
                                a.QtyLoading,
                                a.PriceLoading,
                                a.QtyLoadingAdjs,
                                a.PriceLoadingAdjs,
                                a.BeginingBalanceSewingQty,
                                a.BeginingBalanceSewingPrice,
                                a.QtySewingIn,
                                a.PriceSewingIn,
                                a.QtySewingOut,
                                a.PriceSewingOut,
                                a.QtySewingInTransfer,
                                a.PriceSewingInTransfer,
                                a.WipSewingToPacking,
                                a.WipSewingToPackingPrice,
                                a.WipFinishingOut,
                                a.WipFinishingOutPrice,
                                a.QtySewingRetur,
                                a.PriceSewingRetur,
                                a.QtySewingAdj,
                                a.PriceSewingAdj,
                                a.BeginingBalanceFinishingQty,
                                a.BeginingBalanceFinishingPrice,
                                a.FinishingInQty,
                                a.FinishingInPrice,
                                a.BeginingBalanceSubconQty,
                                a.BeginingBalanceSubconPrice,
                                a.SubconInQty,
                                a.SubconInPrice,
                                a.SubconOutQty,
                                a.SubconOutPrice,
                                a.FinishingOutQty,
                                a.FinishingOutPrice,
                                a.FinishingOutAvalQty,
                                a.FinishingOutAvalPrice,
                                a.FinishingInTransferQty,
                                a.FinishingInTransferPrice,
                                a.FinishingAdjQty,
                                a.FinishingAdjPrice,
                                a.FinishingReturQty,
                                a.FinishingReturPrice,
                                a.BeginingBalanceExpenditureGood,
                                a.BeginingBalanceExpenditureGoodPrice,
                                a.ExpenditureGoodRetur,
                                a.ExpenditureGoodReturPrice,
                                a.PackingInQty,
                                a.PackingInPrice,
                                a.PackingOutQty,
                                a.PackingOutPrice,
                                a.OtherQty,
                                a.OtherPrice,
                                a.SampleQty,
                                a.SamplePrice,
                                a.ExpenditureGoodAdj,
                                a.ExpenditureGoodAdjPrice,
                                a.ExpenditureGoodInTransfer,
                                a.ExpenditureGoodInTransferPrice,
                                a.QtyLoadingInTransfer,
                                a.PriceLoadingInTransfer
                            })
                .GroupBy(x => new { x.FareNew, x.Fare, x.BasicPrice, x.FC, x.Ro, x.Article, x.Comodity }, (key, group) => new monitoringUnionView
                {
                    ro = key.Ro,
                    article = key.Article,
                    comodity = key.Comodity,
                    fc = key.FC,
                    fare = key.Fare,
                    farenew = key.FareNew,
                    basicprice = key.BasicPrice,
                    qtycutting = group.Sum(s => s.QtyCuttingOut),
                    priceCuttingOut = group.Sum(s => s.PriceCuttingOut),
                    qtCuttingSubkon = group.Sum(s => s.QtyCuttingsubkon),
                    priceCuttingSubkon = group.Sum(s => s.PriceCuttingsubkon),
                    QtyCuttingToPacking = group.Sum(s => s.QtyCuttingToPacking),
                    PriceCuttingToPacking = group.Sum(s => s.PriceCuttingToPacking),
                    qtyCuttingIn = group.Sum(s => s.QtyCuttingIn),
                    priceCuttingIn = group.Sum(s => s.PriceCuttingIn),
                    begining = group.Sum(s => s.BeginingBalanceCuttingQty),
                    beginingcuttingPrice = group.Sum(s => s.BeginingBalanceCuttingPrice),
                    qtyavalsew = group.Sum(s => s.AvalSewing),
                    priceavalsew = group.Sum(s => s.AvalSewingPrice),
                    qtyavalcut = group.Sum(s => s.AvalCutting),
                    priceavalcut = group.Sum(s => s.AvalCuttingPrice),
                    beginingloading = group.Sum(s => s.BeginingBalanceLoadingQty),
                    beginingloadingPrice = group.Sum(s => s.BeginingBalanceLoadingPrice),
                    qtyLoadingIn = group.Sum(s => s.QtyLoadingIn),
                    priceLoadingIn = group.Sum(s => s.PriceLoadingIn),
                    qtyloading = group.Sum(s => s.QtyLoading),
                    priceloading = group.Sum(s => s.PriceLoading),
                    qtyLoadingAdj = group.Sum(s => s.QtyLoadingAdjs),
                    priceLoadingAdj = group.Sum(s => s.PriceLoadingAdjs),
                    beginingSewing = group.Sum(s => s.BeginingBalanceSewingQty),
                    beginingSewingPrice = group.Sum(s => s.BeginingBalanceSewingPrice),
                    sewingIn = group.Sum(s => s.QtySewingIn),
                    sewingInPrice = group.Sum(s => s.PriceSewingIn),
                    sewingintransfer = group.Sum(s => s.QtySewingInTransfer),
                    sewingintransferPrice = group.Sum(s => s.PriceSewingInTransfer),
                    sewingout = group.Sum(s => s.QtySewingOut),
                    sewingoutPrice = group.Sum(s => s.PriceSewingOut),
                    sewingretur = group.Sum(s => s.QtySewingRetur),
                    sewingreturPrice = group.Sum(s => s.PriceSewingRetur),
                    WipSewingToPacking = group.Sum(s => s.WipSewingToPacking),
                    WipSewingToPackingPrice = group.Sum(s => s.WipSewingToPackingPrice),
                    wipfinishing = group.Sum(s => s.WipFinishingOut),
                    wipfinishingPrice = group.Sum(s => s.WipFinishingOutPrice),
                    sewingadj = group.Sum(s => s.QtySewingAdj),
                    sewingadjPrice = group.Sum(s => s.PriceSewingAdj),
                    finishingin = group.Sum(s => s.FinishingInQty),
                    finishinginPrice = group.Sum(s => s.FinishingInPrice),
                    finishingintransfer = group.Sum(s => s.FinishingInTransferQty),
                    finishingintransferPrice = group.Sum(s => s.FinishingInTransferPrice),
                    finishingadj = group.Sum(s => s.FinishingAdjQty),
                    finishingadjPrice = group.Sum(s => s.FinishingAdjPrice),
                    finishingout = group.Sum(s => s.FinishingOutQty),
                    finishingoutPrice = group.Sum(s => s.FinishingOutPrice),
                    finishingoutaval = group.Sum(s => s.FinishingOutAvalQty),
                    finishingoutavalPrice = group.Sum(s => s.FinishingOutAvalPrice),
                    finishinigretur = group.Sum(s => s.FinishingReturQty),
                    finishinigreturPrice = group.Sum(s => s.FinishingReturPrice),
                    beginingbalanceFinishing = group.Sum(s => s.BeginingBalanceFinishingQty),
                    beginingbalanceFinishingPrice = group.Sum(s => s.BeginingBalanceFinishingPrice),
                    beginingbalancesubcon = group.Sum(s => s.BeginingBalanceSubconQty),
                    beginingbalancesubconPrice = group.Sum(s => s.BeginingBalanceSubconPrice),
                    subconIn = group.Sum(s => s.SubconInQty),
                    subconInPrice = group.Sum(s => s.SubconInPrice),
                    subconout = group.Sum(s => s.SubconOutQty),
                    subconoutPrice = group.Sum(s => s.SubconOutPrice),
                    packingInQty = group.Sum(s => s.PackingInQty),
                    packingInPrice = group.Sum(s => s.PackingInPrice),
                    packingOutQty = group.Sum(s => s.PackingOutQty),
                    packingOutPrice = group.Sum(s => s.PackingOutPrice),
                    otherqty = group.Sum(s => s.OtherQty),
                    otherprice = group.Sum(s => s.OtherPrice),
                    sampleQty = group.Sum(s => s.SampleQty),
                    samplePrice = group.Sum(s => s.SamplePrice),
                    expendAdj = group.Sum(s => s.ExpenditureGoodAdj),
                    expendAdjPrice = group.Sum(s => s.ExpenditureGoodAdjPrice),
                    expendRetur = group.Sum(s => s.ExpenditureGoodRetur),
                    expendReturPrice = group.Sum(s => s.ExpenditureGoodReturPrice),
                    //finishinginqty =group.Sum(s=>s.FinishingInQty)
                    beginingBalanceExpenditureGood = group.Sum(s => s.BeginingBalanceExpenditureGood),
                    beginingBalanceExpenditureGoodPrice = group.Sum(s => s.BeginingBalanceExpenditureGoodPrice),
                    expenditureInTransfer = group.Sum(s => s.ExpenditureGoodInTransfer),
                    expenditureInTransferPrice = group.Sum(s => s.ExpenditureGoodInTransferPrice),
                    qtyloadingInTransfer = group.Sum(s => s.QtyLoadingInTransfer),
                    priceloadingInTransfer = group.Sum(s => s.PriceLoadingInTransfer)



                }).AsEnumerable();

            var querySum = querySumAwal.GroupBy(x => new { x.farenew, x.fare, x.basicprice, x.fc, x.ro, x.article, x.comodity }, (key, group) => new monitoringUnionView
            {
                ro = key.ro,
                article = key.article,
                comodity = key.comodity,
                fc = key.fc,
                fare = key.fare,
                farenew = key.farenew,
                basicprice = key.basicprice,
                qtycutting = group.Sum(s => s.qtycutting),
                priceCuttingOut = group.Sum(s => s.priceCuttingOut),
                qtCuttingSubkon = group.Sum(s => s.qtCuttingSubkon),
                priceCuttingSubkon = group.Sum(s => s.priceCuttingSubkon),
                QtyCuttingToPacking = group.Sum(s => s.QtyCuttingToPacking),
                PriceCuttingToPacking = group.Sum(s => s.PriceCuttingToPacking),
                qtyCuttingIn = group.Sum(s => s.qtyCuttingIn),
                priceCuttingIn = group.Sum(s => s.priceCuttingIn),
                begining = group.Sum(s => s.begining),
                beginingcuttingPrice = group.Sum(s => s.beginingcuttingPrice),
                qtyavalsew = group.Sum(s => s.qtyavalsew),
                priceavalsew = group.Sum(s => s.priceavalsew),
                qtyavalcut = group.Sum(s => s.qtyavalcut),
                priceavalcut = group.Sum(s => s.priceavalcut),
                beginingloading = group.Sum(s => s.beginingloading),
                beginingloadingPrice = group.Sum(s => s.beginingloadingPrice),
                qtyLoadingIn = group.Sum(s => s.qtyLoadingIn),
                priceLoadingIn = group.Sum(s => s.priceLoadingIn),
                qtyloading = group.Sum(s => s.qtyloading),
                priceloading = group.Sum(s => s.priceloading),
                qtyLoadingAdj = group.Sum(s => s.qtyLoadingAdj),
                priceLoadingAdj = group.Sum(s => s.priceLoadingAdj),
                beginingSewing = group.Sum(s => s.beginingSewing),
                beginingSewingPrice = group.Sum(s => s.beginingSewingPrice),
                sewingIn = group.Sum(s => s.sewingIn),
                sewingInPrice = group.Sum(s => s.sewingInPrice),
                sewingintransfer = group.Sum(s => s.sewingintransfer),
                sewingintransferPrice = group.Sum(s => s.sewingintransferPrice),
                sewingout = group.Sum(s => s.sewingout),
                sewingoutPrice = group.Sum(s => s.sewingoutPrice),
                sewingretur = group.Sum(s => s.sewingretur),
                sewingreturPrice = group.Sum(s => s.sewingreturPrice),
                WipSewingToPacking = group.Sum(s => s.WipSewingToPacking),
                WipSewingToPackingPrice = group.Sum(s => s.WipSewingToPacking),
                wipfinishing = group.Sum(s => s.wipfinishing),
                wipfinishingPrice = group.Sum(s => s.wipfinishingPrice),
                sewingadj = group.Sum(s => s.sewingadj),
                sewingadjPrice = group.Sum(s => s.sewingadjPrice),
                finishingin = group.Sum(s => s.finishingin),
                finishinginPrice = group.Sum(s => s.finishinginPrice),
                finishingintransfer = group.Sum(s => s.finishingintransfer),
                finishingintransferPrice = group.Sum(s => s.finishingintransferPrice),
                finishingadj = group.Sum(s => s.finishingadj),
                finishingadjPrice = group.Sum(s => s.finishingadjPrice),
                finishingout = group.Sum(s => s.finishingout),
                finishingoutPrice = group.Sum(s => s.finishingoutPrice),
                finishingoutaval = group.Sum(s => s.finishingoutaval),
                finishingoutavalPrice = group.Sum(s => s.finishingoutavalPrice),
                finishinigretur = group.Sum(s => s.finishinigretur),
                finishinigreturPrice = group.Sum(s => s.finishinigreturPrice),
                beginingbalanceFinishing = group.Sum(s => s.beginingbalanceFinishing),
                beginingbalanceFinishingPrice = group.Sum(s => s.beginingbalanceFinishingPrice),
                beginingbalancesubcon = group.Sum(s => s.beginingbalancesubcon),
                beginingbalancesubconPrice = group.Sum(s => s.beginingbalancesubconPrice),
                subconIn = group.Sum(s => s.subconIn),
                subconInPrice = group.Sum(s => s.subconInPrice),
                subconout = group.Sum(s => s.subconout),
                subconoutPrice = group.Sum(s => s.subconoutPrice),
                packingInQty = group.Sum(s => s.packingInQty),
                packingInPrice = group.Sum(s => s.packingInPrice),
                packingOutQty = group.Sum(s => s.packingOutQty),
                packingOutPrice = group.Sum(s => s.packingOutPrice),
                otherqty = group.Sum(s => s.otherqty),
                otherprice = group.Sum(s => s.otherprice),
                sampleQty = group.Sum(s => s.sampleQty),
                samplePrice = group.Sum(s => s.samplePrice),
                expendAdj = group.Sum(s => s.expendAdj),
                expendAdjPrice = group.Sum(s => s.expendAdjPrice),
                expendRetur = group.Sum(s => s.expendRetur),
                expendReturPrice = group.Sum(s => s.expendReturPrice),
                //finishinginqty =group.Sum(s=>s.FinishingInQty)
                beginingBalanceExpenditureGood = group.Sum(s => s.beginingBalanceExpenditureGood),
                beginingBalanceExpenditureGoodPrice = group.Sum(s => s.beginingBalanceExpenditureGoodPrice),
                expenditureInTransfer = group.Sum(s => s.expenditureInTransfer),
                expenditureInTransferPrice = group.Sum(s => s.expenditureInTransferPrice),
                qtyloadingInTransfer = group.Sum(s => s.qtyloadingInTransfer),
                priceloadingInTransfer = group.Sum(s => s.priceloadingInTransfer)


            }).ToList();

            var getComoditiExpe = (from a in garmentSubconPackingOutRepository.Query
                                   where a.PackingOutDate >= dateBalance
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

            var BasicPrices = (from a in sumbasicPrice
                               join b in sumFCs on a.RO equals b.RO into sumFCes
                               from bb in sumFCes.DefaultIfEmpty()
                               //join c in queryBalance on a.RO equals c.Ro into queryBalances
                               //from cc in queryBalances.DefaultIfEmpty()
                               where ros.Contains(a.RO)
                               select new
                               {
                                   BasicPrice =
                                   //Math.Round(Convert.ToDouble(a.BasicPrice / a.Count), 2) 
                                   //* 
                                   //Math.Round(Convert.ToDouble((bb.FC / bb.Count) == 0 ? cc.BasicPrice : Math.Round(Convert.ToDouble(a.BasicPrice / a.Count), 2)) * Convert.ToDouble(bb.FC / bb.Count), 2),
                                   Math.Round(Convert.ToDouble((bb != null ? bb.FC / bb.Count : 0) == 0 ?  0 : Convert.ToDouble(a != null ? a.BasicPrice / a.Count : 0)) * Convert.ToDouble(bb != null ? bb.FC / bb.Count : 0), 2),
                                   realization = a.RO
                               }).Distinct().ToList();

            var dtos = (from a in querySum
                        join b in BasicPrices on a.ro equals b.realization
                        select new GarmentMonitoringProductionStockFlowDto
                        {
                            Article = a.article,
                            Ro = a.ro,
                            FC = Math.Round(Convert.ToDouble(a.fc), 2),
                            Fare = a.fare,
                            BasicPrice = b.BasicPrice,

                            BeginingBalanceCuttingQty = a.begining < 0 ? 0 : a.begining,
                            BeginingBalanceCuttingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.begining, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.begining, 2),
                            QtyCuttingToPacking = Math.Round(a.QtyCuttingToPacking, 2),
                            PriceCuttingToPacking = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.QtyCuttingToPacking, 2),
                            QtyCuttingsubkon = Math.Round(a.qtCuttingSubkon, 2),
                            PriceCuttingsubkon = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtCuttingSubkon, 2),
                            QtyCuttingIn = Math.Round(a.qtyCuttingIn, 2),
                            PriceCuttingIn = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyCuttingIn, 2),
                            QtyCuttingOut = Math.Round(a.qtycutting, 2),
                            PriceCuttingOut = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtycutting, 2),
                            Comodity = a.comodity,
                            AvalCutting = Math.Round(a.qtyavalcut, 2),
                            AvalCuttingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyavalcut, 2),
                            AvalSewing = Math.Round(a.qtyavalsew, 2),
                            AvalSewingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyavalsew, 2),
                            EndBalancCuttingeQty = Math.Round(a.begining + a.qtyCuttingIn - a.qtycutting - a.QtyCuttingToPacking - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew, 2) < 0 ? 0 : Math.Round(a.begining + a.qtyCuttingIn - a.qtycutting - a.QtyCuttingToPacking - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew, 2),
                            EndBalancCuttingePrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * (a.begining + a.qtyCuttingIn - a.qtycutting - a.QtyCuttingToPacking - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew), 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * (a.begining + a.qtyCuttingIn - a.qtycutting - a.QtyCuttingToPacking - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew), 2),
                            BeginingBalanceLoadingQty = Math.Round(a.beginingloading, 2) < 0 ? 0 : Math.Round(a.beginingloading, 2),
                            BeginingBalanceLoadingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.beginingloading, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.beginingloading, 2),
                            QtyLoadingIn = Math.Round(a.qtyLoadingIn, 2),
                            PriceLoadingIn = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyLoadingIn, 2),
                            QtyLoadingInTransfer = Math.Round(a.qtyloadingInTransfer, 2),
                            PriceLoadingInTransfer = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyloadingInTransfer, 2),
                            QtyLoading = Math.Round(a.qtyloading, 2),
                            PriceLoading = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyloading, 2),
                            QtyLoadingAdjs = Math.Round(a.qtyLoadingAdj, 2),
                            PriceLoadingAdjs = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyLoadingAdj, 2),
                            EndBalanceLoadingQty = (Math.Round(a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj, 2)) < 0 ? 0 : (Math.Round(a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj, 2)),
                            EndBalanceLoadingPrice = (Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * (a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj), 2)) < 0 ? 0 : (Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * (a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj), 2)),
                            BeginingBalanceSewingQty = Math.Round(a.beginingSewing, 2),
                            BeginingBalanceSewingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.beginingSewing, 2),
                            QtySewingIn = Math.Round(a.sewingIn, 2),
                            PriceSewingIn = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingIn, 2),
                            QtySewingOut = Math.Round(a.sewingout, 2),
                            PriceSewingOut = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingout, 2),
                            QtySewingInTransfer = Math.Round(a.sewingintransfer, 2),
                            PriceSewingInTransfer = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingintransfer, 2),
                            QtySewingRetur = Math.Round(a.sewingretur, 2),
                            PriceSewingRetur = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingretur, 2),
                            WipSewingToPacking = Math.Round(a.WipSewingToPacking, 2),
                            WipSewingToPackingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.WipSewingToPacking, 2),
                            WipFinishingOut = Math.Round(a.wipfinishing, 2),
                            WipFinishingOutPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.wipfinishing, 2),
                            QtySewingAdj = Math.Round(a.sewingadj, 2),
                            PriceSewingAdj = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingadj, 2),
                            EndBalanceSewingQty = Math.Round(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.WipSewingToPacking - a.wipfinishing - a.sewingretur - a.sewingadj, 2),
                            EndBalanceSewingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * Math.Round(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.WipSewingToPacking - a.wipfinishing - a.sewingretur - a.sewingadj, 2), 2),
                            BeginingBalanceFinishingQty = Math.Round(a.beginingbalanceFinishing, 2),
                            BeginingBalanceFinishingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.beginingbalanceFinishing, 2),
                            //FinishingInExpenditure = Math.Round(a.finishingout + a.subconout, 2),
                            FinishingInExpenditurepPrice = Math.Round((((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingout) + (((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.subconout), 2),
                            FinishingInQty = Math.Round(a.finishingin, 2),
                            FinishingInPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingin, 2),
                            FinishingOutQty = Math.Round(a.finishingout, 2),
                            FinishingOutPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingout, 2),
                            FinishingOutAval = Math.Round(a.finishingoutaval, 2),
                            FinishingOutAvalPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingoutaval, 2),
                            BeginingBalanceSubconQty = Math.Round(a.beginingbalancesubcon, 2),
                            BeginingBalanceSubconPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.beginingbalancesubcon, 2),
                            SubconInQty = Math.Round(a.subconIn, 2),
                            SubconInPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.subconIn, 2),
                            SubconOutQty = Math.Round(a.subconout, 2),
                            SubconOutPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.subconout, 2),
                            EndBalanceSubconQty = Math.Round(a.beginingbalancesubcon + a.subconIn - a.subconout, 2),
                            EndBalanceSubconPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * (a.beginingbalancesubcon + a.subconIn - a.subconout), 2),
                            FinishingInTransferQty = Math.Round(a.finishingintransfer, 2),
                            FinishingInTransferPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingintransfer, 2),
                            FinishingReturQty = Math.Round(a.finishinigretur, 2),
                            FinishingReturPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishinigretur, 2),
                            FinishingAdjQty = Math.Round(a.finishingadj, 2),
                            FinishingAdjPRice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingadj, 2),
                            BeginingBalanceExpenditureGood = Math.Round(a.beginingBalanceExpenditureGood, 2),
                            BeginingBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.beginingBalanceExpenditureGood, 2),
                            EndBalanceFinishingQty = Math.Round(a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur - a.finishingoutaval, 2),
                            EndBalanceFinishingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * (a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur - a.finishingoutaval), 2),
                            //PackingInQty = Math.Round(a.packingInQty, 2),
                            FinishingInExpenditure = Math.Round(a.packingInQty, 2),
                            PackingInPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.packingInQty, 2),
                            PackingOutQty = Math.Round(a.packingOutQty, 2),
                            PackingOutPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.packingOutQty, 2),
                            SampleQty = Math.Round(a.sampleQty, 2),
                            SamplePrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.sampleQty, 2),
                            OtherQty = Math.Round(a.otherqty, 2),
                            OtherPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.otherqty, 2),
                            ExpenditureGoodAdj = Math.Round(a.expendAdj, 2),
                            ExpenditureGoodAdjPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.expendAdj, 2),
                            ExpenditureGoodRetur = Math.Round(a.expendRetur, 2),
                            ExpenditureGoodReturPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.expendRetur, 2),
                            ExpenditureGoodInTransfer = Math.Round(a.expenditureInTransfer, 2),
                            ExpenditureGoodInTransferPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.expenditureInTransfer, 2),
                            EndBalanceExpenditureGood = Math.Round(a.beginingBalanceExpenditureGood /*+ a.finishingout*/ + a.subconout + a.expendRetur + a.expenditureInTransfer /*+ a.packingInQty*/ + a.packingOutQty - a.otherqty - a.sampleQty - a.expendAdj, 2),
                            EndBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * (a.beginingBalanceExpenditureGood + a.finishingout + a.subconout + a.expendRetur + a.expenditureInTransfer + a.packingInQty - a.packingOutQty - a.otherqty - a.sampleQty - a.expendAdj), 2),
                            FareNew = a.farenew,
                            CuttingNew = Math.Round(a.farenew * Convert.ToDecimal(a.begining + a.qtyCuttingIn - a.qtycutting - a.QtyCuttingToPacking - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew), 2),
                            LoadingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingloading + a.qtyLoadingIn - a.qtyloading - a.qtyLoadingAdj), 2),
                            SewingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.WipSewingToPacking - a.wipfinishing - a.sewingretur - a.sewingadj), 2),
                            FinishingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur), 2),
                            ExpenditureNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingBalanceExpenditureGood + a.finishingout + a.subconout + a.expendRetur + a.expenditureInTransfer + a.packingInQty - a.packingOutQty - a.otherqty - a.sampleQty - a.expendAdj), 2),
                            SubconNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingbalancesubcon + a.subconIn - a.subconout), 2)
                        }).ToList();

            var data = from a in dtos
                       where a.BeginingBalanceCuttingQty > 0 || a.QtyCuttingIn > 0 || a.QtyCuttingOut > 0 || a.QtyCuttingsubkon > 0 || a.QtyCuttingToPacking > 0 || a.EndBalancCuttingeQty > 0 ||
                        a.BeginingBalanceLoadingQty > 0 || a.QtyLoading > 0 || a.QtyLoadingAdjs > 0 || a.QtyLoadingIn > 0 || a.QtyLoadingInTransfer > 0 || a.EndBalanceLoadingQty > 0 ||
                        a.BeginingBalanceSewingQty > 0 || a.QtySewingAdj > 0 || a.QtySewingIn > 0 || a.QtySewingInTransfer > 0 || a.QtySewingOut > 0 || a.QtySewingRetur > 0 || a.WipSewingToPacking > 0 || a.WipFinishingOut > 0 || a.EndBalanceSewingQty > 0 ||
                        a.BeginingBalanceSubconQty > 0 || a.EndBalanceSubconQty > 0 || a.SubconInQty > 0 || a.SubconOutQty > 0 || a.AvalCutting > 0 || a.AvalSewing > 0 ||
                        a.BeginingBalanceFinishingQty > 0 || a.FinishingAdjQty > 0 || a.FinishingInExpenditure > 0 || a.FinishingInQty > 0 || a.FinishingInTransferQty > 0 || a.FinishingOutQty > 0 || a.FinishingReturQty > 0 ||
                        a.BeginingBalanceExpenditureGood > 0 || a.ExpenditureGoodAdj > 0 || a.ExpenditureGoodInTransfer > 0 || a.ExpenditureGoodRemainingQty > 0 || a.ExpenditureGoodRetur > 0 || a.EndBalanceExpenditureGood > 0
                       select a;

            //var data2 = data.Count();

            var roList = (from a in data
                          select a.Ro).Distinct().ToList();
            //var roBalance = from a in garmentBalanceProductionStockRepository.Query
            //                select new CostCalViewModel { comodityName = a.Comodity, buyerCode = a.BuyerCode, hours = a.Hours, qtyOrder = a.QtyOrder, ro = a.Ro };

            CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(roList, request.token);

            //foreach (var item in roBalance)
            //{
            //    costCalculation.data.Add(item);
            //}

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
                               FC = item.FC,
                               Fare = item.Fare,
                               BasicPrice = item.BasicPrice,
                               BuyerCode = item.BuyerCode == null ? b.buyerCode : item.BuyerCode,
                               Comodity = item.Comodity == null ? b.comodityName : item.Comodity,
                               QtyOrder = b.qtyOrder,
                               Hours = item.Hours == 0 ? b.hours : item.Hours,
                               BeginingBalanceCuttingQty = item.BeginingBalanceCuttingQty,
                               BeginingBalanceCuttingPrice = item.BeginingBalanceCuttingPrice,
                               QtyCuttingToPacking = item.QtyCuttingToPacking,
                               PriceCuttingToPacking = item.PriceCuttingToPacking,
                               QtyCuttingsubkon = item.QtyCuttingsubkon,
                               PriceCuttingsubkon = item.PriceCuttingsubkon,
                               QtyCuttingIn = item.QtyCuttingIn,
                               PriceCuttingIn = item.PriceCuttingIn,
                               QtyCuttingOut = item.QtyCuttingOut,
                               PriceCuttingOut = item.PriceCuttingOut,
                               AvalCutting = item.AvalCutting,
                               AvalCuttingPrice = item.AvalCuttingPrice,
                               AvalSewing = item.AvalSewing,
                               AvalSewingPrice = item.AvalSewingPrice,
                               EndBalancCuttingeQty = item.EndBalancCuttingeQty,
                               EndBalancCuttingePrice = item.EndBalancCuttingePrice,
                               BeginingBalanceLoadingQty = item.BeginingBalanceLoadingQty,
                               BeginingBalanceLoadingPrice = item.BeginingBalanceLoadingPrice,
                               QtyLoadingIn = item.QtyLoadingIn,
                               PriceLoadingIn = item.PriceLoadingIn,
                               QtyLoadingInTransfer = item.QtyLoadingInTransfer,
                               PriceLoadingInTransfer = item.PriceLoadingInTransfer,
                               QtyLoading = item.QtyLoading,
                               PriceLoading = item.PriceLoading,
                               QtyLoadingAdjs = item.QtyLoadingAdjs,
                               PriceLoadingAdjs = item.PriceLoadingAdjs,
                               EndBalanceLoadingQty = item.EndBalanceLoadingQty,
                               EndBalanceLoadingPrice = item.EndBalanceLoadingPrice,
                               BeginingBalanceSewingQty = item.BeginingBalanceSewingQty,
                               BeginingBalanceSewingPrice = item.BeginingBalanceSewingPrice,
                               QtySewingIn = item.QtySewingIn,
                               PriceSewingIn = item.PriceSewingIn,
                               QtySewingOut = item.QtySewingOut,
                               PriceSewingOut = item.PriceSewingOut,
                               QtySewingInTransfer = item.QtySewingInTransfer,
                               PriceSewingInTransfer = item.PriceSewingInTransfer,
                               QtySewingRetur = item.QtySewingRetur,
                               PriceSewingRetur = item.PriceSewingRetur,
                               WipSewingToPacking = item.WipSewingToPacking,
                               WipSewingToPackingPrice = item.WipSewingToPackingPrice,
                               WipFinishingOut = item.WipFinishingOut,
                               WipFinishingOutPrice = item.WipFinishingOutPrice,
                               QtySewingAdj = item.QtySewingAdj,
                               PriceSewingAdj = item.PriceSewingAdj,
                               EndBalanceSewingQty = item.EndBalanceSewingQty,
                               EndBalanceSewingPrice = item.EndBalanceSewingPrice,
                               BeginingBalanceFinishingQty = item.BeginingBalanceFinishingQty,
                               BeginingBalanceFinishingPrice = item.BeginingBalanceFinishingPrice,
                               FinishingInExpenditure = item.FinishingInExpenditure,
                               FinishingInExpenditurepPrice = (item.FinishingInExpenditure * Convert.ToDouble(item.Fare) + (item.FinishingInExpenditure * item.BasicPrice)),
                               FinishingInQty = item.FinishingInQty,
                               FinishingInPrice = item.FinishingInPrice,
                               FinishingOutQty = item.FinishingOutQty,
                               FinishingOutPrice = item.FinishingOutPrice,
                               FinishingOutAval = item.FinishingOutAval,
                               FinishingOutAvalPrice = item.FinishingOutAvalPrice,
                               BeginingBalanceSubconQty = item.BeginingBalanceSubconQty,
                               BeginingBalanceSubconPrice = item.BeginingBalanceSubconPrice,
                               SubconInQty = item.SubconInQty,
                               SubconInPrice = item.SubconInPrice,
                               SubconOutQty = item.SubconOutQty,
                               SubconOutPrice = item.SubconOutPrice,
                               EndBalanceSubconQty = item.EndBalanceSubconQty,
                               EndBalanceSubconPrice = item.EndBalanceSubconPrice,
                               FinishingInTransferQty = item.FinishingInTransferQty,
                               FinishingInTransferPrice = item.FinishingInTransferPrice,
                               FinishingReturQty = item.FinishingReturQty,
                               FinishingReturPrice = item.FinishingReturPrice,
                               FinishingAdjQty = item.FinishingAdjQty,
                               FinishingAdjPRice = item.FinishingAdjPRice,
                               BeginingBalanceExpenditureGood = item.BeginingBalanceExpenditureGood,
                               BeginingBalanceExpenditureGoodPrice = item.BeginingBalanceExpenditureGoodPrice,
                               EndBalanceFinishingQty = item.EndBalanceFinishingQty,
                               EndBalanceFinishingPrice = item.EndBalanceFinishingPrice,
                               PackingInQty = item.PackingInQty,
                               PackingInPrice = item.PackingInPrice,
                               PackingOutQty = item.PackingOutQty,
                               PackingOutPrice = item.PackingOutPrice,
                               SampleQty = item.SampleQty,
                               SamplePrice = item.SamplePrice,
                               OtherQty = item.OtherQty,
                               OtherPrice = item.OtherPrice,
                               ExpenditureGoodAdj = item.ExpenditureGoodAdj,
                               ExpenditureGoodAdjPrice = item.ExpenditureGoodAdjPrice,
                               ExpenditureGoodRetur = item.ExpenditureGoodRetur,
                               ExpenditureGoodReturPrice = item.ExpenditureGoodReturPrice,
                               ExpenditureGoodInTransfer = item.ExpenditureGoodInTransfer,
                               ExpenditureGoodInTransferPrice = item.ExpenditureGoodInTransferPrice,
                               EndBalanceExpenditureGood = item.EndBalanceExpenditureGood,
                               EndBalanceExpenditureGoodPrice = item.EndBalanceExpenditureGoodPrice,
                               FareNew = item.FareNew,
                               CuttingNew = item.CuttingNew,
                               LoadingNew = item.LoadingNew,
                               SewingNew = item.SewingNew,
                               FinishingNew = item.FinishingNew,
                               ExpenditureNew = item.ExpenditureNew,
                               SubconNew = item.SubconNew,
                               ExpenditureGoodRemainingPrice = item.ExpenditureGoodRemainingPrice,
                               ExpenditureGoodRemainingQty = item.ExpenditureGoodRemainingQty,
                               FinishingTransferExpenditure = item.FinishingTransferExpenditure,
                               FinishingTransferExpenditurePrice = item.FinishingTransferExpenditurePrice,
                               MaterialUsage = item.FinishingInExpenditure * item.BasicPrice,
                               PriceUsage = item.FinishingInExpenditure * Convert.ToDouble(item.Fare),
                               SubconSewingInQty = 0,
                               SubconSewingOutQty = 0,
                               SubconExpenditureGoodInQty = 0,
                               SubconExpenditureGoodQty = 0,
                           }).OrderBy(x=>x.Ro).ToList();

          
            garmentMonitoringProductionFlow.garmentMonitorings = dataend.OrderBy(x => x.Ro).ToList();
            garmentMonitoringProductionFlow.count = dataend.Count();

			monitoringDtos = dataend.ToList();
 
            GarmentMonitoringProductionStockFlowDto total = new GarmentMonitoringProductionStockFlowDto()
            {
                MaterialUsage = dataend.Sum(x=>x.MaterialUsage),
                PriceUsage = dataend.Sum(x => x.PriceUsage),
                BeginingBalanceCuttingQty = dataend.Sum(x => x.BeginingBalanceCuttingQty),
                BeginingBalanceCuttingPrice = dataend.Sum(x => x.BeginingBalanceCuttingPrice),
                QtyCuttingToPacking = dataend.Sum(x => x.QtyCuttingToPacking),
                PriceCuttingToPacking = dataend.Sum(x => x.PriceCuttingToPacking),
                QtyCuttingsubkon = dataend.Sum(x => x.QtyCuttingsubkon),
                PriceCuttingsubkon = dataend.Sum(x => x.PriceCuttingsubkon),
                QtyCuttingIn = dataend.Sum(x => x.QtyCuttingIn),
                PriceCuttingIn = dataend.Sum(x => x.PriceCuttingIn),
                QtyCuttingOut = dataend.Sum(x => x.QtyCuttingOut),
                PriceCuttingOut = dataend.Sum(x => x.PriceCuttingOut),
                AvalCutting = dataend.Sum(x => x.AvalCutting),
                AvalCuttingPrice = dataend.Sum(x => x.AvalCuttingPrice),
                AvalSewing = dataend.Sum(x => x.AvalSewing),
                AvalSewingPrice = dataend.Sum(x => x.AvalSewingPrice),
                EndBalancCuttingeQty = dataend.Sum(x => x.EndBalancCuttingeQty),
                EndBalancCuttingePrice = dataend.Sum(x => x.EndBalancCuttingePrice),
                BeginingBalanceLoadingQty = dataend.Sum(x => x.BeginingBalanceLoadingQty),
                BeginingBalanceLoadingPrice = dataend.Sum(x => x.BeginingBalanceLoadingPrice),
                QtyLoadingIn = dataend.Sum(x => x.QtyLoadingIn),
                PriceLoadingIn = dataend.Sum(x => x.PriceLoadingIn),
                QtyLoadingInTransfer = dataend.Sum(x => x.QtyLoadingInTransfer),
                PriceLoadingInTransfer = dataend.Sum(x => x.PriceLoadingInTransfer),
                QtyLoading = dataend.Sum(x => x.QtyLoading),
                PriceLoading = dataend.Sum(x => x.PriceLoading),
                QtyLoadingAdjs = dataend.Sum(x => x.QtyLoadingAdjs),
                PriceLoadingAdjs = dataend.Sum(x => x.PriceLoadingAdjs),
                EndBalanceLoadingQty = dataend.Sum(x => x.EndBalanceLoadingQty),
                EndBalanceLoadingPrice = dataend.Sum(x => x.EndBalanceLoadingPrice),
                BeginingBalanceSewingQty = dataend.Sum(x => x.BeginingBalanceSewingQty),
                BeginingBalanceSewingPrice = dataend.Sum(x => x.BeginingBalanceSewingPrice),
                QtySewingIn = dataend.Sum(x => x.QtySewingIn),
                PriceSewingIn = dataend.Sum(x => x.PriceSewingIn),
                QtySewingOut = dataend.Sum(x => x.QtySewingOut),
                PriceSewingOut = dataend.Sum(x => x.PriceSewingOut),
                QtySewingInTransfer = dataend.Sum(x => x.QtySewingInTransfer),
                PriceSewingInTransfer = dataend.Sum(x => x.PriceSewingInTransfer),
                QtySewingRetur = dataend.Sum(x => x.QtySewingRetur),
                PriceSewingRetur = dataend.Sum(x => x.PriceSewingRetur),
                WipSewingToPacking = dataend.Sum(x => x.WipSewingToPacking),
                WipSewingToPackingPrice = dataend.Sum(x => x.WipSewingToPackingPrice),
                WipFinishingOut = dataend.Sum(x => x.WipFinishingOut),
                WipFinishingOutPrice = dataend.Sum(x => x.WipFinishingOutPrice),
                QtySewingAdj = dataend.Sum(x => x.QtySewingAdj),
                PriceSewingAdj = dataend.Sum(x => x.PriceSewingAdj),
                EndBalanceSewingQty = dataend.Sum(x => x.EndBalanceSewingQty),
                EndBalanceSewingPrice = dataend.Sum(x => x.EndBalanceSewingPrice),
                BeginingBalanceFinishingQty = dataend.Sum(x => x.BeginingBalanceFinishingQty),
                BeginingBalanceFinishingPrice = dataend.Sum(x => x.BeginingBalanceFinishingPrice),
                FinishingInExpenditure = dataend.Sum(x => x.FinishingInExpenditure),
                FinishingInExpenditurepPrice = dataend.Sum(x => x.FinishingInExpenditurepPrice),
                FinishingInQty = dataend.Sum(x => x.FinishingInQty),
                FinishingInPrice = dataend.Sum(x => x.FinishingInPrice),
                FinishingOutQty = dataend.Sum(x => x.FinishingOutQty),
                FinishingOutPrice = dataend.Sum(x => x.FinishingOutPrice),
                FinishingOutAval = dataend.Sum(x => x.FinishingOutAval),
                FinishingOutAvalPrice = dataend.Sum(x => x.FinishingOutAvalPrice),
                BeginingBalanceSubconQty = dataend.Sum(x => x.BeginingBalanceSubconQty),
                BeginingBalanceSubconPrice = dataend.Sum(x => x.BeginingBalanceSubconPrice),
                SubconInQty = dataend.Sum(x => x.SubconInQty),
                SubconInPrice = dataend.Sum(x => x.SubconInPrice),
                SubconOutQty = dataend.Sum(x => x.SubconOutQty),
                SubconOutPrice = dataend.Sum(x => x.SubconOutPrice),
                EndBalanceSubconQty = dataend.Sum(x => x.EndBalanceSubconQty),
                EndBalanceSubconPrice = dataend.Sum(x => x.EndBalanceSubconPrice),
                FinishingInTransferQty = dataend.Sum(x => x.FinishingInTransferQty),
                FinishingInTransferPrice = dataend.Sum(x => x.FinishingInTransferPrice),
                FinishingReturQty = dataend.Sum(x => x.FinishingReturQty),
                FinishingReturPrice = dataend.Sum(x => x.FinishingReturPrice),
                FinishingAdjQty = dataend.Sum(x => x.FinishingAdjQty),
                FinishingAdjPRice = dataend.Sum(x => x.FinishingAdjPRice),
                BeginingBalanceExpenditureGood = dataend.Sum(x => x.BeginingBalanceExpenditureGood),
                BeginingBalanceExpenditureGoodPrice = dataend.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                EndBalanceFinishingQty = dataend.Sum(x => x.EndBalanceFinishingQty),
                EndBalanceFinishingPrice = dataend.Sum(x => x.EndBalanceFinishingPrice),
                PackingInQty = dataend.Sum(x => x.PackingInQty),
                PackingInPrice = dataend.Sum(x => x.PackingInPrice),
                PackingOutQty = dataend.Sum(x => x.PackingOutQty),
                PackingOutPrice = dataend.Sum(x => x.PackingOutPrice),
                SampleQty = dataend.Sum(x => x.SampleQty),
                SamplePrice = dataend.Sum(x => x.SamplePrice),
                OtherQty = dataend.Sum(x => x.OtherQty),
                OtherPrice = dataend.Sum(x => x.OtherPrice),
                ExpenditureGoodAdj = dataend.Sum(x => x.ExpenditureGoodAdj),
                ExpenditureGoodAdjPrice = dataend.Sum(x => x.ExpenditureGoodAdjPrice),
                ExpenditureGoodRetur = dataend.Sum(x => x.ExpenditureGoodRetur),
                ExpenditureGoodReturPrice = dataend.Sum(x => x.ExpenditureGoodReturPrice),
                ExpenditureGoodInTransfer = dataend.Sum(x => x.ExpenditureGoodInTransfer),
                ExpenditureGoodInTransferPrice = dataend.Sum(x => x.ExpenditureGoodInTransferPrice),
                EndBalanceExpenditureGood = dataend.Sum(x => x.EndBalanceExpenditureGood),
                EndBalanceExpenditureGoodPrice = dataend.Sum(x => x.EndBalanceExpenditureGoodPrice),

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
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING7", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING3", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING4", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING5", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING6", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING9", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING10", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING11", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING4", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING3", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING4", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING5", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING6", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING7", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING9", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING10", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING11", DataType = typeof(string) });

                reportDataTable.Rows.Add("", "", "", "",
				"Saldo Awal WIP Cutting", "Cutting In (WIP Cutting)", "Cutting Out / HP(WIP Loading)", "Cutting Out To Packing"/*, "Cutting Out Subkon"*/, "Aval Komponen dari Cutting", "Aval Komponen dari Sewing", "Saldo Akhir WIP Cutting",
				"Saldo Awal Loading", "Loading In", "Loading In Transfer", "Loading Out (WIP Sewing)	",/* "Adjs Loading",*/ "Saldo Akhir Loading",
				"Saldo Awal WIP Sewing", "Sewing In (WIP Sewing)", "Sewing Out (WIP Finishing)", /*"Subkon Masuk", "Subkon Keluar",*/ "Sewing In Transfer", "Sewing Out WIP Packing", "Sewing Out Transfer WIP Finishing", "Retur ke Cutting",/* "Adjs Sewing",*/ "Saldo Akhir WIP Sewing",
				"Saldo Awal WIP Finishing", "Finishing In (WIP Finishing)", "Saldo Awal WIP Subkon", /*"Subkon In", "Subkon Out",*/ "Saldo Akhir WIP Subkon", "Finishing Out (WIP BJ)", "Finishing Out Aval", /*"Adjs Finishing",*/ "Retur ke Sewing", "Saldo Akhir WIP Finishing",
				"Saldo Awal Barang jadi", "Barang Jadi In/ (WIP BJ)", "Barang Jadi In Transfer", "Penerimaan Lain-lain (Retur)", /*"Pengiriman Export", */"Packing Keluar", /*"Pengiriman Lain-lain/Sample", "Subkon Masuk", "Subkon Keluar", "Adjust Barang Jadi",*/ "Saldo Akhir Barang Jadi"
				);
			}
			else
			{

				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FC", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SMV", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TARIF", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HARGA BAHAN BAKU", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING513", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING614", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING18", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING19", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING20", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING18", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING19", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING20", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING21", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING22", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING18", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING19", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING20", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING21", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PACKING22", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)7", DataType = typeof(string) });

				reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "",
									"Saldo Awal WIP Cutting","", "Cutting In (WIP Cutting)","", "Cutting Out / HP(WIP Loading)","", "Cutting Out Transfer","", "Cutting Out Subkon","", "Aval Komponen dari Cutting","", "Aval Komponen dari Sewing","", "Saldo Akhir WIP Cutting","",
									"Saldo Awal Loading","", "Loading In","", "Loading In Transfer", "", "Loading Out (WIP Sewing)	","", "Adjs Loading","", "Saldo Akhir Loading","",
									"Saldo Awal WIP Sewing","", "Sewing In (WIP Sewing)","", "Sewing Out (WIP Finishing)","","Subkon Masuk", "Subkon Keluar", "Sewing In Transfer","", "Sewing Out Tranfer WIP Sewing","", "Sewing Out Transfer WIP Finishing","", "Retur ke Cutting","", "Adjs Sewing","", "Saldo Akhir WIP Sewing","",
									"Saldo Awal WIP Finishing","", "Finishing In (WIP Finishing)","", "Saldo Awal WIP Subkon","", "Subkon In","", "Subkon Out","", "Saldo Akhir WIP Subkon","", "Finishing Out (WIP BJ)","", "Finishing In Transfer","", "Adjs Finishing","", "Retur ke Sewing","", "Saldo Akhir WIP Finishing","",
									"Saldo Awal Barang jadi","", "Packing Masuk","", "Finishing Transfer","", "Penerimaan Lain-lain (Retur)","", "Standar Konversi Biaya","", "Pengiriman Export","", "Pengiriman Gudang Sisa","", /*"Pengiriman Lain-lain/Sample","", "Subkon Masuk", "Subkon Keluar", "Adjust Barang Jadi",*/"", "Saldo Akhir Barang Jadi","",
									"Tarif","Cutting","Loading","Sewing","Finishing", "Subkon","Barang Jadi"
									);
				reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "",
					"Qty","Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", 

                    "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Qty", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Tarif", "Pemakaian Bahan Baku", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Qty", "Qty", "Harga", "Qty","Harga",
					"","","","","","",""
					);
				
			}
			int counter = 6;

			foreach (var report in garmentMonitoringProductionFlow.garmentMonitorings)
			{
				if (request.type != "bookkeeping")
				{
                    reportDataTable.Rows.Add(
                        report.Ro, report.Article, report.Comodity, report.QtyOrder, report.BeginingBalanceCuttingQty
                    , report.QtyCuttingIn, report.QtyCuttingOut, report.QtyCuttingToPacking, /*report.QtyCuttingsubkon,*/ report.AvalCutting
                    , report.AvalSewing, report.EndBalancCuttingeQty, report.BeginingBalanceLoadingQty, report.QtyLoadingIn, report.QtyLoadingInTransfer
                    , report.QtyLoading,/* report.QtyLoadingAdjs,*/ report.EndBalanceLoadingQty, report.BeginingBalanceSewingQty, report.QtySewingIn
                    , report.QtySewingOut/*, report.SubconSewingInQty, report.SubconSewingOutQty*/, report.QtySewingInTransfer, report.WipSewingToPacking
                    , report.WipFinishingOut, report.QtySewingRetur, /*report.QtySewingAdj,*/ report.EndBalanceSewingQty, report.BeginingBalanceFinishingQty
                    , report.FinishingInQty, report.BeginingBalanceSubconQty, /*report.SubconInQty, report.SubconOutQty,*/ report.EndBalanceSubconQty
                    , report.FinishingOutQty, report.FinishingOutAval, /*report.FinishingAdjQty,*/ report.FinishingReturQty, report.EndBalanceFinishingQty
                    , report.BeginingBalanceExpenditureGood, report.FinishingInExpenditure, report.FinishingInTransferQty, report.ExpenditureGoodRetur/*, report.PackingInQty*/
                    , report.PackingOutQty/*, report.SampleQty, report.SubconExpenditureGoodInQty, report.SubconExpenditureGoodQty, report.ExpenditureGoodAdj*/
                    , report.EndBalanceExpenditureGood);
                    counter++;
				}
				else
				{
					reportDataTable.Rows.Add(report.Ro, report.Article, report.BuyerCode, report.Comodity, report.QtyOrder, report.FC, report.Hours, report.Fare, report.BasicPrice,
						report.BeginingBalanceCuttingQty, report.BeginingBalanceCuttingPrice, report.QtyCuttingIn,report.PriceCuttingIn, report.QtyCuttingOut,report.PriceCuttingOut, report.QtyCuttingToPacking,report.PriceCuttingToPacking, report.QtyCuttingsubkon,report.PriceCuttingsubkon, report.AvalCutting,report.AvalCuttingPrice, report.AvalSewing,report.AvalSewingPrice, report.EndBalancCuttingeQty,report.EndBalancCuttingePrice,
						report.BeginingBalanceLoadingQty, report.BeginingBalanceLoadingPrice, report.QtyLoadingIn, report.PriceLoadingIn,report.QtyLoadingInTransfer,report.PriceLoadingInTransfer, report.QtyLoading, report.PriceLoading, report.QtyLoadingAdjs, report.PriceLoadingAdjs, report.EndBalanceLoadingQty, report.EndBalanceLoadingPrice,
						report.BeginingBalanceSewingQty, report.BeginingBalanceSewingPrice, report.QtySewingIn,report.PriceSewingIn, report.QtySewingOut,report.PriceSewingOut, report.SubconSewingInQty, report.SubconSewingOutQty, report.QtySewingInTransfer,report.PriceSewingInTransfer, report.WipSewingToPacking, report.WipSewingToPackingPrice, report.WipFinishingOut,report.WipFinishingOutPrice, report.QtySewingRetur,report.PriceSewingRetur, report.QtySewingAdj,report.PriceSewingAdj, report.EndBalanceSewingQty,report.EndBalanceSewingPrice,
						report.BeginingBalanceFinishingQty,report.BeginingBalanceFinishingPrice, report.FinishingInQty,report.FinishingInPrice, report.BeginingBalanceSubconQty,report.BeginingBalanceSubconPrice, report.SubconInQty,report.SubconInPrice, report.SubconOutQty,report.SubconOutPrice, report.EndBalanceSubconQty,report.EndBalanceSubconPrice, report.FinishingOutQty,report.FinishingOutPrice, report.FinishingInTransferQty,report.FinishingInTransferPrice, report.FinishingAdjQty,report.FinishingAdjPRice, report.FinishingReturQty,report.FinishingReturPrice, report.EndBalanceFinishingQty,report.EndBalanceFinishingPrice,
						report.BeginingBalanceExpenditureGood,report.BeginingBalanceExpenditureGoodPrice, report.FinishingInExpenditure,report.FinishingInExpenditurepPrice, report.FinishingInTransferQty,report.FinishingInTransferPrice, report.ExpenditureGoodRetur,report.ExpenditureGoodReturPrice,report.PriceUsage, report.MaterialUsage, report.PackingInQty,report.PackingInPrice, report.OtherQty,report.OtherPrice, report.SampleQty,report.SamplePrice, report.SubconExpenditureGoodInQty, report.SubconExpenditureGoodQty, report.ExpenditureGoodAdj,report.ExpenditureGoodAdjPrice, report.EndBalanceExpenditureGood,report.EndBalanceExpenditureGoodPrice,
						report.FareNew, report.CuttingNew, report.LoadingNew, report.SewingNew, report.FinishingNew,report.SubconNew, report.ExpenditureNew
						);
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
					worksheet.Cells["A1"].Value = "Report Produksi Terima Subcon";
					worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
					worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
					worksheet.Cells["A" + 1 + ":AL" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 2 + ":AL" + 2 + ""].Merge = true;
					worksheet.Cells["A" + 3 + ":AL" + 3 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":AL" + 3 + ""].Style.Font.Size = 15;
					worksheet.Cells["A" + 1 + ":AL" + 3 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 1 + ":AL" + 6 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 1 + ":AL" + 6 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
					worksheet.Cells["E" + 5 + ":AL" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

					//worksheet.Cells["E" + 5 + ":L" + 5 + ""].Merge = true;
					//worksheet.Cells["M" + 5 + ":R" + 5 + ""].Merge = true;
					//worksheet.Cells["S" + 5 + ":AA" + 5 + ""].Merge = true;
					//worksheet.Cells["AB" + 5 + ":AL" + 5 + ""].Merge = true;
					//worksheet.Cells["AM" + 5 + ":AU" + 5 + ""].Merge = true;

					worksheet.Cells["E" + 5 + ":K" + 5 + ""].Merge = true;
                    worksheet.Cells["L" + 5 + ":P" + 5 + ""].Merge = true;
                    worksheet.Cells["Q" + 5 + ":X" + 5 + ""].Merge = true;
					worksheet.Cells["Y" + 5 + ":AF" + 5 + ""].Merge = true;
					worksheet.Cells["AG" + 5 + ":AL" + 5 + ""].Merge = true;
					worksheet.Cells["A" + counter + ":D" + counter + ""].Merge = true;
					worksheet.Cells["A" + 5 + ":AL" + 6 + ""].Style.Font.Bold = true;
					worksheet.Cells["E" + 6 + ":AL" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					worksheet.Cells["A" + 5 + ":AL" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AL" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AL" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AL" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + counter + ":AL" + counter + ""].Style.Font.Bold = true;
                    foreach (var cell in worksheet.Cells["D" + 7 + ":AL" + (counter + 1) + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                }
				else
				{
					worksheet.Cells["A1"].Value = "Report Produksi"; worksheet.Cells["A" + 1 + ":AT" + 1 + ""].Merge = true;
					worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
					worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
					worksheet.Cells["A" + 1 + ":DD" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 2 + ":DD" + 2 + ""].Merge = true;
					worksheet.Cells["A" + 3 + ":DD" + 3 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":DD" + 3 + ""].Style.Font.Size = 15;
					worksheet.Cells["A" + 1 + ":DD" + 3 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 1 + ":DD" + 3 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 1 + ":DD" + 3 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
					worksheet.Cells["E" + 5 + ":L" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["J" + 5 + ":Y" + 5 + ""].Merge = true;
					worksheet.Cells["Z" + 5 + ":AK" + 5 + ""].Merge = true;
					worksheet.Cells["AL" + 5 + ":BE" + 5 + ""].Merge = true;
					worksheet.Cells["BF" + 5 + ":CA" + 5 + ""].Merge = true;
					worksheet.Cells["CB" + 5 + ":CW" + 5 + ""].Merge = true;
					worksheet.Cells["CX" + 5 + ":DD" + 5 + ""].Merge = true;
					worksheet.Cells["A" + 5 + ":A" + 7 + ""].Merge = true;
					worksheet.Cells["B" + 5 + ":B" + 7 + ""].Merge = true;
					worksheet.Cells["C" + 5 + ":C" + 7 + ""].Merge = true;
					worksheet.Cells["D" + 5 + ":D" + 7 + ""].Merge = true;
					worksheet.Cells["E" + 5 + ":E" + 7 + ""].Merge = true;
					worksheet.Cells["F" + 5 + ":F" + 7 + ""].Merge = true;
					worksheet.Cells["G" + 5 + ":G" + 7 + ""].Merge = true;
					worksheet.Cells["H" + 5 + ":H" + 7 + ""].Merge = true;
					worksheet.Cells["I" + 5 + ":I" + 7 + ""].Merge = true;
					worksheet.Cells["J" + 6 + ":K" + 6 + ""].Merge = true;
					worksheet.Cells["L" + 6 + ":M" + 6 + ""].Merge = true;
					worksheet.Cells["N" + 6 + ":O" + 6 + ""].Merge = true;
					worksheet.Cells["P" + 6 + ":Q" + 6 + ""].Merge = true;
					worksheet.Cells["R" + 6 + ":S" + 6 + ""].Merge = true;
					worksheet.Cells["T" + 6 + ":U" + 6 + ""].Merge = true;
					worksheet.Cells["V" + 6 + ":W" + 6 + ""].Merge = true;
					worksheet.Cells["X" + 6 + ":Y" + 6 + ""].Merge = true;
					worksheet.Cells["Z" + 6 + ":AA" + 6 + ""].Merge = true;
					worksheet.Cells["AB" + 6 + ":AC" + 6 + ""].Merge = true;
					worksheet.Cells["AD" + 6 + ":AE" + 6 + ""].Merge = true;
					worksheet.Cells["AF" + 6 + ":AG" + 6 + ""].Merge = true;
					worksheet.Cells["AH" + 6 + ":AI" + 6 + ""].Merge = true;
					worksheet.Cells["AJ" + 6 + ":AK" + 6 + ""].Merge = true;
					worksheet.Cells["AL" + 6 + ":AM" + 6 + ""].Merge = true;
					worksheet.Cells["AN" + 6 + ":AO" + 6 + ""].Merge = true;
					worksheet.Cells["AP" + 6 + ":AQ" + 6 + ""].Merge = true;

					
					worksheet.Cells["AT" + 6 + ":AU" + 6 + ""].Merge = true;
					worksheet.Cells["AV" + 6 + ":AW" + 6 + ""].Merge = true;
					worksheet.Cells["AX" + 6 + ":AY" + 6 + ""].Merge = true;
					worksheet.Cells["Az" + 6 + ":BA" + 6 + ""].Merge = true;
					worksheet.Cells["BB" + 6 + ":BC" + 6 + ""].Merge = true;
					worksheet.Cells["BD" + 6 + ":BE" + 6 + ""].Merge = true;
					worksheet.Cells["BF" + 6 + ":BG" + 6 + ""].Merge = true;
					worksheet.Cells["BH" + 6 + ":BI" + 6 + ""].Merge = true;
					worksheet.Cells["BJ" + 6 + ":BK" + 6 + ""].Merge = true;
					worksheet.Cells["BL" + 6 + ":BM" + 6 + ""].Merge = true;
					worksheet.Cells["BN" + 6 + ":BO" + 6 + ""].Merge = true;
					worksheet.Cells["BP" + 6 + ":BQ" + 6 + ""].Merge = true;
					worksheet.Cells["BR" + 6 + ":BS" + 6 + ""].Merge = true;
					worksheet.Cells["BT" + 6 + ":BU" + 6 + ""].Merge = true;
					worksheet.Cells["BV" + 6 + ":BW" + 6 + ""].Merge = true;
					worksheet.Cells["BX" + 6 + ":BY" + 6 + ""].Merge = true;
					worksheet.Cells["BZ" + 6 + ":CA" + 6 + ""].Merge = true;
					worksheet.Cells["CB" + 6 + ":CC" + 6 + ""].Merge = true;
					worksheet.Cells["CD" + 6 + ":CE" + 6 + ""].Merge = true;
					worksheet.Cells["CF" + 6 + ":CG" + 6 + ""].Merge = true;
					worksheet.Cells["CH" + 6 + ":CI" + 6 + ""].Merge = true;
					worksheet.Cells["CJ" + 6 + ":CK" + 6 + ""].Merge = true;
					worksheet.Cells["CL" + 6 + ":CM" + 6 + ""].Merge = true;
					worksheet.Cells["CN" + 6 + ":CO" + 6 + ""].Merge = true;
					worksheet.Cells["CP" + 6 + ":CQ" + 6 + ""].Merge = true;
					worksheet.Cells["CT" + 6 + ":CU" + 6 + ""].Merge = true;
                    worksheet.Cells["CV" + 6 + ":CW" + 6 + ""].Merge = true;
                    worksheet.Cells["A" + (counter + 1) + ":i" + (counter + 1) + ""].Merge = true;
					worksheet.Cells["A" + 5 + ":DD" + 7 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + (counter + 1) + ":DD" + (counter + 1) + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 5 + ":DD" + 7 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 5 + ":DD" + 6 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["E" + 8 + ":DD" + (counter + 1) + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					
					worksheet.Cells["A" + 5 + ":DD" + (counter + 1) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":DD" + (counter + 1) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":DD" + (counter + 1) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":DD" + (counter + 1) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    foreach (var cell in worksheet.Cells["E" + 8 + ":DD" + (counter + 1) + ""])
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