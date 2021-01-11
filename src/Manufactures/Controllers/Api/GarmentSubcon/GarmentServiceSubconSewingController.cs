using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barebone.Controllers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Dtos.GarmentSubcon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("service-subcon-sewings")]
    public class GarmentServiceSubconSewingController : ControllerApiBase
    {
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;
        private readonly IGarmentServiceSubconSewingItemRepository _garmentServiceSubconSewingItemRepository;

        public GarmentServiceSubconSewingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSubconSewingRepository = Storage.GetRepository<IGarmentServiceSubconSewingRepository>();

            _garmentServiceSubconSewingItemRepository = Storage.GetRepository<IGarmentServiceSubconSewingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconSewingRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentServiceSubconSewingItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSubconSewingListDto> garmentServiceSubconSewingListDtos = _garmentServiceSubconSewingRepository
                .Find(query)
                .Select(ServiceSubconSewing => new GarmentServiceSubconSewingListDto(ServiceSubconSewing))
                .ToList();

            var dtoIds = garmentServiceSubconSewingListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSubconSewingItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSubconSewingId))
                .Select(s => new { s.Identity, s.ServiceSubconSewingId, s.ProductCode, s.Color, s.Quantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            Parallel.ForEach(garmentServiceSubconSewingListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ServiceSubconSewingId == dto.Id);
                dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
                dto.Products = currentItems.Select(i => i.ProductCode).Distinct().ToList();
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentServiceSubconSewingListDtos, info: new
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

            GarmentServiceSubconSewingDto garmentServiceSubconSewingDto = _garmentServiceSubconSewingRepository.Find(o => o.Identity == guid).Select(serviceSubconSewing => new GarmentServiceSubconSewingDto(serviceSubconSewing)
            {
                Items = _garmentServiceSubconSewingItemRepository.Find(o => o.ServiceSubconSewingId == serviceSubconSewing.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(serviceSubconSewingItem => new GarmentServiceSubconSewingItemDto(serviceSubconSewingItem)).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSubconSewingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSubconSewingCommand command)
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

    }
}
