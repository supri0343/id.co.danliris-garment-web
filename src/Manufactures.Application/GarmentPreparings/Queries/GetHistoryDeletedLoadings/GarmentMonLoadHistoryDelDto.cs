using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeletedLoadings
{
    public class GarmentMonLoadHistoryDelDto
    {
        public GarmentMonLoadHistoryDelDto()
        {
        }
        public Guid Id { get; set; }
        public string deletedBys { get; set; } // DeletedBy
        public DateTimeOffset? deletedDates { get; set; } // DeletedDate
        public string loadingNos { get; set; } // LoadingNo
        public DateTimeOffset? loadingDates { get; set; } // LoadingDate
        public string unitNames { get; set; } // UnitName
        public string roNos { get; set; } // RONo
        public string sewingDoNos { get; set; } // SewingDONo
        public string commodityNames { get; set; } // ComodityName

        public string productCodes { get; set; } // ProductCode
        public string sizeNames { get; set; } // SizeName
        public double quantities { get; set; } // Quantity
        public string colors { get; set; } // Color

        public GarmentMonLoadHistoryDelDto(GarmentMonLoadHistoryDelDto garmentMonitoringLoadingsHistoryDelDto)
        {
            Id = garmentMonitoringLoadingsHistoryDelDto.Id;
            deletedBys = garmentMonitoringLoadingsHistoryDelDto.deletedBys;
            deletedDates = garmentMonitoringLoadingsHistoryDelDto.deletedDates;
            loadingNos = garmentMonitoringLoadingsHistoryDelDto.loadingNos;
            loadingDates = garmentMonitoringLoadingsHistoryDelDto.loadingDates;
            unitNames = garmentMonitoringLoadingsHistoryDelDto.unitNames;
            roNos = garmentMonitoringLoadingsHistoryDelDto.roNos;
            sewingDoNos = garmentMonitoringLoadingsHistoryDelDto.sewingDoNos;
            commodityNames = garmentMonitoringLoadingsHistoryDelDto.commodityNames;
            productCodes = garmentMonitoringLoadingsHistoryDelDto.productCodes;
            sizeNames = garmentMonitoringLoadingsHistoryDelDto.sizeNames;
            quantities = garmentMonitoringLoadingsHistoryDelDto.quantities;
            colors = garmentMonitoringLoadingsHistoryDelDto.colors;
        }

    }
}
