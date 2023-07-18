using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.Queries.GarmentSampleGarmentWashReport
{
    public class GarmentSampleGarmentWashReportDto
    {
        public GarmentSampleGarmentWashReportDto()
        {
        }

        public Guid SSCSId { get; internal set; }
        public string SSCSNo { get; internal set; }
        public DateTimeOffset SSCSDate { get; internal set; }

        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }

        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }

        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }

        public string DesignColour { get; internal set; }

        public double Quantity { get; internal set; }

        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
    
        public GarmentSampleGarmentWashReportDto(GarmentSampleGarmentWashReportDto garmentSampleGarmentWashReportDto)
        {
            SSCSId = garmentSampleGarmentWashReportDto.SSCSId;
            SSCSNo = garmentSampleGarmentWashReportDto.SSCSNo;
            SSCSDate = garmentSampleGarmentWashReportDto.SSCSDate;
            BuyerId = garmentSampleGarmentWashReportDto.BuyerId;
            BuyerCode = garmentSampleGarmentWashReportDto.BuyerCode;
            BuyerName = garmentSampleGarmentWashReportDto.BuyerName;
            ComodityId = garmentSampleGarmentWashReportDto.ComodityId;
            ComodityCode = garmentSampleGarmentWashReportDto.ComodityCode;
            ComodityName = garmentSampleGarmentWashReportDto.ComodityName;
            UnitId = garmentSampleGarmentWashReportDto.UnitId;
            UnitCode = garmentSampleGarmentWashReportDto.UnitCode;
            UnitName = garmentSampleGarmentWashReportDto.UnitName;
            DesignColour = garmentSampleGarmentWashReportDto.DesignColour;
            Quantity = garmentSampleGarmentWashReportDto.Quantity;
            UomId = garmentSampleGarmentWashReportDto.UomId;
            UomUnit = garmentSampleGarmentWashReportDto.UomUnit;          
        }
    }
}
