using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;

using Manufactures.Domain.GarmentPreparings.ReadModels;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.ExpenditureROResult;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;

namespace Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeletedLoadings
{
    public class GetXlsMonLoadHistoryDelQueryHandler : IQueryHandler<GetXlsMonLoadHistoryDelQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;

		public GetXlsMonLoadHistoryDelQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}
		public async Task<MemoryStream> Handle(GetXlsMonLoadHistoryDelQuery request, CancellationToken cancellationToken)
		{
			DateTime? dateFrom = request.dateFrom;
			DateTime? dateTo = request.dateTo;

			DateTime d1 = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
			DateTime d2 = dateTo == null ? DateTime.Now : (DateTime)dateTo;

			var monLoadsHisDelQuery = from a in (from aa in garmentLoadingRepository.Query.IgnoreQueryFilters()
												 where
												  //aa.UnitId == request.unit 
												  aa.DeletedDate.Value.AddHours(7).Date >= d1.Date
												 && aa.DeletedDate.Value.AddHours(7).Date <= d2.Date
												   //aa.LoadingDate.Value.AddHours(7).Date >= d1
												   //  && aa.LoadingDate.Value.AddHours(7).Date <= d2
												   // && aa.Deleted == false
												   && aa.Deleted == true
												 select new
												 {
													 aa.Identity,
													 aa.Article,
													 aa.RONo,
													 aa.UnitName,
													 aa.DeletedBy,
													 aa.DeletedDate,
													 aa.LoadingNo,
													 aa.LoadingDate,
													 aa.SewingDONo,
													 aa.ComodityName

												 })
									  join b in garmentLoadingItemRepository.Query.IgnoreQueryFilters() on a.Identity equals b.LoadingId
									  select new
									  {
										  deletedBys = a.DeletedBy, // DeletedBy
										  deletedDates = a.DeletedDate, // DeletedDate
										  loadingNos = a.LoadingNo, // LoadingNo
										  loadingDates = a.LoadingDate, // LoadingDate
										  unitNames = a.UnitName, // UnitName
										  roNos = a.RONo, // RONo
										  sewingDoNos = a.SewingDONo, // SewingDONo
										  commodityNames = a.ComodityName, // ComodityName
										  productCodes = b.ProductCode, // ProductCode
										  sizeNames = b.SizeName, // SizeName
										  quantities = b.Quantity, // Quantity
										  colors = b.Color// Color
									  };

			GarmentMonLoadHistoryDelListViewModel garmentMonLoadHistoryDelListViewModel = new GarmentMonLoadHistoryDelListViewModel();
			List<GarmentMonLoadHistoryDelDto> monLoadsHisDelDtos = new List<GarmentMonLoadHistoryDelDto>();
			foreach (var item in monLoadsHisDelQuery)
			{
				GarmentMonLoadHistoryDelDto garmentMonLoadHistoryDelDto = new GarmentMonLoadHistoryDelDto()
				{
					deletedBys = item.deletedBys,
					deletedDates = item.deletedDates,
					loadingNos = item.loadingNos,
					loadingDates = item.loadingDates,
					unitNames = item.unitNames,
					roNos = item.roNos,
					sewingDoNos = item.sewingDoNos,
					commodityNames = item.commodityNames,
					productCodes = item.productCodes,
					sizeNames = item.sizeNames,
					quantities = item.quantities,
					colors = item.colors
				};
				monLoadsHisDelDtos.Add(garmentMonLoadHistoryDelDto);
				garmentMonLoadHistoryDelListViewModel.garmentMonitorings = monLoadsHisDelDtos;
			}
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NOMOR", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "USER DELETE", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL DELETE", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO LOADING", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL LOADING", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "UNIT", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO RO", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO SEWING DO", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KOMODITI", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KODE BARANG", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SIZE", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "JUMLAH SATUAN", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "WARNA", DataType = typeof(string) });
			int counter = 1;
			if (garmentMonLoadHistoryDelListViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in garmentMonLoadHistoryDelListViewModel.garmentMonitorings)
				{
					reportDataTable.Rows.Add(counter,
						report.deletedBys, // DeletedBy
						report.deletedDates, // DeletedDate
						report.loadingNos, // LoadingNo
						report.loadingDates, // LoadingDate
						report.unitNames, // UnitName
						report.roNos, // RONo
						report.sewingDoNos, // SewingDONo
						report.commodityNames, // ComodityName
						report.productCodes, // ProductCode
						report.sizeNames, // SizeName
						report.quantities, // Quantity
						report.colors // Color
						);
					counter++;
				}
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

				worksheet.Cells["A2"].LoadFromDataTable(reportDataTable, true, OfficeOpenXml.Table.TableStyles.Light16);

				var stream = new MemoryStream();

				package.SaveAs(stream);

				return stream; // Mengembalikan stream setelah semua pekerjaan selesai.
			}
		}
	}
}
