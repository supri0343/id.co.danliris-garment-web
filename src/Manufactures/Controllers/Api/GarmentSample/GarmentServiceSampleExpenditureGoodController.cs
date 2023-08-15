using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
//using Manufactures.Application.GarmentSample.Queries.GarmentSampleGarmentWashReport;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Dtos.GarmentSample;
using Manufactures.Dtos.GarmentSample.GarmentServiceSampleExpenditureGoodDtoos;
using Manufactures.Dtos.GarmentSample.GarmentServiceSampleExpenditureGoodItemDtos;
using Manufactures.Dtos.GarmentSample.GarmentServiceSampleExpenditureGoodListDto;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Manufactures.Controllers.Api.GarmentSampleExpeditureGood
{
    [ApiController]
    [Authorize]
    [Route("service-sample-expenditure-good")]
    public class GarmentServiceSampleExpeditureGoodController : ControllerApiBase
    {
        private readonly IGarmentServiceSampleExpenditureGoodRepository _garmentServiceSampleExpenditureGoodRepository;
        private readonly IGarmentServiceSampleExpenditureGoodtemRepository _garmentServiceSampleExpenditureGoodItemRepository;

        public GarmentServiceSampleExpeditureGoodController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSampleExpenditureGoodRepository = Storage.GetRepository<IGarmentServiceSampleExpenditureGoodRepository>();
            _garmentServiceSampleExpenditureGoodItemRepository = Storage.GetRepository<IGarmentServiceSampleExpenditureGoodtemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSampleExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentServiceSampleExpenditureGoodItem.Sum(b =>b.Quantity ));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSampleExpenditureGoodListDto> garmentServiceSampleSewingListDtos = _garmentServiceSampleExpenditureGoodRepository
                .Find(query)
                .Select(ServiceSampleSewing => new GarmentServiceSampleExpenditureGoodListDto(ServiceSampleSewing))
                .ToList();

            var dtoIds = garmentServiceSampleSewingListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSampleExpenditureGoodItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSampleExpenditureGoodId))
                .Select(s => new { s.Identity, s.ServiceSampleExpenditureGoodId ,s.Quantity})
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            Parallel.ForEach(garmentServiceSampleSewingListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ServiceSampleExpenditureGoodId == dto.Id);
                //dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
                //dto.Products = currentItems.Select(i => i.ProductCode).Distinct().ToList();
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

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

            GarmentServiceSampleExpenditureGoodDto garmentServiceSampleSewingDto = _garmentServiceSampleExpenditureGoodRepository.Find(o => o.Identity == guid).Select(serviceSampleSewing => new GarmentServiceSampleExpenditureGoodDto(serviceSampleSewing)
            {
                Items = _garmentServiceSampleExpenditureGoodItemRepository.Find(o => o.ServiceSampleExpenditureGoodId == serviceSampleSewing.Identity).Select(SampleItem => new GarmentServiceSampleExpenditureGoodItemDto(SampleItem)).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSampleSewingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSampleExpenditureGoodCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSampleExpenditureGoodCommand command)
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

            RemoveGarmentServiceSampleExpenditureGoodCommand command = new RemoveGarmentServiceSampleExpenditureGoodCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var getRO = _garmentServiceSampleExpenditureGoodItemRepository.Query.Where(x => keyword.Contains(x.RONo)).Select(s => new { s.RONo }).ToList().Count();

            if (getRO != 0)
            {
                var query = _garmentServiceSampleExpenditureGoodItemRepository.ReadItem(page, size, order, keyword, filter);
                var count = query.Count();
                query = query.Skip((page - 1) * size).Take(size);

                var garmentServiceSampleSewingDto = _garmentServiceSampleExpenditureGoodRepository.Find(_garmentServiceSampleExpenditureGoodRepository.Query).Select(o => new GarmentServiceSampleExpenditureGoodDto(o)).ToArray();
                var garmentServiceSampleSewingItemDto = _garmentServiceSampleExpenditureGoodItemRepository.Find(query).Select(o => new GarmentServiceSampleExpenditureGoodItemDto(o)).ToArray();
              

                Parallel.ForEach(garmentServiceSampleSewingDto, itemDto =>
                {
                    var garmentServiceSampleSewingItems = garmentServiceSampleSewingItemDto.Where(x => x.ServiceSampleExpenditureGoodId == itemDto.Id && x.RONo == keyword).OrderBy(x => x.Id).ToList();

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
                    garmentServiceSampleSewingDto = QueryHelper<GarmentServiceSampleExpenditureGoodDto>.Order(garmentServiceSampleSewingDto.AsQueryable(), OrderDictionary).ToArray();
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
                var query = _garmentServiceSampleExpenditureGoodRepository.Read(page, size, order, keyword, filter);
                var count = query.Count();
                query = query.Skip((page - 1) * size).Take(size);

                var garmentServiceSampleSewingDto = _garmentServiceSampleExpenditureGoodRepository.Find(query).Select(o => new GarmentServiceSampleExpenditureGoodDto(o)).ToArray();
                var garmentServiceSampleSewingItemDto = _garmentServiceSampleExpenditureGoodItemRepository.Find(_garmentServiceSampleExpenditureGoodItemRepository.Query).Select(o => new GarmentServiceSampleExpenditureGoodItemDto(o)).ToList();
          

                Parallel.ForEach(garmentServiceSampleSewingDto, itemDto =>
                {
                    var garmentServiceSampleSewingItems = garmentServiceSampleSewingItemDto.Where(x => x.ServiceSampleExpenditureGoodId == itemDto.Id).OrderBy(x => x.Id).ToList();

                    itemDto.Items = garmentServiceSampleSewingItems;
                
                });

                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentServiceSampleSewingDto = QueryHelper<GarmentServiceSampleExpenditureGoodDto>.Order(garmentServiceSampleSewingDto.AsQueryable(), OrderDictionary).ToArray();
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

        //[HttpGet("item")]
        //public async Task<IActionResult> GetItems(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        //{
        //    VerifyUser();

        //    var query = _garmentServiceSampleExpenditureGoodItemRepository.ReadItem(page, size, order, keyword, filter);
        //    var count = query.Count();

        //    //var garmentServiceSampleSewingDto = _garmentServiceSampleExpenditureGoodRepository.Find(query).Select(o => new GarmentServiceSampleSewingDto(o)).ToArray();
        //    var garmentServiceSampleSewingItemDto = _garmentServiceSampleExpenditureGoodItemRepository.Find(query).Select(o => new GarmentServiceSampleSewingItemDto(o)).ToArray();
        //    var garmentServiceSampleSewingDetailDto = _garmentServiceSampleSewingDetailRepository.Find(_garmentServiceSampleSewingDetailRepository.Query).Select(o => new GarmentServiceSampleSewingDetailDto(o)).ToList();


        //    Parallel.ForEach(garmentServiceSampleSewingItemDto, itemDto =>
        //    {
        //        var garmentServiceSampleSewingDetails = garmentServiceSampleSewingDetailDto.Where(x => x.ServiceSampleSewingItemId == itemDto.Id).OrderBy(x => x.Id).ToList();

        //        itemDto.Details = garmentServiceSampleSewingDetails;
        //    });

        //    if (order != "{}")
        //    {
        //        Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
        //        garmentServiceSampleSewingItemDto = QueryHelper<GarmentServiceSampleSewingItemDto>.Order(garmentServiceSampleSewingItemDto.AsQueryable(), OrderDictionary).ToArray();
        //    }

        //    await Task.Yield();
        //    return Ok(garmentServiceSampleSewingItemDto, info: new
        //    {
        //        page,
        //        size,
        //        count
        //    });
        //}
        //[HttpGet("get-pdf/{id}")]
        //public async Task<IActionResult> GetPdf(string id)
        //{
        //    Guid guid = Guid.Parse(id);

        //    VerifyUser();

        //    GarmentServiceSampleSewingDto garmentServiceSampleSewingDto = _garmentServiceSampleExpenditureGoodRepository.Find(o => o.Identity == guid).Select(serviceSampleSewing => new GarmentServiceSampleSewingDto(serviceSampleSewing)
        //    {
        //        Items = _garmentServiceSampleExpenditureGoodItemRepository.Find(o => o.ServiceSampleSewingId == serviceSampleSewing.Identity).Select(SampleItem => new GarmentServiceSampleSewingItemDto(SampleItem)
        //        {
        //            Details = _garmentServiceSampleSewingDetailRepository.Find(o => o.ServiceSampleSewingItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleSewingDetailDto(SampleDetail)
        //            {

        //            }).ToList()
        //        }).ToList()

        //    }
        //    ).FirstOrDefault();
        //    var stream = GarmentServiceSampleSewingPDFTemplate.Generate(garmentServiceSampleSewingDto);

        //    return new FileStreamResult(stream, "application/pdf")
        //    {
        //        FileDownloadName = $"{garmentServiceSampleSewingDto.ServiceSampleSewingNo}.pdf"
        //    };
        //}

        //[HttpGet("download")]
        //public async Task<IActionResult> GetXlsSampleGarmentWashReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        //{
        //    try
        //    {
        //        GetXlsGarmentSampleGarmentWashReporQuery query = new GetXlsGarmentSampleGarmentWashReporQuery(page, size, order, dateFrom, dateTo);
        //        byte[] xlsInBytes;

        //        var xls = await Mediator.Send(query);

        //        string filename = String.Format("Laporan Sample Jasa Garment Wash.xlsx");

        //        xlsInBytes = xls.ToArray();
        //        var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //        return file;
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        //    }
        //}
    }
}
