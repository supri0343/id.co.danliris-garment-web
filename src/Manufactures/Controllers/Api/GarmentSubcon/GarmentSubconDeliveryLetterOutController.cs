using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOComponentServiceReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOCuttingSewingReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOGarmentWashReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOSewingReport;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Dtos;
using Manufactures.Dtos.GarmentSubcon;
using Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconExpenditureGoodDtoos;
using Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconExpenditureGoodItemDtos;
using Manufactures.Helpers.PDFTemplates.GarmentSubcon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-delivery-letter-outs")]
    public class GarmentSubconDeliveryLetterOutController : ControllerApiBase
    {
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutDetailRepository _garmentSubconDeliveryLetterOutDetailRepository;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;
        private readonly IGarmentServiceSubconSewingItemRepository _garmentServiceSubconSewingItemRepository;
        private readonly IGarmentServiceSubconSewingDetailRepository _garmentServiceSubconSewingDetailRepository;
        private readonly IGarmentServiceSubconExpenditureGoodRepository _garmentServiceSubconExpenditureGoodRepository;
        private readonly IGarmentServiceSubconExpenditureGoodtemRepository _garmentServiceSubconExpenditureGoodItemRepository;

        public GarmentSubconDeliveryLetterOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSubconDeliveryLetterOutRepository = Storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = Storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentSubconDeliveryLetterOutDetailRepository = Storage.GetRepository<IGarmentSubconDeliveryLetterOutDetailRepository>();
            _garmentSubconContractRepository = Storage.GetRepository<IGarmentSubconContractRepository>();
            _garmentCuttingOutRepository = Storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentCuttingOutItemRepository = Storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentServiceSubconSewingRepository = Storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconSewingItemRepository = Storage.GetRepository<IGarmentServiceSubconSewingItemRepository>();
            _garmentServiceSubconSewingDetailRepository = Storage.GetRepository<IGarmentServiceSubconSewingDetailRepository>();
            _garmentServiceSubconExpenditureGoodRepository = Storage.GetRepository<IGarmentServiceSubconExpenditureGoodRepository>();
            _garmentServiceSubconExpenditureGoodItemRepository = Storage.GetRepository<IGarmentServiceSubconExpenditureGoodtemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconDeliveryLetterOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSubconDeliveryLetterOutItem.Sum(b => b.Quantity));

            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSubconDeliveryLetterOutListDto> garmentSubconDeliveryLetterOutListDtos = _garmentSubconDeliveryLetterOutRepository
                .Find(query)
                .Select(subcon => new GarmentSubconDeliveryLetterOutListDto(subcon))
                .ToList();

            var garmentdeliveryLetterOutItemDto = _garmentSubconDeliveryLetterOutItemRepository.
                Find(_garmentSubconDeliveryLetterOutItemRepository.Query).
                Select(o => new GarmentSubconDeliveryLetterOutItemDto(o))
                .ToList();

            var garmentSubconDeliveryLetterOutDetailDto = _garmentSubconDeliveryLetterOutDetailRepository.
                Find(_garmentSubconDeliveryLetterOutDetailRepository.Query).
                Select(o => new GarmentSubconDeliveryLetterOutDetailDto(o)).
                ToList();

            Parallel.ForEach(garmentSubconDeliveryLetterOutListDtos, dto =>
            {
                var currentItems = garmentdeliveryLetterOutItemDto.Where(s => s.SubconDeliveryLetterOutId == dto.Id).ToList();
                dto.Items = currentItems;

                Parallel.ForEach(dto.Items, DetailDto =>
                {
                    var garmentSubconDeliveryLetterOutDetails = garmentSubconDeliveryLetterOutDetailDto.Where(x => x.SubconDeliveryLetterOutItemId == DetailDto.Id).OrderBy(s => s.Id).ToList();
                    DetailDto.Details = garmentSubconDeliveryLetterOutDetails;
                });

            });

            await Task.Yield();
            return Ok(garmentSubconDeliveryLetterOutListDtos, info: new
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

            GarmentSubconDeliveryLetterOutDto garmentSubconDeliveryLetterOutDto = _garmentSubconDeliveryLetterOutRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconDeliveryLetterOutDto(subcon)
            {
                Items = _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subcon.Identity).Select(subconItem => new GarmentSubconDeliveryLetterOutItemDto(subconItem)
                {
                    Details = _garmentSubconDeliveryLetterOutDetailRepository.Find(o => o.SubconDeliveryLetterOutItemId == subconItem.Identity).Select(subconDetail => new GarmentSubconDeliveryLetterOutDetailDto(subconDetail) {}).ToList()
            }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSubconDeliveryLetterOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconDeliveryLetterOutCommand command)
        {
            try
            {

                VerifyUser();

                var order = await Mediator.Send(command);

                //if(command.SubconCategory== "SUBCON CUTTING SEWING")
                //    await PutGarmentUnitExpenditureNoteCreate(command.UENId);
                if (command.SubconCategory == "SUBCON CUTTING SEWING")
                {
                    var uenIds = command.Items.Select(x => x.UENId).Distinct();

                    foreach (var a in uenIds.Distinct())
                    {
                        await PutGarmentUnitExpenditureNoteCreate(a);
                    }
                }else if (command.SubconCategory == "SUBCON JASA KOMPONEN" || command.SubconCategory == "SUBCON SEWING")
                {
                    List<int> uenIds = new List<int>();
                    command.Items.ForEach(x => x.Details.ForEach(r => uenIds.Add(r.UENId)));

                    foreach (var a in uenIds.Distinct())
                    {
                        await PutGarmentUnitExpenditureNoteCreate(a);
                    }
                }
                    return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconDeliveryLetterOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSubconDeliveryLetterOutDto = _garmentSubconDeliveryLetterOutRepository.Find(query).Select(o => new GarmentSubconDeliveryLetterOutDto(o)).ToArray();
            var garmentSubconDeliveryLetterOutItemDto = _garmentSubconDeliveryLetterOutItemRepository.Find(_garmentSubconDeliveryLetterOutItemRepository.Query).Select(o => new GarmentSubconDeliveryLetterOutItemDto(o)).ToList();
            var garmentSubconDeliveryLetterOutDetailDto = _garmentSubconDeliveryLetterOutDetailRepository.Find(_garmentSubconDeliveryLetterOutDetailRepository.Query).Select(o => new GarmentSubconDeliveryLetterOutDetailDto(o)).ToList();

            Parallel.ForEach(garmentSubconDeliveryLetterOutDto, itemDto =>
            {
                var garmentSubconDeliveryLetterOutItems = garmentSubconDeliveryLetterOutItemDto.Where(x => x.SubconDeliveryLetterOutId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentSubconDeliveryLetterOutItems;
                Parallel.ForEach(itemDto.Items, DetailDto =>
                {
                    var garmentSubconDeliveryLetterOutDetails = garmentSubconDeliveryLetterOutDetailDto.Where(x => x.SubconDeliveryLetterOutItemId == DetailDto.Id).OrderBy(s => s.Id).ToList();
                    DetailDto.Details = garmentSubconDeliveryLetterOutDetails;
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSubconDeliveryLetterOutDto = QueryHelper<GarmentSubconDeliveryLetterOutDto>.Order(garmentSubconDeliveryLetterOutDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSubconDeliveryLetterOutDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconDeliveryLetterOutCommand command)
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
          
            RemoveGarmentSubconDeliveryLetterOutCommand command = new RemoveGarmentSubconDeliveryLetterOutCommand(guid);
            var garmentSubconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Find(x => x.Identity == guid).Select(o => new GarmentSubconDeliveryLetterOutDto(o)).FirstOrDefault();
            var itemsData = _garmentSubconDeliveryLetterOutItemRepository.Find(s => s.SubconDeliveryLetterOutId == garmentSubconDeliveryLetterOut.Id).Select(x => new GarmentSubconDeliveryLetterOutItemDto(x)).ToList();
            var detailData = _garmentSubconDeliveryLetterOutDetailRepository.Find(x => x.SubconDeliveryLetterOutItemId == itemsData.Select(s => s.Id).First()).Select(x => new GarmentSubconDeliveryLetterOutDetailDto(x)).ToList();

            var order = await Mediator.Send(command);

            if( garmentSubconDeliveryLetterOut.SubconCategory == "SUBCON JASA KOMPONEN" || garmentSubconDeliveryLetterOut.SubconCategory == "SUBCON SEWING")
            {
                if(detailData.Count > 0)
                {
                    foreach (var a in detailData.Select(x => x.UENId).Distinct())
                    {
                        await PutGarmentUnitExpenditureNoteDelete(a);
                    }
                }
            }else if (garmentSubconDeliveryLetterOut.SubconCategory == "SUBCON CUTTING SEWING")
            {
                foreach (var a in itemsData.Select(x => x.UENId).Distinct()) 
                {
                    await PutGarmentUnitExpenditureNoteDelete(a);
                }
            }

            return Ok(order.Identity);
        }

        [HttpGet("get-pdf/{id}")]
        public async Task<IActionResult> GetPdf(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSubconDeliveryLetterOutDto garmentSubconDeliveryLetterOutDto = _garmentSubconDeliveryLetterOutRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconDeliveryLetterOutDto(subcon)
            {
                Items = _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subcon.Identity).Select(subconItem => new GarmentSubconDeliveryLetterOutItemDto(subconItem)
                {
                    SubconCutting = _garmentCuttingOutRepository.Find(o => o.Identity == subconItem.SubconId).Select(cutOut => new GarmentSubconCuttingOutDto(cutOut)
                    {
                        Items = _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).Select(cutOutItem => new GarmentSubconCuttingOutItemDto(cutOutItem)
                        { }).ToList()
                    }).FirstOrDefault(),

                    SubconSewing = _garmentServiceSubconSewingRepository.Find(o => o.Identity == subconItem.SubconId).Select(sSewing => new GarmentServiceSubconSewingDto(sSewing)
                    {
                        Items = _garmentServiceSubconSewingItemRepository.Find(o => o.ServiceSubconSewingId == sSewing.Identity).Select(sSewingItem => new GarmentServiceSubconSewingItemDto(sSewingItem)
                        {
                            Details = _garmentServiceSubconSewingDetailRepository.Find(o => o.ServiceSubconSewingItemId == sSewingItem.Identity).Select(subconDetail => new GarmentServiceSubconSewingDetailDto(subconDetail)
                            {

                            }).ToList()
                        }).ToList()
                    }).FirstOrDefault(),

                    SubconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Find(o => o.Identity == subconItem.SubconId).Select(sExpend => new GarmentServiceSubconExpenditureGoodDto(sExpend)
                    {
                        Items = _garmentServiceSubconExpenditureGoodItemRepository.Find(o => o.ServiceSubconExpenditureGoodId == sExpend.Identity).Select(sExpendItem => new GarmentServiceSubconExpenditureGoodItemDto(sExpendItem) { }).ToList()
                    }).FirstOrDefault()


                }).ToList()
            }).FirstOrDefault();
            var subconContractDto = _garmentSubconContractRepository.Find(a => a.Identity == garmentSubconDeliveryLetterOutDto.SubconContractId).Select(a => new GarmentSubconContractDto(a)).FirstOrDefault();
            var stream = GarmentSubconDeliveryLetterOutPDFTemplate.Generate(garmentSubconDeliveryLetterOutDto, subconContractDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentSubconDeliveryLetterOutDto.DLNo}.pdf"
            };
        }
        //
        [HttpGet("raw-material/download")]
        public async Task<IActionResult> GetXlsSubconDLORawMaterialReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            try
            {
                GetXlsGarmentSubconDLORawMaterialReportQuery query = new GetXlsGarmentSubconDLORawMaterialReportQuery(page, size, order, dateFrom, dateTo);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = String.Format("Laporan SJ SubCon Bahan Baku.xlsx");

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //
        [HttpGet("service-component/download")]
        public async Task<IActionResult> GetXlsSubconDLOComponentServiceReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            try
            {
                GetXlsGarmentSubconDLOComponentServiceReportQuery query = new GetXlsGarmentSubconDLOComponentServiceReportQuery(page, size, order, dateFrom, dateTo);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = String.Format("Laporan SJ SubCon Jasa Komponen.xlsx");

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //
        [HttpGet("service-garment-wash/download")]
        public async Task<IActionResult> GetXlsSubconDLOGarmentWashReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            try
            {
                GetXlsGarmentSubconDLOGarmentWashReportQuery query = new GetXlsGarmentSubconDLOGarmentWashReportQuery(page, size, order, dateFrom, dateTo);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = String.Format("Laporan SubCon Jasa Garment Wash.xlsx");

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //
        [HttpGet("garment-cutting-sewing/download")]
        public async Task<IActionResult> GetXlsSubconDLOCuttingSewingReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            try
            {
                GetXlsGarmentSubconDLOCuttingSewingReportQuery query = new GetXlsGarmentSubconDLOCuttingSewingReportQuery(page, size, order, dateFrom, dateTo);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = String.Format("Laporan SubCon Jasa Garment Cutting Sewing.xlsx");

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //
        [HttpGet("garment-sewing/download")]
        public async Task<IActionResult> GetXlsSubconDLOSewingReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            try
            {
                GetXlsGarmentSubconDLOSewingReportQuery query = new GetXlsGarmentSubconDLOSewingReportQuery(page, size, order, dateFrom, dateTo);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = String.Format("Laporan SubCon Jasa Garment Sewing.xlsx");

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}