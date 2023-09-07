using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconMonitoringOut
{
    public class GarmentSubconMonitoringOutDto
    {
        public GarmentSubconMonitoringOutDto()
        { 
        }
        public string bcNoOut { get; internal set; }
        public DateTimeOffset bcDateOut { get; internal set; }
        public double quantityOut { get; internal set; }
        public string uomOut { get; internal set; }
        public string jobType { get; internal set; }
        public string subconNo { get; internal set; }
        public string bpjNo { get; internal set; }
        public DateTimeOffset dueDate { get; internal set; }

        public string bonNo { get; internal set; }
        public string gamentDONo { get; internal set; }
        public double subconContractQuantity { get; internal set; }
        public string roNo { get; internal set; }
        //public DateTimeOffset bcDateIn { get; internal set; }
        //public string quantityIn { get; internal set; }
        //public string uomIn { get; internal set; }

        public GarmentSubconMonitoringOutDto(GarmentSubconMonitoringOutDto garmentSubconMonitoringOutDto)
        {
            bcNoOut = garmentSubconMonitoringOutDto.bcNoOut;
            bcDateOut = garmentSubconMonitoringOutDto.bcDateOut;
            quantityOut = garmentSubconMonitoringOutDto.quantityOut;
            uomOut = garmentSubconMonitoringOutDto.uomOut;
            jobType = garmentSubconMonitoringOutDto.jobType;
            subconNo = garmentSubconMonitoringOutDto.subconNo;
            bonNo = garmentSubconMonitoringOutDto.bonNo;
            bpjNo = garmentSubconMonitoringOutDto.bpjNo;
            dueDate = garmentSubconMonitoringOutDto.dueDate;
            subconContractQuantity = garmentSubconMonitoringOutDto.subconContractQuantity;
            roNo = garmentSubconMonitoringOutDto.roNo;
        }
    }
}
