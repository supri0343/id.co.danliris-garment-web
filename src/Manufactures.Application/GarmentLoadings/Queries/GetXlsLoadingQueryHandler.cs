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
		public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
		{
			List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel>();
			CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();
			foreach (var item in ro)
			{
				var garmentUnitExpenditureNoteUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/{item}";
				var httpResponse = _http.GetAsync(garmentUnitExpenditureNoteUri, token).Result;

				if (httpResponse.IsSuccessStatusCode)
				{
					var a = await httpResponse.Content.ReadAsStringAsync();
					Dictionary<string, object> keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);
					var data = JsonConvert.DeserializeObject<CostCalViewModel>(keyValues.GetValueOrDefault("data").ToString());
					CostCalViewModel expenditureROViewModel = new CostCalViewModel
					{
						ro = data.ro,
						buyerCode = data.buyerCode,
						hours = data.hours,
						qtyOrder = data.qtyOrder,
						comodityName = data.comodityName
					};
					costCalViewModels.Add(expenditureROViewModel);
				}
				else
				{
					await GetDataCostCal(ro, token);
				}
			}
			costCalculationGarmentDataProductionReport.data = costCalViewModels;
			return costCalculationGarmentDataProductionReport;
		}
		class monitoringView
		{
			public string roJob { get; internal set; }
			public string article { get; internal set; }
			public double qtyOrder { get; internal set; }
			public double stock { get; internal set; }
			public string style { get; internal set; }
			public double cuttingQtyPcs { get; internal set; }
			public double loadingQtyPcs { get; internal set; }
			public string uomUnit { get; internal set; }
			public double remainQty { get; internal set; }
		}

		public async Task<MemoryStream> Handle(GetXlsLoadingQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));


			var QueryRoCuttingOut = (from a in garmentCuttingOutRepository.Query
									 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
									 where a.UnitFromId == request.unit && a.CuttingOutDate <= dateTo
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
								  select new monitoringView { loadingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.CuttingOutDate < dateFrom ? b.TotalCuttingOut : 0, cuttingQtyPcs = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut :0, roJob = a.RONo, article = a.Article, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };
			var QueryLoading = from a in garmentLoadingRepository.Query
							   join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
							   where a.UnitId == request.unit && a.LoadingDate <= dateTo
							   select new monitoringView { loadingQtyPcs = a.LoadingDate >= dateFrom ? b.Quantity : 0, cuttingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.LoadingDate < dateFrom ? -b.Quantity : 0, roJob = a.RONo, article = a.Article, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault() };
			var queryNow = QueryCuttingOut.Union(QueryLoading);
			var querySum = queryNow.ToList().GroupBy(x => new { x.qtyOrder, x.roJob, x.article, x.uomUnit, x.style }, (key, group) => new
			{
				QtyOrder = key.qtyOrder,
				RoJob = key.roJob,
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
					uomUnit = item.UomUnit,
					qtyOrder = item.QtyOrder,
					cuttingQtyPcs = item.CuttingQtyPcs,
					loadingQtyPcs = item.Loading,
					stock = item.Stock,
					style = item.Style,
					remainQty = item.Stock + item.CuttingQtyPcs - item.Loading
				};
				monitoringDtos.Add(dto);
			}
			listViewModel.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Masuk", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Keluar", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string ) });
			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings)
					reportDataTable.Rows.Add(report.roJob, report.article, report.qtyOrder, report.style , report.stock, report.cuttingQtyPcs, report.loadingQtyPcs , report.remainQty, report.uomUnit);

			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				worksheet.Cells["A1"].LoadFromDataTable(reportDataTable, true);
				var stream = new MemoryStream();
				package.SaveAs(stream);

				return stream;
			}
		}
	}
}
