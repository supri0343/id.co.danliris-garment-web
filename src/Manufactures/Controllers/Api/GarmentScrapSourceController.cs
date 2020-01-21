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
	[Route("scrap-sources")]
	public class GarmentScrapSourceController : Barebone.Controllers.ControllerApiBase
	{
		private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;

		public GarmentScrapSourceController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentScrapSourceRepository = Storage.GetRepository<IGarmentScrapSourceRepository>();

		}

		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapSourceRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapSourceDto> listDtos = _garmentScrapSourceRepository
				.Find(query)
				.Select(data => new GarmentScrapSourceDto(data))
				.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
			{
				page,
				size,
				count
			});
		}
	}
}
