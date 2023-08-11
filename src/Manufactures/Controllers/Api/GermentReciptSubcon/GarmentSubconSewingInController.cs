using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using Manufactures.Dtos;
using Manufactures.Dtos.GermentReciptSubcon.GarmentSewingIn;
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
    [Route("subcon-sewing-ins")]
    public class GarmentSubconSewingInController : ControllerApiBase
    {
        private readonly IGarmentSubconSewingInRepository _garmentSewingRepository;
        private readonly IGarmentSubconSewingInItemRepository _garmentSewingItemRepository;

        public GarmentSubconSewingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSewingRepository = Storage.GetRepository<IGarmentSubconSewingInRepository>();
            _garmentSewingItemRepository = Storage.GetRepository<IGarmentSubconSewingInItemRepository>();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSewingInItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);
            var ids = query.Select(x => x.Identity).ToList();
            var sewingInItems = _garmentSewingItemRepository.Query.Where(x => ids.Contains(x.SewingInId)).Select(sewingInItem => new
            {
                sewingInItem.ProductCode,
                sewingInItem.ProductName,
                sewingInItem.Quantity,
                sewingInItem.RemainingQuantity,
                sewingInItem.Color,
                sewingInItem.SewingInId
            }).ToList();
            List<GarmentSubconSewingInListDto> garmentSewingInListDtos = _garmentSewingRepository.Find(query).Select(sewingIn =>
            {;

                var items = sewingInItems.Where(o => o.SewingInId == sewingIn.Identity);

                return new GarmentSubconSewingInListDto(sewingIn)
                {
                    Products = items.Select(i => i.ProductName).Distinct().ToList(),
                    TotalQuantity = Math.Round(items.Sum(i => i.Quantity), 2),
                    TotalRemainingQuantity = Math.Round(items.Sum(i => i.RemainingQuantity), 2),
                    Colors = items.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList()
                };
            }).ToList();

            await Task.Yield();
            return Ok(garmentSewingInListDtos, info: new
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

            GarmentSubconSewingInDto garmentSewingInDto = _garmentSewingRepository.Find(o => o.Identity == guid).Select(sewingIn => new GarmentSubconSewingInDto(sewingIn)
            {
                Items = _garmentSewingItemRepository.Find(o => o.SewingInId == sewingIn.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(sewingInItem => new GarmentSubconSewingInItemDto(sewingInItem)
                ).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSewingInDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconSewingInCommand command)
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

            RemoveGarmentSubconSewingInCommand command = new RemoveGarmentSubconSewingInCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }


        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var newQuery = _garmentSewingRepository.ReadExecute(query);
            await Task.Yield();
            return Ok(newQuery, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("approve")]
        public async Task<IActionResult> ApproveLoading([FromBody] UpdateApproveGarmentSubconSewingInsCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingRepository.ReadComplete(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSewingInItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            var garmentSewingInDto = _garmentSewingRepository.Find(query).Select(o => new GarmentSubconSewingInListDto(o)).ToArray();

            await Task.Yield();
            return Ok(garmentSewingInDto, info: new
            {
                page,
                size,
                total,
                totalQty
            });
        }
    }
}
