using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.Queries
{
    public class ServiceSampleFabricWashDto
    {
        public ServiceSampleFabricWashDto()
        {
        }

        public string serviceSampleFabricWashNo { get; internal set; }
        public DateTimeOffset serviceSampleFabricWashDate { get; internal set; }
        public string unitExpenditureNo { get; internal set; }
        public DateTimeOffset expendituredate { get; internal set; }
        public string unitSenderCode { get; internal set; }
        public string unitSenderName { get; internal set; }
        public string productCode { get; internal set; }
        public string productName { get; internal set; }
        public string productRemark { get; internal set; }
        public string designcolor { get; internal set; }
        public decimal quantity { get; internal set; }
        public string uomUnit { get; internal set; }
        public ServiceSampleFabricWashDto(ServiceSampleFabricWashDto serviceSampleFabricWashDto)
        {
            serviceSampleFabricWashNo = serviceSampleFabricWashDto.serviceSampleFabricWashNo;
            serviceSampleFabricWashDate = serviceSampleFabricWashDto.serviceSampleFabricWashDate;
            unitExpenditureNo = serviceSampleFabricWashDto.unitExpenditureNo;
            expendituredate = serviceSampleFabricWashDto.expendituredate;
            unitSenderCode = serviceSampleFabricWashDto.unitSenderCode;
            unitSenderName = serviceSampleFabricWashDto.unitSenderName;
            productCode = serviceSampleFabricWashDto.productCode;
            productName = serviceSampleFabricWashDto.productName;
            productRemark = serviceSampleFabricWashDto.productRemark;
            designcolor = serviceSampleFabricWashDto.designcolor;
            quantity = serviceSampleFabricWashDto.quantity;
            uomUnit = serviceSampleFabricWashDto.uomUnit;
        }
    }
}
