using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Linq;
using System.IO;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using System.Data;
using OfficeOpenXml;

namespace Manufactures.Application.GarmentMonitoringProductionFlows.Queries
{
	public class GetXlsMonitoringProductionFlowQueryHandler : IQueryHandler<GetXlsMonitoringProductionFlowQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentSewingOutDetailRepository garmentSewingOutDetailRepository;
		private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentFinishingOutDetailRepository garmentFinishingOutDetailRepository;

		public GetXlsMonitoringProductionFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
			garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
			garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
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
		class monitoringView
		{
			public string Ro { get; internal set; }
			public string BuyerCode { get; internal set; }
			public string Article { get; internal set; }
			public string Comodity { get; internal set; }
			public double QtyOrder { get; internal set; }
			public string Size { get; internal set; }
			public double QtyCutting { get; internal set; }
			public double QtyLoading { get; internal set; }
			public double QtySewing { get; internal set; }
			public double QtyFinishing { get; internal set; }
			public double Wip { get; internal set; }

		}

		public async Task<MemoryStream> Handle(GetXlsMonitoringProductionFlowQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset date = new DateTimeOffset(request.date, new TimeSpan(7, 0, 0));

			var QueryRo = (from a in garmentCuttingOutRepository.Query
						   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
						   where a.UnitFromId == request.unit && a.CuttingOutDate <= date
						   select a.RONo).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);

			var QueryCuttingOut = (from a in garmentCuttingOutRepository.Query
								   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
								   where a.UnitFromId == request.unit && a.CuttingOutDate <= date
								   select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyCutting = c.CuttingOutQuantity, Size = c.SizeName });

			var QueryLoading = (from a in garmentLoadingRepository.Query
								join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId

								where a.UnitId == request.unit && a.LoadingDate <= date
								select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyLoading = b.Quantity, Size = b.SizeName });

			var QuerySewingOutIsDifSize = from a in garmentSewingOutRepository.Query
										  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
										  join c in garmentSewingOutDetailRepository.Query on b.Identity equals c.SewingOutItemId
										  where a.SewingTo == "FINISHING" && a.UnitId == request.unit && a.SewingOutDate <= date
										  select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewing = b.Quantity, Size = c.SizeName };
			var QuerySewingOut = from a in garmentSewingOutRepository.Query
								 join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
								 where a.SewingTo == "FINISHING" && a.UnitId == request.unit && a.SewingOutDate <= date && a.IsDifferentSize == false
								 select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewing = b.Quantity, Size = b.SizeName };
			var QueryFinishingOutisDifSize = from a in garmentFinishingOutRepository.Query
											 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
											 join c in garmentFinishingOutDetailRepository.Query on b.Identity equals c.FinishingOutItemId
											 where a.FinishingTo == "GUDANG JADI" && a.UnitId == request.unit && a.FinishingOutDate <= date
											 select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyFinishing = c.Quantity, Size = b.SizeName };
			var QueryFinishingOut = from a in garmentFinishingOutRepository.Query
									join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									where a.FinishingTo == "GUDANG JADI" && a.UnitId == request.unit && a.FinishingOutDate <= date && a.IsDifferentSize == false
									select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyFinishing = b.Quantity, Size = b.SizeName };


			var queryNow = QueryCuttingOut.Union(QueryLoading).Union(QuerySewingOutIsDifSize).Union(QuerySewingOut).Union(QueryFinishingOut).Union(QueryFinishingOutisDifSize).AsEnumerable();

			var querySum = queryNow.GroupBy(x => new { x.Size, x.Ro, x.Article, x.BuyerCode, x.Comodity, x.QtyOrder }, (key, group) => new
			{
				ro = key.Ro,
				article = key.Article,
				buyer = key.BuyerCode,
				comodity = key.Comodity,
				qtyOrder = key.QtyOrder,
				qtycutting = group.Sum(s => s.QtyCutting),
				qtySewing = group.Sum(s => s.QtySewing),
				qtyLoading = group.Sum(s => s.QtyLoading),
				qtyFinishing = group.Sum(s => s.QtyFinishing),
				size = key.Size,
			}).OrderBy(s => s.ro);


			GarmentMonitoringProductionFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringProductionFlowListViewModel();
			List<GarmentMonitoringProductionFlowDto> monitoringDtos = new List<GarmentMonitoringProductionFlowDto>();
			if (request.ro == null)
			{
				foreach (var item in querySum)
				{
					GarmentMonitoringProductionFlowDto garmentMonitoringDto = new GarmentMonitoringProductionFlowDto()
					{
						Article = item.article,
						Ro = item.ro,
						BuyerCode = item.buyer,
						QtyOrder = item.qtyOrder,
						QtyCutting = item.qtycutting,
						QtySewing = item.qtySewing,
						QtyFinishing = item.qtyFinishing,
						QtyLoading = item.qtyLoading,
						Size = item.size,
						Comodity = item.comodity,
						Wip = item.qtycutting - item.qtyFinishing
					};
					monitoringDtos.Add(garmentMonitoringDto);
				}
			}
			else
			{
				foreach (var item in querySum.Where(s => s.ro == request.ro))
				{
					GarmentMonitoringProductionFlowDto garmentMonitoringDto = new GarmentMonitoringProductionFlowDto()
					{
						Article = item.article,
						Ro = item.ro,
						BuyerCode = item.buyer,
						QtyOrder = item.qtyOrder,
						QtyCutting = item.qtycutting,
						QtySewing = item.qtySewing,
						QtyFinishing = item.qtyFinishing,
						QtyLoading = item.qtyLoading,
						Size = item.size,
						Comodity = item.comodity,
						Wip = item.qtycutting - item.qtyFinishing
					};
					monitoringDtos.Add(garmentMonitoringDto);
				}

			}
			garmentMonitoringProductionFlow.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();

			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Buyer", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Ukuran", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Potong", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Loading", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Sewing", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Finishing", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Dalam Proses", DataType = typeof(double) });
			if (garmentMonitoringProductionFlow.garmentMonitorings.Count > 0)
			{
				foreach (var report in garmentMonitoringProductionFlow.garmentMonitorings)
					reportDataTable.Rows.Add(report.Ro, report.BuyerCode , report.Article , report.Comodity, report.QtyOrder, report.Size, report.QtyCutting , report.QtyLoading , report.QtySewing ,report.QtyFinishing, report.Wip);

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
