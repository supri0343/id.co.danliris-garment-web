using Barebone.Controllers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Dtos.GarmentSubcon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("service-subcon-cuttings")]
    public class GarmentServiceSubconCuttingController : ControllerApiBase
    {
        private readonly IGarmentServiceSubconCuttingRepository _garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository _garmentServiceSubconCuttingItemRepository;

        public GarmentServiceSubconCuttingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSubconCuttingRepository = Storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = Storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconCuttingRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentServiceSubconCuttingItem.Sum(b => b.Quantity));

            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSubconCuttingListDto> garmentServiceSubconCuttingListDtos = _garmentServiceSubconCuttingRepository
                .Find(query)
                .Select(subcon => new GarmentServiceSubconCuttingListDto(subcon))
                .ToList();

            var dtoIds = garmentServiceSubconCuttingListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSubconCuttingItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSubconCuttingId))
                .Select(s => new { s.Identity, s.ServiceSubconCuttingId, s.ProductCode, s.Quantity})
                .ToList();

            Parallel.ForEach(garmentServiceSubconCuttingListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ServiceSubconCuttingId == dto.Id);
                dto.Products = currentItems.Select(d => d.ProductCode).Distinct().ToList();
                dto.TotalQuantity = currentItems.Sum(d => d.Quantity);
            });

            await Task.Yield();
            return Ok(garmentServiceSubconCuttingListDtos, info: new
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

            GarmentServiceSubconCuttingDto garmentServiceSubconCuttingDto = _garmentServiceSubconCuttingRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentServiceSubconCuttingDto(subcon)
            {
                Items = _garmentServiceSubconCuttingItemRepository.Find(o => o.ServiceSubconCuttingId == subcon.Identity).Select(subconItem => new GarmentServiceSubconCuttingItemDto(subconItem)
                {
                    
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSubconCuttingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSubconCuttingCommand command)
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
