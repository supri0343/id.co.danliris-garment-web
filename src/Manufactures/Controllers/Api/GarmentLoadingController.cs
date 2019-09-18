using Barebone.Controllers;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("loadings")]
    public class GarmentLoadingController : ControllerApiBase
    {
        private readonly IMemoryCacheManager _cacheManager;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public GarmentLoadingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentLoadingRepository = Storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = Storage.GetRepository<IGarmentLoadingItemRepository>();
            _garmentSewingDOItemRepository = Storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentLoadingRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();
            List<GarmentLoadingListDto> garmentCuttingInListDtos = _garmentLoadingRepository.Find(query).Select(loading =>
            {
                var items = _garmentLoadingItemRepository.Query.Where(o => o.LoadingId == loading.Identity).Select(loadingItem => new
                {
                    loadingItem.ProductCode,
                    loadingItem.ProductName,
                    loadingItem.Quantity,
                    loadingItem.RemainingQuantity,
                    loadingItem.Color
                }).ToList();

                return new GarmentLoadingListDto(loading)
                {
                    Products = items.Select(i => i.ProductName).ToList(),
                    TotalLoadingQuantity =Math.Round( items.Sum(i => i.Quantity),2),
                    TotalRemainingQuantity= Math.Round(items.Sum(i => i.RemainingQuantity),2),
                    Colors=items.Where(i=>i.Color!=null).Select(i=>i.Color).Distinct().ToList()
                };
            }).ToList();

            await Task.Yield();
            return Ok(garmentCuttingInListDtos, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentLoadingDto garmentLoadingDto = _garmentLoadingRepository.Find(o => o.Identity == guid).Select(loading => new GarmentLoadingDto(loading)
            {
                Items = _garmentLoadingItemRepository.Find(o => o.LoadingId == loading.Identity).Select(loadingItem => new GarmentLoadingItemDto(loadingItem)
                ).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentLoadingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentLoadingCommand command)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentLoadingCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentLoadingCommand command = new RemoveGarmentLoadingCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }
    }
}
