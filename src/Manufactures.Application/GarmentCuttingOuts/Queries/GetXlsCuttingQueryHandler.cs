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

		public async Task<MemoryStream> Handle(GetXlsCuttingQuery request, CancellationToken cancellationToken)
		{
			DateTime dateFrom = request.dateFrom.ToUniversalTime().AddHours(7);
			DateTime dateTo = request.dateTo.ToUniversalTime().AddHours(7);


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
			var QuerySumQtyPreparing = (from a in garmentCuttingInRepository.Query
										join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
										join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
										where _ro.Distinct().Contains(a.RONo)
										select new { QtyPrepare = c.PreparingQuantity, Ro = a.RONo }).GroupBy(t => t.Ro)
										.Select(t => new { RO = t.Key, QtyPrepare = t.Sum(u => u.QtyPrepare) });
			var Fc = from a in QuerySumFC
					 join b in QuerySumQtyPreparing on a.RO equals b.RO
					 select new { ro = a.RO, FC = Convert.ToDouble(a.FC / b.QtyPrepare) };

			var QueryCuttingIn = from a in garmentCuttingInRepository.Query
								 join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
								 join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
								 join d in Fc on a.RONo equals d.ro
								 where a.UnitId == request.unit && a.CuttingInDate <= dateTo
								 select new monitoringView { fc = (d.FC * c.PreparingQuantity), cuttingQtyMeter = 0, remainQty = 0, stock = a.CuttingInDate < dateFrom ? c.CuttingInQuantity : 0, cuttingQtyPcs = a.CuttingInDate >= dateFrom ? c.CuttingInQuantity : 0, roJob = a.RONo, article = a.Article, productCode = c.ProductCode, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), hours = (from cost in costCalculation.data where cost.ro == a.RONo select cost.hours).FirstOrDefault(), expenditure = 0 };

			var QueryCuttingOut = from a in garmentCuttingOutRepository.Query
								  join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								  where a.UnitId == request.unit && a.CuttingOutDate <= dateTo
								  select new monitoringView { fc = 0, cuttingQtyMeter = 0, remainQty = 0, stock = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0, cuttingQtyPcs = b.TotalCuttingOut, roJob = a.RONo, article = a.Article, productCode = b.ProductCode, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), hours = (from cost in costCalculation.data where cost.ro == a.RONo select cost.hours).FirstOrDefault(), expenditure = 0 };

			var QueryAvalComp = from a in garmentAvalComponentRepository.Query
								join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
								where a.UnitId == request.unit && a.Date <= dateTo
								select new monitoringView { fc = 0, cuttingQtyMeter = 0, remainQty = 0, stock = a.Date < dateFrom ? -b.Quantity : 0, cuttingQtyPcs = 0, roJob = a.RONo, article = a.Article, productCode = b.ProductCode, qtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), style = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(), hours = (from cost in costCalculation.data where cost.ro == a.RONo select cost.hours).FirstOrDefault(), expenditure = a.Date >= dateFrom ? b.Quantity : 0 };

			var queryNow = QueryCuttingIn.Union(QueryCuttingOut).Union(QueryAvalComp);

			var querySum = queryNow.ToList().GroupBy(x => new { x.qtyOrder, x.roJob, x.article, x.productCode, x.style, x.hours }, (key, group) => new
			{
				QtyOrder = key.qtyOrder,
				RoJob = key.roJob,
				Fc = group.Sum(s => s.fc),
				Stock = group.Sum(s => s.stock),
				ProductCode = key.productCode,
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
					fc = item.Fc,
					cuttingQtyMeter = item.Fc * item.CuttingQtyPcs

				};
				monitoringCuttingDtos.Add(cuttingDto);
			}
			listViewModel.garmentMonitorings = monitoringCuttingDtos;
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FC", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hours", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Potong (M)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Potong", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Keluar", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) });
			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings)
					reportDataTable.Rows.Add(report.roJob, report.article, report.productCode, report.qtyOrder, report.style, report.fc, report.hours, report.cuttingQtyMeter, report.stock, report.cuttingQtyPcs, report.expenditure,  report.remainQty);

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
		class monitoringView
		{
			public string roJob { get; set; }
			public string article { get; set; }
			public string productCode { get; set; }
			public double qtyOrder { get; set; }
			public string style { get; set; }
			public double fc { get; set; }
			public double hours { get; set; }
			public double cuttingQtyMeter { get; set; }
			public double stock { get; set; }
			public double cuttingQtyPcs { get; set; }
			public double expenditure { get; set; }
			public double remainQty { get; set; }
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
