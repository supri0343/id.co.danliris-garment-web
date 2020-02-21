using Barebone.Controllers;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
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
    [Route("finished-good-stocks")]
    public class GarmentFinishedGoodStockController : ControllerApiBase
    {
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;

        public GarmentFinishedGoodStockController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentFinishedGoodStockRepository = Storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishedGoodStockRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentFinishedGoodStockDto> garmentFinishedGoodStockDtos = _garmentFinishedGoodStockRepository
                .Find(query)
                .Select(finGood => new GarmentFinishedGoodStockDto(finGood))
                .ToList();

            var dtoIds = garmentFinishedGoodStockDtos.Select(s => s.Id).ToList();

            await Task.Yield();
            return Ok(garmentFinishedGoodStockDtos, info: new
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

            GarmentFinishedGoodStockDto garmentFinishedGoodStockDto = _garmentFinishedGoodStockRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentFinishedGoodStockDto(finishOut)
            {
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentFinishedGoodStockDto);
        }
    }
}
