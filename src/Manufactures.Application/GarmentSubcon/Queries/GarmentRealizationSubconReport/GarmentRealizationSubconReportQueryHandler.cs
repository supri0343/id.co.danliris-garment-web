using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using System.Net.Http;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport
{
    public class GarmentRealizationSubconReportQueryHandler : IQueryHandler<GarmentRealizationSubconReportQuery, GarmentRealizationSubconReportListViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentSubconCustomsInRepository garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository garmentSubconCustomsInItemRepository;
        private readonly IGarmentSubconCustomsOutRepository garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconContractRepository garmentSubconContractRepository;
        private readonly IGarmentSubconContractItemRepository garmentSubconContractItemRepository;

        public GarmentRealizationSubconReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
            garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            garmentSubconContractItemRepository = storage.GetRepository<IGarmentSubconContractItemRepository>();
           
            _http = serviceProvider.GetService<IHttpClientService>();
        }

        public async Task<List<monitoringViewINTemp>> GetBCInByContractNo(string contractNo, string token)
        {
            List<monitoringViewINTemp> data = new List<monitoringViewINTemp>();
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint;
            var garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-beacukai/by-contractNo?contractNo={contractNo}";

            var httpResponse = await _http.GetAsync(garmentPurchasingtUriUpdate, token);

            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);

                if (content.GetValueOrDefault("data") == null)
                {
                    data = null;
                }
                else
                {
                    data = JsonConvert.DeserializeObject<List<monitoringViewINTemp>>(content.GetValueOrDefault("data").ToString());
                }
            }
            else
            {
                var err = await httpResponse.Content.ReadAsStringAsync();
            }
            return data;
        }
        class monitoringViewTemp
        {
            public string bcNoOut { get; set; }
            public DateTimeOffset bcDateOut { get; set; }
            public double quantityOut { get; set; }
            public string uomOut { get; set; }
            public string jobtype { get; set; }
            public string subconNo { get; set; }
            public string bpjNo { get; set; }
            public DateTimeOffset dueDate { get; set; }
            public double subconContractQuantity { get; set; }
        }

        public class monitoringViewINTemp
        {
            public string bcNoIn { get; set; }
            public DateTimeOffset bcDateIn { get; set; }
            public double quantityIn { get; set; }
            public string uomIn { get; set; }
            public string fintype { get; set; }
   
        }
      

        public async Task<GarmentRealizationSubconReportListViewModel> Handle(GarmentRealizationSubconReportQuery request, CancellationToken cancellationToken)
        {
            GarmentRealizationSubconReportListViewModel listViewModel = new GarmentRealizationSubconReportListViewModel();
            List<GarmentRealizationSubconReportDto> monitoringDtos = new List<GarmentRealizationSubconReportDto>();
            List<GarmentRealizationSubconReportDto> monitoringDtosOut = new List<GarmentRealizationSubconReportDto>();

            var QueryKeluar = from a in garmentSubconCustomsOutRepository.Query
                              join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                              join c in garmentSubconContractRepository.Query on a.SubconContractId equals c.Identity
                              where a.SubconContractNo == request.subconcontractNo
                              select new monitoringViewTemp
                              {
                                  bcDateOut = a.CustomsOutDate,
                                  bcNoOut = a.CustomsOutNo,
                                  quantityOut = b.Quantity,
                                  uomOut = c.UomUnit,
                                  jobtype = c.JobType,
                                  subconNo = c.ContractNo,
                                  bpjNo = c.BPJNo,
                                  subconContractQuantity = c.Quantity,
                              };

            var QueryKeluar2 = from a in garmentSubconContractRepository.Query
                               join b in garmentSubconContractItemRepository.Query on a.Identity equals b.SubconContractId
                               where a.ContractNo == request.subconcontractNo
                               select new monitoringViewTemp
                               {
                                   quantityOut = b.Quantity,
                                   uomOut = b.UomUnit,
                                   jobtype = b.ProductCode + "-" + b.ProductName,
                                   subconNo = a.ContractNo,
                                   bpjNo = a.BPJNo,
                                   dueDate = a.DueDate,
                                   subconContractQuantity = a.Quantity
                               };

            var QueryKeluar3 = QueryKeluar.Union(QueryKeluar2).AsEnumerable();

            var QueryMasuk = await GetBCInByContractNo(request.subconcontractNo,request.token);

            //var QueryMasuk = from a in garmentSubconCustomsInRepository.Query
            //                 join b in garmentSubconCustomsInItemRepository.Query on a.Identity equals b.SubconCustomsInId
            //                 join c in garmentSubconContractRepository.Query on a.SubconContractId equals c.Identity
            //                 where a.SubconContractNo == request.subconcontractNo
            //                 select new monitoringViewTemp
            //                 {
            //                     bcDateOut = a.BcDate,
            //                     bcNoOut = a.BcNo,
            //                     quantityOut = (double)b.Quantity,
            //                     uomOut = c.UomUnit,
            //                     jobtype = c.JobType,
            //                     subconNo = c.ContractNo,
            //                     bpjNo = c.BPJNo,
            //                     dueDate = c.DueDate,
            //                 };

            var groupKeluar = QueryKeluar3.GroupBy(x => new { x.uomOut, x.jobtype, x.subconNo, x.bpjNo, x.dueDate, x.bcNoOut, x.bcDateOut, x.subconContractQuantity }, (key, group) => new monitoringViewTemp
            {
                uomOut = key.uomOut,
                jobtype = key.jobtype,
                subconNo = key.subconNo,
                bpjNo = key.bpjNo,
                dueDate = key.dueDate,
                subconContractQuantity = key.subconContractQuantity,
                bcNoOut = key.bcNoOut,
                bcDateOut = key.bcDateOut,
                quantityOut = group.Sum(s => s.quantityOut)
            }).OrderBy(x => x.bcDateOut);

            foreach (var i in groupKeluar)
            {
                GarmentRealizationSubconReportDto dto = new GarmentRealizationSubconReportDto
                {
                    bcDateOut = i.bcDateOut,
                    bcNoOut = i.bcNoOut,
                    quantityOut = i.quantityOut,
                    uomOut = i.uomOut,
                    jobType = i.jobtype,
                    subconNo = i.subconNo,
                    bpjNo = i.bpjNo,
                    dueDate = i.dueDate,
                    subconContractQuantity = i.subconContractQuantity,
                };

                monitoringDtosOut.Add(dto);
            }

            var groupMasuk = QueryMasuk.GroupBy(x => new { x.bcDateIn, x.bcNoIn, x.fintype }, (key, group) => new monitoringViewINTemp
            {
                bcDateIn = key.bcDateIn,
                bcNoIn = key.bcNoIn,
                fintype = key.fintype,
                quantityIn = group.Sum(s => s.quantityIn)
            }).OrderBy(x => x.bcDateIn);

            foreach (var i in groupMasuk)
            {
                GarmentRealizationSubconReportDto dto = new GarmentRealizationSubconReportDto
                {
                    bcDateOut = i.bcDateIn,
                    bcNoOut = i.bcNoIn,
                    quantityOut = i.quantityIn,
                    uomOut = "PCS",
                    jobType = i.fintype,
                    //subconNo = i.subconNo,
                    //bpjNo = i.bpjNo,
                    //dueDate = i.dueDate,
                    //subconContractQuantity = i.subconContractQuantity,
                };

                monitoringDtos.Add(dto);
            }

            listViewModel.garmentRealizationSubconReportDtos = monitoringDtos;
            listViewModel.garmentRealizationSubconReportDtosOUT = monitoringDtosOut;
            return listViewModel;
        }
    }
}
