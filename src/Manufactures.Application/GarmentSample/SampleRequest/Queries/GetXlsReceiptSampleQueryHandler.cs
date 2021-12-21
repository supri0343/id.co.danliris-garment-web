using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.GarmentSectionResult;
using System.Threading;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Manufactures.Application.GarmentSample.SampleRequest.Queries
{
    public class GetXlsReceiptSampleQueryHandler : IQueryHandler<GetXlsReceiptSampleQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentSampleRequestRepository garmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository garmentSampleRequestProductRepository;

        public GetXlsReceiptSampleQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
            garmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
        }
        public async Task<GarmentSectionResult> GetSectionNameById(List<int> id, string token)
        {
            List<GarmentSectionViewModel> sectionViewModels = new List<GarmentSectionViewModel>();

            GarmentSectionResult sectionResult = new GarmentSectionResult();
            foreach (var item in id.Distinct())
            {
                var uri = MasterDataSettings.Endpoint + $"master/garment-sections/{item}";
                var httpResponse = await _http.GetAsync(uri, token);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var a = httpResponse.Content.ReadAsStringAsync().Result;
                    Dictionary<string, object> keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);
                    var b = keyValues.GetValueOrDefault("data").ToString();

                    var garmentSection = JsonConvert.DeserializeObject<GarmentSectionViewModel>(b);
                    GarmentSectionViewModel garmentSectionViewModel = new GarmentSectionViewModel
                    {
                        Id = garmentSection.Id,
                        Name = garmentSection.Name
                    };
                    sectionViewModels.Add(garmentSectionViewModel);
                }

            }
            sectionResult.data = sectionViewModels;
            return sectionResult;
        }
        class monitoringView
        {
            public string sampleRequestNo { get; set; }
            public string roNoSample { get; set; }
            public string sampleCategory { get; set; }
            public string sampleType { get; set; }
            public string buyer { get; set; }
            public string style { get; set; }
            public string color { get; set; }
            public string sizeName { get; set; }
            public string sizeDescription { get; set; }
            public double quantity { get; set; }
            public DateTimeOffset sentDate { get; set; }
            public DateTimeOffset receivedDate { get; set; }
            public string garmentSectionName { get; set; }
            public DateTimeOffset sampleRequestDate { get; set; }
        }
        public async Task<MemoryStream> Handle(GetXlsReceiptSampleQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset receivedDateFrom = new DateTimeOffset(request.receivedDateFrom);
            receivedDateFrom.AddHours(7);
            DateTimeOffset receivedDateTo = new DateTimeOffset(request.receivedDateTo);
            receivedDateTo = receivedDateTo.AddHours(7);

            var QuerySampleRequest = from a in (from aa in garmentSampleRequestRepository.Query
                                                where aa.ReceivedDate >= receivedDateFrom && aa.ReceivedDate <= receivedDateTo
                                                select new
                                                {
                                                    aa.Identity,
                                                    aa.RONoSample,
                                                    aa.SampleCategory,
                                                    aa.SampleRequestNo,
                                                    aa.SampleType,
                                                    aa.BuyerCode,
                                                    aa.BuyerName,
                                                    aa.SentDate,
                                                    aa.ReceivedDate,
                                                    aa.Date,
                                                    aa.SectionId
                                                })
                                     join b in garmentSampleRequestProductRepository.Query on a.Identity equals b.SampleRequestId
                                     select new
                                     {
                                         Style = b.Style,
                                         Color = b.Color,
                                         SizeName = b.SizeName,
                                         SizeDescription = b.SizeDescription,
                                         Quantity = b.Quantity,
                                         RoNoSample = a.RONoSample,
                                         SampleCategory = a.SampleCategory,
                                         SampleRequestNo = a.SampleRequestNo,
                                         SampleType = a.SampleType,
                                         BuyerCode = a.BuyerCode,
                                         BuyerName = a.BuyerName,
                                         SentDate = a.SentDate,
                                         ReceivedDate = a.ReceivedDate,
                                         SampleRequestDate = a.Date,
                                         SectionId = a.SectionId
                                     };
            List<int> _sectionId = new List<int>();
            foreach (var item in QuerySampleRequest)
            {
                _sectionId.Add(item.SectionId);

            }
            _sectionId.Distinct();
            GarmentSectionResult garmentSectionResult = await GetSectionNameById(_sectionId, request.token);

            GarmentMonitoringReceiptSampleViewModel sampleViewModel = new GarmentMonitoringReceiptSampleViewModel();
            List<GarmentMonitoringReceiptSampleDto> sampleDtosList = new List<GarmentMonitoringReceiptSampleDto>();
            foreach (var item in QuerySampleRequest)
            {
                GarmentMonitoringReceiptSampleDto receiptSampleDto = new GarmentMonitoringReceiptSampleDto()
                {
                    buyer = item.BuyerCode,
                    color = item.Color,
                    quantity = item.Quantity,
                    receivedDate = item.ReceivedDate,
                    roNoSample = item.RoNoSample,
                    sampleCategory = item.SampleCategory,
                    sampleRequestDate = item.SampleRequestDate,
                    sampleRequestNo = item.SampleRequestNo,
                    sampleType = item.SampleType,
                    sentDate = item.SentDate,
                    sizeDescription = item.SizeDescription,
                    style = item.Style,
                    sizeName = item.SizeName,
                    garmentSectionName = (from aa in garmentSectionResult.data where aa.Id == item.SectionId select aa.Name).FirstOrDefault()
                };
                sampleDtosList.Add(receiptSampleDto);
            }
            sampleViewModel.garmentMonitorings = sampleDtosList;
            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Surat Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kategori Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Color", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Size", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Keterangan Size", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Quantity", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Shipment", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Terima Surat Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Md", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Pembuatan Surat Sample", DataType = typeof(string) });
            int counter = 5;

            if (sampleViewModel.garmentMonitorings.Count > 0)
            {
                foreach (var report in sampleViewModel.garmentMonitorings)
                {
                    reportDataTable.Rows.Add(report.sampleRequestNo, report.roNoSample, report.sampleCategory, report.sampleType, report.buyer, report.style, report.color, report.sizeName, report.sizeDescription, report.quantity, report.sentDate.ToString("dd-MM-yyyy"),report.receivedDate.Value.ToString("dd-MM-yyyy") , report.garmentSectionName, report.sampleRequestDate.ToString("dd-MM-yyyy"));
                    counter++;

                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");


                worksheet.Cells["A" + 5 + ":N" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":N" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":N" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":N" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(5).Style.Font.Bold = true;

                worksheet.Cells["A1"].Value = "Monitoring Penerimaan Sampel";
                worksheet.Cells["A2"].Value = "Periode " + receivedDateFrom.ToString("dd-MM-yyyy") + " s/d " + receivedDateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A3"].Value = "  ";
                worksheet.Cells["A" + 1 + ":N" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":N" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":N" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":N" + 3 + ""].Style.Font.Size = 15;
                worksheet.Cells["A" + 1 + ":N" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 6 + ":I" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["K" + 6 + ":N" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 1 + ":N" + 5 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells["E" + 5 + ":N" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 5 + ":N" + counter + ""].AutoFitColumns();
                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;
            }
             
        }
    }
}
