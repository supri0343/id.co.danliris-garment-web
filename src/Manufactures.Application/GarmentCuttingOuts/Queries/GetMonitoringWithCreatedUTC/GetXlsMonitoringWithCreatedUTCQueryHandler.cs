using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetMonitoringWithCreatedUTC
{
    public class GetXlsMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetXlsMonitoringWithCreatedUTCQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;

        public GetXlsMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
        }

        public async Task<MemoryStream> Handle(GetXlsMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            var _unitName = (from a in _garmentCuttingOutRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var cuttingOutQuery = _garmentCuttingOutRepository.Query
                .Where(co => co.CuttingOutDate.AddHours(7).Date >= dateFrom.Date && co.CuttingOutDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = cuttingOutQuery.Select(co => new GarmentMonitoringCuttingOutWithUTCDto
            {
                Id = co.Identity,
                CutOutNo = co.CutOutNo,
                CuttingOutType = co.CuttingOutType,
                UnitFrom = new UnitDepartment(co.UnitFromId, co.UnitFromCode, co.UnitFromName),
                CuttingOutDate = co.CuttingOutDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                Comodity = new GarmentComodity(co.ComodityId, co.ComodityCode, co.ComodityName),
                CreatedDate = co.CreatedDate
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentCuttingOutItemRepository.Query.Where(x => x.CutOutId == co.Id).OrderBy(x => x.Identity).Select(coi => new GarmentCuttingOutItemDto
                {
                    Id = coi.Identity,
                    CutOutId = coi.CutOutId,
                    CuttingInId = coi.CuttingInId,
                    CuttingInDetailId = coi.CuttingInDetailId,
                    Product = new Product(coi.ProductId, coi.ProductCode, coi.ProductName),
                    DesignColor = coi.DesignColor,
                    TotalCuttingOut = coi.TotalCuttingOut,
                }).ToList();

                foreach (var coi in co.Items)
                {
                    coi.Details = _garmentCuttingOutDetailRepository.Query.Where(x => x.CutOutItemId == coi.Id).OrderBy(x => x.Identity).Select(cod => new GarmentCuttingOutDetailDto
                    {
                        Id = cod.Identity,
                        CutOutItemId = cod.CutOutItemId,
                        Size = new SizeValueObject(cod.SizeId, cod.SizeName),
                        CuttingOutQuantity = cod.CuttingOutQuantity,
                        CuttingOutUom = new Uom(cod.CuttingOutUomId, cod.CuttingOutUomUnit),
                        Color = cod.Color,
                        RemainingQuantity = cod.RemainingQuantity,
                        BasicPrice = cod.BasicPrice,
                        Price = cod.Price,
                    }).ToList();
                }

                co.TotalCuttingOutQuantity = co.Items.Sum(i => i.Details.Sum(d => d.CuttingOutQuantity));
                co.TotalRemainingQuantity = co.Items.Sum(i => i.Details.Sum(d => d.RemainingQuantity));
            }

            double totalCuttingOutQuantity = 0;
            double totalRemainingQuantity = 0;
            foreach (var a in selectedQuery)
            {
              
                totalCuttingOutQuantity += a.TotalCuttingOutQuantity;
                totalRemainingQuantity += a.TotalRemainingQuantity;
            }

            GarmentMonitoringCuttingOutWithUTCDto cuttingOutWithUTCDto = new GarmentMonitoringCuttingOutWithUTCDto
            {
                CutOutNo = "",
                CuttingOutType = "",
                CuttingOutDate = DateTimeOffset.MinValue,
                RONo = "",
                Article = "",
                Unit = new UnitDepartment(0, "", ""),
                CreatedDate = DateTimeOffset.MinValue,
                TotalRemainingQuantity = totalRemainingQuantity,
                TotalCuttingOutQuantity = totalCuttingOutQuantity
            };

            selectedQuery.Add(cuttingOutWithUTCDto);

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Cutting Out", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Cut Out", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pembuatan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Tujuan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Artikel", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Out", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(string) });

            int counter = 5;
            int index = 1;

            if(selectedQuery.Count > 0)
            {
                foreach(var report in selectedQuery)
                {
                    reportDataTable.Rows.Add(index++, report.CutOutNo, report.RONo, report.CuttingOutDate.ToString("dd-MMM-yyyy"), report.CreatedDate.ToString("dd-MMM-yyyy"), report.Unit.Name,
                        report.Article, report.TotalCuttingOutQuantity, report.TotalRemainingQuantity);
                    counter++;
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "Report Cutting Out";
                worksheet.Cells["A" + 1 + ":I" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":I" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":I" + 3 + ""].Merge = true;
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A3"].Value = "Konfeksi " + (_unitName != null ? _unitName : "ALL");

                worksheet.Cells["A" + 1 + ":I" + 5 + ""].Style.Font.Bold = true;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells.AutoFitColumns();

                worksheet.Cells["A" + (counter) + ":G" + (counter) + ""].Merge = true;
                worksheet.Cells["A" + (counter)].Value = "TOTAL";
                worksheet.Cells["A" + (counter)].Style.Font.Bold = true;
                worksheet.Cells["A" + (counter)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;
            }
        }

    }

}
