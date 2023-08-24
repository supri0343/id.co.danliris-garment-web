using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
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
using static Manufactures.Application.GarmentFinishingOuts.Queries.GetMonitoringWithCreatedUTC.GarmentMonitoringFinishingOutWithUTCDto;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetMonitoringWithCreatedUTC
{
    public class GetXlsMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetXlsMonitoringWithCreatedUTCQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;

        public GetXlsMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = _storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = _storage.GetRepository<IGarmentFinishingOutItemRepository>();
        }

        public async Task<MemoryStream> Handle(GetXlsMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            var _unitName = (from a in _garmentFinishingOutRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var Query = _garmentFinishingOutRepository.Query
                .Where(co => co.FinishingOutDate.AddHours(7).Date >= dateFrom.Date && co.FinishingOutDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = Query.Select(co => new GarmentMonitoringFinishingOutWithUTCDto
            {
                Id = co.Identity,
                FinishingOutNo = co.FinishingOutNo,
                FinishingTo = co.FinishingTo,
                UnitTo = new UnitDepartment(co.UnitToId, co.UnitToCode, co.UnitToName),
                FinishingOutDate = co.FinishingOutDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                CreatedDate = co.CreatedDate
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentFinishingOutItemRepository.Query.Where(x => x.FinishingOutId == co.Id).OrderBy(x => x.Identity).Select(coi => new Item
                {
                    Quantity = coi.Quantity,
                    RemainingQuantity = coi.RemainingQuantity,
                }).ToList();

                co.TotalQuantity = co.Items.Sum(i => i.Quantity);
                co.TotalRemainingQuantity = co.Items.Sum(i => i.RemainingQuantity);
            }

            double totalQuantity = 0;
            double totalRemainingQuantity = 0;
            foreach (var a in selectedQuery)
            {

                totalQuantity += a.TotalQuantity;
                totalRemainingQuantity += a.TotalRemainingQuantity;
            }

            GarmentMonitoringFinishingOutWithUTCDto dto = new GarmentMonitoringFinishingOutWithUTCDto
            {
                FinishingOutNo = "",
                FinishingTo = "",
                FinishingOutDate = DateTimeOffset.MinValue,
                RONo = "",
                Article = "",
                Unit = new UnitDepartment(0, "", ""),
                UnitTo = new UnitDepartment(0, "", ""),
                CreatedDate = DateTimeOffset.MinValue,
                TotalRemainingQuantity = totalRemainingQuantity,
                TotalQuantity = totalQuantity
            };

            selectedQuery.Add(dto);

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Finishing Out", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Finishing Out", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pembuatan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Finishing Out", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Tujuan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tujuan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Artikel", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Out", DataType = typeof(string) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(string) });

            int counter = 5;
            int index = 1;

            if(selectedQuery.Count > 0)
            {
                foreach(var report in selectedQuery)
                {
                    reportDataTable.Rows.Add(index++, report.FinishingOutNo, report.RONo, report.FinishingOutDate.ToString("dd-MMM-yyyy"), report.CreatedDate.ToString("dd-MMM-yyyy"), report.Unit.Name,
                        report.UnitTo.Name,report.FinishingTo, report.Article, report.TotalQuantity/*, report.TotalRemainingQuantity*/);
                    counter++;
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "Report Finishing Out";
                worksheet.Cells["A" + 1 + ":K" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":K" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":K" + 3 + ""].Merge = true;
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A3"].Value = "Konfeksi " + (_unitName != null ? _unitName : "ALL");

                worksheet.Cells["A" + 1 + ":K" + 5 + ""].Style.Font.Bold = true;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells.AutoFitColumns();

                worksheet.Cells["A" + (counter) + ":I" + (counter) + ""].Merge = true;
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
