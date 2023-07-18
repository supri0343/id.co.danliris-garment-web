using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleShrinkagePanels
{
    public class GarmentServiceSampleShrinkagePanelListDto : BaseDto
    {
        public GarmentServiceSampleShrinkagePanelListDto(GarmentServiceSampleShrinkagePanel garmentServiceSampleShrinkagePanelList)
        {
            Id = garmentServiceSampleShrinkagePanelList.Identity;
            ServiceSampleShrinkagePanelNo = garmentServiceSampleShrinkagePanelList.ServiceSampleShrinkagePanelNo;
            ServiceSampleShrinkagePanelDate = garmentServiceSampleShrinkagePanelList.ServiceSampleShrinkagePanelDate;
            Remark = garmentServiceSampleShrinkagePanelList.Remark;
            CreatedBy = garmentServiceSampleShrinkagePanelList.AuditTrail.CreatedBy;
            IsUsed = garmentServiceSampleShrinkagePanelList.IsUsed;
            QtyPacking = garmentServiceSampleShrinkagePanelList.QtyPacking;
            UomUnit = garmentServiceSampleShrinkagePanelList.UomUnit;
            Items = new List<GarmentServiceSampleShrinkagePanelItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSampleShrinkagePanelNo { get; set; }
        public DateTimeOffset ServiceSampleShrinkagePanelDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public List<GarmentServiceSampleShrinkagePanelItemDto> Items { get; set; }
    }
}
