using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Linq;
using OfficeOpenXml;
using System.Data;
using System.Globalization;
using OfficeOpenXml.Style;
using Infrastructure.Domain.Queries;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconMonitoringOut
{
    public class GetXLSGarmentSubconMonitoringOutQueryHandler : IQueryHandler<GetXLSGarmentSubconMonitoringOutQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentSubconCustomsInRepository garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository garmentSubconCustomsInItemRepository;
        private readonly IGarmentSubconCustomsOutRepository garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconContractRepository garmentSubconContractRepository;
        private readonly IGarmentSubconContractItemRepository garmentSubconContractItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutRepository garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutDetailRepository garmentSubconDeliveryLetterOutDetailRepository;
        private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
        private readonly IGarmentServiceSubconCuttingRepository garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository garmentServiceSubconCuttingItemRepository;
        private readonly IGarmentServiceSampleCuttingRepository garmentServiceSampleCuttingRepository;
        private readonly IGarmentServiceSampleCuttingItemRepository garmentServiceSampleCuttingItemRepository;
        private readonly IGarmentFinishingInRepository garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository garmentFinishingInItemRepository;
        private readonly IGarmentServiceSubconExpenditureGoodRepository garmentServiceSubconExpenditureGoodRepository;
        private readonly IGarmentServiceSubconExpenditureGoodtemRepository garmentServiceSubconExpenditureGoodItemRepository;
        private readonly IGarmentServiceSubconShrinkagePanelRepository garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconShrinkagePanelItemRepository garmentServiceSubconShrinkagePanelItemRepository;
        private readonly IGarmentServiceSubconFabricWashRepository garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconFabricWashItemRepository garmentServiceSubconFabricWashItemRepository;
        private readonly IGarmentServiceSubconFabricWashDetailRepository garmentServiceSubconFabricWashDetailRepository;
        public GetXLSGarmentSubconMonitoringOutQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
            garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            garmentSubconContractItemRepository = storage.GetRepository<IGarmentSubconContractItemRepository>();
            garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            garmentSubconDeliveryLetterOutItemRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            garmentSubconDeliveryLetterOutDetailRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutDetailRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
            garmentServiceSampleCuttingRepository = storage.GetRepository<IGarmentServiceSampleCuttingRepository>();
            garmentServiceSampleCuttingItemRepository = storage.GetRepository<IGarmentServiceSampleCuttingItemRepository>();
            garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            garmentServiceSubconExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodRepository>();
            garmentServiceSubconExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodtemRepository>();
            garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            garmentServiceSubconShrinkagePanelItemRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelItemRepository>();
            garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            garmentServiceSubconFabricWashItemRepository = storage.GetRepository<IGarmentServiceSubconFabricWashItemRepository>();
            garmentServiceSubconFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSubconFabricWashDetailRepository>();



            _http = serviceProvider.GetService<IHttpClientService>();
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
            public string bonNo { get; internal set; }
            public string roNo { get; internal set; }
            public double subconContractQuantity { get; set; }
            public int UENId { get; set; }
            public string UENNo { get; set; }
        }

        public class monitoringViewINTemp
        {
            public string bcNoIn { get; set; }
            public DateTimeOffset bcDateIn { get; set; }
            public double quantityIn { get; set; }
            public string uomUnitIn { get; set; }
            public string fintype { get; set; }
            public string boNo { get; set; }
            public string roNo { get; set; }
            public string productName { get; set; }

            public string gamentDONo { get; set; }

            public string urnNo { get; set; }
            public int gamentDOId { get; set; }
            public string subconContractId { get; set; }

        }

        public class RoTemp
        {
            public int id { get; set; }
            public string roNo { get; set; }
            public string uenNo { get; set; }

        }
        public async Task<List<monitoringViewINTemp>> GetBCInByContractNo(string contractNo, string subconContractType, string subconCategory, string token)
        {
            List<monitoringViewINTemp> data = new List<monitoringViewINTemp>();
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint;
            var garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-beacukai/subcon-monitoring-by-contractNo?contractNo={contractNo}&subconContractType={subconContractType}&subconCategory={subconCategory}";

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

        public async Task<List<RoTemp>> GetROInByUENId(string UENId, string UENNo, string token)
        {
            List<RoTemp> data = new List<RoTemp>();
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint;
            var garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-unit-expenditure-notes/get-ro?uenId={UENId}&uenNo={UENNo}";

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
                    data = JsonConvert.DeserializeObject<List<RoTemp>>(content.GetValueOrDefault("data").ToString());
                }
            }
            else
            {
                var err = await httpResponse.Content.ReadAsStringAsync();
            }
            return data;
        }
        public async Task<List<monitoringViewINTemp>> GetBCforFinInSubcon(string contractNo, string token)
        {
            List<monitoringViewINTemp> data = new List<monitoringViewINTemp>();
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint;
            var garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-beacukai/get-bon-fin-in-subcon?contractNo={contractNo}";

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

        public async Task<List<monitoringViewINTemp>> GetDOUrn(string contractNo, string subconContractType, string subconCategory, string token)
        {
            List<monitoringViewINTemp> data = new List<monitoringViewINTemp>();
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint;
            var garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-beacukai/get-bc-do-urn-subcon?contractNo={contractNo}&subconContractType={subconContractType}&subconCategory={subconCategory}";

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



        public async Task<MemoryStream> Handle(GetXLSGarmentSubconMonitoringOutQuery request, CancellationToken cancellationToken)
        {
            //GarmentSubconMonitoringOutViewModel listViewModel = new GarmentSubconMonitoringOutViewModel();
            //List<GarmentSubconMonitoringOutDto> monitoringDtos = new List<GarmentSubconMonitoringOutDto>();
            //List<GarmentSubconMonitoringOutDto> monitoringDtosOut = new List<GarmentSubconMonitoringOutDto>();

            //var QueryKeluar = from a in garmentSubconCustomsOutRepository.Query
            //                  join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
            //                  join c in garmentSubconContractRepository.Query on a.SubconContractId equals c.Identity
            //                  where a.SubconContractNo == request.subconcontractNo
            //                  select new monitoringViewTemp
            //                  {
            //                      bcDateOut = a.CustomsOutDate,
            //                      bcNoOut = a.CustomsOutNo,
            //                      quantityOut = b.Quantity,
            //                      uomOut = c.UomUnit,
            //                      jobtype = c.JobType,
            //                      subconNo = c.ContractNo,
            //                      bpjNo = c.BPJNo,
            //                      SubconDLOutNo = b.SubconDLOutNo,
            //                      subconContractQuantity = c.Quantity,
            //                  };

            //var QueryKeluar2 = from a in garmentSubconContractRepository.Query
            //                   join b in garmentSubconContractItemRepository.Query on a.Identity equals b.SubconContractId
            //                   where a.ContractNo == request.subconcontractNo
            //                   select new monitoringViewTemp
            //                   {
            //                       quantityOut = b.Quantity,
            //                       uomOut = b.UomUnit,
            //                       jobtype = b.ProductCode + "-" + b.ProductName,
            //                       subconNo = a.ContractNo,
            //                       bpjNo = a.BPJNo,
            //                       dueDate = a.DueDate,
            //                       subconContractQuantity = a.Quantity
            //                   };

            //var QueryKeluar3 = QueryKeluar.Union(QueryKeluar2).AsEnumerable();

            //var QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.token);

            //foreach (var i in QueryKeluar3)
            //{
            //    GarmentSubconMonitoringOutDto dto = new GarmentSubconMonitoringOutDto
            //    {
            //        bcDateOut = i.bcDateOut,
            //        bcNoOut = i.bcNoOut,
            //        quantityOut = i.quantityOut,
            //        uomOut = i.uomOut,
            //        jobType = i.jobtype,
            //        subconNo = i.subconNo,
            //        bonNo = i.SubconDLOutNo,
            //        bpjNo = i.bpjNo,
            //        dueDate = i.dueDate,
            //        subconContractQuantity = i.subconContractQuantity,
            //    };

            //    monitoringDtosOut.Add(dto);
            //}

            //foreach (var i in QueryMasuk)
            //{
            //    GarmentSubconMonitoringOutDto dto = new GarmentSubconMonitoringOutDto
            //    {
            //        bcDateOut = i.bcDateIn,
            //        bcNoOut = i.bcNoIn,
            //        quantityOut = i.quantityIn,
            //        uomOut = "PCS",
            //        jobType = i.fintype,
            //        gamentDONo = i.gamentDONo,
            //        //bpjNo = i.bpjNo,
            //        //dueDate = i.dueDate,
            //        //subconContractQuantity = i.subconContractQuantity,
            //    };

            //    monitoringDtos.Add(dto);
            //}

            //listViewModel.garmentSubconMonitoringOutDtosIn = monitoringDtos;
            //listViewModel.garmentSubconMonitoringOutDtosOut = monitoringDtosOut;

            GarmentSubconMonitoringOutViewModel listViewModel = new GarmentSubconMonitoringOutViewModel();
            List<GarmentSubconMonitoringOutDto> monitoringDtos = new List<GarmentSubconMonitoringOutDto>();
            List<GarmentSubconMonitoringOutDto> monitoringDtosOut = new List<GarmentSubconMonitoringOutDto>();
            //IEnumerable<monitoringViewTemp> QueryKeluar3;
            List<monitoringViewTemp> QueryKeluar3 = new List<monitoringViewTemp>();// = new IQueryable<monitoringViewTemp>() ;
            List<monitoringViewINTemp> QueryMasuk = new List<monitoringViewINTemp>();// = new IQueryable<monitoringViewTemp>() ;

            //if (

            //    (request.subconContractType == "SUBCON JASA" && (request.subconCategory == "SUBCON JASA GARMENT WASH" || request.subconCategory == "SUBCON JASA BARANG JADI"))

            //    ||

            //    (request.subconContractType == "SUBCON BAHAN BAKU" && (request.subconCategory == "SUBCON BB SHRINKAGE/PANEL" || request.subconCategory == "SUBCON BB FABRIC WASH/PRINT"))

            //    )
            //{

            //    var QueryKeluar = from a in garmentSubconCustomsOutRepository.Query
            //                      join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
            //                      join c in garmentSubconContractRepository.Query on a.SubconContractId equals c.Identity
            //                      where a.SubconContractNo == request.subconcontractNo
            //                      select new monitoringViewTemp
            //                      {
            //                          bcDateOut = a.CustomsOutDate,
            //                          bcNoOut = a.CustomsOutNo,
            //                          quantityOut = b.Quantity,
            //                          uomOut = c.UomUnit,
            //                          jobtype = c.JobType,
            //                          subconNo = c.ContractNo,
            //                          bpjNo = c.BPJNo,
            //                          bonNo = b.SubconDLOutNo,
            //                          subconContractQuantity = c.Quantity,
            //                      };

            //    var QueryKeluar2 = from a in garmentSubconContractRepository.Query
            //                       join b in garmentSubconContractItemRepository.Query on a.Identity equals b.SubconContractId
            //                       where a.ContractNo == request.subconcontractNo
            //                       select new monitoringViewTemp
            //                       {
            //                           quantityOut = b.Quantity,
            //                           uomOut = b.UomUnit,
            //                           jobtype = b.ProductCode + "-" + b.ProductName,
            //                           subconNo = a.ContractNo,
            //                           bpjNo = a.BPJNo,
            //                           dueDate = a.DueDate,
            //                           subconContractQuantity = a.Quantity
            //                       };

            //    QueryKeluar3 = QueryKeluar.Union(QueryKeluar2).ToList();
            //    QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);

            //}

            if (request.subconContractType == "SUBCON GARMENT" && request.subconCategory == "SUBCON CUTTING SEWING")
            {

                var QueryKeluar = (from a in garmentSubconCustomsOutRepository.Query
                                   join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                                   join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                                   join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId
                                   join i in garmentSubconContractRepository.Query on a.SubconContractId equals i.Identity

                                   where i.ContractNo == request.subconcontractNo
                                   //where a.SubconContractNo == request.subconcontractNo
                                   select new monitoringViewTemp
                                   {
                                       bcDateOut = a.CustomsOutDate,
                                       bcNoOut = a.CustomsOutNo,
                                       quantityOut = b.Quantity,
                                       uomOut = d.UomUnit,
                                       jobtype = d.ProductName,
                                       subconNo = c.ContractNo,
                                       //bpjNo = c.BPJNo,
                                       bonNo = b.SubconDLOutNo,
                                       subconContractQuantity = d.Quantity,
                                       UENId = d.UENId

                                   });

                var UENId = string.Join(",", QueryKeluar.Select(x => x.UENId).Distinct());

                var GetROFromUEN = await GetROInByUENId(UENId, null, request.token);

                //BUK Subcon
                QueryKeluar3 = (from a in QueryKeluar
                                join b in GetROFromUEN on a.UENId equals b.id
                                select new monitoringViewTemp
                                {
                                    bcDateOut = a.bcDateOut,
                                    bcNoOut = a.bcNoOut,
                                    quantityOut = a.quantityOut,
                                    uomOut = a.uomOut,
                                    jobtype = a.jobtype,
                                    subconNo = a.subconNo,
                                    //bpjNo = c.BPJNo,
                                    bonNo = a.bonNo,
                                    subconContractQuantity = a.subconContractQuantity,
                                    UENId = a.UENId,
                                    roNo = b.roNo

                                }).ToList();

                //var getIdSubconContract = garmentSubconContractRepository.Query.FirstOrDefault(x => x.ContractNo == request.subconcontractNo);


                // Bon Fininshing In Subcon
                var getBc = await GetBCforFinInSubcon(request.subconcontractNo, request.token);

                var queryInFinishingInSubcon = (from a in garmentFinishingInRepository.Query
                                                join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                                join c in getBc on a.DOId equals c.gamentDOId
                                                join d in garmentSubconContractRepository.Query on c.subconContractId.ToString() equals d.Identity.ToString()
                                                where
                                                a.FinishingInType.ToLower() == "pembelian"
                                                && d.IsCustoms

                                                select new monitoringViewINTemp
                                                {
                                                    bcDateIn = c.bcDateIn,
                                                    bcNoIn = c.bcNoIn,
                                                    boNo = a.FinishingInNo,
                                                    roNo = a.RONo,
                                                    productName = b.ProductName,
                                                    quantityIn = b.Quantity,
                                                    uomUnitIn = b.UomUnit
                                                }

                    );

                //BUM sisa Subcon Fabric & ACC
                var getBCDOUrn = await GetDOUrn(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
                var queryInSubconRemain = (from a in getBCDOUrn
                                           join b in garmentSubconContractRepository.Query on a.subconContractId.ToString() equals b.Identity.ToString()
                                           where b.IsCustoms

                                           select new monitoringViewINTemp
                                           {
                                               bcDateIn = a.bcDateIn,
                                               bcNoIn = a.bcNoIn,
                                               boNo = a.urnNo,
                                               roNo = a.roNo,
                                               productName = a.productName,
                                               quantityIn = a.quantityIn,
                                               uomUnitIn = a.uomUnitIn
                                           }



                    );


                QueryMasuk = queryInFinishingInSubcon.Union(queryInSubconRemain).ToList();

            }

            else if (request.subconContractType == "SUBCON GARMENT" && request.subconCategory == "SUBCON SEWING")
            {
                //Bon Cutting OUT Subcon
                var Query = (from a in garmentSubconCustomsOutRepository.Query
                             join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                             join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                             join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId
                             join e in garmentSubconDeliveryLetterOutDetailRepository.Query on d.Identity equals e.SubconDeliveryLetterOutItemId
                             join f in garmentCuttingOutRepository.Query on d.SubconId equals f.Identity
                             join g in garmentCuttingOutItemRepository.Query on f.Identity equals g.CutOutId
                             join h in garmentCuttingOutDetailRepository.Query on g.Identity equals h.CutOutItemId
                             join i in garmentSubconContractRepository.Query on a.SubconContractId equals i.Identity

                             where i.ContractNo == request.subconcontractNo
                             select new monitoringViewTemp
                             {
                                 bcDateOut = a.CustomsOutDate,
                                 bcNoOut = a.CustomsOutNo,
                                 bonNo = d.SubconNo,
                                 roNo = d.RONo,
                                 jobtype = g.ProductName,
                                 quantityOut = d.Quantity,
                                 uomOut = h.CuttingOutUomUnit,

                                 //subconNo = c.ContractNo,
                                 //bpjNo = c.BPJNo,

                                 //subconContractQuantity = d.Quantity,
                                 //UENId = d.UENId

                             }).ToList();
                //BUK SUBCON(ACC)
                var Query1 = (from a in garmentSubconCustomsOutRepository.Query
                              join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                              join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                              join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId
                              join e in garmentSubconDeliveryLetterOutDetailRepository.Query on d.Identity equals e.SubconDeliveryLetterOutItemId
                              join i in garmentSubconContractRepository.Query on a.SubconContractId equals i.Identity

                              where i.ContractNo == request.subconcontractNo


                              // where a.SubconContractNo == request.subconcontractNo
                              select new monitoringViewTemp
                              {
                                  bcDateOut = a.CustomsOutDate,
                                  bcNoOut = a.CustomsOutNo,
                                  bonNo = e.UENNo,
                                  //roNo = d.RONo,
                                  jobtype = e.ProductName,
                                  quantityOut = e.Quantity,
                                  uomOut = e.UomOutUnit,
                                  UENId = e.UENItemId

                              });

                var UENId = string.Join(",", Query1.Select(x => x.UENId).Distinct());

                var GetROFromUEN = GetROInByUENId(UENId, null, request.token);

                var Query1join = (from a in Query1
                                  join b in GetROFromUEN.Result on a.UENId equals b.id
                                  select new monitoringViewTemp
                                  {
                                      bcDateOut = a.bcDateOut,
                                      bcNoOut = a.bcNoOut,
                                      bonNo = a.bonNo,
                                      roNo = b.roNo,
                                      jobtype = a.jobtype,
                                      quantityOut = a.quantityOut,
                                      uomOut = a.uomOut,

                                  }).ToList();

                QueryKeluar3 = Query.Union(Query1join).ToList();

                //Bon Finishing In Subcon
                var getBc = await GetBCforFinInSubcon(request.subconcontractNo, request.token);

                var queryInFinishingInSubcon = (from a in garmentFinishingInRepository.Query
                                                join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                                join c in getBc on a.DOId equals c.gamentDOId
                                                join d in garmentSubconContractRepository.Query on c.subconContractId.ToString() equals d.Identity.ToString()
                                                where
                                                a.FinishingInType.ToLower() == "pembelian"
                                                && d.IsCustoms

                                                select new monitoringViewINTemp
                                                {
                                                    bcDateIn = c.bcDateIn,
                                                    bcNoIn = c.bcNoIn,
                                                    boNo = a.FinishingInNo,
                                                    roNo = a.RONo,
                                                    productName = b.ProductName,
                                                    quantityIn = b.Quantity,
                                                    uomUnitIn = b.UomUnit
                                                }

                    );
                //BUM Sisa Subcon
                var getBCDOUrn = await GetDOUrn(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
                var queryInSubconRemain = (from a in getBCDOUrn
                                           join b in garmentSubconContractRepository.Query on a.subconContractId.ToString() equals b.Identity.ToString()
                                           where b.IsCustoms

                                           select new monitoringViewINTemp
                                           {
                                               bcDateIn = a.bcDateIn,
                                               bcNoIn = a.bcNoIn,
                                               boNo = a.urnNo,
                                               roNo = a.roNo,
                                               productName = a.productName,
                                               quantityIn = a.quantityIn,
                                               uomUnitIn = a.uomUnitIn
                                           }



                    );


                QueryMasuk = queryInFinishingInSubcon.Union(queryInSubconRemain).ToList();


            }
            else if (request.subconContractType == "SUBCON JASA" && request.subconCategory == "SUBCON JASA KOMPONEN")
            {
                //BON PackingList  
                var Query = (from a in garmentSubconCustomsOutRepository.Query
                             join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                             join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                             join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId
                             join f in garmentServiceSubconCuttingRepository.Query on d.SubconId equals f.Identity
                             join g in garmentServiceSubconCuttingItemRepository.Query on f.Identity equals g.ServiceSubconCuttingId
                             join h in garmentSubconContractRepository.Query on a.SubconContractId equals h.Identity
                             where h.ContractNo == request.subconcontractNo
                             select new monitoringViewTemp
                             {
                                 bcDateOut = a.CustomsOutDate,
                                 bcNoOut = a.CustomsOutNo,
                                 bonNo = d.SubconNo,
                                 roNo = g.RONo,
                                 jobtype = h.JobType,
                                 quantityOut = d.Quantity,
                                 uomOut = h.UomUnit,
                                 // UENId = e.UENItemId

                             });
                var QuerySample = (from a in garmentSubconCustomsOutRepository.Query
                                   join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                                   join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                                   join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId
                                   join f in garmentServiceSampleCuttingRepository.Query on d.SubconId equals f.Identity
                                   join g in garmentServiceSampleCuttingItemRepository.Query on f.Identity equals g.ServiceSampleCuttingId
                                   join h in garmentSubconContractRepository.Query on a.SubconContractId equals h.Identity
                                   where a.SubconContractNo == request.subconcontractNo
                                   select new monitoringViewTemp
                                   {
                                       bcDateOut = a.CustomsOutDate,
                                       bcNoOut = a.CustomsOutNo,
                                       bonNo = d.SubconNo,
                                       roNo = g.RONo,
                                       jobtype = h.JobType,
                                       quantityOut = d.Quantity,
                                       uomOut = h.UomUnit,
                                       // UENId = e.UENItemId
                                   }


                    );
                var QueryUnionPL = Query.Union(QuerySample);

                //BUK ACC
                var QueryBUK = (from a in garmentSubconCustomsOutRepository.Query
                                join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                                join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                                join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId
                                join e in garmentSubconDeliveryLetterOutDetailRepository.Query on d.Identity equals e.SubconDeliveryLetterOutItemId
                                where a.SubconContractNo == request.subconcontractNo
                                select new monitoringViewTemp
                                {

                                    bcDateOut = a.CustomsOutDate,
                                    bcNoOut = a.CustomsOutNo,
                                    bonNo = e.UENNo,
                                    //roNo = g.RONo,
                                    jobtype = e.ProductName,
                                    quantityOut = e.Quantity,
                                    uomOut = e.UomUnit,
                                    UENId = e.UENId
                                });

                var UENId = string.Join(",", QueryBUK.Select(x => x.UENId).Distinct());

                var GetROFromUEN = GetROInByUENId(UENId, null, request.token);

                var QueryBUKJoin = (from a in QueryBUK
                                    join b in GetROFromUEN.Result on a.UENId equals b.id
                                    select new monitoringViewTemp
                                    {
                                        bcDateOut = a.bcDateOut,
                                        bcNoOut = a.bcNoOut,
                                        bonNo = a.bonNo,
                                        roNo = b.roNo,
                                        jobtype = a.jobtype,
                                        quantityOut = a.quantityOut,
                                        uomOut = a.uomOut,

                                    });

                QueryKeluar3 = QueryUnionPL.Union(QueryBUKJoin).ToList();

                var getBCDOUrn = await GetDOUrn(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
                var queryInSubconRemain = (from a in getBCDOUrn
                                           join b in garmentSubconContractRepository.Query on a.subconContractId.ToString() equals b.Identity.ToString()
                                           where b.IsCustoms

                                           select new monitoringViewINTemp
                                           {
                                               bcDateIn = a.bcDateIn,
                                               bcNoIn = a.bcNoIn,
                                               boNo = a.urnNo,
                                               roNo = a.roNo,
                                               productName = a.productName,
                                               quantityIn = a.quantityIn,
                                               uomUnitIn = a.uomUnitIn
                                           }



                    );

                QueryMasuk = queryInSubconRemain.ToList();

            }
            else if (request.subconContractType == "SUBCON JASA" && request.subconCategory == "SUBCON JASA GARMENT WASH")
            {
                var Query = (from a in garmentSubconCustomsOutRepository.Query
                             join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                             join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                             join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId

                             join e in garmentSubconContractRepository.Query on a.SubconContractId equals e.Identity
                             where e.ContractNo == request.subconcontractNo
                             select new monitoringViewTemp
                             {
                                 bcDateOut = a.CustomsOutDate,
                                 bcNoOut = a.CustomsOutNo,
                                 bonNo = c.DLNo,
                                 roNo = d.RONo,
                                 jobtype = e.JobType,
                                 quantityOut = d.Quantity,
                                 uomOut = e.UomUnit,
                                 // UENId = e.UENItemId

                             });
                QueryKeluar3 = Query.ToList();
                var QueryMasuk1 = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);

                QueryMasuk = QueryMasuk1.Select(a => new monitoringViewINTemp()
                {
                    bcDateIn = a.bcDateIn,
                    bcNoIn = a.bcNoIn,
                    boNo = a.gamentDONo,
                    roNo = a.roNo,
                    productName = a.productName,
                    quantityIn = a.quantityIn,
                    uomUnitIn = a.uomUnitIn

                }).ToList();
            }
            else if (request.subconContractType == "SUBCON JASA" && request.subconCategory == "SUBCON JASA BARANG JADI")
            {

                var Query = (from a in garmentSubconCustomsOutRepository.Query
                             join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                             join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                             join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId

                             join e in garmentSubconContractRepository.Query on a.SubconContractId equals e.Identity
                             join f in garmentServiceSubconExpenditureGoodRepository.Query on d.SubconId equals f.Identity
                             join g in garmentServiceSubconExpenditureGoodItemRepository.Query on f.Identity equals g.ServiceSubconExpenditureGoodId

                             where e.ContractNo == request.subconcontractNo
                             select new monitoringViewTemp
                             {
                                 bcDateOut = a.CustomsOutDate,
                                 bcNoOut = a.CustomsOutNo,
                                 bonNo = c.DLNo,
                                 roNo = g.RONo,
                                 jobtype = e.JobType,
                                 quantityOut = d.Quantity,
                                 uomOut = e.UomUnit,
                                 // UENId = e.UENItemId

                             });
                QueryKeluar3 = Query.ToList();

                var QueryMasuk1 = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
                QueryMasuk = QueryMasuk1.Select(a => new monitoringViewINTemp()
                {
                    bcDateIn = a.bcDateIn,
                    bcNoIn = a.bcNoIn,
                    boNo = a.gamentDONo,
                    roNo = a.roNo,
                    productName = a.productName,
                    quantityIn = a.quantityIn,
                    uomUnitIn = a.uomUnitIn

                }).ToList();
            }

            else if (request.subconContractType == "SUBCON BAHAN BAKU" && request.subconCategory == "SUBCON BB SHRINKAGE/PANEL")
            {
                var Query = (from a in garmentSubconCustomsOutRepository.Query
                             join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                             join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                             join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId

                             join e in garmentSubconContractRepository.Query on a.SubconContractId equals e.Identity
                             join f in garmentServiceSubconShrinkagePanelRepository.Query on d.SubconId equals f.Identity
                             join g in garmentServiceSubconShrinkagePanelItemRepository.Query on f.Identity equals g.ServiceSubconShrinkagePanelId


                             where e.ContractNo == request.subconcontractNo
                             select new monitoringViewTemp
                             {
                                 bcDateOut = a.CustomsOutDate,
                                 bcNoOut = a.CustomsOutNo,
                                 bonNo = c.DLNo,
                                 //roNo = g.RONo,
                                 jobtype = e.JobType,
                                 quantityOut = d.Quantity,
                                 uomOut = e.UomUnit,
                                 UENNo = g.UnitExpenditureNo,



                             });
                var UENNo = string.Join(",", Query.Select(x => x.UENNo).Distinct());

                var GetROFromUEN = await GetROInByUENId(null, UENNo, request.token);
                var Queryjoin = (from a in Query
                                 join b in GetROFromUEN on a.UENNo equals b.uenNo
                                 select new monitoringViewTemp
                                 {
                                     bcDateOut = a.bcDateOut,
                                     bcNoOut = a.bcNoOut,
                                     bonNo = a.bonNo,
                                     roNo = b.roNo,
                                     jobtype = a.jobtype,
                                     quantityOut = a.quantityOut,
                                     uomOut = a.uomOut,

                                 }).ToList();
                QueryKeluar3 = Queryjoin.ToList();
                //QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
                var QueryMasuk1 = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
                QueryMasuk = QueryMasuk1.Select(a => new monitoringViewINTemp()
                {
                    bcDateIn = a.bcDateIn,
                    bcNoIn = a.bcNoIn,
                    boNo = a.gamentDONo,
                    roNo = a.roNo,
                    productName = a.productName,
                    quantityIn = a.quantityIn,
                    uomUnitIn = a.uomUnitIn

                }).ToList();
            }
            else if (request.subconContractType == "SUBCON BAHAN BAKU" && request.subconCategory == "SUBCON BB FABRIC WASH/PRINT")
            {
                var Query = (from a in garmentSubconCustomsOutRepository.Query
                             join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                             join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                             join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId
                             join e in garmentSubconContractRepository.Query on a.SubconContractId equals e.Identity
                             join f in garmentServiceSubconFabricWashRepository.Query on d.SubconId equals f.Identity
                             join g in garmentServiceSubconFabricWashItemRepository.Query on f.Identity equals g.ServiceSubconFabricWashId
                             join h in garmentServiceSubconFabricWashDetailRepository.Query on g.Identity equals h.ServiceSubconFabricWashItemId

                             where e.ContractNo == request.subconcontractNo
                             select new monitoringViewTemp
                             {
                                 bcDateOut = a.CustomsOutDate,
                                 bcNoOut = a.CustomsOutNo,
                                 bonNo = c.DLNo,
                                 //roNo = g.RONo,
                                 jobtype = e.JobType,
                                 quantityOut = (double)h.Quantity,
                                 uomOut = e.UomUnit,
                                 UENNo = g.UnitExpenditureNo

                             }
                             );
                var UENNo = string.Join(",", Query.Select(x => x.UENNo).Distinct());
                var GetROFromUEN = GetROInByUENId(null, UENNo, request.token);
                var Queryjoin = (from a in Query
                                 join b in GetROFromUEN.Result on a.UENId equals b.id
                                 select new monitoringViewTemp
                                 {
                                     bcDateOut = a.bcDateOut,
                                     bcNoOut = a.bcNoOut,
                                     bonNo = a.bonNo,
                                     roNo = b.roNo,
                                     jobtype = a.jobtype,
                                     quantityOut = a.quantityOut,
                                     uomOut = a.uomOut,

                                 }).ToList();
                QueryKeluar3 = Queryjoin.ToList();
                //QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
                var QueryMasuk1 = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
                QueryMasuk = QueryMasuk1.Select(a => new monitoringViewINTemp()
                {
                    bcDateIn = a.bcDateIn,
                    bcNoIn = a.bcNoIn,
                    boNo = a.gamentDONo,
                    roNo = a.roNo,
                    productName = a.productName,
                    quantityIn = a.quantityIn,
                    uomUnitIn = a.uomUnitIn

                }).ToList();
            }

            //QueryKeluar3 = QueryKeluar3.AsEnumerable();


            //var QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);

            foreach (var i in QueryKeluar3)
            {
                GarmentSubconMonitoringOutDto dto = new GarmentSubconMonitoringOutDto
                {
                    bcDateOut = i.bcDateOut,
                    bcNoOut = i.bcNoOut,
                    quantityOut = i.quantityOut,
                    uomOut = i.uomOut,
                    jobType = i.jobtype,
                    subconNo = i.subconNo,
                    bonNo = i.bonNo,
                    bpjNo = i.bpjNo,
                    dueDate = i.dueDate,
                    subconContractQuantity = i.subconContractQuantity,
                    roNo = i.roNo

                };

                monitoringDtosOut.Add(dto);
            }

            foreach (var i in QueryMasuk)
            {
                GarmentSubconMonitoringOutDto dto = new GarmentSubconMonitoringOutDto
                {
                    bcDateOut = i.bcDateIn,
                    bcNoOut = i.bcNoIn,
                    quantityOut = i.quantityIn,
                    uomOut = i.uomUnitIn,
                    jobType = i.fintype,
                    bonNo = i.boNo,
                    roNo = i.roNo
                    //bpjNo = i.bpjNo,
                    //dueDate = i.dueDate,
                    //subconContractQuantity = i.subconContractQuantity,
                };

                monitoringDtos.Add(dto);
            }

            listViewModel.garmentSubconMonitoringOutDtosIn = monitoringDtos;
            listViewModel.garmentSubconMonitoringOutDtosOut = monitoringDtosOut;

            var reportDataTableIN = new DataTable();
            var reportDataTableOut = new DataTable();
            var nodatatable = new DataTable();
            var headers = new string[] { "BC 261", "BC 261_1", "BON/BUKTI TRANSAKSI", "RO","JENIS BARANG SUBCON", "JUMLAH BARANG", "SATUAN" };
            var headers2 = new string[] { "BC 262", "BC 262_1", "BON/BUKTI TRANSAKSI", "RO", "JENIS BARANG HASIL SUBCON", "JUMLAH BARANG", "SATUAN" };
            var subheaders = new string[] { "NO BC", "TANGGAL" };

            for (int i = 0; i < 5; i++)
            {
                reportDataTableIN.Columns.Add(new DataColumn() { ColumnName = headers2[i], DataType = typeof(string) });
            }

            for (int i = 0; i < 5; i++)
            {
                reportDataTableOut.Columns.Add(new DataColumn() { ColumnName = headers[i], DataType = typeof(string) });
            }

            reportDataTableIN.Columns.Add(new DataColumn() { ColumnName = headers2[5], DataType = typeof(Double) });
            reportDataTableIN.Columns.Add(new DataColumn() { ColumnName = headers2[6], DataType = typeof(string) });
            reportDataTableOut.Columns.Add(new DataColumn() { ColumnName = headers[5], DataType = typeof(Double) });
            reportDataTableOut.Columns.Add(new DataColumn() { ColumnName = headers[6], DataType = typeof(string) });

            nodatatable.Columns.Add(new DataColumn { ColumnName = "NO", DataType = typeof(Double) });

            //var indexin = 1;

            foreach (var item in listViewModel.garmentSubconMonitoringOutDtosIn)
            {

                string date = item.bcDateOut == null ? "-" : item.bcDateOut.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                reportDataTableIN.Rows.Add(item.bcNoOut, date, item.bonNo, item.roNo, item.jobType, item.quantityOut, item.uomOut);
            }
            //var indexout = 1;
            foreach (var item in listViewModel.garmentSubconMonitoringOutDtosOut)
            {

                string date = item.bcDateOut == null ? "-" : item.bcDateOut.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                reportDataTableOut.Rows.Add(item.bcNoOut, date, item.bonNo, item.roNo, item.jobType, item.quantityOut, item.uomOut);
            }
            var index = 1;
            if (listViewModel.garmentSubconMonitoringOutDtosIn.Count() > listViewModel.garmentSubconMonitoringOutDtosOut.Count())
            {
                foreach (var i in listViewModel.garmentSubconMonitoringOutDtosIn)
                {
                    nodatatable.Rows.Add(index);
                    index++;
                }
            }
            else
            {
                foreach (var i in listViewModel.garmentSubconMonitoringOutDtosOut)
                {
                    nodatatable.Rows.Add(index);
                    index++;
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                var col = (char)('A' + 10);

                worksheet.Cells[$"A1:{col}1"].Value = "LAPORAN RALISASI PENGELUARAN (BC. 2.6.1) DAN PEMASUKAN (BC. 2.6.2)";
                worksheet.Cells[$"A1:{col}1"].Merge = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells[$"A2:{col}2"].Value = "SUBKONTRAK PT. DAN LIRIS KEPADA";
                worksheet.Cells[$"A2:{col}2"].Merge = true;
                worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var buyer = (from a in garmentSubconContractRepository.Query where a.ContractNo == request.subconcontractNo select a).FirstOrDefault();

                worksheet.Cells[$"A3:{col}3"].Value = buyer.SupplierName;
                worksheet.Cells[$"A3:{col}3"].Merge = true;
                worksheet.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //worksheet.Cells["A4"].Value = "NOMOR KONTRAK :"; worksheet.Cells["B4"].Value = buyer.ContractNo;
                //worksheet.Cells["A5"].Value = "BPJ NO :"; worksheet.Cells["B5"].Value = buyer.BPJNo;
                //worksheet.Cells["A6"].Value = "TGL JATUH TEMPO :"; worksheet.Cells["B6"].Value = buyer.DueDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                worksheet.Cells["B8"].Value = headers[0];
                worksheet.Cells["B8:C8"].Merge = true;
                worksheet.Cells["B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["B8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["B8:C8"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                worksheet.Cells["I8"].Value = headers2[0];
                worksheet.Cells["I8:J8"].Merge = true;
                worksheet.Cells["I8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["I8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["I8:J8"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                foreach (var i in Enumerable.Range(0, 1))
                {
                    col = (char)('A' + i);
                    worksheet.Cells[$"{col}8"].Value = "NO";
                    worksheet.Cells[$"{col}8:{col}9"].Merge = true;
                    worksheet.Cells[$"{col}8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"{col}8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"{col}8:{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                }

                foreach (var i in Enumerable.Range(0, 5))
                {
                    col = (char)('D' + i);
                    worksheet.Cells[$"{col}8"].Value = headers[i + 2];
                    worksheet.Cells[$"{col}8:{col}9"].Merge = true;
                    worksheet.Cells[$"{col}8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"{col}8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"{col}8:{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                }

                foreach (var i in Enumerable.Range(0, 5))
                {
                    col = (char)('K' + i);
                    worksheet.Cells[$"{col}8"].Value = headers2[i + 2];
                    worksheet.Cells[$"{col}8:{col}9"].Merge = true;
                    worksheet.Cells[$"{col}8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"{col}8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"{col}8:{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                }

                for (var i = 0; i < 2; i++)
                {
                    col = (char)('B' + i);
                    worksheet.Cells[$"{col}9"].Value = subheaders[i];
                    worksheet.Cells[$"{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    var col2 = (char)('I' + i);
                    worksheet.Cells[$"{col2}9"].Value = subheaders[i];
                    worksheet.Cells[$"{col2}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                }

                worksheet.Cells["B10"].LoadFromDataTable(reportDataTableOut, false, OfficeOpenXml.Table.TableStyles.Light16);
                worksheet.Cells["I10"].LoadFromDataTable(reportDataTableIN, false, OfficeOpenXml.Table.TableStyles.Light16);
                worksheet.Cells["A10"].LoadFromDataTable(nodatatable, false, OfficeOpenXml.Table.TableStyles.Light16);

                #region MergeOut              
                Dictionary<string, int> counts = new Dictionary<string, int>();
                    Dictionary<string, int> countsType = new Dictionary<string, int>();
                    //var docNo = Data.ToArray();
                    int value;

                    foreach (var a in listViewModel.garmentSubconMonitoringOutDtosOut)
                    {
                        if (counts.TryGetValue(a.bcNoOut, out value))
                        {
                            counts[a.bcNoOut]++;
                        }
                        else
                        {
                            counts[a.bcNoOut] = 1;
                        }
                    }

                    int indexMergeOut = 10;
                    foreach (KeyValuePair<string, int> b in counts)
                    {
                        worksheet.Cells["B" + indexMergeOut + ":B" + (indexMergeOut + b.Value - 1)].Merge = true;
                        worksheet.Cells["B" + indexMergeOut + ":B" + (indexMergeOut + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;



                        indexMergeOut += b.Value;

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    }
                #endregion

                #region MergeIn
                Dictionary<string, int> countsIn = new Dictionary<string, int>();
                Dictionary<string, int> countsTypeIn = new Dictionary<string, int>();
                //var docNo = Data.ToArray();
                int valueIn;

                foreach (var a in listViewModel.garmentSubconMonitoringOutDtosIn)
                {
                    if (countsIn.TryGetValue(a.bcNoOut, out valueIn))
                    {
                        countsIn[a.bcNoOut]++;
                    }
                    else
                    {
                        countsIn[a.bcNoOut] = 1;
                    }
                }

                int indexMergeIn = 10;
                foreach (KeyValuePair<string, int> b in counts)
                {
                    worksheet.Cells["I" + indexMergeIn + ":I" + (indexMergeIn + b.Value - 1)].Merge = true;
                    worksheet.Cells["I" + indexMergeIn + ":I" + (indexMergeIn + b.Value - 1)].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;



                    indexMergeIn += b.Value;

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                #endregion



                worksheet.Cells["B2"].Value = subheaders[0];
                worksheet.Cells["C2"].Value = subheaders[1];
                worksheet.Cells["H2"].Value = subheaders[0];
                worksheet.Cells["I2"].Value = subheaders[1];

                if (listViewModel.garmentSubconMonitoringOutDtosIn.Count() > listViewModel.garmentSubconMonitoringOutDtosOut.Count())
                {
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "T O T A L  . . . . . . . . . . . . . . .";
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}:D{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Merge = true;
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}:D{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}:D{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}:D{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"G{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = listViewModel.garmentSubconMonitoringOutDtosOut.Sum(x => x.quantityOut);
                    worksheet.Cells[$"N{10 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = listViewModel.garmentSubconMonitoringOutDtosIn.Sum(x => x.quantityOut);
                    //    worksheet.Cells[$"A{12 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "Kesimpulan";
                    //    worksheet.Cells[$"A{13 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "Pengeluaran dan pemasukan barang tersebut diatas";
                    //    worksheet.Cells[$"A{14 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "Sesuai dengan BC. 261 dengan BC. 262";
                    //    worksheet.Cells[$"A{15 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "Izin Subkon = ";
                    //    worksheet.Cells[$"B{15 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = buyer.Quantity;
                    //    worksheet.Cells[$"A{16 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "Realisasi = ";
                    //    worksheet.Cells[$"B{16 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = listViewModel.garmentSubconMonitoringOutDtosOut.Sum(x => x.quantityOut);
                    //    worksheet.Cells[$"A{17 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "Sisa = ";
                    //    worksheet.Cells[$"B{17 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = buyer.Quantity - listViewModel.garmentSubconMonitoringOutDtosOut.Sum(x => x.quantityOut);
                    //    worksheet.Cells[$"I{20 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = string.Format("Sukoharjo, {0}", DateTimeOffset.Now.ToString("dd MMM yyyy", new CultureInfo("id-ID")));
                    //    worksheet.Cells[$"I{21 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "Mengetahui";
                    //    worksheet.Cells[$"I{21 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //    worksheet.Cells[$"I{21 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //    worksheet.Cells[$"I{22 + listViewModel.garmentSubconMonitoringOutDtosIn.Count()}"].Value = "Pemeriksa Bea dan Cukai Pertama/Ahli Pertama";
                }
                else
                {
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "T O T A L  . . . . . . . . . . . . . . .";
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}:D{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Merge = true;
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}:D{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}:D{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}:D{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"G{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = listViewModel.garmentSubconMonitoringOutDtosOut.Sum(x => x.quantityOut);
                    worksheet.Cells[$"N{10 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = listViewModel.garmentSubconMonitoringOutDtosIn.Sum(x => x.quantityOut);
                    //    worksheet.Cells[$"A{12 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "Kesimpulan";
                    //    worksheet.Cells[$"A{13 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "Pengeluaran dan pemasukan barang tersebut diatas";
                    //    worksheet.Cells[$"A{14 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "Sesuai dengan BC. 261 dengan BC. 262";
                    //    worksheet.Cells[$"A{15 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "Izin Subkon (total barang subcon) = ";
                    //    worksheet.Cells[$"B{15 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = buyer.Quantity;
                    //    worksheet.Cells[$"A{16 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "Realisasi barang keluar = ";
                    //    worksheet.Cells[$"B{16 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = listViewModel.garmentSubconMonitoringOutDtosOut.Sum(x => x.quantityOut);
                    //    worksheet.Cells[$"A{17 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "Sisa barang belum keluar = ";
                    //    worksheet.Cells[$"B{17 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = buyer.Quantity - listViewModel.garmentSubconMonitoringOutDtosOut.Sum(x => x.quantityOut);
                    //    worksheet.Cells[$"I{20 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = string.Format("Sukoharjo, {0}", DateTimeOffset.Now.ToString("dd MMM yyyy", new CultureInfo("id-ID")));
                    //    worksheet.Cells[$"I{21 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "Mengetahui";
                    //    worksheet.Cells[$"I{21 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //    worksheet.Cells[$"I{21 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //    worksheet.Cells[$"I{22 + listViewModel.garmentSubconMonitoringOutDtosOut.Count()}"].Value = "Pemeriksa Bea dan Cukai Pertama/Ahli Pertama";
                }

                var countDataIn = listViewModel.garmentSubconMonitoringOutDtosIn.Count();
                    worksheet.Cells[$"M{10 + countDataIn + 1}"].Value = "Sesuai dengan BC.262";
                worksheet.Cells[$"M{10 + countDataIn + 2}"].Value = "Barang Keluar = ";
                worksheet.Cells[$"N{10 + countDataIn + 2}"].Value = listViewModel.garmentSubconMonitoringOutDtosOut.Sum(x => x.quantityOut);
                worksheet.Cells[$"M{10 + countDataIn + 3}"].Value = "Realisasi Barang Masuk = ";
                worksheet.Cells[$"N{10 + countDataIn + 3}"].Value = listViewModel.garmentSubconMonitoringOutDtosIn.Sum(x => x.quantityOut);
                worksheet.Cells[$"M{10 + countDataIn + 4}"].Value = "Sisa barang yang belum masuk = ";
                worksheet.Cells[$"N{10 + countDataIn + 4}"].Value = listViewModel.garmentSubconMonitoringOutDtosOut.Sum(b => b.quantityOut) - listViewModel.garmentSubconMonitoringOutDtosIn.Sum(a => a.quantityOut);

                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;
            }
        }

    }
}
