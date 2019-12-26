using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.ExpenditureROResult;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;

namespace Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare
{
	public class GetXlsPrepareQueryHandler : IQueryHandler<GetXlsPrepareQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;

		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentAvalProductRepository garmentAvalProductRepository;
		private readonly IGarmentAvalProductItemRepository garmentAvalProductItemRepository;
		private readonly IGarmentDeliveryReturnRepository garmentDeliveryReturnRepository;
		private readonly IGarmentDeliveryReturnItemRepository garmentDeliveryReturnItemRepository;

		public GetXlsPrepareQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
			garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
			garmentDeliveryReturnRepository = storage.GetRepository<IGarmentDeliveryReturnRepository>();
			garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}
		class monitoringView
		{
			public string roJob { get; set; }
			public string article { get; set; }
			public string buyerCode { get; set; }
			public string productCode { get; set; }
			public string uomUnit { get; set; }
			public string roAsal { get; set; }
			public string remark { get; set; }
			public double stock { get; set; }
			public double receipt { get; set; }
			public double mainFabricExpenditure { get; set; }
			public double nonMainFabricExpenditure { get; set; }
			public double expenditure { get; set; }
			public double aval { get; set; }
			public double remainQty { get; set; }
		}

		async Task<ExpenditureROResult> GetExpenditureById(List<int> id, string token)
		{
			List<ExpenditureROViewModel> expenditureRO = new List<ExpenditureROViewModel>();

			ExpenditureROResult expenditureROResult = new ExpenditureROResult();
			foreach (var item in id)
			{
				var garmentUnitExpenditureNoteUri = PurchasingDataSettings.Endpoint + $"garment-unit-expenditure-notes/ro-asal/{item}";
				var httpResponse = _http.GetAsync(garmentUnitExpenditureNoteUri, token).Result;


				if (httpResponse.IsSuccessStatusCode)
				{
					var a = await httpResponse.Content.ReadAsStringAsync();
					Dictionary<string, object> keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);
					var b = keyValues.GetValueOrDefault("data");


					var expenditure = JsonConvert.DeserializeObject<ExpenditureROViewModel>(keyValues.GetValueOrDefault("data").ToString());
					ExpenditureROViewModel expenditureROViewModel = new ExpenditureROViewModel
					{
						ROAsal = expenditure.ROAsal,
						DetailExpenditureId = expenditure.DetailExpenditureId
					};
					expenditureRO.Add(expenditureROViewModel);
				}
				else
				{
					await GetExpenditureById(id, token);
				}
			}
			expenditureROResult.data = expenditureRO;
			return expenditureROResult;
		}
		public async Task<MemoryStream> Handle(GetXlsPrepareQuery request, CancellationToken cancellationToken)
		{
			DateTime dateFrom = request.dateFrom.ToUniversalTime();
			DateTime dateTo = request.dateTo.AddDays(1).ToUniversalTime();
			var QueryMutationPrepareNow = from a in garmentPreparingRepository.Query
										  join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
										  where a.UnitId == request.unit && a.ProcessDate <= dateTo
										  select new { RO = a.RONo, Articles = a.Article, Id = a.Identity, DetailExpend = b.UENItemId, Processdate = a.ProcessDate };
			List<int> detailExpendId = new List<int>();
			foreach (var item in QueryMutationPrepareNow.Distinct())
			{
				detailExpendId.Add(item.DetailExpend);
			}
			ExpenditureROResult dataExpenditure = await GetExpenditureById(detailExpendId, request.token);
			var QueryMutationPrepareItemsROASAL = (from a in QueryMutationPrepareNow
												   join b in garmentPreparingItemRepository.Query on a.Id equals b.GarmentPreparingId
												   join c in dataExpenditure.data on b.UENItemId equals c.DetailExpenditureId
												   select new { prepareitemid = b.Identity, prepareId = b.GarmentPreparingId, comodityDesc = b.DesignColor, ROs = a.RO, QtyPrepare = b.Quantity, ProductCodes = b.ProductCode, article = a.Articles, roasal = c.ROAsal, remaingningQty = b.RemainingQuantity });
			var QueryCuttingDONow = from a in garmentCuttingInRepository.Query
									join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
									join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
									where a.UnitId == request.unit && a.CuttingInDate <= dateTo
									select new monitoringView { expenditure = 0, aval = 0, buyerCode = "", uomUnit = "", stock = a.CuttingInDate < dateFrom ? -c.PreparingQuantity : 0, nonMainFabricExpenditure = a.CuttingType == "Non Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0, mainFabricExpenditure = a.CuttingType == "Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0, remark = c.DesignColor, roJob = a.RONo, receipt = 0, productCode = c.ProductCode, article = a.Article, roAsal = (from a in QueryMutationPrepareItemsROASAL where a.prepareId == b.PreparingId select a.roasal).FirstOrDefault(), remainQty = 0 };
			var QueryMutationPrepareItemNow = (from d in QueryMutationPrepareNow
											   join e in garmentPreparingItemRepository.Query on d.Id equals e.GarmentPreparingId
											   join c in dataExpenditure.data on e.UENItemId equals c.DetailExpenditureId
											   select new monitoringView { buyerCode = "", uomUnit = "", stock = d.Processdate < dateFrom ? e.Quantity : 0, mainFabricExpenditure = 0, nonMainFabricExpenditure = 0, remark = e.DesignColor, roJob = d.RO, receipt = (d.Processdate >= dateFrom ? e.Quantity : 0), productCode = e.ProductCode, article = d.Articles, roAsal = c.ROAsal, remainQty = e.RemainingQuantity });
			var QueryAval = from a in garmentAvalProductRepository.Query
							join b in garmentAvalProductItemRepository.Query on a.Identity equals b.APId
							join c in garmentPreparingItemRepository.Query on Guid.Parse(b.PreparingItemId) equals c.Identity
							join d in garmentPreparingRepository.Query on c.GarmentPreparingId equals d.Identity
							where a.AvalDate <= dateTo && d.UnitId == request.unit
							select new monitoringView { expenditure = 0, aval = b.Quantity, buyerCode = "", uomUnit = "", stock = a.AvalDate < dateFrom ? -b.Quantity : 0, mainFabricExpenditure = 0, nonMainFabricExpenditure = 0, remark = b.DesignColor, roJob = a.RONo, receipt = 0, productCode = b.ProductCode, article = a.Article, roAsal = (from aa in QueryMutationPrepareItemsROASAL where aa.prepareitemid == Guid.Parse(b.PreparingItemId) select aa.roasal).FirstOrDefault(), remainQty = 0 };
			var asss = dateTo;
			var QueryDeliveryReturn = from a in garmentDeliveryReturnRepository.Query
									  join b in garmentDeliveryReturnItemRepository.Query on a.Identity equals b.DRId
									  join c in garmentPreparingItemRepository.Query on b.PreparingItemId equals Convert.ToString(c.Identity)
									  where a.ReturnDate <= dateTo && a.UnitId == request.unit
									  select new monitoringView { expenditure = b.Quantity, aval = 0, buyerCode = "", uomUnit = "", stock = a.ReturnDate < dateFrom ? -b.Quantity : 0, mainFabricExpenditure = 0, nonMainFabricExpenditure = 0, remark = b.DesignColor, roJob = a.RONo, receipt = 0, productCode = b.ProductCode, article = a.Article, roAsal = (from aa in QueryMutationPrepareItemsROASAL where aa.prepareitemid == Guid.Parse(b.PreparingItemId) select aa.roasal).FirstOrDefault(), remainQty = 0 };

			var queryNow = QueryMutationPrepareItemNow.Union(QueryCuttingDONow).Union(QueryAval).Union(QueryDeliveryReturn).AsEnumerable();

			var querySum = queryNow.GroupBy(x => new { x.roAsal, x.roJob, x.article, x.buyerCode, x.productCode, x.remark }, (key, group) => new
			{
				ROAsal = key.roAsal,
				ROJob = key.roJob,
				stock = group.Sum(s => s.stock),
				ProductCode = key.productCode,
				Article = key.article,
				Remark = key.remark,
				mainFabricExpenditure = group.Sum(s => s.mainFabricExpenditure),
				nonmainFabricExpenditure = group.Sum(s => s.nonMainFabricExpenditure),
				receipt = group.Sum(s => s.receipt),
				Aval = group.Sum(s => s.aval),
				drQty = group.Sum(s => s.expenditure)
			});


			GarmentMonitoringPrepareListViewModel garmentMonitoringPrepareListViewModel = new GarmentMonitoringPrepareListViewModel();
			List<GarmentMonitoringPrepareDto> monitoringPrepareDtos = new List<GarmentMonitoringPrepareDto>();
			foreach (var item in querySum)
			{
				GarmentMonitoringPrepareDto garmentMonitoringPrepareDto = new GarmentMonitoringPrepareDto()
				{
					article = item.Article,
					roJob = item.ROJob,
					productCode = item.ProductCode,
					roAsal = item.ROAsal,
					uomUnit = "MT",
					remainQty = item.stock + item.receipt - item.nonmainFabricExpenditure - item.mainFabricExpenditure - item.Aval - item.drQty,
					stock = item.stock,
					remark = item.Remark,
					receipt = item.receipt,
					aval = item.Aval,
					nonMainFabricExpenditure = item.nonmainFabricExpenditure,
					mainFabricExpenditure = item.mainFabricExpenditure,
					expenditure = item.drQty

				};
				monitoringPrepareDtos.Add(garmentMonitoringPrepareDto);
			}
				garmentMonitoringPrepareListViewModel.garmentMonitorings = monitoringPrepareDtos;

			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Asal Barang", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Keterangan Barang", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Masuk", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Keluar Ke Cutting(MAIN FABRIC)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Keluar Ke Cutting(NON MAIN FABRIC)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG Keluar ke Gudang", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Aval", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) });
			if (garmentMonitoringPrepareListViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in garmentMonitoringPrepareListViewModel.garmentMonitorings)
					reportDataTable.Rows.Add(report.roJob,report.article,report.productCode,report.uomUnit,report.roAsal,report.remainQty,report.stock,report.receipt,report.mainFabricExpenditure,report.nonMainFabricExpenditure,report.expenditure,report.aval,report.remainQty);

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
