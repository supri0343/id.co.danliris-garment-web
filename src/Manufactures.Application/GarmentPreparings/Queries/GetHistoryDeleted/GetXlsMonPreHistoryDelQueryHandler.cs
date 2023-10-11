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
using OfficeOpenXml.Style;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeleted;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeleted
{
	public class GetXlsMonPreHistoryDelQueryHandler : IQueryHandler<GetXlsMonPreHistoryDelQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;

		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;

		public GetXlsMonPreHistoryDelQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}
		public async Task<MemoryStream> Handle(GetXlsMonPreHistoryDelQuery request, CancellationToken cancellationToken)
		{
			DateTime? dateFrom = request.dateFrom;
			DateTime? dateTo = request.dateTo;

			DateTime d1 = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
			DateTime d2 = dateTo == null ? DateTime.Now : (DateTime)dateTo;

			var monpreHisDelQuery = from a in (from aa in garmentPreparingRepository.Query.IgnoreQueryFilters()
											   where
												//aa.UnitId == request.unit 
												aa.ProcessDate.Value.AddHours(7).Date >= d1
											   && aa.ProcessDate.Value.AddHours(7).Date <= d2

										 && aa.Deleted == true
											   select new
											   {
												   aa.Identity,
												   aa.Article,
												   aa.BuyerCode,
												   aa.RONo,
												   aa.ProcessDate,
												   aa.UnitName,
												   aa.DeletedBy,
												   aa.DeletedDate,
												   aa.BuyerName,
												   aa.UENNo
											   })
									join b in garmentPreparingItemRepository.Query.IgnoreQueryFilters() on a.Identity equals b.GarmentPreparingId
									select new
									{
										buyer = a.BuyerCode,
										RO = a.RONo,
										articles = a.Article,
										Id = a.Identity,
										detailExpend = b.UENItemId,
										processDates = a.ProcessDate,

										uenNO = a.UENNo,
										unit = a.UnitName,
										deletedBy = a.DeletedBy,
										deletedDate = a.DeletedDate,
										buyesNamed = a.BuyerName,
										produkCodes = b.ProductCode,
										kuantityes = b.Quantity,
										uomUnits = b.UomUnit,
										basicPrieces = b.BasicPrice
									};

			GarmentMonPreHistoryDelViewModel garmentMonPreHistoryDelViewModel = new GarmentMonPreHistoryDelViewModel();
			List<GarmentMonPreHistoryDelDto> monPreHistoryDelDtos = new List<GarmentMonPreHistoryDelDto>();
			//List<monPreHistoryDelViewTemp> ShowUp = new List<monPreHistoryDelViewTemp>();
			foreach (var item in monpreHisDelQuery)
			{
				GarmentMonPreHistoryDelDto garmentMonPreHistoryDelDto = new GarmentMonPreHistoryDelDto()
				{

					buyer = item.buyer,
					articles = item.articles,
					Id = item.Id,
					detailExpend = item.detailExpend,

					processDates = item.processDates,
					RO = item.RO,
					uenNO = item.uenNO,
					unit = item.unit,
					deletedBy = item.deletedBy,
					deletedDate = item.deletedDate,
					buyesNamed = item.buyesNamed,
					produkCodes = item.produkCodes,
					kuantityes = item.kuantityes,
					uomUnits = item.uomUnits,
					basicPrieces = item.basicPrieces

				};
				monPreHistoryDelDtos.Add(garmentMonPreHistoryDelDto);
				garmentMonPreHistoryDelViewModel.garmentMonitorings = monPreHistoryDelDtos;
			}
				var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NOMOR", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "USER DELETE", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL DELETE", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "UNIT", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL PREPARING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO BUK", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BUYER", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KODE BARANG", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "JUMLAH", DataType = typeof(double) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SATUAN", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HARGA", DataType = typeof(double) });
				int counter = 1;
				//if (garmentMonPreHistoryDelViewModel.garmentMonitorings.Count > 0 || garmentMonPreHistoryDelViewModel.garmentMonitorings != null)
				if (garmentMonPreHistoryDelViewModel.garmentMonitorings != null)
				{
					foreach (var report in garmentMonPreHistoryDelViewModel.garmentMonitorings)
					{
						reportDataTable.Rows.Add(counter,report.deletedBy, report.deletedDate, report.unit, report.processDates, report.uenNO, report.RO, report.buyesNamed, report.produkCodes, report.kuantityes, report.uomUnits, report.basicPrieces);
						counter++;
					}
				}
            else
            {
				reportDataTable.Rows.Add("", "", "","", "","", "", "", "", 0, "", 0);
				
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
