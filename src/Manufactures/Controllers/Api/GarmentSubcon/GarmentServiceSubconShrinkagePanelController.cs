using Barebone.Controllers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconShrinkagePanels;
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
    [Route("service-subcon-shrinkage-panels")]
    public class GarmentServiceSubconShrinkagePanelController : ControllerApiBase
    {
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconShrinkagePanelItemRepository _garmentServiceSubconShrinkagePanelItemRepository;
        private readonly IGarmentServiceSubconShrinkagePanelDetailRepository _garmentServiceSubconShrinkagePanelDetailRepository;

        public GarmentServiceSubconShrinkagePanelController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSubconShrinkagePanelRepository = Storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconShrinkagePanelItemRepository = Storage.GetRepository<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _garmentServiceSubconShrinkagePanelDetailRepository = Storage.GetRepository<IGarmentServiceSubconShrinkagePanelDetailRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconShrinkagePanelRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSubconShrinkagePanelListDto> garmentServiceSubconShrinkagePanelListDtos = _garmentServiceSubconShrinkagePanelRepository
                .Find(query)
                .Select(ServiceSubconShrinkagePanel => new GarmentServiceSubconShrinkagePanelListDto(ServiceSubconShrinkagePanel))
                .ToList();

            var dtoIds = garmentServiceSubconShrinkagePanelListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSubconShrinkagePanelItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSubconShrinkagePanelId))
                .Select(s => new { s.Identity, s.ServiceSubconShrinkagePanelId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            await Task.Yield();
            return Ok(garmentServiceSubconShrinkagePanelListDtos, info: new
            {
                page,
                size,
                total,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentServiceSubconShrinkagePanelDto garmentServiceSubconShrinkagePanelDto = _garmentServiceSubconShrinkagePanelRepository.Find(o => o.Identity == guid).Select(serviceSubconShrinkagePanel => new GarmentServiceSubconShrinkagePanelDto(serviceSubconShrinkagePanel)
            {
                Items = _garmentServiceSubconShrinkagePanelItemRepository.Find(o => o.ServiceSubconShrinkagePanelId == serviceSubconShrinkagePanel.Identity).Select(subconItem => new GarmentServiceSubconShrinkagePanelItemDto(subconItem)
                {
                    Details = _garmentServiceSubconShrinkagePanelDetailRepository.Find(o => o.ServiceSubconShrinkagePanelItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconShrinkagePanelDetailDto(subconDetail)).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSubconShrinkagePanelDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSubconShrinkagePanelCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSubconShrinkagePanelCommand command)
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

            RemoveGarmentServiceSubconShrinkagePanelCommand command = new RemoveGarmentServiceSubconShrinkagePanelCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }
    }
}
