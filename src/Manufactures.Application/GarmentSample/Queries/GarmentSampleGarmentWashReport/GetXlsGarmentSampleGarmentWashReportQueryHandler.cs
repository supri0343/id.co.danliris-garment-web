using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
//using Manufactures.Domain.GarmentSample.CustomsOuts.Repositories;
//using Manufactures.Domain.GarmentSample.SampleContracts.Repositories;
//using Manufactures.Domain.GarmentSample.SampleCustomsIns.Repositories;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Data;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;

namespace Manufactures.Application.GarmentSample.Queries.GarmentSampleGarmentWashReport
{
    public class GetXlsGarmentSampleGarmentWashReportQueryHandler : IQueryHandler<GetXlsGarmentSampleGarmentWashReporQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentServiceSampleSewingRepository garmentSampleSewingRepository;
        private readonly IGarmentServiceSampleSewingItemRepository garmentSampleSewingItemRepository;
        private readonly IGarmentServiceSampleSewingDetailRepository garmentSampleSewingDetailRepository;

        public GetXlsGarmentSampleGarmentWashReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSampleSewingRepository = storage.GetRepository<IGarmentServiceSampleSewingRepository>();
            garmentSampleSewingItemRepository = storage.GetRepository<IGarmentServiceSampleSewingItemRepository>();
            garmentSampleSewingDetailRepository = storage.GetRepository<IGarmentServiceSampleSewingDetailRepository>();

            _http = serviceProvider.GetService<IHttpClientService>();
        }

        class monitoringViewTemp
        {
            public Guid SSCSId { get; internal set; }
            public string SSCSNo { get; internal set; }
            public DateTimeOffset SSCSDate { get; internal set; }

            public int BuyerId { get; internal set; }
            public string BuyerCode { get; internal set; }
            public string BuyerName { get; internal set; }

            public int ComodityId { get; internal set; }
            public string ComodityCode { get; internal set; }
            public string ComodityName { get; internal set; }

            public int UnitId { get; internal set; }
            public string UnitCode { get; internal set; }
            public string UnitName { get; internal set; }

            public string DesignColour { get; internal set; }

            public double Quantity { get; internal set; }

            public int UomId { get; internal set; }
            public string UomUnit { get; internal set; }
        }

        public async Task<MemoryStream> Handle(GetXlsGarmentSampleGarmentWashReporQuery request, CancellationToken cancellationToken)
        {      
            var Query = (from a in garmentSampleSewingRepository.Query
                         join b in garmentSampleSewingItemRepository.Query on a.Identity equals b.ServiceSampleSewingId
                         join c in garmentSampleSewingDetailRepository.Query on b.Identity equals c.ServiceSampleSewingItemId
                         where a.Deleted == false && b.Deleted == false && b.Deleted==false
                         && a.ServiceSampleSewingDate.AddHours(7).Date >= request.dateFrom
                         && a.ServiceSampleSewingDate.AddHours(7).Date <= request.dateTo.Date

                         select new monitoringViewTemp
                         {
                            SSCSId = a.Identity,
                            SSCSNo = a.ServiceSampleSewingNo,
                            SSCSDate = a.ServiceSampleSewingDate,
                            BuyerId = a.BuyerId,
                            BuyerCode = a.BuyerCode,
                            BuyerName = a.BuyerName,
                            ComodityId = b.ComodityId,
                            ComodityCode = b.ComodityCode,
                            ComodityName = b.ComodityName,
                            UnitId = c.UnitId,
                            UnitCode = c.UnitCode,
                            UnitName = c.UnitName,
                            DesignColour = c.DesignColor,
                            Quantity = c.Quantity,
                            UomId = c.UomId,
                            UomUnit = c.UomUnit,

                         }).GroupBy(x => new { x.SSCSNo, x.SSCSDate,  x.BuyerCode, x.BuyerName, x.ComodityCode, x.ComodityName, x.UnitCode, x.UnitName, x.DesignColour, x.UomUnit }, (key, group) => new 
                         {
                             SSCSNo = key.SSCSNo,
                             SSCSDate = key.SSCSDate,
                             BuyerCode = key.BuyerCode,
                             BuyerName = key.BuyerName,
                             ComodityCode = key.ComodityCode,
                             ComodityName = key.ComodityName,
                             UnitCode = key.UnitCode,
                             UnitName = key.UnitName,
                             DesignColour = key.DesignColour,
                             Quantity = group.Sum(x => x.Quantity),
                             UomUnit = key.UomUnit,                           
                         }).ToList();


            GarmentSampleGarmentWashReportListViewModel listViewModel = new GarmentSampleGarmentWashReportListViewModel();
            List<GarmentSampleGarmentWashReportDto> rekapgarmentwash = new List<GarmentSampleGarmentWashReportDto>();

            foreach (var i in Query)
            {
                GarmentSampleGarmentWashReportDto report = new GarmentSampleGarmentWashReportDto
                {
                    SSCSNo = i.SSCSNo,
                    SSCSDate = i.SSCSDate,
                    BuyerCode = i.BuyerCode,
                    BuyerName = i.BuyerName,
                    ComodityCode = i.ComodityCode,
                    ComodityName = i.ComodityName,
                    UnitCode = i.UnitCode,
                    UnitName = i.UnitName,
                    DesignColour = i.DesignColour,
                    Quantity = i.Quantity,
                    UomUnit = i.UomUnit,                   
                };

                rekapgarmentwash.Add(report);
            }

            listViewModel.garmentSampleGarmentWashReportDto = rekapgarmentwash;

            DataTable reportDataTable = new DataTable();
            //ExcelPackage package = new ExcelPackage();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Sample Jasa Sewing", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Sample Jasa Sewing", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Buyer", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Buyer", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Komoditi", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Komoditi", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Unit", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Unit", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Desain Warna", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Quantity", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });

            var index = 1;
            int idx = 1;
            var rCount = 0;
            Dictionary<string, string> Rowcount = new Dictionary<string, string>();

            if (listViewModel.garmentSampleGarmentWashReportDto.Count() == 0)
            {
                reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "", 0, "");
            }
            else
            {                
                foreach (var item in listViewModel.garmentSampleGarmentWashReportDto)
                {
                    idx++;
                    if (!Rowcount.ContainsKey(item.SSCSNo))
                    {
                        rCount = 0;
                        var index1 = idx;
                        Rowcount.Add(item.SSCSNo, index1.ToString());
                    }
                    else
                    {
                        rCount += 1;
                        Rowcount[item.SSCSNo] = Rowcount[item.SSCSNo] + "-" + rCount.ToString();
                        var val = Rowcount[item.SSCSNo].Split("-");
                        if ((val).Length > 0)
                        {
                            Rowcount[item.SSCSNo] = val[0] + "-" + rCount.ToString();
                        }
                    }

                    string sscsDate = item.SSCSDate == null ? "-" : item.SSCSDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    reportDataTable.Rows.Add(item.SSCSNo, sscsDate, item.BuyerCode, item.BuyerName, item.ComodityCode, item.ComodityName, item.UnitCode, item.UnitName, item.DesignColour, item.Quantity, item.UomUnit);
                }
            }

            using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells["A4"].LoadFromDataTable(reportDataTable, true);

                var countdata = Query.Count();

                worksheet.Cells["A" + 1 + ":K" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "LAPORAN REKAP Sample JASA | GARMENT WASH";
                worksheet.Cells["A2"].Value = "Periode " + request.dateFrom.ToString("dd-MM-yyyy") + " S/D " + request.dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A" + 1 + ":K" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":K" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":K" + 4 + ""].Style.Font.Bold = true;
                
                if (countdata > 0)
                {
                    worksheet.Cells["K" + 5 + ":K" + (4 + countdata) + ""].Merge = true;
                    worksheet.Cells["K" + 5 + ":K" + (4 + countdata) + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"A{(5 + countdata)}:I{(5 + countdata)}"].Merge = true;
                    worksheet.Cells[$"A{(5 + countdata)}:J{(5 + countdata)}"].Style.Font.Bold = true;
                    //ADD SUMMARY OF QUANTITY
                    worksheet.Cells[$"A{(5 + countdata)}"].Value = "TOTAL REKAP Sample JASA | GARMENT WASH :";
                    worksheet.Cells[$"J{(5 + countdata)}"].Formula = "SUM(" + worksheet.Cells["J" + 5 + ":J" + (4 + countdata) + ""].Address + ")";
                    worksheet.Cells[$"K{(5 + countdata)}"].Value = "PCS";
                    worksheet.Calculate();
                }

                //
                foreach (var a in Rowcount)
                {
                    var UnitrowNum = a.Value.Split("-");
                    int rowNum2 = 1;
                    int rowNum1 = Convert.ToInt32(UnitrowNum[0]);
                    if (UnitrowNum.Length > 1)
                    {
                        rowNum2 = Convert.ToInt32(rowNum1) + Convert.ToInt32(UnitrowNum[1]);
                    }
                    else
                    {
                        rowNum2 = Convert.ToInt32(rowNum1);
                    }

                    worksheet.Cells[$"A{(rowNum1 + 3)}:A{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"A{(rowNum1 + 3)}:A{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"A{(rowNum1 + 3)}:A{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"B{(rowNum1 + 3)}:B{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"B{(rowNum1 + 3)}:B{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"B{(rowNum1 + 3)}:B{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"C{(rowNum1 + 3)}:C{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"C{(rowNum1 + 3)}:C{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"C{(rowNum1 + 3)}:C{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"D{(rowNum1 + 3)}:D{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"D{(rowNum1 + 3)}:D{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"D{(rowNum1 + 3)}:D{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"E{(rowNum1 + 3)}:E{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"E{(rowNum1 + 3)}:E{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"E{(rowNum1 + 3)}:E{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"F{(rowNum1 + 3)}:F{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"F{(rowNum1 + 3)}:F{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"F{(rowNum1 + 3)}:F{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"G{(rowNum1 + 3)}:G{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"G{(rowNum1 + 3)}:G{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"G{(rowNum1 + 3)}:G{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"H{(rowNum1 + 3)}:H{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"H{(rowNum1 + 3)}:H{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"H{(rowNum1 + 3)}:H{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                }               

                var stream = new MemoryStream();
                package.SaveAs(stream);
				return stream;

            }
        }
    }
}

