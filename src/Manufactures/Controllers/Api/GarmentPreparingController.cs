using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("preparings")]
    public class GarmentPreparingController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IMemoryCacheManager _cacheManager;

        public GarmentPreparingController(IServiceProvider serviceProvider, IMemoryCacheManager cacheManager) : base(serviceProvider)
        {
            _garmentPreparingRepository = Storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = Storage.GetRepository<IGarmentPreparingItemRepository>();
            _cacheManager = cacheManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {

            var query = _garmentPreparingRepository.Read(order, select, filter);
            int totalRows = query.Count();
            var garmentPreparingDto = _garmentPreparingRepository.Find(query).Select(o => new GarmentPreparingDto(o)).ToArray();
            var garmentPreparingItemDto = _garmentPreparingItemRepository.Find(_garmentPreparingItemRepository.Query).Select(o => new GarmentPreparingItemDto(o)).ToList();
            var garmentPreparingItemDtoArray = _garmentPreparingItemRepository.Find(_garmentPreparingItemRepository.Query).Select(o => new GarmentPreparingItemDto(o)).ToArray();

            Parallel.ForEach(garmentPreparingDto, itemDto =>
            {
                var garmentPreparingItems = garmentPreparingItemDto.Where(x => x.GarmentPreparingId == itemDto.Id).ToList();

                itemDto.Items = garmentPreparingItems;
                var selectedUnit = GetUnit(itemDto.UnitId.Id, Token);

                if (selectedUnit != null && selectedUnit.data != null)
                {
                    itemDto.UnitId.Name = selectedUnit.data.Name;
                    itemDto.UnitId.Code = selectedUnit.data.Code;
                }

                Parallel.ForEach(itemDto.Items, orderItem =>
                {
                    var selectedProduct = GetProduct(orderItem.ProductId.Id, Token);
                    var selectedUom = GetUom(orderItem.UomId.Id, Token);

                    if(selectedProduct != null && selectedProduct.data != null)
                    {
                        orderItem.ProductId.Name = selectedProduct.data.Name;
                        orderItem.ProductId.Code = selectedProduct.data.Code;
                    }

                    if (selectedUom != null && selectedUom.data != null)
                    {
                        orderItem.UomId.Unit = selectedUom.data.Unit;
                    }
                });

               itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();
            });

            if (!string.IsNullOrEmpty(keyword))
            {
                garmentPreparingItemDtoArray = garmentPreparingItemDto.Where(x => x.ProductId.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
                List<GarmentPreparingDto> ListTemp = new List<GarmentPreparingDto>();
                foreach(var a in garmentPreparingItemDtoArray)
                {
                    var temp = garmentPreparingDto.Where(x => x.Id.Equals(a.GarmentPreparingId)).ToArray();
                    foreach(var b in temp)
                    {
                        ListTemp.Add(b);
                    }
                }


                var garmentPreparingDtoList = garmentPreparingDto.Where(x => x.UENNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.UnitId.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    //|| x.Items.Where(y => y.ProductId.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    ).ToList();

                var i = 0;
                foreach(var data in ListTemp)
                {
                    foreach(var item in garmentPreparingDto)
                    {
                        if(data.Id == item.Id)
                        {
                            i++;
                        }
                    }
                    if (i == 0)
                    {
                        garmentPreparingDtoList.Add(data);
                    }
                }
            }

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentPreparingDto = QueryHelper<GarmentPreparingDto>.Order(garmentPreparingDto.AsQueryable(), OrderDictionary).ToArray();
            }

            garmentPreparingDto = garmentPreparingDto.Take(size).Skip((page - 1) * size).ToArray();

            await Task.Yield();
            return Ok(garmentPreparingDto, info: new
            {
                page,
                size,
                count = totalRows
            });
        }
    }
}
