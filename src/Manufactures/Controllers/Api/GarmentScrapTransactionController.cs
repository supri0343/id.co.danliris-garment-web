using Manufactures.Domain.GarmentScrapTransactions.Commands;
using Manufactures.Domain.GarmentScrapTransactions.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
	[ApiController]
	[Authorize]
	[Route("scrap-transactions")]
	public class GarmentScrapTransactionController : Barebone.Controllers.ControllerApiBase
	{
		private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;

		public GarmentScrapTransactionController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentScrapTransactionRepository = Storage.GetRepository<IGarmentScrapTransactionRepository>();

		}

		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapTransactionRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapTransactionDto> listDtos = _garmentScrapTransactionRepository
				.Find(query)
				.Select(data => new GarmentScrapTransactionDto(data))
				.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
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

			GarmentScrapTransactionDto listDto = _garmentScrapTransactionRepository.Find(o => o.Identity == guid).Select(data => new GarmentScrapTransactionDto(data)).FirstOrDefault();

			await Task.Yield();
			return Ok(listDto);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PlaceGarmentScrapTransactionCommand command)
		{
			VerifyUser();

			var data = await Mediator.Send(command);

			return Ok(data.Identity);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			Guid guid = Guid.Parse(id);

			VerifyUser();

			RemoveGarmentScrapTransactionCommand command = new RemoveGarmentScrapTransactionCommand(guid);
			var data = await Mediator.Send(command);

			return Ok(data.Identity);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentScrapTransactionCommand command)
		{
			Guid guid = Guid.Parse(id);

			command.SetIdentity(guid);

			VerifyUser();

			var order = await Mediator.Send(command);

			return Ok(order.Identity);
		}
	}
}
