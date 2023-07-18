using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.Queries
{
    public class GetXlsServiceSampleFabricWashQueryHandler : IQueryHandler<GetXlsServiceSampleFabricWashQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentServiceSampleFabricWashRepository garmentServiceSampleFabricWashRepository;
        private readonly IGarmentServiceSampleFabricWashItemRepository garmentServiceSampleFabricWashItemRepository;
        private readonly IGarmentServiceSampleFabricWashDetailRepository garmentServiceSampleFabricWashDetailRepository;

		public GetXlsServiceSampleFabricWashQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentServiceSampleFabricWashRepository = storage.GetRepository<IGarmentServiceSampleFabricWashRepository>();
			garmentServiceSampleFabricWashItemRepository = storage.GetRepository<IGarmentServiceSampleFabricWashItemRepository>();
			garmentServiceSampleFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSampleFabricWashDetailRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}

        class ServiceSampleFabricWashView
        {

            public string ServiceSampleFabricWashNo { get; internal set; }
            public DateTimeOffset serviceSampleFabricWashDate { get; internal set; }
            public string unitExpenditureNo { get; internal set; }
            public DateTimeOffset expendituredate { get; internal set; }
            public string unitSenderCode { get; internal set; }
            public string unitSenderName { get; internal set; }
            public string productCode { get; internal set; }
            public string productName { get; internal set; }
            public string productRemark { get; internal set; }
            public string designcolor { get; internal set; }
            public decimal quantity { get; internal set; }
            public string uomUnit { get; internal set; }
        }

        public async Task<MemoryStream> Handle(GetXlsServiceSampleFabricWashQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            dateTo = dateTo.AddHours(7);


            var Query1 = (from a in garmentServiceSampleFabricWashRepository.Query
                                      join b in garmentServiceSampleFabricWashItemRepository.Query on a.Identity equals b.ServiceSampleFabricWashId
                          join c in garmentServiceSampleFabricWashDetailRepository.Query on b.Identity equals c.ServiceSampleFabricWashItemId
                          where
                                      //a.Deleted == false
                                      //&& b.Deleted == false
                                      //&& c.Deleted == false
                                       a.ServiceSampleFabricWashDate >= dateFrom
                                      && a.ServiceSampleFabricWashDate <= dateTo
                          select new ServiceSampleFabricWashView
                          {

                                          ServiceSampleFabricWashNo = a.ServiceSampleFabricWashNo,
                                          //serviceSampleFabricWashDate = a.ServiceSampleFabricWashDate,
                                          unitExpenditureNo = b.UnitExpenditureNo,
                                          expendituredate = b.ExpenditureDate,
                                          unitSenderCode = b.UnitSenderCode,
                                          unitSenderName = b.UnitSenderName,
                                          productCode = c.ProductCode,
                                          productName = c.ProductName,
                                          //productRemark = c.ProductRemark,
                                          designcolor = c.DesignColor,
                                          quantity = c.Quantity,
                                          uomUnit = c.UomUnit,

                                      });
            var Query = Query1.ToList().GroupBy(x => new { x.ServiceSampleFabricWashNo,/* x.serviceSampleFabricWashDate*/ x.unitExpenditureNo,  x.expendituredate, x.unitSenderCode, x.unitSenderName, x.productCode, x.productName /*x.productRemark*/, x.designcolor, x.quantity, x.uomUnit }, (key, group) => new
            {
                ServiceSampleFabricWashNo = key.ServiceSampleFabricWashNo,
                //serviceSampleFabricWashDate = key.serviceSampleFabricWashDate,
                unitExpenditureNo = key.unitExpenditureNo,
                expendituredate = key.expendituredate,
                unitSenderCode = key.unitSenderCode,
                unitSenderName = key.unitSenderName,
                productCode = key.productCode,
                productName = key.productName,
                //productRemark = key.productRemark,
                designColor = key.designcolor,
                quantity = group.Sum(x => x.quantity),
                uomUnit = key.uomUnit,
            }).OrderBy(s => s.ServiceSampleFabricWashNo);

            GarmentServiceSampleFabricWashListViewModel garmentServiceSampleFabricWashListViewModel = new GarmentServiceSampleFabricWashListViewModel();
            List<ServiceSampleFabricWashDto> ServiceSampleFabricWashDtos = new List<ServiceSampleFabricWashDto>();

            foreach(var item in Query)
            {
                ServiceSampleFabricWashDto dto = new ServiceSampleFabricWashDto()
                {
                    serviceSampleFabricWashNo = item.ServiceSampleFabricWashNo,
                    //serviceSampleFabricWashDate = item.serviceSampleFabricWashDate,
                    unitExpenditureNo = item.unitExpenditureNo,
                    expendituredate = item.expendituredate,
                    unitSenderCode = item.unitSenderCode,
                    unitSenderName = item.unitSenderName,
                    productCode = item.productCode,
                    productName = item.productName,
                    //productRemark = item.productRemark,
                    designcolor = item.designColor,
                    quantity = item.quantity,
                    uomUnit = item.uomUnit

                };
                ServiceSampleFabricWashDtos.Add(dto);
            }

            var data = from c in ServiceSampleFabricWashDtos
                       where c.quantity > 0
                       select c;
            ServiceSampleFabricWashDtos = data.ToList();
            decimal quantity = 0;
            //decimal nominal = 0;
            foreach (var item in data)
            {
                quantity += item.quantity;

            }

            ServiceSampleFabricWashDto garmentServiceSampleFabricWashDtos = new ServiceSampleFabricWashDto()
            {
                serviceSampleFabricWashNo = "",
                //serviceSampleFabricWashDate = DateTimeOffset.UtcNow,
                unitExpenditureNo = "",
                expendituredate = DateTimeOffset.UtcNow,
                unitSenderCode = "",
                unitSenderName = "",
                productCode = "",
                productName = "",
                //productRemark = "",
                designcolor = "",
                quantity = quantity,
                uomUnit = "MT"

            };

            ServiceSampleFabricWashDtos.Add(garmentServiceSampleFabricWashDtos);
            garmentServiceSampleFabricWashListViewModel.serviceSampleFabricWashes = ServiceSampleFabricWashDtos;

            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No. Sample Jasa", DataType = typeof(string) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "anggal Sample BB Fabric Wash / Print", DataType = typeof(DateTimeOffset) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No. Bon Pengeluaran Unit", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pengeluaran", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Asal Unit", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Asal Gudang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(string) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Keterangan Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Design/Color", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah", DataType = typeof(double) });

            //int counter = 4;

            //if (garmentServiceSampleFabricWashListViewModel.serviceSampleFabricWashes.Count > 0)
            //{
            //    foreach (var report in garmentServiceSampleFabricWashListViewModel.serviceSampleFabricWashes)
            //    {
            //        //string ServiceSampleFabricWashDate = report.serviceSampleFabricWashDate.GetValueOrDefault() == new DateTime(1970, 1, 1) || report.serviceSampleFabricWashDate.GetValueOrDefault().ToString("dd MMM yyyy") == "01 Jan 0001" ? "-" : report.serviceSampleFabricWashDate.GetValueOrDefault().ToString("dd MMM yyy");
            //        reportDataTable.Rows.Add(report.serviceSampleFabricWashNo, /*report.serviceSampleFabricWashDate,*/ report.unitExpenditureNo, report.expendituredate, report.unitSenderCode, report.unitSenderName, report.productCode, report.productName, /*report.productRemark,*/ report.designcolor, report.uomUnit, report.quantity);
            //        counter++;

            //    }
            //}

            var index = 1;
            int idx = 1;
            var rCount = 0;
            Dictionary<string, string> Rowcount = new Dictionary<string, string>();

            if (Query.ToArray().Count() == 0)
                reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "", 0);
            else
                foreach (var item in Query)
                {
                    idx++;
                    if (!Rowcount.ContainsKey(item.ServiceSampleFabricWashNo))
                    {
                        rCount = 0;
                        var index1 = idx;
                        Rowcount.Add(item.ServiceSampleFabricWashNo, index1.ToString());
                    }
                    else
                    {
                        rCount += 1;
                        Rowcount[item.ServiceSampleFabricWashNo] = Rowcount[item.ServiceSampleFabricWashNo] + "-" + rCount.ToString();
                        var val = Rowcount[item.ServiceSampleFabricWashNo].Split("-");
                        if ((val).Length > 0)
                        {
                            Rowcount[item.ServiceSampleFabricWashNo] = val[0] + "-" + rCount.ToString();
                        }
                    }

                    var expenditureDate = item.expendituredate.ToString("dd MMM yyyy");
                    //var dateExpenditure = item.expendituredate.HasValue ? item.expendituredate.GetValueOrDefault().ToString("dd/MM/yyyy") : string.Empty;
                    reportDataTable.Rows.Add(item.ServiceSampleFabricWashNo, item.unitExpenditureNo, expenditureDate, item.unitSenderCode, item.unitSenderName, item.productCode, item.productName, item.designColor, item.uomUnit, item.quantity);
                }


            //using (var package = new ExcelPackage())
            //{
            //    var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

            //    worksheet.Cells["A" + 4 + ":J" + 4 + ""].Style.Font.Bold = true;
            //    worksheet.Cells["A1"].Value = "Laporan  Sample Fabric Wash "; worksheet.Cells["A" + 1 + ":J" + 1 + ""].Merge = true;
            //    worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
            //    //worksheet.Cells["A3"].Value = "Konfeksi " + ;
            //    worksheet.Cells["A" + 1 + ":J" + 1 + ""].Merge = true;
            //    worksheet.Cells["A" + 2 + ":J" + 2 + ""].Merge = true;
            //    worksheet.Cells["A" + 3 + ":J" + 3 + ""].Merge = true;
            //    worksheet.Cells["A" + 1 + ":J" + 4 + ""].Style.Font.Bold = true;
            //    worksheet.Cells["A" + 1 + ":J" + 4 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            //    worksheet.Cells["A" + 1 + ":J" + 4 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //    worksheet.Cells.AutoFitColumns();
            //    worksheet.Cells["A4"].LoadFromDataTable(reportDataTable, true);
            //    worksheet.Cells["G" + 2 + ":I" + counter + ""].Style.Numberformat.Format = "#,##0.00";
            //    worksheet.Cells["J" + 2 + ":J" + counter + ""].Style.Numberformat.Format = "#,##0.00";
            //    worksheet.Cells["J" + counter + ":J" + counter + ""].Style.Font.Bold = true;
            //    worksheet.Cells["A" + counter].Value = "T O T A L";
            //    worksheet.Cells["A" + counter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //    worksheet.Cells["A" + counter].Style.Font.Bold = true;
            //    worksheet.Cells["A" + counter + ":I" + counter + ""].Merge = true;
            //    worksheet.Cells["A" + 4 + ":J" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //    worksheet.Cells["A" + 4 + ":J" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //    worksheet.Cells["A" + 4 + ":J" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //    worksheet.Cells["A" + 4 + ":J" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //    worksheet.Cells["G" + (counter) + ":I" + (counter) + ""].Style.Font.Bold = true;
            //    worksheet.Cells["A" + 4 + ":J" + 4 + ""].Style.Font.Bold = true;

            ExcelPackage package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            var countdata = Query.Count();

            worksheet.Cells["A" + 1 + ":J" + 4 + ""].Style.Font.Bold = true;
            worksheet.Cells["A1"].Value = "Laporan  Sample Fabric Wash";
            worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
            worksheet.Cells["A" + 1 + ":J" + 1 + ""].Merge = true;
            worksheet.Cells["A" + 2 + ":J" + 2 + ""].Merge = true;
            worksheet.Cells["A" + 1 + ":J" + 4 + ""].Style.Font.Bold = true;
            

            if (countdata > 0)
            {
                worksheet.Cells["I" + 5 + ":I" + (4 + countdata) + ""].Merge = true;
                worksheet.Cells["I" + 5 + ":I" + (4 + countdata) + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                worksheet.Cells[$"A{(5 + countdata)}:F{(5 + countdata)}"].Merge = true;
                worksheet.Cells[$"A{(5 + countdata)}:J{(5 + countdata)}"].Style.Font.Bold = true;
                //ADD SUMMARY OF QUANTITY
                worksheet.Cells[$"A{(5 + countdata)}"].Value = "TOTAL";
                worksheet.Cells[$"J{(5 + countdata)}"].Formula = "SUM(" + worksheet.Cells["J" + 5 + ":J" + (4 + countdata) + ""].Address + ")";
                worksheet.Calculate();
                
            }

            worksheet.Cells.AutoFitColumns();
            worksheet.Cells["A4"].LoadFromDataTable(reportDataTable, true);
            //worksheet.Cells["A" + 4 + ":J" + countdata + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //worksheet.Cells["A" + 4 + ":J" + countdata + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //worksheet.Cells["A" + 4 + ":J" + countdata + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //worksheet.Cells["A" + 4 + ":J" + countdata + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //worksheet.Cells["A" + 4 + ":J" + 4 + ""].Style.Font.Bold = true;

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
            }

            var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;
            //}
        }
	}
}
