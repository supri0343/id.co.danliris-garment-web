using ExtCore.Data.Abstractions;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents
{
    public class GetXlsAllGarmentAvalComponentsQueryHandler : IQueryHandler<GetXlsAllGarmentAvalComponentsQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentAvalComponentRepository _garmentAvalComponentRepository;
        private readonly IGarmentAvalComponentItemRepository _garmentAvalComponentItemRepository;

        public GetXlsAllGarmentAvalComponentsQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
            _garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
        }

        class viewModels
        {
            public string AvalComponentNo { get; internal set; }
            public string UnitName { get; internal set; }
            public string AvalComponentType { get; internal set; }
            public string RONo { get; internal set; }
            public string Article { get; internal set; }
            public double Quantities { get; internal set; }
            public DateTimeOffset Date { get; internal set; }
            public string CreatedBy { get; internal set; }
        }

        public async Task<MemoryStream> Handle(GetXlsAllGarmentAvalComponentsQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset DateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(0, 0, 0));
            DateFrom.AddHours(7);
            DateTimeOffset DateTo = new DateTimeOffset(request.dateTo, new TimeSpan(0, 0, 0));
            DateTo.AddHours(7);

            var query = (from a in _garmentAvalComponentRepository.Query
                         join b in _garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                         where a.Date >= DateFrom && a.Date <= DateTo
                         select new viewModels
                         {
                             AvalComponentNo = a.AvalComponentNo,
                             UnitName = a.UnitName,
                             AvalComponentType = a.AvalComponentType,
                             RONo = a.RONo,
                             Article = a.Article,
                             Quantities = b.Quantity,
                             Date = a.Date,
                             CreatedBy = a.CreatedBy,
                         }).GroupBy(x => new { x.AvalComponentNo, x.UnitName, x.AvalComponentType, x.RONo, x.Article, x.Date, x.CreatedBy }, (key, group) => new viewModels
                         {
                             AvalComponentNo = key.AvalComponentNo,
                             UnitName = key.UnitName,
                             AvalComponentType = key.AvalComponentType,
                             RONo = key.RONo,
                             Article = key.Article,
                             Quantities = group.Sum(x => x.Quantities),
                             Date = key.Date,
                             CreatedBy = key.CreatedBy,
                         });

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Bon Aval", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Bon Aval", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Asal Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Aval Komponen", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Staff", DataType = typeof(string) });
            var index = 1;
            if(query.Count() > 0)
            {
                foreach(var item in query)
                {
                    var tgl = item.Date.AddHours(7).ToString("dd MMM yyyy");
                    reportDataTable.Rows.Add(index++, item.AvalComponentNo, item.UnitName, item.AvalComponentType, item.RONo, item.Article, item.Quantities,tgl, item.CreatedBy);
                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A" + 5 + ":I" + 5 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "Laporan Aval Komponen "; worksheet.Cells["A" + 1 + ":L" + 1 + ""].Merge = true;
                worksheet.Cells["A2"].Value = "Periode " + DateFrom.ToString("dd-MM-yyyy") + " s/d " + DateTo.ToString("dd-MM-yyyy");

                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;
            }

        }
    }
}
