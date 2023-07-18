using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleShrinkagePanels
{
    public class GarmentServiceSampleShrinkagePanelItemDto : BaseDto
    {
        public GarmentServiceSampleShrinkagePanelItemDto(GarmentServiceSampleShrinkagePanelItem garmentServiceSampleShrinkagePanelItem)
        {
            Id = garmentServiceSampleShrinkagePanelItem.Identity;
            ServiceSampleShrinkagePanelId = garmentServiceSampleShrinkagePanelItem.ServiceSampleShrinkagePanelId;
            UnitExpenditureNo = garmentServiceSampleShrinkagePanelItem.UnitExpenditureNo;
            ExpenditureDate = garmentServiceSampleShrinkagePanelItem.ExpenditureDate;
            UnitSender = new UnitSender(garmentServiceSampleShrinkagePanelItem.UnitSenderId.Value, garmentServiceSampleShrinkagePanelItem.UnitSenderCode, garmentServiceSampleShrinkagePanelItem.UnitSenderName);
            UnitRequest = new UnitRequest(garmentServiceSampleShrinkagePanelItem.UnitRequestId.Value, garmentServiceSampleShrinkagePanelItem.UnitRequestCode, garmentServiceSampleShrinkagePanelItem.UnitRequestName);
            Details = new List<GarmentServiceSampleShrinkagePanelDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleShrinkagePanelId { get; set; }
        public string UnitExpenditureNo { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public UnitSender UnitSender { get; set; }
        public UnitRequest UnitRequest { get; set; }
        public virtual List<GarmentServiceSampleShrinkagePanelDetailDto> Details { get; internal set; }
    }
}
