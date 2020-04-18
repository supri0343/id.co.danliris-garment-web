using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using System.Data;
using OfficeOpenXml;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using OfficeOpenXml.Style;

namespace Manufactures.Application.GarmentLoadings.Queries
{
	public class GetXlsLoadingQueryHandler : IQueryHandler<GetXlsLoadingQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;

		public GetXlsLoadingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();

			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();


		}
        async Task<HOrderDataProductionReport> GetDataHOrder(List<string> ro, string token)
        {
            HOrderDataProductionReport hOrderDataProductionReport = new HOrderDataProductionReport();

            var listRO = string.Join(",", ro.Distinct());
            var costCalculationUri = SalesDataSettings.Endpoint + $"local-merchandiser/horders/data-production-report-by-no/{listRO}";
            var httpResponse = await _http.GetAsync(costCalculationUri, token);

            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();
                var listData = JsonConvert.DeserializeObject<List<HOrderViewModel>>(dataString);

                foreach (var item in ro)
                {
                    var data = listData.SingleOrDefault(s => s.No == item);
                    if (data != null)
                    {
                        hOrderDataProductionReport.data.Add(data);
                    }
                }
            }

            return hOrderDataProductionReport;
        }

        public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
        {
            CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

            var listRO = string.Join(",", ro.Distinct());
            var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/{listRO}";
            var httpResponse = await _http.GetAsync(costCalculationUri, token);

            var freeRO = new List<string>();

            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();
                var listData = JsonConvert.DeserializeObject<List<CostCalViewModel>>(dataString);

                foreach (var item in ro)
                {
                    var data = listData.SingleOrDefault(s => s.ro == item);
                    if (data != null)
                    {
                        costCalculationGarmentDataProductionReport.data.Add(data);
                    }
                    else
                    {
                        freeRO.Add(item);
                    }
                }
            }

            HOrderDataProductionReport hOrderDataProductionReport = await GetDataHOrder(ro, token);

            Dictionary<string, string> comodities = new Dictionary<string, string>();
            if (hOrderDataProductionReport.data.Count > 0)
            {
                var comodityCodes = hOrderDataProductionReport.data.Select(s => s.Kode).Distinct().ToList();
                var filter = "{\"(" + string.Join(" || ", comodityCodes.Select(s => "Code==" + "\\\"" + s + "\\\"")) + ")\" : \"true\"}";

                var masterGarmentComodityUri = MasterDataSettings.Endpoint + $"master/garment-comodities?filter=" + filter;
                var garmentComodityResponse = _http.GetAsync(masterGarmentComodityUri).Result;
                var garmentComodityResult = new GarmentComodityResult();
                if (garmentComodityResponse.IsSuccessStatusCode)
                {
                    garmentComodityResult = JsonConvert.DeserializeObject<GarmentComodityResult>(garmentComodityResponse.Content.ReadAsStringAsync().Result);
                    //comodities = garmentComodityResult.data.ToDictionary(d => d.Code, d => d.Name);
                    foreach (var comodity in garmentComodityResult.data)
                    {
                        comodities[comodity.Code] = comodity.Name;
                    }
                }
            }

            foreach (var hOrder in hOrderDataProductionReport.data)
            {
                costCalculationGarmentDataProductionReport.data.Add(new CostCalViewModel
                {
                    ro = hOrder.No,
                    buyerCode = hOrder.Codeby,
                    comodityName = comodities.GetValueOrDefault(hOrder.Kode),
                    hours = (double)hOrder.Sh_Cut,
                    qtyOrder = (double)hOrder.Qty
                });
            }

            return costCalculationGarmentDataProductionReport;
        }

        class monitoringView
		{
			public string roJob { get; internal set; }
			public string article { get; internal set; }
			public string buyerCode { get; internal set; }
			public double qtyOrder { get; internal set; }
			public double stock { get; internal set; }
			public string style { get; internal set; }
			public double cuttingQtyPcs { get; internal set; }
			public double loadingQtyPcs { get; internal set; }
			public string uomUnit { get; internal set; }
			public double remainQty { get; internal set; }
			public decimal price { get; internal set; }
		}

		public async Task<MemoryStream> Handle(GetXlsLoadingQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));

			var QueryRoCuttingOut = (from a in garmentCuttingOutRepository.Query
									 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
									 where a.UnitId == request.unit && a.CuttingOutDate <= dateTo
									 select a.RONo).Distinct();
			var QueryRoLoading = (from a in garmentLoadingRepository.Query
								  join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
								  where a.UnitId == request.unit && a.LoadingDate <= dateTo
								  select a.RONo).Distinct();
			var QueryRo = QueryRoCuttingOut.Union(QueryRoLoading).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);

			var QueryCuttingOut = from a in garmentCuttingOutRepository.Query
								  join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								  where a.UnitId == request.unit && a.CuttingOutDate <= dateTo
								  select new monitoringView { price = 0, buyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), loadingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.CuttingOutDate < dateFrom ? b.TotalCuttingOut : 0, cuttingQtyPcs = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0, roJob = a.RONo, article = a.Article, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };
			var QueryLoading = from a in garmentLoadingRepository.Query
							   join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
							   where a.UnitId == request.unit && a.LoadingDate <= dateTo
							   select new monitoringView { price = Convert.ToDecimal(b.Price), buyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), loadingQtyPcs = a.LoadingDate >= dateFrom ? b.Quantity : 0, cuttingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.LoadingDate < dateFrom ? -b.Quantity : 0, roJob = a.RONo, article = a.Article, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };
			var queryNow = QueryCuttingOut.Union(QueryLoading);
			var querySum = queryNow.ToList().GroupBy(x => new { x.buyerCode, x.qtyOrder, x.roJob, x.article, x.uomUnit, x.style }, (key, group) => new
			{
				QtyOrder = key.qtyOrder,
				RoJob = key.roJob,
				buyer = key.buyerCode,
				price = group.Sum(s => s.price),
				Style = key.style,
				Stock = group.Sum(s => s.stock),
				UomUnit = key.uomUnit,
				Article = key.article,
				CuttingQtyPcs = group.Sum(s => s.cuttingQtyPcs),
				Loading = group.Sum(s => s.loadingQtyPcs)
			}).OrderBy(s => s.RoJob);
			GarmentMonitoringLoadingListViewModel listViewModel = new GarmentMonitoringLoadingListViewModel();
			List<GarmentMonitoringLoadingDto> monitoringDtos = new List<GarmentMonitoringLoadingDto>();
			foreach (var item in querySum)
			{
				GarmentMonitoringLoadingDto dto = new GarmentMonitoringLoadingDto
				{
					roJob = item.RoJob,
					article = item.Article,
					buyerCode = item.buyer,
					uomUnit = item.UomUnit,
					qtyOrder = item.QtyOrder,
					cuttingQtyPcs = item.CuttingQtyPcs,
					loadingQtyPcs = item.Loading,
					stock = item.Stock,
					style = item.Style,
					price = item.price,
					remainQty = item.Stock + item.CuttingQtyPcs - item.Loading
				};
				monitoringDtos.Add(dto);
			}
			listViewModel.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Buyer", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Harga (M)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Masuk", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Keluar", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string ) });
			int counter = 1;
			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings)
				{
					reportDataTable.Rows.Add(report.roJob, report.article, report.buyerCode, report.qtyOrder, report.style, report.price, report.stock, report.cuttingQtyPcs, report.loadingQtyPcs, report.remainQty, report.uomUnit);
					counter++;
				}
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				worksheet.Cells["A1"].LoadFromDataTable(reportDataTable, true);
				worksheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["D" + 2 + ":D" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["F" + 2 + ":F" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["G" + 2 + ":G" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["H" + 2 + ":H" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["I" + 2 + ":I" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["J" + 2 + ":J" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				 
				var stream = new MemoryStream();

				if (request.type != "bookkeeping")
				{
					worksheet.Column(3).Hidden = true;
					worksheet.Column(6).Hidden = true;
				}
				package.SaveAs(stream);

				return stream;
			}
		}
	}
}
