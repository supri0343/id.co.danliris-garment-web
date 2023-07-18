using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleFabricWashes
{
    public class GarmentServiceSampleFabricWashItemDto : BaseDto
    {
        public GarmentServiceSampleFabricWashItemDto(GarmentServiceSampleFabricWashItem garmentServiceSampleFabricWashItem)
        {
            Id = garmentServiceSampleFabricWashItem.Identity;
            ServiceSampleFabricWashId = garmentServiceSampleFabricWashItem.ServiceSampleFabricWashId;
            UnitExpenditureNo = garmentServiceSampleFabricWashItem.UnitExpenditureNo;
            ExpenditureDate = garmentServiceSampleFabricWashItem.ExpenditureDate;
            UnitSender = new UnitSender(garmentServiceSampleFabricWashItem.UnitSenderId.Value, garmentServiceSampleFabricWashItem.UnitSenderCode, garmentServiceSampleFabricWashItem.UnitSenderName);
            UnitRequest = new UnitRequest(garmentServiceSampleFabricWashItem.UnitRequestId.Value, garmentServiceSampleFabricWashItem.UnitRequestCode, garmentServiceSampleFabricWashItem.UnitRequestName);
            Details = new List<GarmentServiceSampleFabricWashDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleFabricWashId { get; set; }
        public string UnitExpenditureNo { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public UnitSender UnitSender { get; set; }
        public UnitRequest UnitRequest { get; set; }
        public virtual List<GarmentServiceSampleFabricWashDetailDto> Details { get; internal set; }
    }
}
