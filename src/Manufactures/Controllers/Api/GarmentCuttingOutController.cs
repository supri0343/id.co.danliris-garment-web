using Barebone.Controllers;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("cutting-outs")]
    public class GarmentCuttingOutController : ControllerApiBase
    {
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public GarmentCuttingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentCuttingOutRepository = Storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = Storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = Storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            _garmentCuttingInRepository = Storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = Storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = Storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSewingDORepository = Storage.GetRepository<IGarmentSewingDORepository>();
            _garmentSewingDOItemRepository = Storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentCuttingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();
            List<GarmentCuttingOutListDto> garmentCuttingOutListDtos = _garmentCuttingOutRepository.Find(query).Select(cutOut =>
            {
                var items = _garmentCuttingOutItemRepository.Query.Where(o => o.CutOutId == cutOut.Identity).Select(cutOutItem => new
                {
                    cutOutItem.ProductCode,
                    details = _garmentCuttingOutDetailRepository.Query.Where(o => o.CutOutItemId == cutOutItem.Identity).Select(cutOutDetail => new
                    {
                        cutOutDetail.CuttingOutQuantity,
                        cutOutDetail.RemainingQuantity,
                    })
                }).ToList();

                return new GarmentCuttingOutListDto(cutOut)
                {
                    Products = items.Select(i => i.ProductCode).ToList(),
                    TotalCuttingOutQuantity = items.Sum(i => i.details.Sum(d => d.CuttingOutQuantity)),
                    TotalRemainingQuantity = items.Sum(i => i.details.Sum(d => d.RemainingQuantity))
                };
            }).ToList();

            await Task.Yield();
            return Ok(garmentCuttingOutListDtos, info: new
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

            GarmentCuttingOutDto garmentCuttingOutDto = _garmentCuttingOutRepository.Find(o => o.Identity == guid).Select(cutOut => new GarmentCuttingOutDto(cutOut)
            {
                Items = _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).Select(cutOutItem => new GarmentCuttingOutItemDto(cutOutItem)
                {
                    Details = _garmentCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).Select(cutOutDetail => new GarmentCuttingOutDetailDto(cutOutDetail)
                    {
                        //PreparingRemainingQuantity = _garmentPreparingItemRepository.Query.Where(o => o.Identity == cutInDetail.PreparingItemId).Select(o => o.RemainingQuantity).FirstOrDefault() + cutInDetail.PreparingQuantity,
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentCuttingOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentCuttingOutCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentCuttingOutCommand command)
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
            var usedData = false;
            var garmentSewingDO = _garmentSewingDORepository.Query.Where(o => o.CuttingOutId == guid).Select(o => new GarmentSewingDO(o)).Single();

            _garmentSewingDOItemRepository.Find(x => x.SewingDOId == garmentSewingDO.Identity).ForEach(async sewingDOItem =>
            {
                if (sewingDOItem.RemainingQuantity < sewingDOItem.Quantity)
                {
                    usedData = true;
                }
            });

            if(usedData == true)
            {
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Data Sudah Digunakan di Sewing In"
                });
            } else
            {
                RemoveGarmentCuttingOutCommand command = new RemoveGarmentCuttingOutCommand(guid);
                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
        }

       
    }
}