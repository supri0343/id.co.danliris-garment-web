using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingIns.Queries.GetHistoryDeleted
{
	public class GarmentMonCuttingHistoryDelDto
	{
		public GarmentMonCuttingHistoryDelDto()
		{
		}
		public Guid Id { get; set; }
		public string deletedBy { get; set; }
		public string RO { get; set; }
		public string unit { get; set; }
		public DateTimeOffset? deletedDate { get; set; }
		public DateTimeOffset? cuttingInDate { get; set; }
		public string cuttingInNo { get; set; }
		public string cuttingInType { get; set; }
		public string cuttingFrom { get; set; }
		public string itemCode { get; set; }
		public double preparingQty { get; set; }
		public string uomUnitsPrep { get; set; }
		public double cuttingInQty { get; set; }
		public double remainingQty { get; set; }
		public double fc { get; set; }
		public string uomUnitCuttingIn { get; set; }
		public double basicPrice { get; set; }
		public GarmentMonCuttingHistoryDelDto(GarmentMonCuttingHistoryDelDto garmentMonitoringPrepareHistoryDelDto)
		{
			Id = garmentMonitoringPrepareHistoryDelDto.Id;
			deletedBy = garmentMonitoringPrepareHistoryDelDto.deletedBy;
			unit = garmentMonitoringPrepareHistoryDelDto.unit;
			RO = garmentMonitoringPrepareHistoryDelDto.RO;
			cuttingInNo = garmentMonitoringPrepareHistoryDelDto.cuttingInNo;
			deletedDate = garmentMonitoringPrepareHistoryDelDto.deletedDate;
			cuttingInDate = garmentMonitoringPrepareHistoryDelDto.cuttingInDate;
			cuttingInType = garmentMonitoringPrepareHistoryDelDto.cuttingInType;
			cuttingFrom = garmentMonitoringPrepareHistoryDelDto.cuttingFrom;
			itemCode = garmentMonitoringPrepareHistoryDelDto.itemCode;
			unit = garmentMonitoringPrepareHistoryDelDto.unit;
			preparingQty = garmentMonitoringPrepareHistoryDelDto.preparingQty;
			uomUnitsPrep = garmentMonitoringPrepareHistoryDelDto.uomUnitsPrep;
			cuttingInQty = garmentMonitoringPrepareHistoryDelDto.cuttingInQty;
			remainingQty = garmentMonitoringPrepareHistoryDelDto.remainingQty;
			fc = garmentMonitoringPrepareHistoryDelDto.fc;
			uomUnitCuttingIn = garmentMonitoringPrepareHistoryDelDto.uomUnitCuttingIn;
			basicPrice = garmentMonitoringPrepareHistoryDelDto.basicPrice;
		}
	}
}
