using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries
{
    public class GetMonitoringSampleFinishingQueryHandler : IQueryHandler<GetSampleFinishingMonitoringQuery, GarmentSampleFinishingMonitoringListViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingOutRepository garmentSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository garmentSewingOutItemRepository;
        private readonly IGarmentSampleFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentSamplePreparingRepository garmentPreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentSampleCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository garmentCuttingInDetailRepository;
        private readonly IGarmentSampleFinishingMonitoringReportRepository garmentMonitoringFinishingReportRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository GarmentSampleRequestProductRepository;

        public GetMonitoringSampleFinishingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            garmentMonitoringFinishingReportRepository = storage.GetRepository<IGarmentSampleFinishingMonitoringReportRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
        }

        class monitoringView
        {
            public string roJob { get; internal set; }
            public string article { get; internal set; }
            public string buyerCode { get; internal set; }
            public double qtyOrder { get; internal set; }
            public double stock { get; internal set; }
            public string style { get; internal set; }
            public double sewingQtyPcs { get; internal set; }
            public double finishingQtyPcs { get; internal set; }
            public string uomUnit { get; internal set; }
            public double remainQty { get; internal set; }
            public decimal price { get; internal set; }
        }
        class ViewBasicPrices
        {
            public string RO { get; internal set; }
            public decimal BasicPrice { get; internal set; }
            public int Count { get; internal set; }
            //Enhance Jason Aug 2021
            public double AvgBasicPrice { get; set; }
        }
        class ViewFC
        {
            public string RO { get; internal set; }
            public double FC { get; internal set; }
            public int Count { get; internal set; }
            //Enhance Jason Aug 2021
            public double AvgFC { get; set; }
        }
        public async Task<GarmentSampleFinishingMonitoringListViewModel> Handle(GetSampleFinishingMonitoringQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            dateTo = dateTo.AddHours(7);


            var sumbasicPrice = (from a in (from prep in garmentPreparingRepository.Query 
                                            select new { prep.RONo, prep.Identity })
                                 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
                                 select new { a.RONo, b.BasicPrice })
                        .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                        {
                            RO = key.RONo,
                            BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
                            Count = group.Count(),
                            AvgBasicPrice = (double)(Convert.ToDecimal(group.Sum(s => s.BasicPrice)) / group.Count())
                        });

            var sumFCs = (from a in garmentCuttingInRepository.Query
                          where a.CuttingType == "Main Fabric" &&
                          a.CuttingInDate <= dateTo
                          join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new { a.FC, a.RONo, FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC), c.CuttingInQuantity })
                       .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                       {
                           RO = key.RONo,
                           FC = group.Sum(s => (s.FCs)),
                           Count = group.Sum(s => s.CuttingInQuantity),
                           AvgFC = group.Sum(s => (s.FCs)) / group.Sum(s => s.CuttingInQuantity)
                       });
            GarmentSampleFinishingMonitoringListViewModel listViewModel = new GarmentSampleFinishingMonitoringListViewModel();
            List<GarmentSampleFinishingMonitoringDto> monitoringDtos = new List<GarmentSampleFinishingMonitoringDto>();

            var querySum = garmentMonitoringFinishingReportRepository.Read(request.unit, request.dateFrom, request.dateTo).ToList();

            foreach (var item in querySum)
            {
                GarmentSampleFinishingMonitoringDto dto = new GarmentSampleFinishingMonitoringDto
                {
                    roJob = item.RoJob,
                    article = item.Article,
                    uomUnit = item.UomUnit,
                    sewingOutQtyPcs = item.SewingQtyPcs,
                    //finishingOutQtyPcs = item.Finishing,
                    finishingOutQtyPcs = item.FinishingQtyPcs,
                    stock = item.Stock,
                    remainQty = item.Stock + item.SewingQtyPcs - item.FinishingQtyPcs
                };
                monitoringDtos.Add(dto);
            }

            listViewModel.garmentMonitorings = monitoringDtos;

            //Enhance Jason Aug 2021 : Only Show Data Needed on UI
            var data = from a in monitoringDtos
                       where a.stock > 0 || a.sewingOutQtyPcs > 0 || a.finishingOutQtyPcs > 0 || a.remainQty > 0
                       select a;

            var roList = (from a in data
                          select a.roJob).Distinct().ToList();

            var sample = from s in GarmentSampleRequestRepository.Query
                         select new
                         {
                             s.RONoSample,
                             s.ComodityName,
                            // s.BuyerCode,
                             Quantity = GarmentSampleRequestProductRepository.Query.Where(p => s.Identity == p.SampleRequestId).Sum(a => a.Quantity)
                         };

            foreach (var garment in data)
            {
                garment.style = (from sr in sample where sr.RONoSample == garment.roJob select sr.ComodityName).FirstOrDefault();

                garment.qtyOrder = (from sr in sample where sr.RONoSample == garment.roJob select sr.Quantity).FirstOrDefault();
            }
            listViewModel.garmentMonitorings = data.ToList();

            return listViewModel;
        }
    }
}
