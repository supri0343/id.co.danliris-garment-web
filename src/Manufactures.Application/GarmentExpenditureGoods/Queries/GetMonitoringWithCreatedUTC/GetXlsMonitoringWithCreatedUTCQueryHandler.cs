using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
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
using static Manufactures.Application.GarmentExpenditureGoods.Queries.GetMonitoringWithCreatedUTC.GarmentMonitoringExpenditureGoodWithUTCDto;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetMonitoringWithCreatedUTC
{
    public class GetXlsMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetXlsMonitoringWithCreatedUTCQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;

        public GetXlsMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodRepository = _storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = _storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
        }

        public async Task<MemoryStream> Handle(GetXlsMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            var _unitName = (from a in _garmentExpenditureGoodRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var Query = _garmentExpenditureGoodRepository.Query
                .Where(co => co.ExpenditureDate.AddHours(7).Date >= dateFrom.Date && co.ExpenditureDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = Query.Select(co => new GarmentMonitoringExpenditureGoodWithUTCDto
            {
                Id = co.Identity,
                ExpenditureGoodNo = co.ExpenditureGoodNo,
                ExpenditureType = co.ExpenditureType,
                ExpenditureDate = co.ExpenditureDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                CreatedDate = co.CreatedDate,
                Invoice = co.Invoice,
                Buyer = new Buyer(co.BuyerId, co.BuyerCode, co.BuyerName),
                Description = co.Description
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentExpenditureGoodItemRepository.Query.Where(x => x.ExpenditureGoodId == co.Id).OrderBy(x => x.Identity).Select(coi => new Item
                {
                    Quantity = coi.Quantity
                }).ToList();

                co.TotalQuantity = co.Items.Sum(i => i.Quantity);
            }

            double totalQuantity = 0;
            foreach (var a in selectedQuery)
            {
                totalQuantity += a.TotalQuantity;
            }

            GarmentMonitoringExpenditureGoodWithUTCDto dto = new GarmentMonitoringExpenditureGoodWithUTCDto
            {
                ExpenditureGoodNo = "",
                ExpenditureType = "",
                ExpenditureDate = DateTimeOffset.MinValue,
                RONo = "",
                Article = "",
                Unit = new UnitDepartment(0, "", ""),
                CreatedDate = DateTimeOffset.MinValue,
                Invoice = "",
                Buyer = new Buyer(0, "", ""),
                Description = "",
                TotalQuantity = totalQuantity
            };

            selectedQuery.Add(dto);

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Bon Keluar", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Bon Keluar", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pembuatan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Pengeluaran", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tipe Pengeluaran", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Artikel", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Out", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(string) });

            int counter = 5;
            int index = 1;

            if(selectedQuery.Count > 0)
            {
                foreach(var report in selectedQuery)
                {
                    reportDataTable.Rows.Add(index++, report.ExpenditureGoodNo, report.RONo, report.ExpenditureDate.ToString("dd-MMM-yyyy"), report.CreatedDate.ToString("dd-MMM-yyyy"), report.Unit.Name,
                        report.ExpenditureType, report.Article, report.TotalQuantity, report.Buyer.Name);
                    counter++;
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "Report Pengeluaran Barang Jadi";
                worksheet.Cells["A" + 1 + ":J" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":J" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":J" + 3 + ""].Merge = true;
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A3"].Value = "Konfeksi " + (_unitName != null ? _unitName : "ALL");

                worksheet.Cells["A" + 1 + ":J" + 5 + ""].Style.Font.Bold = true;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells.AutoFitColumns();

                worksheet.Cells["A" + (counter) + ":H" + (counter) + ""].Merge = true;
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
