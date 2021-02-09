﻿using ExtCore.Data.Abstractions;
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
using Manufactures.Domain.GarmentPreparings.Repositories;
using System.Net.Http;
using System.Text;
using Manufactures.Domain.GarmentCuttingIns.Repositories;

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
		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentBalanceLoadingRepository garmentBalanceLoadingRepository;

		public GetXlsLoadingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentBalanceLoadingRepository = storage.GetRepository<IGarmentBalanceLoadingRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
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
			var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/";

			var httpContent = new StringContent(JsonConvert.SerializeObject(listRO), Encoding.UTF8, "application/json");

			var httpResponse = await _http.SendAsync(HttpMethod.Get, costCalculationUri, token, httpContent);

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
			else
			{
				var err = await httpResponse.Content.ReadAsStringAsync();

			}

			//HOrderDataProductionReport hOrderDataProductionReport = await GetDataHOrder(freeRO, token);

			//Dictionary<string, string> comodities = new Dictionary<string, string>();
			//if (hOrderDataProductionReport.data.Count > 0)
			//{
			//    var comodityCodes = hOrderDataProductionReport.data.Select(s => s.Kode).Distinct().ToList();
			//    var filter = "{\"(" + string.Join(" || ", comodityCodes.Select(s => "Code==" + "\\\"" + s + "\\\"")) + ")\" : \"true\"}";

			//    var masterGarmentComodityUri = MasterDataSettings.Endpoint + $"master/garment-comodities?filter=" + filter;
			//    var garmentComodityResponse = _http.GetAsync(masterGarmentComodityUri).Result;
			//    var garmentComodityResult = new GarmentComodityResult();
			//    if (garmentComodityResponse.IsSuccessStatusCode)
			//    {
			//        garmentComodityResult = JsonConvert.DeserializeObject<GarmentComodityResult>(garmentComodityResponse.Content.ReadAsStringAsync().Result);
			//        //comodities = garmentComodityResult.data.ToDictionary(d => d.Code, d => d.Name);
			//        foreach (var comodity in garmentComodityResult.data)
			//        {
			//            comodities[comodity.Code] = comodity.Name;
			//        }
			//    }
			//}

			//foreach (var hOrder in hOrderDataProductionReport.data)
			//{
			//    costCalculationGarmentDataProductionReport.data.Add(new CostCalViewModel
			//    {
			//        ro = hOrder.No,
			//        buyerCode = hOrder.Codeby,
			//        comodityName = comodities.GetValueOrDefault(hOrder.Kode),
			//        hours = (double)hOrder.Sh_Cut,
			//        qtyOrder = (double)hOrder.Qty
			//    });
			//}

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
		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal BasicPrice { get; internal set; }
			public int Count { get; internal set; }
		}
		class ViewFC
		{
			public string RO { get; internal set; }
			public double FC { get; internal set; }
			public int Count { get; internal set; }
		}
		public async Task<MemoryStream> Handle(GetXlsLoadingQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));
			DateTimeOffset dateBalance = (from a in garmentBalanceLoadingRepository.Query.OrderByDescending(s => s.CreatedDate)

										  select a.CreatedDate).FirstOrDefault();
			var QueryRoCuttingOut = (from a in garmentCuttingOutRepository.Query
									 where a.UnitId == request.unit && a.CuttingOutDate <= dateTo && a.CuttingOutDate >= dateBalance
									 select a.RONo).Distinct();
			var QueryRoLoading = (from a in garmentLoadingRepository.Query
								  where a.UnitId == request.unit && a.LoadingDate <= dateTo && a.LoadingDate >= dateBalance
								  select a.RONo).Distinct();
			var QueryRo = QueryRoCuttingOut.Union(QueryRoLoading).Distinct();

			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			//CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
			var sumbasicPrice = (from a in garmentPreparingRepository.Query
								 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
								 where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) &&*/
								 a.UnitId == request.unit
								 select new { a.RONo, b.BasicPrice })
					.GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
					{
						RO = key.RONo,
						BasicPrice = Math.Round(Convert.ToDecimal(group.Sum(s => s.BasicPrice)), 2),
						Count = group.Count()
					});
			var sumFCs = (from a in garmentCuttingInRepository.Query
						  where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && */ a.CuttingType == "Main Fabric" &&
						 a.UnitId == request.unit && a.CuttingInDate <= dateTo
						  select new { a.FC, a.RONo })
						 .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
						 {
							 RO = key.RONo,
							 FC = group.Sum(s => s.FC),
							 Count = group.Count()
						 });
			var _unitName = (from a in garmentCuttingOutRepository.Query
							 where a.UnitId == request.unit
							 select a.UnitName).FirstOrDefault();
			var queryBalanceLoading = from a in garmentBalanceLoadingRepository.Query
									  where a.CreatedDate < dateFrom && a.UnitId == request.unit //&& a.RoJob == "2010810"
									  select new monitoringView { price = a.Price, buyerCode = a.BuyerCode, loadingQtyPcs = a.LoadingQtyPcs, remainQty = 0, stock = a.Stock, cuttingQtyPcs = 0, roJob = a.RoJob, article = a.Article, qtyOrder = a.QtyOrder, style = a.Style, uomUnit = "PCS" };

			var QueryCuttingOut = from a in (from aa in garmentCuttingOutRepository.Query
											 where aa.UnitId == request.unit && aa.CuttingOutDate <= dateTo//&& aa.RONo== "2010810"
											 select aa)
								  join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								  select new monitoringView { price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), loadingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? b.TotalCuttingOut : 0, cuttingQtyPcs = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0, roJob = a.RONo, article = a.Article, style = a.ComodityName };
			var QueryLoading = from a in (from aa in garmentLoadingRepository.Query
										  where aa.UnitId == request.unit && aa.LoadingDate <= dateTo //&& aa.RONo == "2010810"
										  select aa)
							   join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
							   select new monitoringView { price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), loadingQtyPcs = a.LoadingDate >= dateFrom ? b.Quantity : 0, cuttingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.LoadingDate < dateFrom && a.LoadingDate > dateBalance ? -b.Quantity : 0, roJob = a.RONo, article = a.Article, style = a.ComodityName };
			var queryNow = queryBalanceLoading.Union(QueryCuttingOut).Union(QueryLoading);

			var querySum = queryNow.ToList().GroupBy(x => new { x.roJob, x.article, x.uomUnit }, (key, group) => new
			{

				RoJob = key.roJob,
				Stock = group.Sum(s => s.stock),
				UomUnit = key.uomUnit,
				Article = key.article,
				CuttingQtyPcs = group.Sum(s => s.cuttingQtyPcs),
				Loading = group.Sum(s => s.loadingQtyPcs)
			}).OrderBy(s => s.RoJob);
			GarmentMonitoringLoadingListViewModel listViewModel = new GarmentMonitoringLoadingListViewModel();
			List<GarmentMonitoringLoadingDto> monitoringDtos = new List<GarmentMonitoringLoadingDto>();
			//List<string> ro = new List<string>();
			foreach (var item in querySum)
			{
				GarmentMonitoringLoadingDto dtos = new GarmentMonitoringLoadingDto
				{
					roJob = item.RoJob,
					article = item.Article,
					uomUnit = item.UomUnit,
					cuttingQtyPcs = Math.Round(item.CuttingQtyPcs, 2),
					loadingQtyPcs = Math.Round(item.Loading, 2),
					stock = Math.Round(item.Stock, 2),
					remainQty = Math.Round(item.Stock + item.CuttingQtyPcs - item.Loading, 2),

				};

				monitoringDtos.Add(dtos);
			}

			listViewModel.garmentMonitorings = monitoringDtos;
			var data = from a in monitoringDtos
					   where a.stock > 0 || a.loadingQtyPcs > 0 || a.cuttingQtyPcs > 0 || a.remainQty > 0
					   select a;
			var roList = (from a in data
						  select a.roJob).Distinct().ToList();

			var roBalance = from a in garmentBalanceLoadingRepository.Query
							select new CostCalViewModel { comodityName = a.Style, buyerCode = a.BuyerCode, hours = a.Hours, qtyOrder = a.QtyOrder, ro = a.RoJob };

			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(roList, request.token);

			foreach (var item in roBalance)
			{
				costCalculation.data.Add(item);
			}

			foreach (var garment in data)
			{
				garment.buyerCode = garment.buyerCode == null ? (from cost in costCalculation.data where cost.ro == garment.roJob select cost.buyerCode).FirstOrDefault() : garment.buyerCode;
				garment.style = garment.style == null ? (from cost in costCalculation.data where cost.ro == garment.roJob select cost.comodityName).FirstOrDefault() : garment.style;
				garment.price = Math.Round(Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDecimal((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault()) == 0 ? Convert.ToDecimal((from a in queryBalanceLoading.ToList() where a.roJob == garment.roJob select a.price).FirstOrDefault()) : Math.Round(Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDecimal((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault());
				garment.nominal = Math.Round((Convert.ToDecimal(garment.stock + garment.cuttingQtyPcs - garment.loadingQtyPcs)) * garment.price, 2);
				garment.qtyOrder = (from cost in costCalculation.data where cost.ro == garment.roJob select cost.qtyOrder).FirstOrDefault();
			}

			monitoringDtos = data.ToList();
			double stocks = 0;
			double cuttingQtyPcs = 0;
			double loadingQtyPcs = 0;
			decimal nominals = 0;
		foreach (var item in data)
			{
				stocks += item.stock;
				cuttingQtyPcs += item.cuttingQtyPcs;
				loadingQtyPcs += item.loadingQtyPcs;
				nominals += item.nominal;

			}
			GarmentMonitoringLoadingDto dto = new GarmentMonitoringLoadingDto
			{
				roJob = "",
				article = "",
				buyerCode = "",
				uomUnit = "",
				qtyOrder = 0,
				cuttingQtyPcs = cuttingQtyPcs,
				loadingQtyPcs = loadingQtyPcs,
				stock = stocks,
				style = "",
				price = 0,
				remainQty = stocks + cuttingQtyPcs - loadingQtyPcs,
				nominal = nominals
			};
			monitoringDtos.Add(dto);

			listViewModel.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Buyer", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Harga (PCS)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Masul", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Keluar", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nominal Sisa", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string ) });
			int counter = 5;
			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings)
				{
					reportDataTable.Rows.Add(report.roJob, report.article, report.buyerCode, report.qtyOrder, report.style,Math.Round( report.price,2), report.stock, report.cuttingQtyPcs, report.loadingQtyPcs, report.remainQty, report.nominal, report.uomUnit);
					counter++;
				}
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				worksheet.Cells["A" + 5 + ":L" + 5 + ""].Style.Font.Bold = true;
				worksheet.Cells["A1"].Value = "Report Loading "; worksheet.Cells["A" + 1 + ":L" + 1 + ""].Merge = true;
				worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
				worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
				worksheet.Cells["A" + 1 + ":L" + 1 + ""].Merge = true;
				worksheet.Cells["A" + 2 + ":L" + 2 + ""].Merge = true;
				worksheet.Cells["A" + 3 + ":L" + 3 + ""].Merge = true;
				worksheet.Cells["A" + 1 + ":L" + 3 + ""].Style.Font.Size = 15;
				worksheet.Cells["A" + 1 + ":L" + 5 + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 1 + ":L" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				worksheet.Cells["A" + 1 + ":L" + 5 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
				worksheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["D" + 6 + ":D" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["F" + 6 + ":K" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Cells["F" + 6 + ":K" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["F" + 6 + ":F" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["F" + (counter) + ":K" + (counter) + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 1 + ":L" + 1 + ""].Style.Font.Bold = true;
				var stream = new MemoryStream();

				if (request.type != "bookkeeping")
				{
					worksheet.Cells["A" + (counter) + ":E" + (counter) + ""].Merge = true;

					worksheet.Column(3).Hidden = true;
					worksheet.Column(6).Hidden = true;
				}else
				{
					worksheet.Cells["A" + (counter) + ":F" + (counter) + ""].Merge = true;
				}
				package.SaveAs(stream);

				return stream;
			}
		}
	}
}
