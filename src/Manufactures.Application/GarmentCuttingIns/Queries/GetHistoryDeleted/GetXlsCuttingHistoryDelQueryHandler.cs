using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Domain.Queries;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using OfficeOpenXml;

namespace Manufactures.Application.GarmentCuttingIns.Queries.GetHistoryDeleted
{
	public class GetXlsCuttingHistoryDelQueryHandler : IQueryHandler<GetXlsCuttingHistoryDelQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;

		public GetXlsCuttingHistoryDelQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();

			_http = serviceProvider.GetService<IHttpClientService>();
		}

        public async Task<MemoryStream> Handle(GetXlsCuttingHistoryDelQuery request, CancellationToken cancellationToken)
        {
			DateTime? dateFrom = request.dateFrom;
			DateTime? dateTo = request.dateTo;

			DateTime d1 = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
			DateTime d2 = dateTo == null ? DateTime.Now : (DateTime)dateTo;

			var query = from aa in garmentCuttingInRepository.Query.IgnoreQueryFilters()
						join bb in garmentCuttingInItemRepository.Query.IgnoreQueryFilters() on aa.Identity equals bb.CutInId
						join cc in garmentCuttingInDetailRepository.Query.IgnoreQueryFilters() on bb.Identity equals cc.CutInItemId
						where
						 aa.Deleted == true &&
						 aa.DeletedDate.Value.Date >= d1.Date
						&& aa.DeletedDate.Value.Date <= d2.Date
						select new
						{
							RO = aa.RONo,
							CuttingInDate = aa.CuttingInDate,
							CuttingInNo = aa.CutInNo,
							CutInDate = aa.CuttingInDate,
							CuttingInType = aa.CuttingType,
							CuttingFrom = aa.CuttingFrom,
							ProductCode = cc.ProductCode,
							PreparingQuantity = cc.PreparingQuantity,
							PreparingUomUnit = cc.PreparingUomUnit,
							CuttingInQuantity = cc.CuttingInQuantity,
							RemainingQuantity = cc.RemainingQuantity,
							CuttingInUomUnit = cc.CuttingInUomUnit,
							BasicPrice = cc.BasicPrice,
							Unit = aa.UnitName,
							FC = aa.FC,
							DeletedBy = cc.DeletedBy,
							DeletedDate = cc.DeletedDate
						};


			GarmentMonCuttingHistoryDelViewModel garmentMonCuttingHistoryViewModel = new GarmentMonCuttingHistoryDelViewModel();
			List<GarmentMonCuttingHistoryDelDto> monPreHistoryDelDtos = new List<GarmentMonCuttingHistoryDelDto>();
			//List<monPreHistoryDelViewTemp> ShowUp = new List<monPreHistoryDelViewTemp>();
			foreach (var item in query)
			{
				GarmentMonCuttingHistoryDelDto delDto = new GarmentMonCuttingHistoryDelDto()
				{

					RO = item.RO,
					cuttingInQty = item.CuttingInQuantity,
					cuttingFrom = item.CuttingFrom,
					cuttingInType = item.CuttingInType,
					cuttingInDate = item.CutInDate,
					cuttingInNo = item.CuttingInNo,
					remainingQty = item.RemainingQuantity,
					itemCode = item.ProductCode,
					unit = item.Unit,
					deletedBy = item.DeletedBy,
					deletedDate = item.DeletedDate,
					fc = item.FC,
					preparingQty = item.PreparingQuantity,
					uomUnitCuttingIn = item.CuttingInUomUnit,
					uomUnitsPrep = item.PreparingUomUnit,
					basicPrice = item.BasicPrice

				};
				monPreHistoryDelDtos.Add(delDto);
			}
			garmentMonCuttingHistoryViewModel.garmentMonitorings = monPreHistoryDelDtos;
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NOMOR", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "USER DELETE", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL DELETE", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO CUTTING IN", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL CUTTING IN", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "UNIT", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TIPE CUTTING", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "ASAL CUTTING", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO RO", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KODE BARANG", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "JUMLAH PREPARING OUT", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SATUAN", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "JUMLAH POTONG", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SISA", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SATUAN POTONG", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HARGA", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FC", DataType = typeof(double) });

			int counter = 1;
			//if (garmentMonPreHistoryDelViewModel.garmentMonitorings.Count > 0 || garmentMonPreHistoryDelViewModel.garmentMonitorings != null)
			if (garmentMonCuttingHistoryViewModel.garmentMonitorings != null)
			{
				foreach (var report in garmentMonCuttingHistoryViewModel.garmentMonitorings)
				{
					reportDataTable.Rows.Add(counter, report.deletedBy, report.deletedDate, report.cuttingInNo,report.cuttingInDate,report.unit,report.cuttingInType,report.cuttingFrom,report.RO,report.itemCode,report.preparingQty,
						
						report.uomUnitsPrep,report.cuttingInQty, report.remainingQty, report.uomUnitCuttingIn,report.basicPrice,report.fc);
					counter++;
				}
			}
			else
			{
				reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "", "",0,"", 0, 0, "", 0, 0);

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
