using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using System.Data;
using OfficeOpenXml;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using OfficeOpenXml.Style;

namespace Manufactures.Application.GarmentCuttingOuts.Queries
{
	public class GetXlsCuttingQueryHandler : IQueryHandler<GetXlsCuttingQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentAvalComponentRepository garmentAvalComponentRepository;
		private readonly IGarmentAvalComponentItemRepository garmentAvalComponentItemRepository;

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

			HOrderDataProductionReport hOrderDataProductionReport = await GetDataHOrder(freeRO, token);

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

		public async Task<MemoryStream> Handle(GetXlsCuttingQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));


			var QueryRoCuttingOut = (from a in garmentCuttingOutRepository.Query
									 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
									 where a.UnitId == request.unit && a.CuttingOutDate <= dateTo
									 select a.RONo).Distinct();
			var QueryRoCuttingIn = (from a in garmentCuttingInRepository.Query
									join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
									where a.UnitId == request.unit && a.CuttingInDate <= dateTo
									select a.RONo).Distinct();
			var QueryRoAvalComp = (from a in garmentAvalComponentRepository.Query
								   join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
								   where a.UnitId == request.unit && a.Date <= dateTo
								   select a.RONo).Distinct();
			var QueryRo = QueryRoCuttingOut.Union(QueryRoCuttingIn).Union(QueryRoAvalComp).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);

			var QuerySumFC = (from a in garmentCuttingInRepository.Query
							  where _ro.Distinct().Contains(a.RONo)
							  select new { fC = a.FC, Ro = a.RONo }).GroupBy(t => t.Ro).Select(t => new { RO = t.Key, FC = t.Sum(u => u.fC) });
			//var QuerySumQtyPreparing = (from a in garmentCuttingInRepository.Query
			//							join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
			//							join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
			//							where _ro.Distinct().Contains(a.RONo)
			//							select new { QtyPrepare = c.PreparingQuantity, Ro = a.RONo }).GroupBy(t => t.Ro)
			//							.Select(t => new { RO = t.Key, QtyPrepare = t.Sum(u => u.QtyPrepare) });
			var QuerySumQtyPreparing = (from a in garmentCuttingInRepository.Query
											//join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
											//join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
											//where _ro.Distinct().Contains(a.RONo)
										select new { Ro = a.RONo })
										.Select(t => new { RO = t.Ro });
			var Fc = from a in QuerySumFC
					 join b in QuerySumQtyPreparing on a.RO equals b.RO
					 select new { ro = a.RO, FC = Convert.ToDouble(a.FC / QuerySumQtyPreparing.Count()) };

			var QueryCuttingIn = from a in garmentCuttingInRepository.Query
								 join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
								 join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
								 join d in Fc on a.RONo equals d.ro
								 where a.UnitId == request.unit && a.CuttingInDate <= dateTo
								 select new monitoringView { buyerCode= (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), price = Convert.ToDecimal(c.Price), fc = (d.FC * c.PreparingQuantity), cuttingQtyMeter = 0, remainQty = 0, stock = a.CuttingInDate < dateFrom ? c.CuttingInQuantity : 0, cuttingQtyPcs = a.CuttingInDate >= dateFrom ? c.CuttingInQuantity : 0, roJob = a.RONo, article = a.Article, productCode = c.ProductCode, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), hours = (from cost in costCalculation.data where cost.ro == a.RONo select cost.hours).FirstOrDefault(), expenditure = 0 };

			var QueryCuttingOut = from a in garmentCuttingOutRepository.Query
								  join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								  where a.UnitFromId == request.unit && a.CuttingOutDate <= dateTo
								  select new monitoringView { buyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(),price = 0, fc = 0, cuttingQtyMeter = 0, remainQty = 0, stock = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0, cuttingQtyPcs = 0, roJob = a.RONo, article = a.Article, productCode = b.ProductCode, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), hours = (from cost in costCalculation.data where cost.ro == a.RONo select cost.hours).FirstOrDefault(), expenditure = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0 };

			var QueryAvalComp = from a in garmentAvalComponentRepository.Query
								join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
								where a.UnitId == request.unit && a.Date <= dateTo
								select new monitoringView { buyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(),price = 0, fc = 0, cuttingQtyMeter = 0, remainQty = 0, stock = a.Date < dateFrom ? -b.Quantity : 0, cuttingQtyPcs = 0, roJob = a.RONo, article = a.Article, productCode = b.ProductCode, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), hours = (from cost in costCalculation.data where cost.ro == a.RONo select cost.hours).FirstOrDefault(), expenditure = a.Date >= dateFrom ? b.Quantity : 0 };

			var queryNow = QueryCuttingIn.Union(QueryCuttingOut).Union(QueryAvalComp);

			var querySum = queryNow.ToList().GroupBy(x => new { x.buyerCode, x.qtyOrder, x.roJob, x.article, x.productCode, x.style, x.hours }, (key, group) => new
			{
				QtyOrder = key.qtyOrder,
				RoJob = key.roJob,
				Fc = group.Sum(s => s.fc),
				Stock = group.Sum(s => s.stock),
				ProductCode = key.productCode,
				buyer = key.buyerCode,
				bPrice = group.Sum(s => s.price),
				Article = key.article,
				Style = key.style,
				CuttingQtyPcs = group.Sum(s => s.cuttingQtyPcs),
				CuttingQtyMeter = group.Sum(s => s.cuttingQtyMeter),
				Expenditure = group.Sum(s => s.expenditure),
				Hours = key.hours
			}).OrderBy(s => s.RoJob);
			GarmentMonitoringCuttingListViewModel listViewModel = new GarmentMonitoringCuttingListViewModel();
			List<GarmentMonitoringCuttingDto> monitoringCuttingDtos = new List<GarmentMonitoringCuttingDto>();
			
			foreach (var item in querySum)
			{
				GarmentMonitoringCuttingDto cuttingDto = new GarmentMonitoringCuttingDto
				{
					roJob = item.RoJob,
					article = item.Article,
					productCode = item.ProductCode,
					style = item.Style,
					hours = item.Hours,
					qtyOrder = item.QtyOrder,
					cuttingQtyPcs = item.CuttingQtyPcs,
					expenditure = item.Expenditure,
					stock = item.Stock,
					remainQty = item.Stock + item.CuttingQtyPcs - item.Expenditure,
					fc = Math.Round(item.Fc, 2),
					cuttingQtyMeter = Math.Round(item.Fc * item.CuttingQtyPcs, 2),
					price = item.bPrice,
					buyerCode = item.buyer

				};
				monitoringCuttingDtos.Add(cuttingDto);
			}
			
			var data = from a in monitoringCuttingDtos
					   where a.stock > 0 || a.expenditure > 0 || a.cuttingQtyPcs > 0 || a.remainQty > 0
					   select a;
			double stocks = 0;
			double cuttingQtyPcs = 0;
			double expenditure = 0;
			foreach (var item in data)
			{
				stocks += item.stock;
				cuttingQtyPcs += item.cuttingQtyPcs;
				expenditure += item.expenditure;

			}
			monitoringCuttingDtos = data.ToList();
			GarmentMonitoringCuttingDto cuttingDtos = new GarmentMonitoringCuttingDto
			{
				roJob = "",
				article = "",
				productCode = "",
				style = "",
				hours = 0,
				qtyOrder = 0,
				cuttingQtyPcs = cuttingQtyPcs,
				expenditure = expenditure,
				stock = stocks,
				remainQty = stocks + cuttingQtyPcs - expenditure,
				fc = 0,
				cuttingQtyMeter = 0,
				price = 0,
				buyerCode = ""

			};
			monitoringCuttingDtos.Add(cuttingDtos);
			listViewModel.garmentMonitorings = monitoringCuttingDtos;


			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Buyer", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FC", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hours", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Potong (M)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Harga (M)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Potong", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Keluar", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) });
			int counter = 1;
			
			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings)
				{
					reportDataTable.Rows.Add(report.roJob, report.article, report.productCode, report.buyerCode, report.qtyOrder, report.style, report.fc, report.hours, report.cuttingQtyMeter, report.price, report.stock, report.cuttingQtyPcs, report.expenditure, report.remainQty);
					counter++;
					
				}
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				worksheet.Cells["A1"].LoadFromDataTable(reportDataTable, true);
				
				worksheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["E" + 2 + ":E" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["I" + 2 + ":I" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["J" + 2 + ":J" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["K" + 2 + ":K" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(12).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["L" + 2 + ":L" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(13).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["M" + 2 + ":M" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(14).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["N" + 2 + ":N" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Cells["A" + 1 + ":N" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 1 + ":N" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 1 + ":N" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 1 + ":N" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + (counter) + ":H" + (counter) + ""].Merge = true;
				worksheet.Cells["I" + (counter) + ":N" + (counter) + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 1 + ":N" + 1 + ""].Style.Font.Bold = true;
				var stream = new MemoryStream();
				if (request.type != "bookkeeping")
				{
					worksheet.Column(4).Hidden = true;
					worksheet.Column(10).Hidden = true;
				}
				package.SaveAs(stream);

				return stream;
			}
		}
		class monitoringView
		{
			public string roJob { get; set; }
			public string article { get; set; }
			public string productCode { get; set; }
			public string buyerCode { get; set; }
			public double qtyOrder { get; set; }
			public string style { get; set; }
			public double fc { get; set; }
			public double hours { get; set; }
			public double cuttingQtyMeter { get; set; }
			public double stock { get; set; }
			public double cuttingQtyPcs { get; set; }
			public double expenditure { get; set; }
			public double remainQty { get; set; }
			public decimal price { get; set; }
		}
		public GetXlsCuttingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();

			garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
			garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();


		}
	}
}
