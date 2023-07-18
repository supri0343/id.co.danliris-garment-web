using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSample.Queries.GarmentSampleGarmentWashReport;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Dtos.GarmentSample;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("service-sample-sewings")]
    public class GarmentServiceSampleSewingController : ControllerApiBase
    {
        private readonly IGarmentServiceSampleSewingRepository _garmentServiceSampleSewingRepository;
        private readonly IGarmentServiceSampleSewingItemRepository _garmentServiceSampleSewingItemRepository;
        private readonly IGarmentServiceSampleSewingDetailRepository _garmentServiceSampleSewingDetailRepository;

        public GarmentServiceSampleSewingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSampleSewingRepository = Storage.GetRepository<IGarmentServiceSampleSewingRepository>();
            _garmentServiceSampleSewingDetailRepository = Storage.GetRepository<IGarmentServiceSampleSewingDetailRepository>();
            _garmentServiceSampleSewingItemRepository = Storage.GetRepository<IGarmentServiceSampleSewingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSampleSewingRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentServiceSampleSewingItem.Sum(b => b.GarmentServiceSampleSewingDetail.Sum(c => c.Quantity)));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSampleSewingListDto> garmentServiceSampleSewingListDtos = _garmentServiceSampleSewingRepository
                .Find(query)
                .Select(ServiceSampleSewing => new GarmentServiceSampleSewingListDto(ServiceSampleSewing))
                .ToList();

            var dtoIds = garmentServiceSampleSewingListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSampleSewingItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSampleSewingId))
                .Select(s => new { s.Identity, s.ServiceSampleSewingId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            //Parallel.ForEach(garmentServiceSampleSewingListDtos, dto =>
            //{
            //    var currentItems = items.Where(w => w.ServiceSampleSewingId == dto.Id);
            //    //dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
            //    //dto.Products = currentItems.Select(i => i.ProductCode).Distinct().ToList();
            //    //dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            //});

            await Task.Yield();
            return Ok(garmentServiceSampleSewingListDtos, info: new
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

            GarmentServiceSampleSewingDto garmentServiceSampleSewingDto = _garmentServiceSampleSewingRepository.Find(o => o.Identity == guid).Select(serviceSampleSewing => new GarmentServiceSampleSewingDto(serviceSampleSewing)
            {
                Items = _garmentServiceSampleSewingItemRepository.Find(o => o.ServiceSampleSewingId == serviceSampleSewing.Identity).Select(SampleItem => new GarmentServiceSampleSewingItemDto(SampleItem)
                {
                    Details = _garmentServiceSampleSewingDetailRepository.Find(o => o.ServiceSampleSewingItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleSewingDetailDto(SampleDetail)
                    {

                    }).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSampleSewingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSampleSewingCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSampleSewingCommand command)
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

            RemoveGarmentServiceSampleSewingCommand command = new RemoveGarmentServiceSampleSewingCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var getRO = _garmentServiceSampleSewingItemRepository.Query.Where(x => keyword.Contains(x.RONo)).Select(s => new { s.RONo }).ToList().Count();

            if (getRO != 0)
            {
                var query = _garmentServiceSampleSewingItemRepository.ReadItem(page, size, order, keyword, filter);
                var count = query.Count();

                query = query.Skip((page - 1) * size).Take(size);
               
                var garmentServiceSampleSewingDto = _garmentServiceSampleSewingRepository.Find(_garmentServiceSampleSewingRepository.Query).Select(o => new GarmentServiceSampleSewingDto(o)).ToArray();
                var garmentServiceSampleSewingItemDto = _garmentServiceSampleSewingItemRepository.Find(query).Select(o => new GarmentServiceSampleSewingItemDto(o)).ToArray();
                var garmentServiceSampleSewingDetailDto = _garmentServiceSampleSewingDetailRepository.Find(_garmentServiceSampleSewingDetailRepository.Query).Select(o => new GarmentServiceSampleSewingDetailDto(o)).ToList();

                Parallel.ForEach(garmentServiceSampleSewingDto, itemDto =>
                {
                    var garmentServiceSampleSewingItems = garmentServiceSampleSewingItemDto.Where(x => x.ServiceSampleSewingId == itemDto.Id && x.RONo == keyword).OrderBy(x => x.Id).ToList();

                    itemDto.Items = garmentServiceSampleSewingItems;
                    //    Parallel.ForEach(itemDto.Items, detailDto =>
                    //    {
                    //        var garmentCuttingInDetails = garmentServiceSampleSewingDetailDto.Where(x => x.ServiceSampleSewingItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    //        detailDto.Details = garmentCuttingInDetails;
                    //    });
                });

                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentServiceSampleSewingDto = QueryHelper<GarmentServiceSampleSewingDto>.Order(garmentServiceSampleSewingDto.AsQueryable(), OrderDictionary).ToArray();
                }

 

                await Task.Yield();
                return Ok(garmentServiceSampleSewingDto, info: new
                {
                    page,
                    size,
                    count
                });
            }
            else
            {
                var query = _garmentServiceSampleSewingRepository.Read(page, size, order, keyword, filter);
                
                var count = query.Count();
                query = query.Skip((page - 1) * size).Take(size);
                var garmentServiceSampleSewingDto = _garmentServiceSampleSewingRepository.Find(query).Select(o => new GarmentServiceSampleSewingDto(o)).ToArray();
                var garmentServiceSampleSewingItemDto = _garmentServiceSampleSewingItemRepository.Find(_garmentServiceSampleSewingItemRepository.Query).Select(o => new GarmentServiceSampleSewingItemDto(o)).ToList();
                var garmentServiceSampleSewingDetailDto = _garmentServiceSampleSewingDetailRepository.Find(_garmentServiceSampleSewingDetailRepository.Query).Select(o => new GarmentServiceSampleSewingDetailDto(o)).ToList();

                Parallel.ForEach(garmentServiceSampleSewingDto, itemDto =>
                {
                    var garmentServiceSampleSewingItems = garmentServiceSampleSewingItemDto.Where(x => x.ServiceSampleSewingId == itemDto.Id).OrderBy(x => x.Id).ToList();

                    itemDto.Items = garmentServiceSampleSewingItems;
                    Parallel.ForEach(itemDto.Items, detailDto =>
                    {
                        var garmentCuttingInDetails = garmentServiceSampleSewingDetailDto.Where(x => x.ServiceSampleSewingItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                        detailDto.Details = garmentCuttingInDetails;
                    });
                });

                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentServiceSampleSewingDto = QueryHelper<GarmentServiceSampleSewingDto>.Order(garmentServiceSampleSewingDto.AsQueryable(), OrderDictionary).ToArray();
                }

                await Task.Yield();
                return Ok(garmentServiceSampleSewingDto, info: new
                {
                    page,
                    size,
                    count
                });
            }
            
        }

        [HttpGet("item")]
        public async Task<IActionResult> GetItems(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSampleSewingItemRepository.ReadItem(page, size, order, keyword, filter);
            var count = query.Count();

            //var garmentServiceSampleSewingDto = _garmentServiceSampleSewingRepository.Find(query).Select(o => new GarmentServiceSampleSewingDto(o)).ToArray();
            var garmentServiceSampleSewingItemDto = _garmentServiceSampleSewingItemRepository.Find(query).Select(o => new GarmentServiceSampleSewingItemDto(o)).ToArray();
            var garmentServiceSampleSewingDetailDto = _garmentServiceSampleSewingDetailRepository.Find(_garmentServiceSampleSewingDetailRepository.Query).Select(o => new GarmentServiceSampleSewingDetailDto(o)).ToList();


            Parallel.ForEach(garmentServiceSampleSewingItemDto, itemDto =>
            {
                var garmentServiceSampleSewingDetails = garmentServiceSampleSewingDetailDto.Where(x => x.ServiceSampleSewingItemId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Details = garmentServiceSampleSewingDetails;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSampleSewingItemDto = QueryHelper<GarmentServiceSampleSewingItemDto>.Order(garmentServiceSampleSewingItemDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSampleSewingItemDto, info: new
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

            GarmentServiceSampleSewingDto garmentServiceSampleSewingDto = _garmentServiceSampleSewingRepository.Find(o => o.Identity == guid).Select(serviceSampleSewing => new GarmentServiceSampleSewingDto(serviceSampleSewing)
            {
                Items = _garmentServiceSampleSewingItemRepository.Find(o => o.ServiceSampleSewingId == serviceSampleSewing.Identity).Select(SampleItem => new GarmentServiceSampleSewingItemDto(SampleItem)
                {
                    Details = _garmentServiceSampleSewingDetailRepository.Find(o => o.ServiceSampleSewingItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleSewingDetailDto(SampleDetail)
                    {

                    }).ToList()
                }).ToList()

            }
            ).FirstOrDefault();
            var stream = GarmentServiceSampleSewingPDFTemplate.Generate(garmentServiceSampleSewingDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentServiceSampleSewingDto.ServiceSampleSewingNo}.pdf"
            };
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetXlsSampleGarmentWashReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            try
            {
                GetXlsGarmentSampleGarmentWashReporQuery query = new GetXlsGarmentSampleGarmentWashReporQuery(page, size, order, dateFrom, dateTo);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = String.Format("Laporan Sample Jasa Garment Wash.xlsx");

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
