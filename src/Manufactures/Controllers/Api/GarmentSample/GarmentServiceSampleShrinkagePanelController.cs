using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.ExcelTemplates;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Dtos.GarmentSample.GarmentServiceSampleShrinkagePanels;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("service-sample-shrinkage-panels")]
    public class GarmentServiceSampleShrinkagePanelController : ControllerApiBase
    {
        private readonly IGarmentServiceSampleShrinkagePanelRepository _garmentServiceSampleShrinkagePanelRepository;
        private readonly IGarmentServiceSampleShrinkagePanelItemRepository _garmentServiceSampleShrinkagePanelItemRepository;
        private readonly IGarmentServiceSampleShrinkagePanelDetailRepository _garmentServiceSampleShrinkagePanelDetailRepository;

        public GarmentServiceSampleShrinkagePanelController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSampleShrinkagePanelRepository = Storage.GetRepository<IGarmentServiceSampleShrinkagePanelRepository>();
            _garmentServiceSampleShrinkagePanelItemRepository = Storage.GetRepository<IGarmentServiceSampleShrinkagePanelItemRepository>();
            _garmentServiceSampleShrinkagePanelDetailRepository = Storage.GetRepository<IGarmentServiceSampleShrinkagePanelDetailRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSampleShrinkagePanelRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSampleShrinkagePanelListDto> garmentServiceSampleShrinkagePanelListDtos = _garmentServiceSampleShrinkagePanelRepository
                .Find(query)
                .Select(ServiceSampleShrinkagePanel => new GarmentServiceSampleShrinkagePanelListDto(ServiceSampleShrinkagePanel))
                .ToList();

            var dtoIds = garmentServiceSampleShrinkagePanelListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSampleShrinkagePanelItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSampleShrinkagePanelId))
                .Select(s => new { s.Identity, s.ServiceSampleShrinkagePanelId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            await Task.Yield();
            return Ok(garmentServiceSampleShrinkagePanelListDtos, info: new
            {
                page,
                size,
                total,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentServiceSampleShrinkagePanelDto garmentServiceSampleShrinkagePanelDto = _garmentServiceSampleShrinkagePanelRepository.Find(o => o.Identity == guid).Select(serviceSampleShrinkagePanel => new GarmentServiceSampleShrinkagePanelDto(serviceSampleShrinkagePanel)
            {
                Items = _garmentServiceSampleShrinkagePanelItemRepository.Find(o => o.ServiceSampleShrinkagePanelId == serviceSampleShrinkagePanel.Identity).Select(SampleItem => new GarmentServiceSampleShrinkagePanelItemDto(SampleItem)
                {
                    Details = _garmentServiceSampleShrinkagePanelDetailRepository.Find(o => o.ServiceSampleShrinkagePanelItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleShrinkagePanelDetailDto(SampleDetail)).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSampleShrinkagePanelDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSampleShrinkagePanelCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                //Update isPreparing UEN 
                var listUenNo = command.Items.Select(x => x.UnitExpenditureNo).Distinct();
                if (listUenNo.Count() > 0)
                {
                    var joinUenNo = string.Join(",", listUenNo);
                    await PutGarmentUnitExpenditureNoteByNo(joinUenNo, true);
                }
                
                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSampleShrinkagePanelCommand command)
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
            //Get List of UENNo
            List<string> listUenNo = new List<string>();
            _garmentServiceSampleShrinkagePanelRepository.Find(x => x.Identity == guid).ForEach(async header =>
                _garmentServiceSampleShrinkagePanelItemRepository.Find(x => x.ServiceSampleShrinkagePanelId == header.Identity).ForEach(async item =>
                {
                    listUenNo.Add(item.UnitExpenditureNo);
                })
            );

            RemoveGarmentServiceSampleShrinkagePanelCommand command = new RemoveGarmentServiceSampleShrinkagePanelCommand(guid);
            var order = await Mediator.Send(command);

            if (listUenNo.Count() > 0)
            {
                var joinUenNo = string.Join(",", listUenNo.Distinct());
                await PutGarmentUnitExpenditureNoteByNo(joinUenNo, false);
            }


            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var getUENNo = _garmentServiceSampleShrinkagePanelItemRepository.Query.Where(x => keyword.Contains(x.UnitExpenditureNo)).Select(s => new { s.UnitExpenditureNo }).ToList().Count();

            if (getUENNo != 0)
            {
                var query = _garmentServiceSampleShrinkagePanelItemRepository.ReadItem(page, size, order, keyword, filter);
                var count = query.Count();

                var garmentServiceSampleShrinkagePanelDto = _garmentServiceSampleShrinkagePanelRepository.Find(_garmentServiceSampleShrinkagePanelRepository.Query).Select(o => new GarmentServiceSampleShrinkagePanelDto(o)).ToArray();
                var garmentServiceSampleShrinkagePanelItemDto = _garmentServiceSampleShrinkagePanelItemRepository.Find(query).Select(o => new GarmentServiceSampleShrinkagePanelItemDto(o)).ToList();
                var garmentServiceSampleShrinkagePanelDetailDto = _garmentServiceSampleShrinkagePanelDetailRepository.Find(_garmentServiceSampleShrinkagePanelDetailRepository.Query).Select(o => new GarmentServiceSampleShrinkagePanelDetailDto(o)).ToList();

                Parallel.ForEach(garmentServiceSampleShrinkagePanelDto, itemDto =>
                {
                    var garmentServiceSampleShrinkagePanelItems = garmentServiceSampleShrinkagePanelItemDto.Where(x => x.ServiceSampleShrinkagePanelId == itemDto.Id).OrderBy(x => x.Id).ToList();

                    itemDto.Items = garmentServiceSampleShrinkagePanelItems;
                    Parallel.ForEach(itemDto.Items, detailDto =>
                    {
                        var garmentServiceSampleShrinkagePanelDetails = garmentServiceSampleShrinkagePanelDetailDto.Where(x => x.ServiceSampleShrinkagePanelItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                        detailDto.Details = garmentServiceSampleShrinkagePanelDetails;
                    });
                });

                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentServiceSampleShrinkagePanelDto = QueryHelper<GarmentServiceSampleShrinkagePanelDto>.Order(garmentServiceSampleShrinkagePanelDto.AsQueryable(), OrderDictionary).ToArray();
                }

                await Task.Yield();
                return Ok(garmentServiceSampleShrinkagePanelDto, info: new
                {
                    page,
                    size,
                    count
                });
            }
            else
            {
                var query = _garmentServiceSampleShrinkagePanelRepository.Read(page, size, order, keyword, filter);
                var count = query.Count();

                var garmentServiceSampleShrinkagePanelDto = _garmentServiceSampleShrinkagePanelRepository.Find(query).Select(o => new GarmentServiceSampleShrinkagePanelDto(o)).ToArray();
                var garmentServiceSampleShrinkagePanelItemDto = _garmentServiceSampleShrinkagePanelItemRepository.Find(_garmentServiceSampleShrinkagePanelItemRepository.Query).Select(o => new GarmentServiceSampleShrinkagePanelItemDto(o)).ToList();
                var garmentServiceSampleShrinkagePanelDetailDto = _garmentServiceSampleShrinkagePanelDetailRepository.Find(_garmentServiceSampleShrinkagePanelDetailRepository.Query).Select(o => new GarmentServiceSampleShrinkagePanelDetailDto(o)).ToList();

                Parallel.ForEach(garmentServiceSampleShrinkagePanelDto, itemDto =>
                {
                    var garmentServiceSampleShrinkagePanelItems = garmentServiceSampleShrinkagePanelItemDto.Where(x => x.ServiceSampleShrinkagePanelId == itemDto.Id).OrderBy(x => x.Id).ToList();

                    itemDto.Items = garmentServiceSampleShrinkagePanelItems;
                    Parallel.ForEach(itemDto.Items, detailDto =>
                    {
                        var garmentServiceSampleShrinkagePanelDetails = garmentServiceSampleShrinkagePanelDetailDto.Where(x => x.ServiceSampleShrinkagePanelItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                        detailDto.Details = garmentServiceSampleShrinkagePanelDetails;
                    });
                });

                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentServiceSampleShrinkagePanelDto = QueryHelper<GarmentServiceSampleShrinkagePanelDto>.Order(garmentServiceSampleShrinkagePanelDto.AsQueryable(), OrderDictionary).ToArray();
                }

                await Task.Yield();
                return Ok(garmentServiceSampleShrinkagePanelDto, info: new
                {
                    page,
                    size,
                    count
                });
            }
        }

        [HttpGet("get-pdf/{id}")]
        public async Task<IActionResult> GetPdf(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            //int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentServiceSampleShrinkagePanelDto garmentServiceSampleShrinkagePanelDto = _garmentServiceSampleShrinkagePanelRepository.Find(o => o.Identity == guid).Select(Sample => new GarmentServiceSampleShrinkagePanelDto(Sample)
            {
                Items = _garmentServiceSampleShrinkagePanelItemRepository.Find(o => o.ServiceSampleShrinkagePanelId == Sample.Identity).Select(SampleItem => new GarmentServiceSampleShrinkagePanelItemDto(SampleItem)
                {
                    Details = _garmentServiceSampleShrinkagePanelDetailRepository.Find(o => o.ServiceSampleShrinkagePanelItemId == SampleItem.Identity).Select(SampleDetail => new GarmentServiceSampleShrinkagePanelDetailDto(SampleDetail)
                    {
                        Composition = GetProduct(SampleDetail.ProductId.Value, WorkContext.Token).data.Composition
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            var stream = GarmentServiceSampleShrinkagePanelPDFTemplate.Generate(garmentServiceSampleShrinkagePanelDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentServiceSampleShrinkagePanelDto.ServiceSubconShrinkagePanelNo}.pdf"
            };
        }

        [HttpGet("getXls")]
        public async Task<IActionResult> GetXls(DateTime dateFrom, DateTime dateTo, string token)
        {
            try
            {
                VerifyUser();
                GetXlsSampleServiceSampleShrinkagePanelsQuery query = new GetXlsSampleServiceSampleShrinkagePanelsQuery(dateFrom, dateTo, token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Sample Shrinkage BB";

                if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

                if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");

                filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch(Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
