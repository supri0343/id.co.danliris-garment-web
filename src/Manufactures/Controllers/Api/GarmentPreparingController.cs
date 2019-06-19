using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("preparings")]
    public class GarmentPreparingController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IMemoryCacheManager _cacheManager;

        public GarmentPreparingController(IServiceProvider serviceProvider, IMemoryCacheManager cacheManager) : base(serviceProvider)
        {
            _garmentPreparingRepository = Storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = Storage.GetRepository<IGarmentPreparingItemRepository>();
            _cacheManager = cacheManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                VerifyUser();
                var query = _garmentPreparingRepository.Read(order, select, filter);
                int totalRows = query.Count();
                var garmentPreparingDto = _garmentPreparingRepository.Find(query).Select(o => new GarmentPreparingDto(o)).ToArray();
                var garmentPreparingItemDto = _garmentPreparingItemRepository.Find(_garmentPreparingItemRepository.Query).Select(o => new GarmentPreparingItemDto(o)).ToList();
                var garmentPreparingItemDtoArray = _garmentPreparingItemRepository.Find(_garmentPreparingItemRepository.Query).Select(o => new GarmentPreparingItemDto(o)).ToArray();

                Parallel.ForEach(garmentPreparingDto, itemDto =>
                {
                    var garmentPreparingItems = garmentPreparingItemDto.Where(x => x.GarmentPreparingId == itemDto.Id).ToList();

                    itemDto.Items = garmentPreparingItems;
                    var selectedUnit = GetUnit(itemDto.UnitId.Id, WorkContext.Token).data.Name;

                    //if (selectedUnit != null && selectedUnit.data != null)
                    //{
                    //    itemDto.UnitId.Name = selectedUnit.data.Name;
                    //    itemDto.UnitId.Code = selectedUnit.data.Code;
                    //}

                    Parallel.ForEach(itemDto.Items, orderItem =>
                    {
                        var selectedProduct = GetGarmentProduct(orderItem.Product.Id, WorkContext.Token);
                        var selectedUom = GetUom(orderItem.Uom.Id, WorkContext.Token);

                        if (selectedProduct != null && selectedProduct.data != null)
                        {
                            orderItem.Product.Name = selectedProduct.data.Name;
                            orderItem.Product.Code = selectedProduct.data.Code;
                        }

                        if (selectedUom != null && selectedUom.data != null)
                        {
                            orderItem.Uom.Unit = selectedUom.data.Unit;
                        }
                    });

                    itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();
                });

                if (!string.IsNullOrEmpty(keyword))
                {
                    garmentPreparingItemDtoArray = garmentPreparingItemDto.Where(x => x.Product.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
                    List<GarmentPreparingDto> ListTemp = new List<GarmentPreparingDto>();
                    foreach (var a in garmentPreparingItemDtoArray)
                    {
                        var temp = garmentPreparingDto.Where(x => x.Id.Equals(a.GarmentPreparingId)).ToArray();
                        foreach (var b in temp)
                        {
                            ListTemp.Add(b);
                        }
                    }


                    var garmentPreparingDtoList = garmentPreparingDto.Where(x => x.UENNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                        || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                        || x.UnitId.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                        || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                        //|| x.Items.Where(y => y.ProductId.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                        ).ToList();

                    var i = 0;
                    foreach (var data in ListTemp)
                    {
                        foreach (var item in garmentPreparingDtoList)
                        {
                            if (data.Id == item.Id)
                            {
                                i++;
                            }
                        }
                        if (i == 0)
                        {
                            garmentPreparingDtoList.Add(data);
                        }
                    }
                    var garmentPreparingDtoListArray = garmentPreparingDtoList.ToArray();
                    if (order != "{}")
                    {
                        Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                        garmentPreparingDtoListArray = QueryHelper<GarmentPreparingDto>.Order(garmentPreparingDtoList.AsQueryable(), OrderDictionary).ToArray();
                    }
                    else
                    {
                        garmentPreparingDtoListArray = garmentPreparingDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
                    }

                    garmentPreparingDtoListArray = garmentPreparingDtoListArray.Take(size).Skip((page - 1) * size).ToArray();

                    await Task.Yield();
                    return Ok(garmentPreparingDtoListArray, info: new
                    {
                        page,
                        size,
                        count = totalRows
                    });
                }
                else
                {
                    if (order != "{}")
                    {
                        Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                        garmentPreparingDto = QueryHelper<GarmentPreparingDto>.Order(garmentPreparingDto.AsQueryable(), OrderDictionary).ToArray();
                    }
                    else
                    {
                        garmentPreparingDto = garmentPreparingDto.OrderByDescending(x => x.LastModifiedDate).ToArray();
                    }

                    garmentPreparingDto = garmentPreparingDto.Take(size).Skip((page - 1) * size).ToArray();

                    await Task.Yield();
                    return Ok(garmentPreparingDto, info: new
                    {
                        page,
                        size,
                        count = totalRows
                    });
                }
            } catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, MasterDataSettings.Endpoint+"test");
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var preparingId = Guid.Parse(id);
            VerifyUser();
            var preparingDto = _garmentPreparingRepository.Find(o => o.Identity == preparingId).Select(o => new GarmentPreparingDto(o)).FirstOrDefault();

            if (preparingDto == null)
                return NotFound();

            var selectedUnit = GetUnit(preparingDto.UnitId.Id, WorkContext.Token).data;
            
            if (selectedUnit != null)
            {
                preparingDto.UnitId.Name = selectedUnit.Name;
                preparingDto.UnitId.Code = selectedUnit.Code;
            }

            var itemConfigs = _garmentPreparingItemRepository.Find(x => x.GarmentPreparingId == preparingDto.Id).Select(o => new GarmentPreparingItemDto(o)).ToList();
            preparingDto.Items = itemConfigs;

            Parallel.ForEach(preparingDto.Items, orderItem =>
            {
                var selectedUOM = GetUom(orderItem.Uom.Id, WorkContext.Token).data;
                var selectedProduct = GetProduct(orderItem.Product.Id, WorkContext.Token).data;

                if (selectedUOM != null)
                {
                    orderItem.Uom.Unit = selectedUOM.Unit;
                }

                if (selectedProduct != null)
                {
                    orderItem.Product.Code = selectedProduct.Code;
                    orderItem.Product.Name = selectedProduct.Name;
                }

            });
            preparingDto.Items = preparingDto.Items.OrderBy(x => x.Id).ToList();
            await Task.Yield();

            return Ok(preparingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceGarmentPreparingCommand command)
        {
            try
            {
                VerifyUser();

                var garmentPreparingValidation = _garmentPreparingRepository.Find(o => o.UENId == command.UENId && o.UENNo == command.UENNo && o.UnitId == command.UnitId.Value
                                && o.ProcessDate == command.ProcessDate && o.RONo == command.RONo && o.Article == command.Article && o.IsCuttingIn == command.IsCuttingIn).Select(o => new GarmentPreparingDto(o)).FirstOrDefault();
                if (garmentPreparingValidation != null)
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        error = "Data sudah ada"
                    });

                var order = await Mediator.Send(command);

                await PutGarmentUnitExpenditureNoteCreate(command.UENId);

                return Ok(order.Identity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, [FromBody]UpdateGarmentPreparingCommand command)
        //{
        //    Guid orderId;
        //    try
        //    {
        //        VerifyUser();
        //        if (!Guid.TryParse(id, out orderId))
        //            return NotFound();

        //        command.SetId(orderId);


        //        var order = await Mediator.Send(command);

        //        return Ok(order.Identity);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }

        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            VerifyUser();
            var preparingId = Guid.Parse(id);

            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            var garmentPreparing = _garmentPreparingRepository.Find(x => x.Identity == preparingId).Select(o => new GarmentPreparingDto(o)).FirstOrDefault();

            var command = new RemoveGarmentPreparingCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);
            await PutGarmentUnitExpenditureNoteDelete (garmentPreparing.UENId);

            return Ok(order.Identity);
        }

    }
}
