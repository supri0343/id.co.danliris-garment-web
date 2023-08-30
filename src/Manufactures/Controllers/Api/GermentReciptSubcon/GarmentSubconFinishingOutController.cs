using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories;
using Manufactures.Dtos.GermentReciptSubcon.GarmentFinishingOut;
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

namespace Manufactures.Controllers.Api.GermentReciptSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-finishing-outs")]
    public class GarmentSubconFinishingOutController : ControllerApiBase
    {
        private readonly IGarmentSubconFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentSubconFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentSubconFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentSubconFinishingInItemRepository _garmentFinishingInItemRepository;

        public GarmentSubconFinishingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentFinishingOutRepository = Storage.GetRepository<IGarmentSubconFinishingOutRepository>();
            _garmentFinishingOutItemRepository = Storage.GetRepository<IGarmentSubconFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = Storage.GetRepository<IGarmentSubconFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = Storage.GetRepository<IGarmentSubconFinishingInItemRepository>();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishingOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentFinishingOutItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentFinishingOutListDto> garmentFinishingOutListDtos = _garmentFinishingOutRepository
                .Find(query)
                .Select(SewOut => new GarmentFinishingOutListDto(SewOut))
                .ToList();

            var dtoIds = garmentFinishingOutListDtos.Select(s => s.Id).ToList();
            var items = _garmentFinishingOutItemRepository.Query
                .Where(o => dtoIds.Contains(o.FinishingOutId))
                .Select(s => new { s.Identity, s.FinishingOutId, s.ProductCode, s.Color, s.Quantity, s.RealQtyOut })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            var details = _garmentFinishingOutDetailRepository.Query
                .Where(o => itemIds.Contains(o.FinishingOutItemId))
                .Select(s => new { s.Identity, s.FinishingOutItemId })
                .ToList();

            Parallel.ForEach(garmentFinishingOutListDtos, dto =>
            {
                var currentItems = items.Where(w => w.FinishingOutId == dto.Id);
                dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
                dto.Products = currentItems.Select(i => i.ProductCode).Distinct().ToList();
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
                dto.TotalRealQtyOut = currentItems.Sum(i => i.RealQtyOut);
            });

            await Task.Yield();
            return Ok(garmentFinishingOutListDtos, info: new
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

            GarmentFinishingOutDto garmentFinishingOutDto = _garmentFinishingOutRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentFinishingOutDto(finishOut)
            {
                Items = _garmentFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(finishOutItem => new GarmentFinishingOutItemDto(finishOutItem)
                {
                    Details = _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).OrderBy(i => i.SizeName).Select(finishOutDetail => new GarmentFinishingOutDetailDto(finishOutDetail)
                    {
                    }).ToList()

                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentFinishingOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconFinishingOutCommand command)
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

            RemoveGarmentSubconFinishingOutCommand command = new RemoveGarmentSubconFinishingOutCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentFinishingOutDto = _garmentFinishingOutRepository.ReadExecute(query);
            //var garmentFinishingOutDto = _garmentFinishingOutRepository.Find(query).Select(o => new GarmentFinishingOutDto(o)).ToArray();
            //var garmentFinishingOutItemDto = _garmentFinishingOutItemRepository.Find(_garmentFinishingOutItemRepository.Query).Select(o => new GarmentFinishingOutItemDto(o)).ToList();
            //var garmentFinishingOutDetailDto = _garmentFinishingOutDetailRepository.Find(_garmentFinishingOutDetailRepository.Query).Select(o => new GarmentFinishingOutDetailDto(o)).ToList();

            //Parallel.ForEach(garmentFinishingOutDto, itemDto =>
            //{
            //    var garmentFinishingOutItems = garmentFinishingOutItemDto.Where(x => x.FinishingOutId == itemDto.Id).OrderBy(x => x.Id).ToList();

            //    itemDto.Items = garmentFinishingOutItems;

            //    Parallel.ForEach(itemDto.Items, detailDto =>
            //    {
            //        var garmentFinishingOutDetails = garmentFinishingOutDetailDto.Where(x => x.FinishingOutItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
            //        detailDto.Details = garmentFinishingOutDetails;
            //    });
            //});

            //if (order != "{}")
            //{
            //    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //    garmentFinishingOutDto = QueryHelper<GarmentFinishingOutDto>.Order(garmentFinishingOutDto.AsQueryable(), OrderDictionary).ToArray();
            //}

            await Task.Yield();
            return Ok(garmentFinishingOutDto, info: new
            {
                page,
                size,
                count
            });
        }

		[HttpGet("color")]
		public async Task<IActionResult> GetColor(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentFinishingOutRepository.ReadColor(page, size, order, keyword, filter);
			var total = query.Count();
			query = query.Skip((page - 1) * size).Take(size);

			List<GarmentFinishingOutListDto> garmentFinishingOutListDtos = _garmentFinishingOutRepository
				.Find(query)
				.Select(SewOut => new GarmentFinishingOutListDto(SewOut))
				.ToList();

			var dtoIds = garmentFinishingOutListDtos.Select(s => s.Id).ToList();
			var items = _garmentFinishingOutItemRepository.Query
				.Where(o => dtoIds.Contains(o.FinishingOutId))
				.Select(s => new { s.Identity, s.FinishingOutId, s.ProductCode, s.Color, s.Quantity, s.RealQtyOut })
				.ToList();

			var itemIds = items.Select(s => s.Identity).ToList();
			 
			List<object> color = new List<object>();
			foreach (var item in items)
			{
				color.Add(new { item.Color });
			}
			await Task.Yield();
			return Ok(color.Distinct(), info: new
			{
				page,
				size,
				color.Count
			});
		}

        //[HttpGet("{id}/{buyer}")]
        //public async Task<IActionResult> GetPdf(string id, string buyer)
        //{
        //    Guid guid = Guid.Parse(id);

        //    VerifyUser();

        //    int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
        //    GarmentFinishingOutDto garmentFinishingOutDto = _garmentFinishingOutRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentFinishingOutDto(finishOut)
        //    {
        //        Items = _garmentFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).Select(finishOutItem => new GarmentFinishingOutItemDto(finishOutItem)
        //        {
        //            Details = _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).Select(finishOutDetail => new GarmentFinishingOutDetailDto(finishOutDetail)
        //            {
        //            }).ToList()

        //        }).ToList()
        //    }
        //    ).FirstOrDefault();
        //    var stream = GarmentFinishingOutPDFTemplate.Generate(garmentFinishingOutDto, buyer);

        //    return new FileStreamResult(stream, "application/pdf")
        //    {
        //        FileDownloadName = $"{garmentFinishingOutDto.FinishingOutNo}.pdf"
        //    };
        //}

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody] UpdateDatesGarmentSubconFinishingOutCommand command)
        {
            VerifyUser();

            if (command.Date == null || command.Date == DateTimeOffset.MinValue)
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Tanggal harus diisi"
                });
            else if (command.Date.Date > DateTimeOffset.Now.Date)
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Tanggal tidak boleh lebih dari hari ini"
                });

            var order = await Mediator.Send(command);

            return Ok();
        }
    }
}
