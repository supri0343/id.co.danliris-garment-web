﻿using Barebone.Controllers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconFabricWashes;
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
    [Route("service-subcon-fabric-washes")]
    public class GarmentServiceSubconFabricWashController : ControllerApiBase
    {
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconFabricWashItemRepository _garmentServiceSubconFabricWashItemRepository;
        private readonly IGarmentServiceSubconFabricWashDetailRepository _garmentServiceSubconFabricWashDetailRepository;

        public GarmentServiceSubconFabricWashController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSubconFabricWashRepository = Storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentServiceSubconFabricWashItemRepository = Storage.GetRepository<IGarmentServiceSubconFabricWashItemRepository>();
            _garmentServiceSubconFabricWashDetailRepository = Storage.GetRepository<IGarmentServiceSubconFabricWashDetailRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconFabricWashRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSubconFabricWashListDto> garmentServiceSubconFabricWashListDtos = _garmentServiceSubconFabricWashRepository
                .Find(query)
                .Select(ServiceSubconFabricWash => new GarmentServiceSubconFabricWashListDto(ServiceSubconFabricWash))
                .ToList();

            var dtoIds = garmentServiceSubconFabricWashListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSubconFabricWashItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSubconFabricWashId))
                .Select(s => new { s.Identity, s.ServiceSubconFabricWashId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            await Task.Yield();
            return Ok(garmentServiceSubconFabricWashListDtos, info: new
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

            GarmentServiceSubconFabricWashDto garmentServiceSubconFabricWashDto = _garmentServiceSubconFabricWashRepository.Find(o => o.Identity == guid).Select(serviceSubconFabricWash => new GarmentServiceSubconFabricWashDto(serviceSubconFabricWash)
            {
                Items = _garmentServiceSubconFabricWashItemRepository.Find(o => o.ServiceSubconFabricWashId == serviceSubconFabricWash.Identity).Select(subconItem => new GarmentServiceSubconFabricWashItemDto(subconItem)
                {
                    Details = _garmentServiceSubconFabricWashDetailRepository.Find(o => o.ServiceSubconFabricWashItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconFabricWashDetailDto(subconDetail)).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSubconFabricWashDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSubconFabricWashCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSubconFabricWashCommand command)
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

            RemoveGarmentServiceSubconFabricWashCommand command = new RemoveGarmentServiceSubconFabricWashCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }
    }
}