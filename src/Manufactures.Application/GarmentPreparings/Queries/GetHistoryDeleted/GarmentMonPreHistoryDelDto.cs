using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeleted
{
    public class GarmentMonPreHistoryDelDto
    {
		public GarmentMonPreHistoryDelDto()
		{
		}
		public Guid Id { get; set; }
		public string deletedBy { get; set; }
		public string buyer { get; set; }
		public string RO { get; set; }
		public string articles { get; set; }
		public int detailExpend { get; set; }
		public DateTimeOffset? processDates { get; set; }
		public string uenNO { get; set; }
		public string unit { get; set; }
		public DateTimeOffset? deletedDate { get; set; }
		public string buyesNamed { get; set; }
		public string produkCodes { get; set; }
		public double kuantityes { get; set; }
		public string uomUnits { get; set; }
		public double basicPrieces { get; set; }
		public GarmentMonPreHistoryDelDto(GarmentMonPreHistoryDelDto garmentMonitoringPrepareHistoryDelDto)
		{
			Id = garmentMonitoringPrepareHistoryDelDto.Id;
			deletedBy = garmentMonitoringPrepareHistoryDelDto.deletedBy;
			buyer = garmentMonitoringPrepareHistoryDelDto.buyer;
			RO = garmentMonitoringPrepareHistoryDelDto.RO;
			articles = garmentMonitoringPrepareHistoryDelDto.articles;
			detailExpend = garmentMonitoringPrepareHistoryDelDto.detailExpend;
			processDates = garmentMonitoringPrepareHistoryDelDto.processDates;
			uenNO = garmentMonitoringPrepareHistoryDelDto.uenNO;
			unit = garmentMonitoringPrepareHistoryDelDto.unit;
			deletedDate = garmentMonitoringPrepareHistoryDelDto.deletedDate;
			buyesNamed = garmentMonitoringPrepareHistoryDelDto.buyesNamed;
			produkCodes = garmentMonitoringPrepareHistoryDelDto.produkCodes;
			kuantityes = garmentMonitoringPrepareHistoryDelDto.kuantityes;
			uomUnits = garmentMonitoringPrepareHistoryDelDto.uomUnits;
			basicPrieces = garmentMonitoringPrepareHistoryDelDto.basicPrieces;
		}
	}
}
