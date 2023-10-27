using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetMonitoringDeleteHistory
{
	public class GarmentMonCuttingOutHistoryDelDto
	{
		public GarmentMonCuttingOutHistoryDelDto()
		{
		}
		public Guid Id { get; set; }
		public string deletedBy { get; set; }
		public string RO { get; set; }
		public string unit { get; set; }
		public DateTimeOffset? deletedDate { get; set; }
		public DateTimeOffset? cuttingOutDate { get; set; }
		public string cutOutNo { get; set; }
		public string comodityCode { get; set; }
		public double totalCuttingOut { get; set; }
		public string itemCode { get; set; }
		public string SizeName { get; set; }
		public double cuttingOutQty { get; set; }
		public string cuttingOutUomUnit { get; set; }
		public string color { get; set; }
		public GarmentMonCuttingOutHistoryDelDto(GarmentMonCuttingOutHistoryDelDto garmentMonitoringPrepareHistoryDelDto)
		{
			Id = garmentMonitoringPrepareHistoryDelDto.Id;
			deletedBy = garmentMonitoringPrepareHistoryDelDto.deletedBy;
			unit = garmentMonitoringPrepareHistoryDelDto.unit;
			RO = garmentMonitoringPrepareHistoryDelDto.RO;
			cuttingOutDate = garmentMonitoringPrepareHistoryDelDto.cuttingOutDate;
			deletedDate = garmentMonitoringPrepareHistoryDelDto.deletedDate;
			cutOutNo = garmentMonitoringPrepareHistoryDelDto.cutOutNo;
			cuttingOutUomUnit = garmentMonitoringPrepareHistoryDelDto.cuttingOutUomUnit;
			totalCuttingOut = garmentMonitoringPrepareHistoryDelDto.totalCuttingOut;
			itemCode = garmentMonitoringPrepareHistoryDelDto.itemCode;
			unit = garmentMonitoringPrepareHistoryDelDto.unit;
			color = garmentMonitoringPrepareHistoryDelDto.color;
			comodityCode = garmentMonitoringPrepareHistoryDelDto.comodityCode;
			cuttingOutUomUnit = garmentMonitoringPrepareHistoryDelDto.cuttingOutUomUnit;
			color = garmentMonitoringPrepareHistoryDelDto.color;
			SizeName = garmentMonitoringPrepareHistoryDelDto.SizeName;
			
		}
	}
}