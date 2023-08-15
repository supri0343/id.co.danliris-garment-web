using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.Queries;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Manufactures.Dtos.GarmentSample;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("service-sample-cuttings")]
    public class GarmentServiceSampleCuttingController : ControllerApiBase
    {
        private readonly IGarmentServiceSampleCuttingRepository _garmentServiceSampleCuttingRepository;
        private readonly IGarmentServiceSampleCuttingItemRepository _garmentServiceSampleCuttingItemRepository;
        private readonly IGarmentServiceSampleCuttingDetailRepository _garmentServiceSampleCuttingDetailRepository;
        private readonly IGarmentServiceSampleCuttingSizeRepository _garmentServiceSampleCuttingSizeRepository;

        public GarmentServiceSampleCuttingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSampleCuttingRepository = Storage.GetRepository<IGarmentServiceSampleCuttingRepository>();
            _garmentServiceSampleCuttingItemRepository = Storage.GetRepository<IGarmentServiceSampleCuttingItemRepository>();
            _garmentServiceSampleCuttingDetailRepository = Storage.GetRepository<IGarmentServiceSampleCuttingDetailRepository>();
            _garmentServiceSampleCuttingSizeRepository = Storage.GetRepository<IGarmentServiceSampleCuttingSizeRepository>();

        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSampleCuttingRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();                
            double totalQty = query.Sum(a => a.GarmentServiceSampleCuttingItem.Sum(b => b.GarmentServiceSampleCuttingDetail.Sum(c=>c.Quantity)));

            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSampleCuttingListDto> garmentServiceSampleCuttingListDtos = _garmentServiceSampleCuttingRepository
                .Find(query)
                .Select(Sample => new GarmentServiceSampleCuttingListDto(Sample))
                .ToList();

            var dtoIds = garmentServiceSampleCuttingListDtos.Select(s => s.Id).ToList();
            //var items = _garmentServiceSampleCuttingItemRepository.Query
            //    .Where(o => dtoIds.Contains(o.ServiceSampleCuttingId))
            //    .Select(s => new { s.Identity, s.ServiceSampleCuttingId, s.ProductCode, s.Quantity})
            //    .ToList();

            //Parallel.ForEach(garmentServiceSampleCuttingListDtos, dto =>
            //{
            //    var currentItems = items.Where(w => w.ServiceSampleCuttingId == dto.Id);
            //    dto.Products = currentItems.Select(d => d.ProductCode).Distinct().ToList();
            //    dto.TotalQuantity = currentItems.Sum(d => d.Quantity);
            //});

            await Task.Yield();
            return Ok(garmentServiceSampleCuttingListDtos, info: new
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

            GarmentServiceSampleCuttingDto garmentServiceSampleCuttingDto = _garmentServiceSampleCuttingRepository.Find(o => o.Identity == guid).Select(Sample => new GarmentServiceSampleCuttingDto(Sample)
            {
                Items = _garmentServiceSampleCuttingItemRepository.Find(o => o.ServiceSampleCuttingId == Sample.Identity).Select(SampleItem => new GarmentServiceSampleCuttingItemDto(SampleItem)
                {
                    Details = _garmentServiceSampleCuttingDetailRepository.Find(o => o.ServiceSampleCuttingItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleCuttingDetailDto(SampleDetail)
                    {
                        Sizes= _garmentServiceSampleCuttingSizeRepository.Find(o => o.ServiceSampleCuttingDetailId == SampleDetail.Identity).Select(SampleSize => new GarmentServiceSampleCuttingSizeDto(SampleSize)
                        {

                        }).ToList()
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSampleCuttingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSampleCuttingCommand command)
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

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSampleCuttingRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentServiceSampleCuttingDto = _garmentServiceSampleCuttingRepository.Find(query).Select(o => new GarmentServiceSampleCuttingDto(o)).ToArray();
            var garmentServiceSampleCuttingItemDto = _garmentServiceSampleCuttingItemRepository.Find(_garmentServiceSampleCuttingItemRepository.Query).Select(o => new GarmentServiceSampleCuttingItemDto(o)).ToList();
            var garmentServiceSampleCuttingDetailDto = _garmentServiceSampleCuttingDetailRepository.Find(_garmentServiceSampleCuttingDetailRepository.Query).Select(o => new GarmentServiceSampleCuttingDetailDto(o)).ToList();
            var garmentServiceSampleCuttingSizeDto = _garmentServiceSampleCuttingSizeRepository.Find(_garmentServiceSampleCuttingSizeRepository.Query).Select(o => new GarmentServiceSampleCuttingSizeDto(o)).ToList();

            Parallel.ForEach(garmentServiceSampleCuttingDto, itemDto =>
            {
                var garmentServiceSampleCuttingItems = garmentServiceSampleCuttingItemDto.Where(x => x.ServiceSampleCuttingId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentServiceSampleCuttingItems;
                Parallel.ForEach(itemDto.Items, detailDto =>
                {
                    var garmentCuttingInDetails = garmentServiceSampleCuttingDetailDto.Where(x => x.ServiceSampleCuttingItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Details = garmentCuttingInDetails;
                    Parallel.ForEach(detailDto.Details, detDto =>
                    {
                        var garmentCuttingSizes = garmentServiceSampleCuttingSizeDto.Where(x => x.ServiceSampleCuttingDetailId == detDto.Id).OrderBy(x => x.Id).ToList();
                        detDto.Sizes = garmentCuttingSizes;
                    });
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSampleCuttingDto = QueryHelper<GarmentServiceSampleCuttingDto>.Order(garmentServiceSampleCuttingDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSampleCuttingDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSampleCuttingCommand command)
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

            RemoveGarmentServiceSampleCuttingCommand command = new RemoveGarmentServiceSampleCuttingCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
            
        }

        [HttpGet("item")]
        public async Task<IActionResult> GetItems(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSampleCuttingItemRepository.ReadItem(page, size, order, keyword, filter);
            var count = query.Count();

            //var garmentServiceSampleCuttingDto = _garmentServiceSampleCuttingRepository.Find(query).Select(o => new GarmentServiceSampleCuttingDto(o)).ToArray();
            var garmentServiceSampleCuttingItemDto = _garmentServiceSampleCuttingItemRepository.Find(query).Select(o => new GarmentServiceSampleCuttingItemDto(o)).ToArray();
            var garmentServiceSampleCuttingDetailDto = _garmentServiceSampleCuttingDetailRepository.Find(_garmentServiceSampleCuttingDetailRepository.Query).Select(o => new GarmentServiceSampleCuttingDetailDto(o)).ToList();
            var garmentServiceSampleCuttingSizeDto = _garmentServiceSampleCuttingSizeRepository.Find(_garmentServiceSampleCuttingSizeRepository.Query).Select(o => new GarmentServiceSampleCuttingSizeDto(o)).ToList();

            Parallel.ForEach(garmentServiceSampleCuttingItemDto, itemDto =>
            {
                var garmentServiceSampleCuttingDetails = garmentServiceSampleCuttingDetailDto.Where(x => x.ServiceSampleCuttingItemId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Details = garmentServiceSampleCuttingDetails;
                Parallel.ForEach(itemDto.Details, detailDto =>
                {
                    var garmentCuttingSizes = garmentServiceSampleCuttingSizeDto.Where(x => x.ServiceSampleCuttingDetailId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Sizes = garmentCuttingSizes;
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSampleCuttingItemDto = QueryHelper<GarmentServiceSampleCuttingItemDto>.Order(garmentServiceSampleCuttingItemDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSampleCuttingItemDto, info: new
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

            //int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentServiceSampleCuttingDto garmentServiceSampleCuttingDto = _garmentServiceSampleCuttingRepository.Find(o => o.Identity == guid).Select(Sample => new GarmentServiceSampleCuttingDto(Sample)
            {
                Items = _garmentServiceSampleCuttingItemRepository.Find(o => o.ServiceSampleCuttingId == Sample.Identity).Select(SampleItem => new GarmentServiceSampleCuttingItemDto(SampleItem)
                {
                    Details = _garmentServiceSampleCuttingDetailRepository.Find(o => o.ServiceSampleCuttingItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleCuttingDetailDto(SampleDetail)
                    {
                        Sizes = _garmentServiceSampleCuttingSizeRepository.Find(o => o.ServiceSampleCuttingDetailId == SampleDetail.Identity).Select(SampleSize => new GarmentServiceSampleCuttingSizeDto(SampleSize)
                        {

                        }).ToList()
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            var stream = GarmentServiceSampleCuttingPDFTemplate.Generate(garmentServiceSampleCuttingDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentServiceSampleCuttingDto.SubconNo}.pdf"
            };
        }


        [HttpGet("download")]
        public async Task<IActionResult> GetXlsMutation(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsServiceSampleCuttingQuery query = new GetXlsServiceSampleCuttingQuery(page, size, Order, dateFrom, dateTo, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Monitoring Sample Jasa Komponen ";

                if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

                if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
                filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
