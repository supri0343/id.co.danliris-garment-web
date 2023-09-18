using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GermentReciptSubcon.GarmentPackingIn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GermentReciptSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-packing-ins")]
    public class GarmentSubconPackingInController : ControllerApiBase
    {
        private readonly IMemoryCacheManager _cacheManager;
        private readonly IGarmentSubconPackingInRepository _garmentPackingInRepository;
        private readonly IGarmentSubconPackingInItemRepository _garmentPackingInItemRepository;

        public GarmentSubconPackingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentPackingInRepository = Storage.GetRepository<IGarmentSubconPackingInRepository>();
            _garmentPackingInItemRepository = Storage.GetRepository<IGarmentSubconPackingInItemRepository>();

        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentPackingInRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSubconPackingInItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentPackingInListDto> garmentCuttingInListDtos = _garmentPackingInRepository.Find(query).Select(loading =>
            {
                var items = _garmentPackingInItemRepository.Query.Where(o => o.PackingInId == loading.Identity).Select(loadingItem => new
                {
                    loadingItem.ProductCode,
                    loadingItem.ProductName,
                    loadingItem.Quantity,
                    loadingItem.RemainingQuantity
                }).ToList();

                return new GarmentPackingInListDto(loading)
                {
                    Products = items.Select(i => i.ProductName).Distinct().ToList(),
                    TotalPackingInQuantity = Math.Round(items.Sum(i => i.Quantity), 2),
                    TotalRemainingQuantity = Math.Round(items.Sum(i => i.RemainingQuantity), 2),

                };
            }).ToList();

            await Task.Yield();
            return Ok(garmentCuttingInListDtos, info: new
            {
                page,
                size,
                total,
                totalQty
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentPackingInDto garmentPackingInDto = _garmentPackingInRepository.Find(o => o.Identity == guid).Select(loading => new GarmentPackingInDto(loading)
            {
                Items = _garmentPackingInItemRepository.Find(o => o.PackingInId == loading.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(loadingItem => new GarmentPackingInItemDto(loadingItem)
                ).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentPackingInDto);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconPackingInCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentSubconPackingInCommand command = new RemoveGarmentSubconPackingInCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentPackingInRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentPackingInDto = _garmentPackingInRepository.Find(query).Select(o => new GarmentPackingInDto(o)).ToList();

            //Enhance Jason Aug 2021
            var finishingInIds = garmentPackingInDto.Select(s => s.Id).ToList();

            //var garmentPackingInItemDto = _garmentPackingInItemRepository.Find(_garmentPackingInItemRepository.Query).Select(o => new GarmentPackingInItemDto(o)).ToList();
            var garmentPackingInItemDto = _garmentPackingInItemRepository.Query
                .Where(w => finishingInIds.Contains(w.PackingInId))
                .Select(o => new GarmentPackingInItemDto(o))
                .OrderBy(o => o.Id).ToList();

            Parallel.ForEach(garmentPackingInDto, itemDto =>
            {
                var garmentPackingInItems = garmentPackingInItemDto.Where(x => x.PackingInId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentPackingInItems;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentPackingInDto = QueryHelper<GarmentPackingInDto>.Order(garmentPackingInDto.AsQueryable(), OrderDictionary).ToList();
            }

            await Task.Yield();
            return Ok(garmentPackingInDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetLoaderByRO(string keyword, string filter = "{}")
        {
            var query = _garmentPackingInRepository.Read(1, int.MaxValue, "{}", "", filter);
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(o => o.RONo.Contains(keyword));

            var rOs = query
                .Select(o => new 
                {
                    RONo =  o.RONo,
                    Article = o.Article,
                    Comodity = new GarmentComodity(o.ComodityId, o.ComodityCode, o.ComodityName)  
                }).Distinct().ToList();

            await Task.Yield();

            return Ok(rOs);
        }

        [HttpPut("approve")]
        public async Task<IActionResult> Approve([FromBody] UpdateApproveGarmentSubconPackingInCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
