using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Application.GarmentAvalProducts.Queries.GetForLoaderAval_BC;
using Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare;
using Manufactures.Application.GarmentPreparings.Queries.GetPrepareTraceable;
using Manufactures.Application.GarmentPreparings.Queries.GetWIP;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories;
using Manufactures.Dtos;
using Manufactures.Dtos.GermentReciptSubcon.GarmentPreparing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GermentReciptSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-preparings")]
    public class GarmentSubconPreparingController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentSubconPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentSubconPreparingItemRepository _garmentPreparingItemRepository;

        public GarmentSubconPreparingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentPreparingRepository = Storage.GetRepository<IGarmentSubconPreparingRepository>();
            _garmentPreparingItemRepository = Storage.GetRepository<IGarmentSubconPreparingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentPreparingRepository.ReadOptimized(order, filter, keyword);
            var newQuery = _garmentPreparingRepository.ReadExecute(query, keyword).ToList();
            int totalRows = query.Count();
            var data = newQuery.Skip((page - 1) * size).Take(size);
            await Task.Yield();
            return Ok(data, info: new
            {
                page,
                size,
                total = totalRows
            });
        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var query = _garmentPreparingRepository.Read(order, select, filter);
            int totalRows = query.Count();
            var garmentPreparingDto = _garmentPreparingRepository.Find(query).Select(o => new GarmentSubconPreparingDto(o)).ToArray();


            if (!string.IsNullOrEmpty(keyword))
            {
                var garmentPreparingDtoList = garmentPreparingDto.Where(x => x.UENNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Unit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || (x.Article != null && x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    //|| x.Items.Where(y => y.ProductId.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    ).ToList();

                var garmentPreparingDtoListArray = garmentPreparingDtoList.ToArray();
                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentPreparingDtoListArray = QueryHelper<GarmentSubconPreparingDto>.Order(garmentPreparingDtoList.AsQueryable(), OrderDictionary).ToArray();
                }
                else
                {
                    garmentPreparingDtoListArray = garmentPreparingDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
                }

                garmentPreparingDtoListArray = garmentPreparingDtoListArray.Skip((page - 1) * size).Take(size).ToArray();

                await Task.Yield();
                return Ok(garmentPreparingDtoListArray, info: new
                {
                    page,
                    size,
                    total = totalRows
                });
            }
            else
            {
                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentPreparingDto = QueryHelper<GarmentSubconPreparingDto>.Order(garmentPreparingDto.AsQueryable(), OrderDictionary).ToArray();
                }
                else
                {
                    garmentPreparingDto = garmentPreparingDto.OrderByDescending(x => x.LastModifiedDate).ToArray();
                }

                garmentPreparingDto = garmentPreparingDto.Skip((page - 1) * size).Take(size).ToArray();

                await Task.Yield();
                return Ok(garmentPreparingDto, info: new
                {
                    page,
                    size,
                    total = totalRows
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var preparingId = Guid.Parse(id);
            VerifyUser();
            var preparingDto = _garmentPreparingRepository.Find(o => o.Identity == preparingId).Select(o => new GarmentSubconPreparingDto(o)).FirstOrDefault();

            if (preparingDto == null)
                return NotFound();

            var itemConfigs = _garmentPreparingItemRepository.Find(x => x.GarmentSubconPreparingId == preparingDto.Id).Select(o => new GarmentSubconPreparingItemDto(o)).ToList();
            preparingDto.Items = itemConfigs;

            preparingDto.Items = preparingDto.Items.OrderBy(x => x.Id).ToList();
            await Task.Yield();

            return Ok(preparingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceSubconGarmentPreparingCommand command)
        {
            try
            {
                VerifyUser();

                var garmentPreparingValidation = _garmentPreparingRepository.Find(o => o.UENId == command.UENId && o.UENNo == command.UENNo && o.UnitId == command.Unit.Id
                                && o.ProcessDate == command.ProcessDate && o.RONo == command.RONo && o.Article == command.Article && o.IsCuttingIn == command.IsCuttingIn).Select(o => new GarmentSubconPreparingDto(o)).FirstOrDefault();
                if (garmentPreparingValidation != null)
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        error = "Data sudah ada"
                    });

                var order = await Mediator.Send(command);

                await PutGarmentSubconUnitExpenditureNoteCreate(command.UENId);

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

            var garmentPreparing = _garmentPreparingRepository.Find(x => x.Identity == preparingId).Select(o => new GarmentSubconPreparingDto(o)).FirstOrDefault();

            var command = new RemoveSubconGarmentPreparingCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);
            await PutGarmentSubconUnitExpenditureNoteDelete (garmentPreparing.UENId);

            return Ok(order.Identity);
        }
		[HttpGet("loader/ro")]
        public async Task<IActionResult> GetLoaderByRO(string keyword, string filter = "{}")
        {
            var query = _garmentPreparingRepository.Read(null, null, filter);
            query = query.Where(x => x.GarmentPreparingItem.Any(a => a.RemainingQuantity > 0));
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(o => o.RONo.Contains(keyword));
            }

            var rOs = query.Select(o => new { o.RONo, o.Article }).Distinct().ToList();

            await Task.Yield();

            return Ok(rOs);
        }
		//[HttpGet("monitoring")]
		//public async Task<IActionResult> GetMonitoring(int unit, DateTime dateFrom, DateTime dateTo,int page = 1, int size = 25, string Order = "{}")
		//{
		//	VerifyUser();
		//	GetMonitoringPrepareQuery query = new GetMonitoringPrepareQuery(page,size, Order, unit,dateFrom,dateTo,WorkContext.Token);
		//	var viewModel = await Mediator.Send(query);

		//	return Ok(viewModel.garmentMonitorings , info: new
		//	{
		//		page,
		//		size,
		//		viewModel.count
		//	});
		//}
		//[HttpGet("download")]
		//public async Task<IActionResult> GetXls(int unit, DateTime dateFrom, DateTime dateTo,string type, int page = 1, int size = 25, string Order = "{}")
		//{
		//	try
		//	{
		//		VerifyUser();
		//		GetXlsPrepareQuery query = new GetXlsPrepareQuery(page, size, Order, unit, dateFrom, dateTo,type, WorkContext.Token);
		//		byte[] xlsInBytes;

		//		var xls = await Mediator.Send(query);

		//		string filename = "Laporan Prepare";

		//		if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

		//		if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
		//		filename += ".xlsx";

		//		xlsInBytes = xls.ToArray();
		//		var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
		//		return file;
		//	}
		//	catch (Exception e)
		//	{
		//		return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
		//	}
		//}

  //      [HttpPut("update-dates")]
  //      public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentPreparingCommand command)
  //      {
  //          VerifyUser();

  //          if (command.Date==null || command.Date == DateTimeOffset.MinValue)
  //              return BadRequest(new
  //              {
  //                  code = HttpStatusCode.BadRequest,
  //                  error = "Tanggal harus diisi"
  //              });
  //          else if (command.Date.Date > DateTimeOffset.Now.Date)
  //              return BadRequest(new
  //              {
  //                  code = HttpStatusCode.BadRequest,
  //                  error = "Tanggal tidak boleh lebih dari hari ini"
  //              });

  //          var order = await Mediator.Send(command);

  //          return Ok();
  //      }

  //      [HttpGet("wip")]
  //      public async Task<IActionResult> GetWIP(DateTime date, int page = 1, int size = 25, string Order = "{}")
  //      {
  //          VerifyUser();
  //          GetWIPQuery query = new GetWIPQuery(page, size, Order,date, WorkContext.Token);
  //          var viewModel = await Mediator.Send(query);

  //          return Ok(viewModel.garmentWIP, info: new
  //          {
  //              page,
  //              size,
  //              viewModel.count
  //          });
  //      }

  //      [HttpGet("wip/download")]
  //      public async Task<IActionResult> GetXlsWIP(DateTime date, int page = 1, int size = 25, string Order = "{}")
  //      {
  //          try
  //          {
  //              VerifyUser();
  //              GetXlsWIPQuery query = new GetXlsWIPQuery(page, size, Order, date, WorkContext.Token);
  //              byte[] xlsInBytes;

  //              var xls = await Mediator.Send(query);

  //              string filename = "Laporan WIP";

  //              if (date != null) filename += " " + ((DateTime)date).ToString("dd-MM-yyyy");
  //              filename += ".xlsx";

  //              xlsInBytes = xls.ToArray();
  //              var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
  //              return file;
  //          }
  //          catch (Exception e)
  //          {
  //              return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
  //          }
  //      }

  //      [HttpGet("byRONO")]
  //      public async Task<IActionResult> GetpreparingbyRONO([FromBody]string RO)
  //      {

  //          VerifyUser();

  //          GetPrepareTraceableQuery query = new GetPrepareTraceableQuery(RO, WorkContext.Token);
  //          var viewModel = await Mediator.Send(query);

  //          return Ok(viewModel.getPrepareTraceableDtos);
  //      }

  //      [HttpGet("with-bc")]
  //      public async Task<IActionResult> GetAvalProductWithBC(int? unit, string ro, int page = 1, int size = 25)
  //      {
  //          VerifyUser();
  //          GetForLoaderAval_BCQuery query = new GetForLoaderAval_BCQuery(unit, ro, WorkContext.Token);
  //          var viewModel = await Mediator.Send(query);

  //          return Ok(viewModel, info: new
  //          {
  //              page,
  //              size,
  //              viewModel.count
  //          });
  //      }
    }
}
