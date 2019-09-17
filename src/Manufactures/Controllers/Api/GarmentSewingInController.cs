using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("sewing-ins")]
    public class GarmentSewingInController : ControllerApiBase
    {
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public GarmentSewingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSewingInRepository = Storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = Storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingInRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();
            List<GarmentSewingInListDto> garmentSewingInListDtos = _garmentSewingInRepository.Find(query).Select(sewingIn =>
            {
                var items = _garmentSewingInItemRepository.Query.Where(o => o.SewingInId == sewingIn.Identity).Select(sewingInItem => new
                {
                    sewingInItem.ProductCode,
                    sewingInItem.Quantity,
                }).ToList();

                return new GarmentSewingInListDto(sewingIn)
                {
                    Products = items.Select(i => i.ProductCode).ToList(),
                    TotalQuantity = items.Sum(i => i.Quantity),
                };
            }).ToList();

            await Task.Yield();
            return Ok(garmentSewingInListDtos, info: new
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

            GarmentSewingInDto garmentSewingIn = _garmentSewingInRepository.Find(o => o.Identity == guid).Select(sewingIn => new GarmentSewingInDto(sewingIn)
            {
                Items = _garmentSewingInItemRepository.Find(o => o.SewingInId == sewingIn.Identity).Select(sewingInItem => new GarmentSewingInItemDto(sewingInItem)).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSewingIn);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSewingInCommand command)
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
            VerifyUser();
            var garmentSewingInId = Guid.Parse(id);

            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            RemoveGarmentSewingInCommand command = new RemoveGarmentSewingInCommand(garmentSewingInId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }
    }
}