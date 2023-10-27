using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetMonitoringDeleteHistory
{
	public class GetXlsCuttingOutHistoryDelQueryHandler : IQueryHandler<GetXlsCuttingOutHistoryDelQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;

		public GetXlsCuttingOutHistoryDelQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();

			_http = serviceProvider.GetService<IHttpClientService>();
		}

        public async Task<MemoryStream> Handle(GetXlsCuttingOutHistoryDelQuery request, CancellationToken cancellationToken)
        {
			DateTime? dateFrom = request.dateFrom;
			DateTime? dateTo = request.dateTo;

			DateTime d1 = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
			DateTime d2 = dateTo == null ? DateTime.Now : (DateTime)dateTo;

			var query = from aa in garmentCuttingOutRepository.Query.IgnoreQueryFilters()
						join bb in garmentCuttingOutItemRepository.Query.IgnoreQueryFilters() on aa.Identity equals bb.CutOutId
						join cc in garmentCuttingOutDetailRepository.Query.IgnoreQueryFilters() on bb.Identity equals cc.CutOutItemId
						where
						 aa.Deleted == true &&
						 aa.DeletedDate.Value.Date >= d1.Date
						&& aa.DeletedDate.Value.Date <= d2.Date
						select new
						{
							RO = aa.RONo,
							CuttingOutDate = aa.CuttingOutDate,
							CuttingOutNo = aa.CutOutNo,
							Comodity = aa.ComodityName,
							ProductCode = bb.ProductCode,
							TotalCuttingOut = bb.TotalCuttingOut,
							Color = cc.Color,
							SizeName = cc.SizeName,
							CuttingOutQty = cc.CuttingOutQuantity,
							CuttingOutUomUnit = cc.CuttingOutUomUnit,
							BasicPrice = cc.BasicPrice,
							Unit = aa.UnitName,
							DeletedBy = cc.DeletedBy,
							DeletedDate = cc.DeletedDate,
							Id = aa.Identity
						};


			GarmentMonCuttingOutHistoryDelViewModel garmentMonCuttingHistoryViewModel = new GarmentMonCuttingOutHistoryDelViewModel();
			List<GarmentMonCuttingOutHistoryDelDto> monPreHistoryDelDtos = new List<GarmentMonCuttingOutHistoryDelDto>();
			//List<monPreHistoryDelViewTemp> ShowUp = new List<monPreHistoryDelViewTemp>();
			foreach (var item in query)
			{
				GarmentMonCuttingOutHistoryDelDto delDto = new GarmentMonCuttingOutHistoryDelDto()
				{

					RO = item.RO,
					cutOutNo = item.CuttingOutNo,
					cuttingOutDate = item.CuttingOutDate,
					comodityCode = item.Comodity,
					SizeName = item.SizeName,
					totalCuttingOut = item.TotalCuttingOut,
					cuttingOutUomUnit = item.CuttingOutUomUnit,
					itemCode = item.ProductCode,
					unit = item.Unit,
					deletedBy = item.DeletedBy,
					deletedDate = item.DeletedDate,
					color = item.Color,
					cuttingOutQty = item.CuttingOutQty,
					Id = item.Id

				};
				monPreHistoryDelDtos.Add(delDto);
			}
			garmentMonCuttingHistoryViewModel.garmentMonitorings = monPreHistoryDelDtos;
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NOMOR", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "USER DELETE", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL DELETE", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO CUTTING OUT", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL CUTTING OUT", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "UNIT", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO RO", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KOMODITI", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KODE BARANG", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "JUMLAH", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "UKURAN", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "JUMLAH POTONG", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SATUAN POTONG", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "WARNA", DataType = typeof(string) });
		

			int counter = 1;
			//if (garmentMonPreHistoryDelViewModel.garmentMonitorings.Count > 0 || garmentMonPreHistoryDelViewModel.garmentMonitorings != null)
			if (garmentMonCuttingHistoryViewModel.garmentMonitorings != null)
			{
				foreach (var report in garmentMonCuttingHistoryViewModel.garmentMonitorings)
				{
					reportDataTable.Rows.Add(counter, report.deletedBy, report.deletedDate, report.cutOutNo, report.cuttingOutDate, report.unit, report.RO, report.comodityCode, report.itemCode, report.totalCuttingOut,

						report.SizeName, report.cuttingOutQty, report.cuttingOutUomUnit, report.color);
					counter++;
				}
			}
			else
			{
				reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "", 0, "", 0, "", 0);

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
