using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.Queries
{
    public class GetXlsServiceSubconFabricWashQueryHandler : IQueryHandler<GetXlsServiceSubconFabricWashQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentServiceSubconFabricWashRepository garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconFabricWashItemRepository garmentServiceSubconFabricWashItemRepository;
        private readonly IGarmentServiceSubconFabricWashDetailRepository garmentServiceSubconFabricWashDetailRepository;

		public GetXlsServiceSubconFabricWashQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
			garmentServiceSubconFabricWashItemRepository = storage.GetRepository<IGarmentServiceSubconFabricWashItemRepository>();
			garmentServiceSubconFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSubconFabricWashDetailRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}

        class ServiceSubconFabricWashView
        {

            public string ServiceSubconFabricWashNo { get; internal set; }
            public DateTimeOffset serviceSubconFabricWashDate { get; internal set; }
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

        public async Task<MemoryStream> Handle(GetXlsServiceSubconFabricWashQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            dateTo = dateTo.AddHours(7);


            var Query1 = (from a in garmentServiceSubconFabricWashRepository.Query
                                      join b in garmentServiceSubconFabricWashItemRepository.Query on a.Identity equals b.ServiceSubconFabricWashId
                          join c in garmentServiceSubconFabricWashDetailRepository.Query on b.Identity equals c.ServiceSubconFabricWashItemId
                          where
                                      a.Deleted == false
                                      && b.Deleted == false
                                      && c.Deleted == false
                                      && a.ServiceSubconFabricWashDate >= dateFrom
                                      && a.ServiceSubconFabricWashDate <= dateTo
                          select new ServiceSubconFabricWashView
                          {

                                          ServiceSubconFabricWashNo = a.ServiceSubconFabricWashNo,
                                          //serviceSubconFabricWashDate = a.ServiceSubconFabricWashDate,
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
            var Query = Query1.ToList().GroupBy(x => new { x.ServiceSubconFabricWashNo,/* x.serviceSubconFabricWashDate*/ x.unitExpenditureNo,  x.expendituredate, x.unitSenderCode, x.unitSenderName, x.productCode, x.productName /*x.productRemark*/, x.designcolor, x.quantity, x.uomUnit }, (key, group) => new
            {
                ServiceSubconFabricWashNo = key.ServiceSubconFabricWashNo,
                //serviceSubconFabricWashDate = key.serviceSubconFabricWashDate,
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
            }).OrderBy(s => s.ServiceSubconFabricWashNo);

            GarmentServiceSubconFabricWashListViewModel garmentServiceSubconFabricWashListViewModel = new GarmentServiceSubconFabricWashListViewModel();
            List<ServiceSubconFabricWashDto> ServiceSubconFabricWashDtos = new List<ServiceSubconFabricWashDto>();

            foreach(var item in Query)
            {
                ServiceSubconFabricWashDto dto = new ServiceSubconFabricWashDto()
                {
                    serviceSubconFabricWashNo = item.ServiceSubconFabricWashNo,
                    //serviceSubconFabricWashDate = item.serviceSubconFabricWashDate,
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
                ServiceSubconFabricWashDtos.Add(dto);
            }

            var data = from c in ServiceSubconFabricWashDtos
                       where c.quantity > 0
                       select c;
            ServiceSubconFabricWashDtos = data.ToList();
            decimal quantity = 0;
            //decimal nominal = 0;
            foreach (var item in data)
            {
                quantity += item.quantity;

            }

            ServiceSubconFabricWashDto garmentServiceSubconFabricWashDtos = new ServiceSubconFabricWashDto()
            {
                serviceSubconFabricWashNo = "",
                //serviceSubconFabricWashDate = DateTimeOffset.UtcNow,
                //unitExpenditureNo = "",
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

            ServiceSubconFabricWashDtos.Add(garmentServiceSubconFabricWashDtos);
            garmentServiceSubconFabricWashListViewModel.serviceSubconFabricWashes = ServiceSubconFabricWashDtos;

            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No. Subcon Jasa", DataType = typeof(string) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "anggal Subcon BB Fabric Wash / Print", DataType = typeof(DateTimeOffset) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No. Bon Pengeluaran Unit", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pengeluaran", DataType = typeof(DateTimeOffset) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Asal Unit", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Asal Gudang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(string) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Keterangan Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Design/Color", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah", DataType = typeof(double) });
            
            int counter = 4;

            if (garmentServiceSubconFabricWashListViewModel.serviceSubconFabricWashes.Count > 0)
            {
                foreach (var report in garmentServiceSubconFabricWashListViewModel.serviceSubconFabricWashes)
                {
                    //string ServiceSubconFabricWashDate = report.serviceSubconFabricWashDate.GetValueOrDefault() == new DateTime(1970, 1, 1) || report.serviceSubconFabricWashDate.GetValueOrDefault().ToString("dd MMM yyyy") == "01 Jan 0001" ? "-" : report.serviceSubconFabricWashDate.GetValueOrDefault().ToString("dd MMM yyy");
                    reportDataTable.Rows.Add(report.serviceSubconFabricWashNo, /*report.serviceSubconFabricWashDate,*/ report.unitExpenditureNo, report.expendituredate, report.unitSenderCode, report.unitSenderName, report.productCode, report.productName, /*report.productRemark,*/ report.designcolor, report.uomUnit, report.quantity);
                    counter++;

                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A" + 4 + ":J" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "Laporan  Subcon Fabric Wash "; worksheet.Cells["A" + 1 + ":J" + 1 + ""].Merge = true;
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                //worksheet.Cells["A3"].Value = "Konfeksi " + ;
                worksheet.Cells["A" + 1 + ":J" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":J" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":J" + 3 + ""].Merge = true;
                //worksheet.Cells["A" + 1 + ":L" + 2 + ""].Style.Font.Size = 15;
                worksheet.Cells["A" + 1 + ":J" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 1 + ":J" + 4 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A" + 1 + ":J" + 4 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A4"].LoadFromDataTable(reportDataTable, true);
                //worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["G" + 2 + ":I" + counter + ""].Style.Numberformat.Format = "#,##0.00";
                //worksheet.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["J" + 2 + ":J" + counter + ""].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells["J" + counter + ":J" + counter + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + counter].Value = "T O T A L";
                worksheet.Cells["A" + counter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["A" + counter].Style.Font.Bold = true;
                worksheet.Cells["A" + counter + ":I" + counter + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":J" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 4 + ":J" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 4 + ":J" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 4 + ":J" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["G" + (counter) + ":I" + (counter) + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 4 + ":J" + 4 + ""].Style.Font.Bold = true;
                var stream = new MemoryStream();


                //var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;
            }
        }
	}
}
