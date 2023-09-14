using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentPackingOut.Commands;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using Manufactures.Dtos;
using Manufactures.Dtos.GermentReciptSubcon.GarmentPackingOut;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.GermentReciptSubcon.Api
{
    [ApiController]
    [Authorize]
    [Route("subcon-packing-outs")]
    public class GarmentSubconPackingOutController : ControllerApiBase
    {

        private readonly IGarmentSubconPackingOutRepository _garmentPackingOutRepository;
        private readonly IGarmentSubconPackingOutItemRepository _garmentPackingOutItemRepository;

        public GarmentSubconPackingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentPackingOutRepository = Storage.GetRepository<IGarmentSubconPackingOutRepository>();
            _garmentPackingOutItemRepository = Storage.GetRepository<IGarmentSubconPackingOutItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentPackingOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentPackingOutListDto> packingOutListDtos = _garmentPackingOutRepository
                .Find(query)
                .Select(ExGood => new GarmentPackingOutListDto(ExGood))
                .ToList();

            var dtoIds = packingOutListDtos.Select(s => s.Id).ToList();
            var items = _garmentPackingOutItemRepository.Query
                .IgnoreQueryFilters()
                .Where(o => dtoIds.Contains(o.PackingOutId))
                .Select(s => new { s.Identity, s.PackingOutId, s.Quantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(packingOutListDtos, dto =>
            {
                var currentItems = items.Where(w => w.PackingOutId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(packingOutListDtos, info: new
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

            GarmentPackingOutDto garmentPackingOutDto = _garmentPackingOutRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentPackingOutDto(finishOut)
            {
                Items = _garmentPackingOutItemRepository.Find(o => o.PackingOutId == finishOut.Identity).OrderBy(a => a.Description).ThenBy(i => i.SizeName).Select(packingOutItem => new GarmentPackingOutItemDto(packingOutItem)
                {
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentPackingOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconPackingOutCommand command)
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

            RemoveGarmentSubconPackingOutCommand command = new RemoveGarmentSubconPackingOutCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }
        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentPackingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentPackingOutDto = _garmentPackingOutRepository.ReadExecute(query);

            await Task.Yield();
            return Ok(garmentPackingOutDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("update-received")]
        public async Task<IActionResult> IsPackingList([FromBody] UpdateIsPackingListGarmentSubconPackingOutCommand command)
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
