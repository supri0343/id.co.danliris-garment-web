using Barebone.Controllers;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Commands;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Repositories;
using Manufactures.Dtos.GarmentSample.SampleReceiptFromBuyers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
	[ApiController]
	[Authorize]
	[Route("garment-sample-receipt-from-buyer")]
	public class GarmentSampleReceiptFromBuyerController : ControllerApiBase
	{
		private readonly IGarmentSampleReceiptFromBuyerRepository _garmentSampleReceiptFromBuyerRepository;
		private readonly IGarmentSampleReceiptFromBuyerItemRepository _garmentSampleReceiptFromBuyerItemRepository;
		public GarmentSampleReceiptFromBuyerController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentSampleReceiptFromBuyerRepository = Storage.GetRepository<IGarmentSampleReceiptFromBuyerRepository>();
			_garmentSampleReceiptFromBuyerItemRepository = Storage.GetRepository<IGarmentSampleReceiptFromBuyerItemRepository>();
		}

		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentSampleReceiptFromBuyerRepository.Read(page, size, order, keyword, filter);
			var total = query.Count();
			double totalQty = query.Sum(a => a.Items.Sum(b => b.ReceiptQuantity));
			query = query.Skip((page - 1) * size).Take(size);

			List<GarmentSampleReceiptFromBuyerListDto> garmentReceiptFromBuyerListDtos = _garmentSampleReceiptFromBuyerRepository
				.Find(query)
				.Select(ExGood => new GarmentSampleReceiptFromBuyerListDto(ExGood))
				.ToList();

			var dtoIds = garmentReceiptFromBuyerListDtos.Select(s => s.Id).ToList();
			var items = _garmentSampleReceiptFromBuyerItemRepository.Query
				.Where(s=>s.Deleted == false)
				 
				.Select(s => new { s.Identity, s.ReceiptId, s.ReceiptQuantity })
				.ToList();

			var itemIds = items.Select(s => s.Identity).ToList();
			Parallel.ForEach(garmentReceiptFromBuyerListDtos, dto =>
			{
				var currentItems = items.Where(w => w.ReceiptId == dto.Id);
				dto.TotalQuantity = currentItems.Sum(i => i.ReceiptQuantity);
			});

			await Task.Yield();
			return Ok(garmentReceiptFromBuyerListDtos, info: new
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

			GarmentSampleReceiptFromBuyerDto garmentReceiptFromBuyerDto = _garmentSampleReceiptFromBuyerRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentSampleReceiptFromBuyerDto(finishOut)
			{
				Items = _garmentSampleReceiptFromBuyerItemRepository.Find(o => o.ReceiptId == finishOut.Identity).OrderBy(a => a.Description).ThenBy(i => i.SizeName).Select(finishOutItem => new GarmentSampleReceiptFromBuyerItemDto(finishOutItem)
				{
				}).ToList()
			}
			).FirstOrDefault();

			await Task.Yield();
			return Ok(garmentReceiptFromBuyerDto);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleReceiptFromBuyerCommand command)
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
		public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleReceiptFromBuyerCommand command)
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
				RemoveGarmentSampleReceiptFromBuyerCommand command = new RemoveGarmentSampleReceiptFromBuyerCommand(guid);

			var order = await Mediator.Send(command);

			return Ok(order.Identity);

		}

	}
}
