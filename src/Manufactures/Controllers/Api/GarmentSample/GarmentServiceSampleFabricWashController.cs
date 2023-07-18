using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.Queries;
using Manufactures.Dtos.GarmentSample.GarmentServiceSampleFabricWashes;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("service-sample-fabric-washes")]
    public class GarmentServiceSampleFabricWashController : ControllerApiBase
    {
        private readonly IGarmentServiceSampleFabricWashRepository _garmentServiceSampleFabricWashRepository;
        private readonly IGarmentServiceSampleFabricWashItemRepository _garmentServiceSampleFabricWashItemRepository;
        private readonly IGarmentServiceSampleFabricWashDetailRepository _garmentServiceSampleFabricWashDetailRepository;

        public GarmentServiceSampleFabricWashController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSampleFabricWashRepository = Storage.GetRepository<IGarmentServiceSampleFabricWashRepository>();
            _garmentServiceSampleFabricWashItemRepository = Storage.GetRepository<IGarmentServiceSampleFabricWashItemRepository>();
            _garmentServiceSampleFabricWashDetailRepository = Storage.GetRepository<IGarmentServiceSampleFabricWashDetailRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSampleFabricWashRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSampleFabricWashListDto> garmentServiceSampleFabricWashListDtos = _garmentServiceSampleFabricWashRepository
                .Find(query)
                .Select(ServiceSampleFabricWash => new GarmentServiceSampleFabricWashListDto(ServiceSampleFabricWash))
                .ToList();

            var dtoIds = garmentServiceSampleFabricWashListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSampleFabricWashItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSampleFabricWashId))
                .Select(s => new { s.Identity, s.ServiceSampleFabricWashId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            await Task.Yield();
            return Ok(garmentServiceSampleFabricWashListDtos, info: new
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

            GarmentServiceSampleFabricWashDto garmentServiceSampleFabricWashDto = _garmentServiceSampleFabricWashRepository.Find(o => o.Identity == guid).Select(serviceSampleFabricWash => new GarmentServiceSampleFabricWashDto(serviceSampleFabricWash)
            {
                Items = _garmentServiceSampleFabricWashItemRepository.Find(o => o.ServiceSampleFabricWashId == serviceSampleFabricWash.Identity).Select(SampleItem => new GarmentServiceSampleFabricWashItemDto(SampleItem)
                {
                    Details = _garmentServiceSampleFabricWashDetailRepository.Find(o => o.ServiceSampleFabricWashItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleFabricWashDetailDto(SampleDetail)).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSampleFabricWashDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSampleFabricWashCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSampleFabricWashCommand command)
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

            RemoveGarmentServiceSampleFabricWashCommand command = new RemoveGarmentServiceSampleFabricWashCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var getUENNo = _garmentServiceSampleFabricWashItemRepository.Query.Where(x => keyword.Contains(x.UnitExpenditureNo)).Select(s => new { s.UnitExpenditureNo }).ToList().Count();

            if (getUENNo != 0)
            {
                var query = _garmentServiceSampleFabricWashItemRepository.ReadItem(page, size, order, keyword, filter);
                var count = query.Count();

                var garmentServiceSampleFabricWashDto = _garmentServiceSampleFabricWashRepository.Find(_garmentServiceSampleFabricWashRepository.Query).Select(o => new GarmentServiceSampleFabricWashDto(o)).ToArray();
                var garmentServiceSampleFabricWashItemDto = _garmentServiceSampleFabricWashItemRepository.Find(query).Select(o => new GarmentServiceSampleFabricWashItemDto(o)).ToArray();
                var garmentServiceSampleFabricWashDetailDto = _garmentServiceSampleFabricWashDetailRepository.Find(_garmentServiceSampleFabricWashDetailRepository.Query).Select(o => new GarmentServiceSampleFabricWashDetailDto(o)).ToList();

                Parallel.ForEach(garmentServiceSampleFabricWashDto, itemDto =>
                {
                    var garmentServiceSampleFabricWashItems = garmentServiceSampleFabricWashItemDto.Where(x => x.ServiceSampleFabricWashId == itemDto.Id).OrderBy(x => x.Id).ToList();

                    itemDto.Items = garmentServiceSampleFabricWashItems;
                    Parallel.ForEach(itemDto.Items, detailDto =>
                    {
                        var garmentServiceSampleFabricWashDetails = garmentServiceSampleFabricWashDetailDto.Where(x => x.ServiceSampleFabricWashItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                        detailDto.Details = garmentServiceSampleFabricWashDetails;
                    });
                });

                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentServiceSampleFabricWashDto = QueryHelper<GarmentServiceSampleFabricWashDto>.Order(garmentServiceSampleFabricWashDto.AsQueryable(), OrderDictionary).ToArray();
                }

                await Task.Yield();
                return Ok(garmentServiceSampleFabricWashDto, info: new
                {
                    page,
                    size,
                    count
                });
            }
            else
            {
                var query = _garmentServiceSampleFabricWashRepository.Read(page, size, order, keyword, filter);
                var count = query.Count();

                var garmentServiceSampleFabricWashDto = _garmentServiceSampleFabricWashRepository.Find(query).Select(o => new GarmentServiceSampleFabricWashDto(o)).ToArray();
                var garmentServiceSampleFabricWashItemDto = _garmentServiceSampleFabricWashItemRepository.Find(_garmentServiceSampleFabricWashItemRepository.Query).Select(o => new GarmentServiceSampleFabricWashItemDto(o)).ToList();
                var garmentServiceSampleFabricWashDetailDto = _garmentServiceSampleFabricWashDetailRepository.Find(_garmentServiceSampleFabricWashDetailRepository.Query).Select(o => new GarmentServiceSampleFabricWashDetailDto(o)).ToList();

                Parallel.ForEach(garmentServiceSampleFabricWashDto, itemDto =>
                {
                    var garmentServiceSampleFabricWashItems = garmentServiceSampleFabricWashItemDto.Where(x => x.ServiceSampleFabricWashId == itemDto.Id).OrderBy(x => x.Id).ToList();

                    itemDto.Items = garmentServiceSampleFabricWashItems;
                    Parallel.ForEach(itemDto.Items, detailDto =>
                    {
                        var garmentServiceSampleFabricWashDetails = garmentServiceSampleFabricWashDetailDto.Where(x => x.ServiceSampleFabricWashItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                        detailDto.Details = garmentServiceSampleFabricWashDetails;
                    });
                });

                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentServiceSampleFabricWashDto = QueryHelper<GarmentServiceSampleFabricWashDto>.Order(garmentServiceSampleFabricWashDto.AsQueryable(), OrderDictionary).ToArray();
                }

                await Task.Yield();
                return Ok(garmentServiceSampleFabricWashDto, info: new
                {
                    page,
                    size,
                    count
                });
            }
            
        }

        [HttpGet("get-pdf/{id}")]
        public async Task<IActionResult> GetPdf(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            //int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentServiceSampleFabricWashDto garmentServiceSampleFabricWashDto = _garmentServiceSampleFabricWashRepository.Find(o => o.Identity == guid).Select(Sample => new GarmentServiceSampleFabricWashDto(Sample)
            {
                Items = _garmentServiceSampleFabricWashItemRepository.Find(o => o.ServiceSampleFabricWashId == Sample.Identity).Select(SampleItem => new GarmentServiceSampleFabricWashItemDto(SampleItem)
                {
                    Details = _garmentServiceSampleFabricWashDetailRepository.Find(o => o.ServiceSampleFabricWashItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleFabricWashDetailDto(SampleDetail)
                    { }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            var stream = GarmentServiceSampleFabricWashPDFTemplate.Generate(garmentServiceSampleFabricWashDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentServiceSampleFabricWashDto.ServiceSampleFabricWashNo}.pdf"
            };
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetXls(DateTime dateFrom, DateTime dateTo, string token, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsServiceSampleFabricWashQuery query = new GetXlsServiceSampleFabricWashQuery(page, size, Order, dateFrom, dateTo, token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Sample BB - FABRIC WASH / PRINT";

                if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

                if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
                filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                //throw e;
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
