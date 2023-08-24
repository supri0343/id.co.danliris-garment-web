using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentLoadings.Repositories;
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
using static Manufactures.Application.GarmentLoadings.Queries.GetMonitoringWithCreatedUTC.GarmentMonitoringLoadingListDto;

namespace Manufactures.Application.GarmentLoadings.Queries.GetMonitoringWithCreatedUTC
{
    public class GetXlsMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetXlsMonitoringWithCreatedUTCQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;

        public GetXlsMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
        }

        public async Task<MemoryStream> Handle(GetXlsMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            var _unitName = (from a in _garmentLoadingRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var LoadingQuery = _garmentLoadingRepository.Query
               .Where(co => co.LoadingDate.AddHours(7).Date >= dateFrom.Date && co.LoadingDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = LoadingQuery.Select(co => new GarmentMonitoringLoadingListDto
            {
                Id = co.Identity,
                LoadingNo = co.LoadingNo,
                UnitFrom = new UnitDepartment(co.UnitFromId, co.UnitFromCode, co.UnitFromName),
                LoadingDate = co.LoadingDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                Comodity = new GarmentComodity(co.ComodityId, co.ComodityCode, co.ComodityName),
                CreatedDate = co.CreatedDate
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentLoadingItemRepository.Query.Where(x => x.LoadingId == co.Id).OrderBy(x => x.Identity).Select(coi => new Item
                {
                    Quantity = coi.Quantity,
                    RemainingQuantity = coi.RemainingQuantity,
                }).ToList();

                co.TotalLoadingQuantity = co.Items.Sum(i => i.Quantity);
                co.TotalRemainingQuantity = co.Items.Sum(i => i.RemainingQuantity);
            }


            double totalQuantity = 0;
            double totalRemainingQuantity = 0;
            foreach (var a in selectedQuery)
            {
              
                totalQuantity += a.TotalLoadingQuantity;
                totalRemainingQuantity += a.TotalRemainingQuantity;
            }

            GarmentMonitoringLoadingListDto dto = new GarmentMonitoringLoadingListDto
            {
                LoadingNo = "",
                LoadingDate = DateTimeOffset.MinValue,
                RONo = "",
                Article = "",
                Unit = new UnitDepartment(0, "", ""),
                CreatedDate = DateTimeOffset.MinValue,
                TotalRemainingQuantity = totalRemainingQuantity,
                TotalLoadingQuantity = totalQuantity
            };

            selectedQuery.Add(dto);

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Loading", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Loading", DataType = typeof(string) });
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
                    reportDataTable.Rows.Add(index++, report.LoadingNo, report.RONo, report.LoadingDate.ToString("dd-MMM-yyyy"), report.CreatedDate.ToString("dd-MMM-yyyy"), report.Unit.Name,
                        report.Article, report.TotalLoadingQuantity, report.TotalRemainingQuantity);
                    counter++;
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "Report Loading";
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
