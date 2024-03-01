using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Dtos;
using Manufactures.Dtos.GermentReciptSubcon.GarmentFinishedGoodStocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GermentReciptSubcon
{
	[ApiController]
	[Authorize]
	[Route("subcon-finished-good-stocks")]
	public class GarmentSubconFinishedGoodStockController : ControllerApiBase
	{
		private readonly IGarmentSubconFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
		public GarmentSubconFinishedGoodStockController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentFinishedGoodStockRepository = Storage.GetRepository<IGarmentSubconFinishedGoodStockRepository>();
		 
		}
		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentFinishedGoodStockRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentSubconFinishedGoodStockAdjustmentDto> listDtos = _garmentFinishedGoodStockRepository
							.Find(query)
							.Where(data => data.Quantity >0)
							.Select(data => new GarmentSubconFinishedGoodStockAdjustmentDto(data))
							.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
			{
				page,
				size,
				count
			});
		}

        [HttpGet("list")]
        public async Task<IActionResult> GetList(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishedGoodStockRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            List<GarmentSubconFinishedGoodStockDto> listDtos = _garmentFinishedGoodStockRepository
                            .Find(query)
                            .Where(data => data.Quantity > 0)
                            .Select(data => new GarmentSubconFinishedGoodStockDto(data))
                            .ToList();

            await Task.Yield();
            return Ok(listDtos, info: new
            {
                page,
                size,
                count
            });
        }

		[HttpGet("get-by-ro")]
		public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentFinishedGoodStockRepository.ReadComplete(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentSubconFinishedGoodStockDto> listDtos = _garmentFinishedGoodStockRepository
							.Find(query)
							.Where(data => data.Quantity > 0)
							.Select(data => new GarmentSubconFinishedGoodStockDto(data))
							.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
			{
				page,
				size,
				count
			});
		}

		[HttpGet("complete")]
		public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();
			var query = _garmentFinishedGoodStockRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();
			var garmentFinishedGoodStockDto = _garmentFinishedGoodStockRepository.Find(query).Select(o => new GarmentSubconFinishedGoodStockAdjustmentDto(o)).Where(o=> o.Quantity >0).ToArray();
			if (order != "{}")
			{
				Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
				garmentFinishedGoodStockDto = QueryHelper<GarmentSubconFinishedGoodStockAdjustmentDto>.Order(garmentFinishedGoodStockDto.AsQueryable(), OrderDictionary).ToArray();
			}
			await Task.Yield();
			return Ok(garmentFinishedGoodStockDto, info: new
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

			GarmentSubconFinishedGoodStockAdjustmentDto garmentFinishingInDto = _garmentFinishedGoodStockRepository.Find(o => o.Identity == guid).Select(loading => new GarmentSubconFinishedGoodStockAdjustmentDto(loading)).FirstOrDefault();

			await Task.Yield();
			return Ok(garmentFinishingInDto);
		}

	}
}
