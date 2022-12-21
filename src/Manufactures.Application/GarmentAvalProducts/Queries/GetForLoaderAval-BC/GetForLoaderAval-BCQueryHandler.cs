using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;


namespace Manufactures.Application.GarmentAvalProducts.Queries.GetForLoaderAval_BC
{
    public class GetForLoaderAval_BCQueryHandler : IQueryHandler<GetForLoaderAval_BCQuery, GetForLoaderAval_BCViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        private readonly IGarmentAvalProductRepository _garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository _garmentAvalProductItemRepository;
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;

        public GetForLoaderAval_BCQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            
            _storage = storage;
            _garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            _garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
            _garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
        }


        class ViewModel
        {
            public Guid preparingId { get; set; }
            public Guid preparingItemId { get; set; }
            //public Product Product { get; set; }
            public Product Product { get; set; }
     
            public string DesignColor { get; set; }
            public double RemainingQuantity { get; set; }
            //public Uom Uom { get; set; }
            public Uom Uom { get; set; }
            public double BasicPrice { get; set; }
            public string UENNo { get; set; }
            public int UENItemId { get; set; }
            public DateTimeOffset? ProcessDate { get; set; }
        }

        public async Task<List<GetForLoaderAval_BC_BCNoDto>> GetDataBC(List<int> uenitemId, string token)
        {
            List<GetForLoaderAval_BC_BCNoDto> data = new List<GetForLoaderAval_BC_BCNoDto>();
            var listUenItemId = string.Join(",", uenitemId);

            var Uri = PurchasingDataSettings.Endpoint + $"bc-for-aval";

            var httpContent = new StringContent(JsonConvert.SerializeObject(listUenItemId), Encoding.UTF8, "application/json");

            var httpResponse = await _http.SendAsync(HttpMethod.Get, Uri, token, httpContent);
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
                    data = JsonConvert.DeserializeObject<List<GetForLoaderAval_BC_BCNoDto>>(content.GetValueOrDefault("data").ToString());
                }
                
            }
            else
            {
                var err = await httpResponse.Content.ReadAsStringAsync();

            }


            return data;
        }
        public async Task<GetForLoaderAval_BCViewModel> Handle(GetForLoaderAval_BCQuery request, CancellationToken cancellationToken)
        {
            var QueryPrep = (from a in _garmentPreparingRepository.Query
                         join b in _garmentPreparingItemRepository.Query
                         on a.Identity equals b.GarmentPreparingId
                         where a.RONo == request.ro && a.UnitId == request.unit && b.RemainingQuantity > 0
                         select new ViewModel
                         {
                             preparingId = a.Identity,
                             preparingItemId = b.Identity,
                             Product = new Product(b.ProductId, b.ProductName, b.ProductCode),
                             DesignColor = b.DesignColor,
                             RemainingQuantity = b.RemainingQuantity,
                             Uom = new Uom(b.UomId, b.UomUnit),
                             BasicPrice = b.BasicPrice,
                             UENNo = a.UENNo,
                             UENItemId = b.UENItemId,
                             ProcessDate = a.ProcessDate

                         }).ToList();

            var ListUenItemId = QueryPrep.Select(x => x.UENItemId).Distinct().ToList();

            var BClist = await GetDataBC(ListUenItemId, request.token);

            GetForLoaderAval_BCViewModel data = new GetForLoaderAval_BCViewModel();
            List<GetForLoaderAval_BCDto> getForLoaderAval_BCDto = new List<GetForLoaderAval_BCDto>();

            foreach (var a in QueryPrep)
            {
                var bc = BClist.Where(x => x.uenitemId == a.UENItemId).FirstOrDefault();

                var result = new GetForLoaderAval_BCDto
                {
                    preparingId = a.preparingId,
                    preparingItemId = a.preparingItemId,
                    Product = a.Product,
                    DesignColor = a.DesignColor,
                    RemainingQuantity = a.RemainingQuantity,
                    Uom = a.Uom,
                    BasicPrice = a.BasicPrice,
                    bcno = bc != null ? bc.bcno : "-",
                    bcdate = bc != null ? bc.bcdate : null,
                    bctype = bc != null ? bc.bctype : "-",
                    poSerialNumber = bc != null ? bc.poSerialNumber : "-",
                    ProcessDate = a.ProcessDate
                    
                };

                getForLoaderAval_BCDto.Add(result);

            }

            data.getForLoaderAval_BCDtos = getForLoaderAval_BCDto;

            //data.getForLoaderAval_BCDtos = QueryAvalList;
            //data.getForLoaderAval_BC_BCNoDtos = BClist;

            return data;

        }
    }
}
