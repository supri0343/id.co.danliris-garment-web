using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconMonitoringOut
{
    public class GarmentSubconMonitoringQueryHandler : IQueryHandler<GarmentSubconMonitoringOutQuery, GarmentSubconMonitoringOutViewModel>
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
        private readonly IGarmentServiceSampleFabricWashRepository garmentServiceSampleFabricWashRepository;
        private readonly IGarmentServiceSampleFabricWashItemRepository garmentServiceSampleFabricWashItemRepository;
        private readonly IGarmentServiceSampleFabricWashDetailRepository garmentServiceSampleFabricWashDetailRepository;
        private readonly IGarmentServiceSampleShrinkagePanelRepository garmentServiceSampleShrinkagePanelRepository;
        private readonly IGarmentServiceSampleShrinkagePanelItemRepository garmentServiceSampleShrinkagePanelItemRepository;
        private readonly IGarmentServiceSampleExpenditureGoodRepository garmentServiceSampleExpenditureGoodRepository;
        private readonly IGarmentServiceSampleExpenditureGoodtemRepository garmentServiceSampleExpenditureGoodItemRepository;

        //private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
        //private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
        //private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;


        public GarmentSubconMonitoringQueryHandler(IStorage storage, IServiceProvider serviceProvider)
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
            garmentServiceSampleFabricWashRepository = storage.GetRepository<IGarmentServiceSampleFabricWashRepository>();
            garmentServiceSampleFabricWashItemRepository = storage.GetRepository<IGarmentServiceSampleFabricWashItemRepository>();
            garmentServiceSampleFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSampleFabricWashDetailRepository>();
            garmentServiceSampleShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelRepository>();
            garmentServiceSampleShrinkagePanelItemRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelItemRepository>();
            garmentServiceSampleExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodRepository>();
            garmentServiceSampleExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodtemRepository>();
            //garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            //garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            //garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();



            _http = serviceProvider.GetService<IHttpClientService>();
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

        public async Task<List<RoTemp>> GetROInByUENId(string UENId, string token)
        {
            List<RoTemp> data = new List<RoTemp>();
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint;
            var garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-unit-expenditure-notes/get-ro?uenId={UENId}";

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

        public async Task<List<RoTemp>> GetROInByUENNo(string UENNo, string token)
        {
            List<RoTemp> data = new List<RoTemp>();
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint;
            var garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-beacukai/get-ro?uenNo={UENNo}";

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

        public async Task<GarmentSubconMonitoringOutViewModel> Handle(GarmentSubconMonitoringOutQuery request, CancellationToken cancellationToken)
        {
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

                var GetROFromUEN = await GetROInByUENId(UENId, request.token);

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

                var GetROFromUEN = GetROInByUENId(UENId,request.token);

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

                var GetROFromUEN = GetROInByUENId(UENId, request.token);

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
                QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
            }
            else if (request.subconContractType == "SUBCON JASA" && request.subconCategory == "SUBCON JASA BARANG JADI")
            {

                var QueryOrder = (from a in garmentSubconCustomsOutRepository.Query
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
                var QuerySample = (from a in garmentSubconCustomsOutRepository.Query
                                   join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                                   join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                                   join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId

                                   join e in garmentSubconContractRepository.Query on a.SubconContractId equals e.Identity
                                   join f in garmentServiceSampleExpenditureGoodRepository.Query on d.SubconId equals f.Identity
                                   join g in garmentServiceSampleExpenditureGoodItemRepository.Query on f.Identity equals g.ServiceSampleExpenditureGoodId

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
                var Query = QueryOrder.Union(QuerySample);
                QueryKeluar3 = Query.ToList();

                QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
            }
            
            else if (request.subconContractType == "SUBCON BAHAN BAKU" && request.subconCategory == "SUBCON BB SHRINKAGE/PANEL")
            {
                var QueryOrder = (from a in garmentSubconCustomsOutRepository.Query
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
                                 
                                 

                             }).ToList();
                var QuerySample = (from a in garmentSubconCustomsOutRepository.Query
                                  join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                                  join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                                  join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId

                                  join e in garmentSubconContractRepository.Query on a.SubconContractId equals e.Identity
                                  join f in garmentServiceSampleShrinkagePanelRepository.Query on d.SubconId equals f.Identity
                                  join g in garmentServiceSampleShrinkagePanelItemRepository.Query on f.Identity equals g.ServiceSampleShrinkagePanelId


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



                                  }).ToList();
                var Query = QueryOrder.Union(QuerySample).ToList();
                var UENNo = string.Join(",", Query.Select(x => x.UENNo).Distinct());

                //var GetROFromUEN = await GetROInByUENId(null, UENNo, request.token);
                var GetROFromUEN = await GetROInByUENNo(UENNo, request.token);
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
                QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
            }
            else if (request.subconContractType == "SUBCON BAHAN BAKU" && request.subconCategory == "SUBCON BB FABRIC WASH/PRINT")
            {
                var QueryOrder = (from a in garmentSubconCustomsOutRepository.Query
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
                             ).ToList();

                var QuerySample = (from a in garmentSubconCustomsOutRepository.Query
                             join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                             join c in garmentSubconDeliveryLetterOutRepository.Query on b.SubconDLOutId equals c.Identity
                             join d in garmentSubconDeliveryLetterOutItemRepository.Query on c.Identity equals d.SubconDeliveryLetterOutId
                             join e in garmentSubconContractRepository.Query on a.SubconContractId equals e.Identity
                             join f in garmentServiceSampleFabricWashRepository.Query on d.SubconId equals f.Identity
                             join g in garmentServiceSampleFabricWashItemRepository.Query on f.Identity equals g.ServiceSampleFabricWashId
                             join h in garmentServiceSampleFabricWashDetailRepository.Query on g.Identity equals h.ServiceSampleFabricWashItemId

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
                             ).ToList();

                var Query = QueryOrder.Union(QuerySample).ToList();
                var UENNo = string.Join(",", Query.Select(x => x.UENNo).Distinct());
                var GetROFromUEN = await GetROInByUENNo(UENNo, request.token);
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
                QueryMasuk = await GetBCInByContractNo(request.subconcontractNo, request.subconContractType, request.subconCategory, request.token);
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
                    uomOut = i.fintype =="FABRIC" ? "MTR" : i.uomUnitIn,
                    jobType = i.fintype,
                    bonNo = i.gamentDONo,
                    roNo = i.roNo
                    //bpjNo = i.bpjNo,
                    //dueDate = i.dueDate,
                    //subconContractQuantity = i.subconContractQuantity,
                };

                monitoringDtos.Add(dto);
            }

            listViewModel.garmentSubconMonitoringOutDtosIn = monitoringDtos;
            listViewModel.garmentSubconMonitoringOutDtosOut = monitoringDtosOut;
            return listViewModel;
        }
    }
}
