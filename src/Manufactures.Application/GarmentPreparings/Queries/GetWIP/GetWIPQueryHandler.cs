using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.MonitoringProductionStockFlow;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentSewingDOs.Repositories;

namespace Manufactures.Application.GarmentPreparings.Queries.GetWIP
{
    public class GetWIPQueryHandler : IQueryHandler<GetWIPQuery, GarmentWIPListViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentPreparingRepository garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
        private readonly IGarmentAvalProductRepository garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository garmentAvalProductItemRepository;
        private readonly IGarmentDeliveryReturnItemRepository garmentDeliveryReturnItemRepository;
        private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
        private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentBalanceMonitoringProductionStockFlowRepository garmentBalanceProductionStockRepository;
        private readonly IGarmentLoadingRepository garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
        private readonly IGarmentAvalComponentRepository garmentAvalComponentRepository;
        private readonly IGarmentAvalComponentItemRepository garmentAvalComponentItemRepository;
        private readonly IGarmentSewingInRepository garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository garmentSewingInItemRepository;
        private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
        private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;
        private readonly IGarmentFinishingInRepository garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository garmentFinishingInItemRepository;
        private readonly IGarmentSewingDORepository garmentSewingDORepository;
        private readonly IGarmentSewingDOItemRepository garmentSewingDOItemRepository;
        //Sample
        private readonly IGarmentSamplePreparingRepository garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository garmentSamplePreparingItemRepository;
        private readonly IGarmentSampleCuttingInRepository garmentSampleCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository garmentSampleCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository garmentSampleCuttingInDetailRepository;
        private readonly IGarmentSampleCuttingOutRepository garmentSampleCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository garmentSampleCuttingOutItemRepository;
        private readonly IGarmentSampleSewingInRepository garmentSampleSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository garmentSampleSewingInItemRepository;
        private readonly IGarmentSampleSewingOutRepository garmentSampleSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository garmentSampleSewingOutItemRepository;
        private readonly IGarmentSampleFinishingInRepository garmentSampleFinishingInRepository;
        private readonly IGarmentSampleFinishingInItemRepository garmentSampleFinishingInItemRepository;
        private readonly IGarmentSampleFinishingOutRepository garmentSampleFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository garmentSampleFinishingOutItemRepository;
        private readonly IGarmentSampleAvalProductRepository garmentSampleAvalProductRepository;
        private readonly IGarmentSampleAvalProductItemRepository garmentSampleAvalProductItemRepository;
        private readonly IGarmentSampleAvalComponentRepository garmentSampleAvalComponentRepository;
        private readonly IGarmentSampleAvalComponentItemRepository garmentSampleAvalComponentItemRepository;
        private readonly IGarmentSampleDeliveryReturnItemRepository garmentSampleDeliveryReturnItemRepository;
        public GetWIPQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
            garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            garmentBalanceProductionStockRepository = storage.GetRepository<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
            garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
            garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
            garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
            garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
            garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            //Sample
            garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            garmentSampleCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            garmentSampleCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            garmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            garmentSampleCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            garmentSampleCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            garmentSampleSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            garmentSampleSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
            garmentSampleSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            garmentSampleSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            garmentSampleFinishingInRepository = storage.GetRepository<IGarmentSampleFinishingInRepository>();
            garmentSampleFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
            garmentSampleFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            garmentSampleFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            garmentSampleAvalProductRepository = storage.GetRepository<IGarmentSampleAvalProductRepository>();
            garmentSampleAvalProductItemRepository = storage.GetRepository<IGarmentSampleAvalProductItemRepository>();
            garmentSampleAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
            garmentSampleAvalComponentItemRepository = storage.GetRepository<IGarmentSampleAvalComponentItemRepository>();
            garmentSampleDeliveryReturnItemRepository = storage.GetRepository<IGarmentSampleDeliveryReturnItemRepository>();


            _http = serviceProvider.GetService<IHttpClientService>();
        }

        public async Task<GarmentProductResult> GetProducts(string codes, string token)
        {
            GarmentProductResult garmentProduct = new GarmentProductResult();

            var httpContent = new StringContent(JsonConvert.SerializeObject(codes), Encoding.UTF8, "application/json");

            var garmentProductionUri = MasterDataSettings.Endpoint + $"master/garmentProducts/byCodes";
            var httpResponse = await _http.SendAsync(HttpMethod.Get, garmentProductionUri, token, httpContent);



            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();

                    var listdata = JsonConvert.DeserializeObject<List<GarmentProductViewModel>>(dataString);

                    foreach (var i in listdata)
                    {
                        garmentProduct.data.Add(i);
                    }
                    //garmentProduct.data = listdata;
                
                

            }
            return garmentProduct;
        }

        class monitoringViewTemp
        {
            public string itemCode { get; internal set; }
            public string unitQty { get; internal set; }
            public double Quantity { get; internal set; }
        }

        class monitoringViewTempCutting
        {
            public string ro { get; internal set; }
            public double Quantity { get; internal set; }
        }

        class monitoringViewsTemp
        {
            public string itemname { get; internal set; }
            public string itemCode { get; internal set; }
            public string unitQty { get; internal set; }
            public double Quantity { get; internal set; }
        }

        public async Task<GarmentComodityResult> GetComodities(string token)
        {
            GarmentComodityResult garmentComodities = new GarmentComodityResult();

            var garmentProductionUri = MasterDataSettings.Endpoint + $"master/garment-comodities/all";

            var httpContent = new StringContent("", Encoding.UTF8, "application/json");

            var httpResponse = await _http.SendAsync(HttpMethod.Get, garmentProductionUri, token, httpContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();

                if (dataString != null)
                {
                    var listdata = JsonConvert.DeserializeObject<List<GarmentComodity>>(dataString);

                    foreach (var i in listdata)
                    {
                        garmentComodities.data.Add(i);
                    }
                    //garmentProduct.data = listdata;
                }
            }
            return garmentComodities;
        }


        public async Task<GarmentWIPListViewModel> Handle(GetWIPQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateTo = new DateTimeOffset(request.Date);

            DateTimeOffset dateBalance = (from a in garmentBalanceProductionStockRepository.Query.OrderByDescending(s => s.CreatedDate)
                                          select a.CreatedDate).FirstOrDefault();
            GarmentWIPListViewModel listViewModel = new GarmentWIPListViewModel();
            List<GarmentWIPDto> monitoringDtos = new List<GarmentWIPDto>();

            GarmentComodityResult GarmentComodities = await GetComodities(request.token);


            #region Preparing+Sample
            var FactPreparePreparing = from a in (from aa in garmentPreparingRepository.Query
                                                      //where aa.ProcessDate.Value.Date < request.Date.Date
                                                      //where aa.ProcessDate.Value.AddHours(7).Year == request.Date.Year
                                                  where aa.ProcessDate.Value.AddHours(7) > dateBalance && aa.ProcessDate.Value.AddHours(7) <= dateTo
                                                  select aa)
                                       join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
                                       select new monitoringViewTemp
                                       {
                                           itemCode = b.ProductCode,
                                           unitQty = b.UomUnit,
                                           Quantity = b.Quantity
                                       };

            var FactPrepareCutting = from a in (from aa in garmentCuttingInRepository.Query
                                                    //where aa.CuttingInDate.Date < request.Date.Date
                                                    //where aa.CuttingInDate.AddHours(7).Year == request.Date.Year
                                                where aa.CuttingInDate.AddHours(7) > dateBalance && aa.CuttingInDate.AddHours(7) <= dateTo
                                                select aa)
                                     join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                     join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                     select new monitoringViewTemp
                                     {
                                         itemCode = c.ProductCode,
                                         unitQty = c.PreparingUomUnit,
                                         Quantity = c.PreparingQuantity * -1
                                     };
            var FactPrepareAvalProduct = from a in (from aa in garmentAvalProductRepository.Query
                                                        //where aa.AvalDate.Value.Date < request.Date.Date
                                                        //where aa.AvalDate.Value.AddHours(7).Year == request.Date.Year
                                                    where aa.AvalDate.Value.AddHours(7) > dateBalance && aa.AvalDate.Value.AddHours(7) <= dateTo
                                                    select aa)
                                         join b in garmentAvalProductItemRepository.Query on a.Identity equals b.APId
                                         select new monitoringViewTemp
                                         {
                                             itemCode = b.ProductCode,
                                             unitQty = b.UomUnit,
                                             Quantity = b.Quantity * -1
                                         };

            var FactPrepareDeliveryReturn = from a in (from aa in garmentPreparingRepository.Query
                                                           //where aa.ProcessDate.Value.Date < request.Date.Date
                                                           //where aa.ProcessDate.Value.AddHours(7).Year == request.Date.Year
                                                       where aa.ProcessDate.Value.AddHours(7) > dateBalance && aa.ProcessDate.Value.AddHours(7) <= dateTo
                                                       select aa)
                                            join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
                                            join c in garmentDeliveryReturnItemRepository.Query on b.Identity.ToString() equals c.PreparingItemId
                                            select new monitoringViewTemp
                                            {
                                                itemCode = c.ProductCode,
                                                unitQty = c.UomUnit,
                                                Quantity = c.Quantity * -1
                                            };


            #region SamplePreparing
            var FactPreparePreparingSample = from a in (from aa in garmentSamplePreparingRepository.Query
                                                            //where aa.ProcessDate.Value.Date < request.Date.Date
                                                            //where aa.ProcessDate.Value.AddHours(7).Year == request.Date.Year
                                                        where aa.ProcessDate.Value.AddHours(7) > dateBalance && aa.ProcessDate.Value.AddHours(7) <= dateTo
                                                        select aa)
                                             join b in garmentSamplePreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
                                             select new monitoringViewTemp
                                             {
                                                 itemCode = b.ProductCode,
                                                 unitQty = b.UomUnit,
                                                 Quantity = b.Quantity
                                             };
            var FactPrepareCuttingSample = from a in (from aa in garmentSampleCuttingInRepository.Query
                                                          //where aa.CuttingInDate.Date < request.Date.Date
                                                          //where aa.CuttingInDate.AddHours(7).Year == request.Date.Year
                                                      where aa.CuttingInDate.AddHours(7) > dateBalance && aa.CuttingInDate.AddHours(7) <= dateTo
                                                      select aa)
                                           join b in garmentSampleCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                           join c in garmentSampleCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                           select new monitoringViewTemp
                                           {
                                               itemCode = c.ProductCode,
                                               unitQty = c.PreparingUomUnit,
                                               Quantity = c.PreparingQuantity * -1
                                           };
            var FactPrepareAvalProductSample = from a in (from aa in garmentSampleAvalProductRepository.Query
                                                              //where aa.AvalDate.Value.Date < request.Date.Date
                                                              //where aa.AvalDate.Value.AddHours(7).Year == request.Date.Year
                                                          where aa.AvalDate.Value.AddHours(7) > dateBalance && aa.AvalDate.Value.AddHours(7) <= dateTo
                                                          select aa)
                                               join b in garmentSampleAvalProductItemRepository.Query on a.Identity equals b.APId
                                               select new monitoringViewTemp
                                               {
                                                   itemCode = b.ProductCode,
                                                   unitQty = b.UomUnit,
                                                   Quantity = b.Quantity * -1
                                               };

            var FactPrepareDeliveryReturnSample = from a in (from aa in garmentSamplePreparingRepository.Query
                                                                 //where aa.ProcessDate.Value.Date < request.Date.Date
                                                                 //where aa.ProcessDate.Value.AddHours(7).Year == request.Date.Year
                                                             where aa.ProcessDate.Value.AddHours(7) > dateBalance && aa.ProcessDate.Value.AddHours(7) <= dateTo
                                                             select aa)
                                                  join b in garmentSamplePreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
                                                  join c in garmentSampleDeliveryReturnItemRepository.Query on b.Identity.ToString() equals c.PreparingItemId
                                                  select new monitoringViewTemp
                                                  {
                                                      itemCode = c.ProductCode,
                                                      unitQty = c.UomUnit,
                                                      Quantity = c.Quantity * -1
                                                  };

            var FactPrepareTempSample = FactPreparePreparingSample.Union(FactPrepareCuttingSample).Union(FactPrepareAvalProductSample).Union(FactPrepareDeliveryReturnSample).AsEnumerable();
            //var FactPrepareTempSample2 = FactPrepareTempSample.GroupBy(x => new { x.itemCode, x.unitQty }, (key, groupdata) => new monitoringViewTemp
            //{
            //    itemCode = key.itemCode,
            //    unitQty = key.unitQty,
            //    Quantity = groupdata.Sum(x => x.Quantity)
            //});
            #endregion

            var FactPrepareTemp = FactPreparePreparing.Union(FactPrepareCutting).Union(FactPrepareAvalProduct).Union(FactPrepareDeliveryReturn).Union(FactPrepareTempSample).AsEnumerable();
            var FactPrepareTemp2 = FactPrepareTemp.GroupBy(x => new { x.itemCode, x.unitQty }, (key, groupdata) => new monitoringViewTemp
            {
                itemCode = key.itemCode,
                unitQty = key.unitQty,
                Quantity = groupdata.Sum(x => x.Quantity)
            }).ToList();

            List<monitoringViewsTemp> SakirPreparing = new List<monitoringViewsTemp>();

            FactPrepareTemp2 = FactPrepareTemp2.Where(x => x.Quantity > 0.01).Select(x => x).ToList();


            List<GarmentProductViewModel> GarmentProducts = new List<GarmentProductViewModel>();

            var code1 = string.Join(",", FactPrepareTemp2.Select(x => x.itemCode).ToList());
            GarmentProductResult GarmentProduct1 = await GetProducts(code1, request.token);

            foreach (var a in GarmentProduct1.data)
            {
                GarmentProducts.Add(a);
            }

            foreach (var a in FactPrepareTemp2.Where(x => x.Quantity > 0.01))
            {

                var GarmentProduct = GarmentProducts.FirstOrDefault(x => x.Code == a.itemCode);

                var Composition = GarmentProduct == null ? "-" : GarmentProduct.Composition;
                var Width = GarmentProduct == null ? "-" : GarmentProduct.Width;
                var Const = GarmentProduct == null ? "-" : GarmentProduct.Const;
                var Yarn = GarmentProduct == null ? "-" : GarmentProduct.Yarn;


                SakirPreparing.Add(new monitoringViewsTemp
                {
                    itemCode = a.itemCode,
                    itemname = string.Concat(Composition, "", Width, "", Const, "", Yarn),
                    Quantity = a.Quantity,
                    unitQty = a.unitQty
                });
            }
            #endregion

            #region Cutting+Sample
            //Old Query
            //var FactCutting = (from a in (from aa in garmentCuttingOutRepository.Query
            //                              //where aa.CuttingOutDate.Date < request.Date.Date
            //                              where aa.CuttingOutDate.Year == request.Date.Year
            //                              select aa)
            //                   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
            //                   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
            //                   select new
            //                   {
            //                       ComodityCode = a.ComodityCode,
            //                       ComodityName = a.ComodityName,
            //                       Quantity = c.CuttingOutQuantity
            //                   }).GroupBy(x => new { x.ComodityCode, x.ComodityName }, (key, group) => new monitoringViewsTemp
            //                   {
            //                       itemname = key.ComodityName,
            //                       itemCode = key.ComodityCode,
            //                       Quantity = group.Sum(x => x.Quantity),
            //                       unitQty = "PCS"
            //                   });

            //New Query
            var queryBalanceCuttings = (from a in garmentBalanceProductionStockRepository.Query
                                        where a.CreatedDate < dateTo && a.BeginingBalanceCuttingQty > 0
                                        select new
                                        {

                                            ComodityName = a.Comodity.TrimEnd(),
                                            Quantity = a.BeginingBalanceCuttingQty
                                        }).GroupBy(x => new { x.ComodityName }, (key, group) => new monitoringViewsTemp
                                        {
                                            itemCode = "",
                                            itemname = key.ComodityName,
                                            Quantity = group.Sum(x => x.Quantity),

                                        });

            List<monitoringViewsTemp> queryBalanceCutting = new List<monitoringViewsTemp>();

            foreach (var a in queryBalanceCuttings)
            {
                var comodity = GarmentComodities.data.FirstOrDefault(x => x.Name.TrimEnd() == a.itemname);

                a.itemCode = comodity != null ? comodity.Code : "";
                queryBalanceCutting.Add(a);

            }

            var QueryCuttingIn = (from a in (from aa in garmentCuttingInRepository.Query
                                             where aa.CuttingInDate.AddHours(7) <= dateTo && aa.CuttingType == "Main Fabric" && aa.CuttingInDate.AddHours(7) > dateBalance
                                             select new
                                             {
                                                 aa.Identity,
                                                 aa.RONo,
                                             })
                                  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                  //join d in garmentCuttingOutItemRepository.Query on a.Identity equals d.CuttingInId
                                  select new monitoringViewTempCutting
                                  {
                                      ro = a.RONo,
                                      Quantity = c.CuttingInQuantity
                                  });

            var QueryCuttingOut = (from a in (from aa in garmentCuttingOutRepository.Query
                                              where aa.CuttingOutDate.AddHours(7) <= dateTo && aa.CuttingOutDate.AddHours(7) > dateBalance
                                              select new { aa.Identity, aa.RONo })
                                   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
                                   select new monitoringViewTempCutting
                                   {
                                       ro = a.RONo,
                                       //Quantity = -b.TotalCuttingOut
                                       Quantity = -c.CuttingOutQuantity
                                   });


            var QueryAvalCompCutting = (from a in (from aa in garmentAvalComponentRepository.Query
                                                   where aa.Date.AddHours(7) <= dateTo && aa.Date.AddHours(7) > dateBalance && aa.AvalComponentType == "CUTTING"
                                                   select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                        join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                                        select new monitoringViewsTemp
                                        {
                                            itemCode = a.ComodityCode,
                                            itemname = a.ComodityName.TrimEnd(),
                                            Quantity = -b.Quantity
                                        });

            var QueryCuttingAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                             where aa.AdjustmentDate.AddHours(7) > dateBalance && aa.AdjustmentDate.AddHours(7) <= dateTo && aa.AdjustmentType == "CUTTING"
                                             select new { aa.Identity, aa.ComodityCode, aa.ComodityName })
                                  join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                  select new monitoringViewsTemp
                                  {
                                      itemCode = a.ComodityCode,
                                      itemname = a.ComodityName.TrimEnd(),
                                      Quantity = -b.Quantity
                                  };

            var queryCutTemp = QueryCuttingIn.Union(QueryCuttingOut).AsQueryable();

            //var sumCuting = queryCutTemp.GroupBy(x => new { x.ro }, (key, group) => new monitoringViewTempCutting
            //{
            //    ro = key.ro,
            //    Quantity = group.Sum(x => x.Quantity),
            //});


            var QueryCuttNow = (from a in queryCutTemp
                                join b in (from data in garmentCuttingOutRepository.Query
                                           select new
                                           {
                                               data.RONo,
                                               data.ComodityCode,
                                               data.ComodityName
                                           }).Distinct() on a.ro equals b.RONo
                                select new monitoringViewsTemp
                                {
                                    itemCode = b.ComodityCode,
                                    itemname = b.ComodityName.TrimEnd(),
                                    Quantity = a.Quantity,

                                }).GroupBy(x => new { x.itemCode, x.itemname }, (key, group) => new monitoringViewsTemp
                                {
                                    itemname = key.itemname,
                                    itemCode = key.itemCode,
                                    Quantity = group.Sum(x => x.Quantity),
                                });

            #region SampleCutting 
            var QueryCuttingInSample = (from a in (from aa in garmentSampleCuttingInRepository.Query
                                                   where aa.CuttingInDate.AddHours(7) <= dateTo && aa.CuttingType == "Main Fabric" && aa.CuttingInDate.AddHours(7) > dateBalance
                                                   select new
                                                   {
                                                       aa.Identity,
                                                       aa.RONo,
                                                   })
                                        join b in garmentSampleCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                        join c in garmentSampleCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                        join d in garmentSampleCuttingOutItemRepository.Query on a.Identity equals d.CuttingInId
                                        select new monitoringViewTempCutting
                                        {
                                            ro = a.RONo,
                                            Quantity = c.CuttingInQuantity
                                        });

            var QueryCuttingOutSample = (from a in (from aa in garmentSampleCuttingOutRepository.Query
                                                    where aa.CuttingOutDate.AddHours(7) <= dateTo && aa.CuttingOutDate.AddHours(7) > dateBalance
                                                    select new { aa.Identity, aa.RONo })
                                         join b in garmentSampleCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
                                         select new monitoringViewTempCutting
                                         {
                                             ro = a.RONo,
                                             Quantity = -b.TotalCuttingOut
                                         });


            var QueryAvalCompCuttingSample = (from a in (from aa in garmentSampleAvalComponentRepository.Query
                                                         where aa.Date.AddHours(7) <= dateTo && aa.Date.AddHours(7) > dateBalance && aa.SampleAvalComponentType == "CUTTING"
                                                         select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                              join b in garmentSampleAvalComponentItemRepository.Query on a.Identity equals b.SampleAvalComponentId
                                              select new monitoringViewsTemp
                                              {
                                                  itemCode = a.ComodityCode,
                                                  itemname = a.ComodityName.TrimEnd(),
                                                  Quantity = -b.Quantity
                                              });



            var queryCutTempSample = QueryCuttingInSample.Union(QueryCuttingOutSample).AsQueryable();

            //var sumCutingSample = queryCutTempSample.GroupBy(x => new { x.ro }, (key, group) => new monitoringViewTempCutting
            //{
            //    ro = key.ro,
            //    Quantity = group.Sum(x => x.Quantity),
            //});

            var QueryCuttNowSample = (from a in queryCutTempSample
                                      join b in (from data in garmentSampleCuttingOutRepository.Query
                                                 select new
                                                 {
                                                     data.RONo,
                                                     data.ComodityCode,
                                                     data.ComodityName
                                                 }).Distinct() on a.ro equals b.RONo
                                      select new monitoringViewsTemp
                                      {
                                          itemCode = b.ComodityCode,
                                          itemname = b.ComodityName.TrimEnd(),
                                          Quantity = a.Quantity,
                                      }).GroupBy(x => new { x.itemCode, x.itemname }, (key, group) => new monitoringViewsTemp
                                      {
                                          itemname = key.itemname,
                                          itemCode = key.itemCode,
                                          Quantity = group.Sum(x => x.Quantity),
                                      });
            #endregion

            var queryUnionCut = queryBalanceCutting.Union(QueryCuttNow).Union(QueryAvalCompCutting).Union(QueryCuttingAdj).Union(QueryCuttNowSample).Union(QueryAvalCompCuttingSample).AsEnumerable();

            var SakirCutting = queryUnionCut.GroupBy(x => new { x.itemCode, x.itemname }, (key, group) => new monitoringViewsTemp
            {
                itemname = key.itemname,
                itemCode = key.itemCode,
                Quantity = group.Sum(x => x.Quantity),
                unitQty = "PCS"
            }).ToList();
            #endregion

            #region Loading
            var queryBalanceLoadings = (from a in garmentBalanceProductionStockRepository.Query
                                        where a.CreatedDate < dateTo && a.BeginingBalanceLoadingQty > 0
                                        select new
                                        {

                                            ComodityName = a.Comodity.TrimEnd(),
                                            Quantity = a.BeginingBalanceLoadingQty
                                        }).GroupBy(x => new { x.ComodityName }, (key, group) => new monitoringViewsTemp
                                        {
                                            itemname = key.ComodityName,
                                            Quantity = group.Sum(x => x.Quantity),
                                        }); ;

            List<monitoringViewsTemp> queryBalanceLoading = new List<monitoringViewsTemp>();

            foreach (var a in queryBalanceLoadings)
            {
                var comodity = GarmentComodities.data.FirstOrDefault(x => x.Name.TrimEnd() == a.itemname);

                a.itemCode = comodity != null ? comodity.Code : "";

                queryBalanceLoading.Add(a);
            }

            var QueryCuttingOutForLoading = (from a in (from aa in garmentSewingDORepository.Query
                                                        where aa.SewingDODate.AddHours(7) <= dateTo && aa.SewingDODate.AddHours(7) > dateBalance
                                                        select new { aa.Identity, aa.ComodityCode, aa.ComodityName })
                                             join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
                                             select new monitoringViewsTemp
                                             {
                                                 itemCode = a.ComodityCode,
                                                 itemname = a.ComodityName.TrimEnd(),
                                                 Quantity = b.Quantity
                                             });

            var QueryLoading = (from a in (from aa in garmentLoadingRepository.Query
                                           where aa.LoadingDate.AddHours(7) <= dateTo && aa.LoadingDate.AddHours(7) > dateBalance
                                           select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
                                select new monitoringViewsTemp
                                {
                                    itemname = a.ComodityCode,
                                    itemCode = a.ComodityName.TrimEnd(),
                                    Quantity = -b.Quantity
                                });

            var QueryLoadingAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                             where aa.AdjustmentDate.AddHours(7) > dateBalance && aa.AdjustmentDate.AddHours(7) <= dateTo && aa.AdjustmentType == "LOADING"
                                             select new { aa.Identity, aa.ComodityCode, aa.ComodityName })
                                  join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                  select new monitoringViewsTemp
                                  {
                                      itemCode = a.ComodityCode,
                                      itemname = a.ComodityName.TrimEnd(),
                                      Quantity = -b.Quantity
                                  };

            var queryLoadingNow = queryBalanceLoading.Union(QueryLoading).Union(QueryCuttingOutForLoading).Union(QueryLoadingAdj).AsEnumerable();

            var SakirLoading = queryLoadingNow.GroupBy(x => new { x.itemname, x.itemCode }, (key, group) => new monitoringViewsTemp
            {
                itemname = key.itemname,
                itemCode = key.itemCode,
                Quantity = group.Sum(x => x.Quantity),
                unitQty = "PCS"
            }).ToList();

            #endregion

            #region Sewing+Sample
            var queryBalanceSewings = (from a in garmentBalanceProductionStockRepository.Query
                                       where a.CreatedDate < dateTo && a.BeginingBalanceSewingQty > 0
                                       select new monitoringViewsTemp
                                       {

                                           itemname = a.Comodity.TrimEnd(),
                                           Quantity = a.BeginingBalanceSewingQty
                                       });
            List<monitoringViewsTemp> queryBalanceSewing = new List<monitoringViewsTemp>();

            foreach (var a in queryBalanceSewings)
            {
                var comodity = GarmentComodities.data.FirstOrDefault(x => x.Name.TrimEnd() == a.itemname);

                a.itemCode = comodity != null ? comodity.Code : "";
                queryBalanceSewing.Add(a);
            }

            var QuerySewingIn = from a in (from aa in garmentSewingInRepository.Query
                                           where aa.SewingInDate.AddHours(7) > dateBalance && aa.SewingInDate.AddHours(7) <= dateTo
                                           select new { aa.Identity, aa.ComodityCode, aa.ComodityName })
                                join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
                                select new monitoringViewsTemp
                                {
                                    itemCode = a.ComodityCode,
                                    itemname = a.ComodityName.TrimEnd(),
                                    Quantity = b.Quantity
                                };

            var QuerySewingOut = from a in (from aa in garmentSewingOutRepository.Query
                                            where aa.SewingOutDate.AddHours(7) > dateBalance && aa.SewingOutDate.AddHours(7) <= dateTo
                                            select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                 join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
                                 select new monitoringViewsTemp
                                 {
                                     itemname = a.ComodityName.TrimEnd(),
                                     itemCode = a.ComodityCode,
                                     Quantity = -b.Quantity
                                 };


            var QuerySewingAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                            where aa.AdjustmentDate.AddHours(7) > dateBalance && aa.AdjustmentDate.AddHours(7) <= dateTo && aa.AdjustmentType == "SEWING"
                                            select new { aa.Identity, aa.ComodityCode, aa.ComodityName })
                                 join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                 select new monitoringViewsTemp
                                 {
                                     itemCode = a.ComodityCode,
                                     itemname = a.ComodityName.TrimEnd(),
                                     Quantity = -b.Quantity
                                 };

            var QueryAvalCompSewing = (from a in (from aa in garmentAvalComponentRepository.Query
                                                  where aa.Date.AddHours(7) <= dateTo && aa.Date.AddHours(7) > dateBalance && aa.AvalComponentType == "SEWING"
                                                  select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                       join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                                       select new monitoringViewsTemp
                                       {
                                           itemCode = a.ComodityCode,
                                           itemname = a.ComodityName.TrimEnd(),
                                           Quantity = -b.Quantity
                                       });

            #region SampleSewing
            var QuerySewingInSample = from a in (from aa in garmentSampleSewingInRepository.Query
                                                 where aa.SewingInDate.AddHours(7) > dateBalance && aa.SewingInDate.AddHours(7) <= dateTo
                                                 select new { aa.Identity, aa.ComodityCode, aa.ComodityName })
                                      join b in garmentSampleSewingInItemRepository.Query on a.Identity equals b.SewingInId
                                      select new monitoringViewsTemp
                                      {
                                          itemCode = a.ComodityCode,
                                          itemname = a.ComodityName.TrimEnd(),
                                          Quantity = b.Quantity
                                      };

            var QuerySewingOutSample = from a in (from aa in garmentSampleSewingOutRepository.Query
                                                  where aa.SewingOutDate.AddHours(7) > dateBalance && aa.SewingOutDate.AddHours(7) <= dateTo
                                                  select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                       join b in garmentSampleSewingOutItemRepository.Query on a.Identity equals b.SampleSewingOutId
                                       select new monitoringViewsTemp
                                       {
                                           itemname = a.ComodityName.TrimEnd(),
                                           itemCode = a.ComodityCode,
                                           Quantity = -b.Quantity
                                       };

            var QueryAvalCompSewingSample = (from a in (from aa in garmentSampleAvalComponentRepository.Query
                                                        where aa.Date.AddHours(7) <= dateTo && aa.Date.AddHours(7) > dateBalance && aa.SampleAvalComponentType == "SEWING"
                                                        select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                             join b in garmentSampleAvalComponentItemRepository.Query on a.Identity equals b.SampleAvalComponentId
                                             select new monitoringViewsTemp
                                             {
                                                 itemCode = a.ComodityCode,
                                                 itemname = a.ComodityName.TrimEnd(),
                                                 Quantity = -b.Quantity
                                             });
            var queryNowSewingSample = QuerySewingInSample.Union(QuerySewingOutSample).Union(QueryAvalCompSewingSample).AsEnumerable();
            var SakirSewingSample = queryNowSewingSample.GroupBy(x => new { x.itemCode, x.itemname }, (key, group) => new monitoringViewsTemp
            {
                itemname = key.itemname,
                itemCode = key.itemCode,
                Quantity = group.Sum(x => x.Quantity),
                unitQty = "PCS"
            }).ToList();

            #endregion

            var queryNowSewing = queryBalanceSewing.Union(QuerySewingIn).Union(QuerySewingOut).Union(QuerySewingAdj).Union(QueryAvalCompSewing).Union(SakirSewingSample).AsEnumerable();
            var SakirSewing = queryNowSewing.GroupBy(x => new { x.itemCode, x.itemname }, (key, group) => new monitoringViewsTemp
            {
                itemname = key.itemname,
                itemCode = key.itemCode,
                Quantity = group.Sum(x => x.Quantity),
                unitQty = "PCS"
            }).ToList();
            #endregion

            #region Finishing+Sample
            //Old Query

            //var FactFinishing = (from a in (from aa in garmentFinishingOutRepository.Query
            //                                    //where aa.FinishingOutDate.Date < request.Date.Date
            //                                where aa.FinishingOutDate.Year == request.Date.Year
            //                                && aa.FinishingTo == "GUDANG JADI"
            //                                select aa)
            //                     join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
            //                     select new
            //                     {
            //                         ComodityCode = a.ComodityCode,
            //                         ComodityName = a.ComodityName,
            //                         Quantity = b.Quantity * -1,
            //                     }).GroupBy(x => new { x.ComodityCode, x.ComodityName }, (key, group) => new monitoringViewsTemp
            //                     {
            //                         itemname = key.ComodityName,
            //                         itemCode = key.ComodityCode,
            //                         Quantity = group.Sum(x => x.Quantity),
            //                         unitQty = "PCS"
            //                     });
            var queryBalanceFinishings = (from a in garmentBalanceProductionStockRepository.Query
                                          where a.CreatedDate < dateTo && a.BeginingBalanceFinishingQty > 0
                                          select new monitoringViewsTemp
                                          {

                                              itemname = a.Comodity.TrimEnd(),
                                              Quantity = a.BeginingBalanceFinishingQty
                                          });

            List<monitoringViewsTemp> queryBalanceFinishing = new List<monitoringViewsTemp>();
            foreach (var a in queryBalanceFinishings)
            {
                var comodity = GarmentComodities.data.FirstOrDefault(x => x.Name.TrimEnd() == a.itemname);

                a.itemCode = comodity != null ? comodity.Code : "";
                queryBalanceFinishing.Add(a);
            }

            var QueryFinishingIn = from a in (from aa in garmentFinishingInRepository.Query
                                              where aa.FinishingInDate.AddHours(7) > dateBalance && aa.FinishingInDate.AddHours(7) <= dateTo
                                              select new { aa.Identity, aa.ComodityCode, aa.ComodityName, })
                                   join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                   select new monitoringViewsTemp
                                   {
                                       itemCode = a.ComodityCode,
                                       itemname = a.ComodityName.TrimEnd(),
                                       Quantity = b.Quantity
                                   };

            var QueryFinishingOut = from a in (from aa in garmentFinishingOutRepository.Query
                                               where aa.FinishingOutDate.AddHours(7) > dateBalance && aa.FinishingOutDate.AddHours(7) <= dateTo
                                               select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                    join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                    select new monitoringViewsTemp
                                    {
                                        itemname = a.ComodityName.TrimEnd(),
                                        itemCode = a.ComodityCode,
                                        Quantity = -b.Quantity
                                    };


            var QueryFinishingAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                               where aa.AdjustmentDate.AddHours(7) > dateBalance && aa.AdjustmentDate.AddHours(7) <= dateTo && aa.AdjustmentType == "FINISHING"
                                               select new { aa.Identity, aa.ComodityCode, aa.ComodityName })
                                    join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                    select new monitoringViewsTemp
                                    {
                                        itemCode = a.ComodityCode,
                                        itemname = a.ComodityName.TrimEnd(),
                                        Quantity = -b.Quantity
                                    };

            #region SampleFinishing
            var QueryFinishingInSample = from a in (from aa in garmentSampleFinishingInRepository.Query
                                                    where aa.FinishingInDate.AddHours(7) > dateBalance && aa.FinishingInDate.AddHours(7) <= dateTo
                                                    select new { aa.Identity, aa.ComodityCode, aa.ComodityName, })
                                         join b in garmentSampleFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                         select new monitoringViewsTemp
                                         {
                                             itemCode = a.ComodityCode,
                                             itemname = a.ComodityName.TrimEnd(),
                                             Quantity = b.Quantity
                                         };

            var QueryFinishingOutSample = from a in (from aa in garmentSampleFinishingOutRepository.Query
                                                     where aa.FinishingOutDate.AddHours(7) > dateBalance && aa.FinishingOutDate.AddHours(7) <= dateTo
                                                     select new { aa.Identity, aa.ComodityName, aa.ComodityCode })
                                          join b in garmentSampleFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                          select new monitoringViewsTemp
                                          {
                                              itemname = a.ComodityName.TrimEnd(),
                                              itemCode = a.ComodityCode,
                                              Quantity = -b.Quantity
                                          };




            var QueryNowFinishingSample = QueryFinishingIn.Union(QueryFinishingOut);

            //var SakirFinishingSample = QueryNowFinishingSample.GroupBy(x => new { x.itemCode, x.itemname }, (key, group) => new monitoringViewsTemp
            //{
            //    itemname = key.itemname,
            //    itemCode = key.itemCode,
            //    Quantity = group.Sum(x => x.Quantity),
            //});
            #endregion

            var QueryNowFinishing = queryBalanceFinishing.Union(QueryFinishingIn).Union(QueryFinishingOut).Union(QueryFinishingAdj).Union(QueryNowFinishingSample).AsEnumerable();

            var SakirFinishing = QueryNowFinishing.GroupBy(x => new { x.itemCode, x.itemname }, (key, group) => new monitoringViewsTemp
            {
                itemname = key.itemname,
                itemCode = key.itemCode,
                Quantity = group.Sum(x => x.Quantity),
                unitQty = "PCS"
            }).ToList();


            #endregion


            //var WIPTemp = FactCutting.Concat(FactFinishing).ToList();

            //var WIPTemp = SakirCutting.Concat(SakirLoading).Concat(SakirSewing).Concat(SakirFinishing).ToList();
            var WIPTemp = SakirCutting.Concat(SakirLoading).Concat(SakirSewing).Concat(SakirFinishing).ToList();

            WIPTemp = WIPTemp.GroupBy(x => new { x.itemCode, x.itemname, x.unitQty }, (key, group) => new monitoringViewsTemp
            {
                itemname = key.itemname,
                itemCode = key.itemCode,
                Quantity = group.Sum(x => x.Quantity),
                unitQty = key.unitQty

            }).ToList();

            //var WIP = WIPTemp.Where(x => x.Quantity > 0).Concat(FactPrepare).ToList();
            var WIP = WIPTemp.Where(x => x.Quantity > 0).Concat(SakirPreparing).ToList();

            foreach (var i in WIP)
            {
                GarmentWIPDto dto = new GarmentWIPDto
                {
                    Kode = i.itemCode,
                    Comodity = i.itemname,
                    UnitQtyName = i.unitQty,
                    WIP = Math.Round(i.Quantity, 2)
                };

                monitoringDtos.Add(dto);
            }

            listViewModel.garmentWIP = monitoringDtos;

            return listViewModel;

        }

    }
}
