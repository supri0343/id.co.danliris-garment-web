using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.ExcelTemplates
{
    public class GetXlsServiceSampleShrinkagePanelsQueryHandler : IQueryHandler<GetXlsSampleServiceSampleShrinkagePanelsQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleShrinkagePanelRepository _garmentServiceSampleShrinkagePanelRepository;
        private readonly IGarmentServiceSampleShrinkagePanelItemRepository _garmentServiceSampleShrinkagePanelItemRepository;
        private readonly IGarmentServiceSampleShrinkagePanelDetailRepository _garmentServiceSampleShrinkagePanelDetailRepository;

        public GetXlsServiceSampleShrinkagePanelsQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelRepository>();
            _garmentServiceSampleShrinkagePanelItemRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelItemRepository>();
            _garmentServiceSampleShrinkagePanelDetailRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelDetailRepository>();
        }

        class SampleView
        {
            public string noBon { get; internal set; }
            public string code { get; internal set; }
            public string name { get; internal set; }
            public string design { get; internal set; }
            public decimal quantity { get; internal set; }
            public string satuan { get; internal set; }

        }

        public async Task<MemoryStream> Handle(GetXlsSampleServiceSampleShrinkagePanelsQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom.AddHours(7));
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo.AddHours(7));

            var query = (from a in _garmentServiceSampleShrinkagePanelRepository.Query
                         join b in _garmentServiceSampleShrinkagePanelItemRepository.Query on a.Identity equals b.ServiceSampleShrinkagePanelId
                         join c in _garmentServiceSampleShrinkagePanelDetailRepository.Query on b.Identity equals c.ServiceSampleShrinkagePanelItemId
                         where /*a.Deleted == false &&*/ a.ServiceSampleShrinkagePanelDate >= dateFrom && a.ServiceSampleShrinkagePanelDate <= dateTo
                         select new SampleView
                         {
                             noBon = a.ServiceSampleShrinkagePanelNo,
                             code = c.ProductCode,
                             name = c.ProductName,
                             design = c.DesignColor,
                             quantity = c.Quantity,
                             satuan = c.UomUnit,
                         }).GroupBy(x => new { x.noBon, x.code, x.name, x.design, x.quantity, x.satuan }, (key, group) => new SampleView
                         {
                             noBon = key.noBon,
                             code = key.code,
                             name = key.name,
                             design = key.design,
                             quantity = key.quantity,
                             satuan = key.satuan
                         });

            XlsSampleShrinkageListViewModel viewModel = new XlsSampleShrinkageListViewModel();
            List<XlsSampleShrinkageDto> SampleDto = new List<XlsSampleShrinkageDto>();

            foreach (var a in query)
            {
                XlsSampleShrinkageDto garmentSampleDto = new XlsSampleShrinkageDto()
                {
                    noBon = a.noBon,
                    code = a.code,
                    name = a.name,
                    design = a.design,
                    quantity = a.quantity,
                    satuan = a.satuan,
                };
                SampleDto.Add(garmentSampleDto);
            }

            viewModel.XlsSampleShrinkageDtos = SampleDto;
            var excel = new DataTable();

            //excel.Columns.Add(new DataColumn() { ColumnName = "No ", DataType = typeof(int) });
            excel.Columns.Add(new DataColumn() { ColumnName = "No Bon Pengeluaran Unit", DataType = typeof(string) });
            excel.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
            excel.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(string) });
            excel.Columns.Add(new DataColumn() { ColumnName = "Design / Color", DataType = typeof(string) });
            excel.Columns.Add(new DataColumn() { ColumnName = "Jumlah", DataType = typeof(decimal) });
            excel.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });

            //var index = 1;
            //int idx = 1;
            //var rCount = 0;
            //Dictionary<string, string> RowCount = new Dictionary<string, string>();
            if (viewModel.XlsSampleShrinkageDtos.Count == 0)
            {
                excel.Rows.Add("", "", "", "", 0.0, "");
            }
            else
            {
                foreach (var report in viewModel.XlsSampleShrinkageDtos)
                {
                    //index++;
                    //idx++;
                    //if(!RowCount.ContainsKey(report.noBon))
                    //{
                    //    rCount = 0;
                    //    var index1 = idx;
                    //    RowCount.Add(report.noBon, index1.ToString());
                    //}
                    //else
                    //{
                    //    rCount += 1;
                    //    RowCount[report.noBon] = RowCount[report.noBon] + "-" + rCount.ToString();
                    //    var val = RowCount[report.noBon].Split("-");
                    //    if ((val).Length > 0)
                    //    {
                    //        RowCount[report.noBon] = val[0] + "-" + rCount.ToString();
                    //    }
                    //}
                    excel.Rows.Add(report.noBon, report.code, report.name, report.design, report.quantity, report.satuan);
                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                var countData = viewModel.XlsSampleShrinkageDtos.Count();

                worksheet.Cells["A" + 1 + ":F" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "Report Sample Service Shrinkage";
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " - " + dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A" + 1 + ":F" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":F" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":F" + 4 + ""].Style.Font.Bold = true;

                if (countData > 0)
                {
                    worksheet.Cells["F" + 5 + ":F" + (4 + countData) + ""].Merge = true;
                    worksheet.Cells["F" + 5 + ":F" + (4 + countData) + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells["C" + 5 + ":C" + (4 + countData) + ""].Merge = true;
                    worksheet.Cells["C" + 5 + ":C" + (4 + countData) + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"A{(5 + countData)}:D{(5 + countData)}"].Merge = true;
                    worksheet.Cells[$"A{(5 + countData)}:F{(5 + countData)}"].Style.Font.Bold = true;

                    worksheet.Cells[$"A{(5 + countData)}"].Value = "TOTAL ";
                    //worksheet.Cells[$"A{(5 + countData)}"].Merge = true;
                    worksheet.Cells[$"E{(5 + countData)}"].Formula = "SUM(" + worksheet.Cells["E" + 5 + ":E" + (4 + countData) + ""].Address + ")";
                    worksheet.Calculate();
                }

                worksheet.Cells.AutoFitColumns();
                worksheet.Cells["A4"].LoadFromDataTable(excel, true);

                //foreach(var a in RowCount)
                //{
                //    var UnitrowNum = a.Value.Split("-");
                //    int rowNum2 = 1;
                //    int rowNum1 = Convert.ToInt32(UnitrowNum[0]);
                //    if (UnitrowNum.Length > 1)
                //    {
                //        rowNum2 = Convert.ToInt32(rowNum1) + Convert.ToInt32(UnitrowNum[1]);
                //    }
                //    else
                //    {
                //        rowNum2 = Convert.ToInt32(rowNum1);
                //    }
                //    worksheet.Cells[$"A{(rowNum1 + 3)} :A{(rowNum2) + 3}"].Merge = true;
                //    worksheet.Cells[$"A{(rowNum1 + 3)} :A{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //    worksheet.Cells[$"A{(rowNum1 + 3)} :A{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    //worksheet.Cells[$"B{(rowNum1 + 3)} :B{(rowNum2) + 3}"].Merge = true;
                    //worksheet.Cells[$"B{(rowNum1 + 3)} :B{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //worksheet.Cells[$"B{(rowNum1 + 3)} :B{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    //worksheet.Cells[$"D{(rowNum1 + 3)} :D{(rowNum2) + 3}"].Merge = true;
                    //worksheet.Cells[$"D{(rowNum1 + 3)} :D{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //worksheet.Cells[$"D{(rowNum1 + 3)} :D{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    //worksheet.Cells[$"F{(rowNum1 + 3)} :F{(rowNum2) + 3}"].Merge = true;
                    //worksheet.Cells[$"F{(rowNum1 + 3)} :F{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //worksheet.Cells[$"F{(rowNum1 + 3)} :F{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                //}

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }

            throw new NotImplementedException();
        }
    }
}
