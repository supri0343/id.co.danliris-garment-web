using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories;
using Manufactures.Dtos.GarmentSubcon.SubconReprocess;
using Manufactures.Helpers.PDFTemplates.GarmentSubcon.SubconReprocess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-reprocesses")]
    public class GarmentSubconReprocessController : ControllerApiBase
    {
        private readonly IGarmentSubconReprocessRepository _garmentSubconReprocessRepository;
        private readonly IGarmentSubconReprocessItemRepository _garmentSubconReprocessItemRepository;
        private readonly IGarmentSubconReprocessDetailRepository _garmentSubconReprocessDetailRepository;

        public GarmentSubconReprocessController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSubconReprocessRepository = Storage.GetRepository<IGarmentSubconReprocessRepository>();
            _garmentSubconReprocessDetailRepository = Storage.GetRepository<IGarmentSubconReprocessDetailRepository>();
            _garmentSubconReprocessItemRepository = Storage.GetRepository<IGarmentSubconReprocessItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconReprocessRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSubconReprocessListDto> garmentSubconReprocessListDtos = _garmentSubconReprocessRepository
                .Find(query)
                .Select(SubconReprocess => new GarmentSubconReprocessListDto(SubconReprocess))
                .ToList();

            var dtoIds = garmentSubconReprocessListDtos.Select(s => s.Id).ToList();
           
            await Task.Yield();
            return Ok(garmentSubconReprocessListDtos, info: new
            {
                page,
                size,
                total
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSubconReprocessDto garmentSubconReprocessDto = _garmentSubconReprocessRepository.Find(o => o.Identity == guid).Select(SubconReprocess => new GarmentSubconReprocessDto(SubconReprocess)
            {
                Items = _garmentSubconReprocessItemRepository.Find(o => o.ReprocessId == SubconReprocess.Identity).Select(reprocessItem => new GarmentSubconReprocessItemDto(reprocessItem)
                {
                    Details = _garmentSubconReprocessDetailRepository.Find(o => o.ReprocessItemId == reprocessItem.Identity).Select(detail => new GarmentSubconReprocessDetailDto(detail)
                    { }).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSubconReprocessDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconReprocessCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconReprocessCommand command)
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

            RemoveGarmentSubconReprocessCommand command = new RemoveGarmentSubconReprocessCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconReprocessRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSubconReprocessDto = _garmentSubconReprocessRepository.Find(query).Select(o => new GarmentSubconReprocessDto(o)).ToArray();
            var garmentSubconReprocessItemDto = _garmentSubconReprocessItemRepository.Find(_garmentSubconReprocessItemRepository.Query).Select(o => new GarmentSubconReprocessItemDto(o)).ToList();
            var garmentSubconReprocessDetailDto = _garmentSubconReprocessDetailRepository.Find(_garmentSubconReprocessDetailRepository.Query).Select(o => new GarmentSubconReprocessDetailDto(o)).ToList();

            Parallel.ForEach(garmentSubconReprocessDto, itemDto =>
            {
                var garmentSubconReprocessItems = garmentSubconReprocessItemDto.Where(x => x.ReprocessId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentSubconReprocessItems;
                Parallel.ForEach(itemDto.Items, detailDto =>
                {
                    var garmentCuttingInDetails = garmentSubconReprocessDetailDto.Where(x => x.ReprocessItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Details = garmentCuttingInDetails;
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSubconReprocessDto = QueryHelper<GarmentSubconReprocessDto>.Order(garmentSubconReprocessDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSubconReprocessDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("item")]
        public async Task<IActionResult> GetItems(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconReprocessItemRepository.ReadItem(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSubconReprocessItemDto = _garmentSubconReprocessItemRepository.Find(query).Select(o => new GarmentSubconReprocessItemDto(o)).ToArray();
            var garmentSubconReprocessDetailDto = _garmentSubconReprocessDetailRepository.Find(_garmentSubconReprocessDetailRepository.Query).Select(o => new GarmentSubconReprocessDetailDto(o)).ToList();

            Parallel.ForEach(garmentSubconReprocessItemDto, itemDto =>
            {
                var garmentServiceSubconSewingDetails = garmentSubconReprocessDetailDto.Where(x => x.ReprocessItemId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Details = garmentServiceSubconSewingDetails;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSubconReprocessItemDto = QueryHelper<GarmentSubconReprocessItemDto>.Order(garmentSubconReprocessItemDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSubconReprocessItemDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("get-pdf/{id}")]
        public async Task<IActionResult> GetPdf(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSubconReprocessDto garmentSubconReprocessDto = _garmentSubconReprocessRepository.Find(o => o.Identity == guid).Select(SubconReprocess => new GarmentSubconReprocessDto(SubconReprocess)
            {
                Items = _garmentSubconReprocessItemRepository.Find(o => o.ReprocessId == SubconReprocess.Identity).Select(reprocessItem => new GarmentSubconReprocessItemDto(reprocessItem)
                {
                    Details = _garmentSubconReprocessDetailRepository.Find(o => o.ReprocessItemId == reprocessItem.Identity).Select(detail => new GarmentSubconReprocessDetailDto(detail)
                    { }).ToList()
                }).ToList()

            }
            ).FirstOrDefault();
            var stream = GarmentSubconReprocessPDFTemplate.Generate(garmentSubconReprocessDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentSubconReprocessDto.ReprocessNo}.pdf"
            };
        }
    }
}
