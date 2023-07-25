using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleShrinkagePanels
{
    public class GarmentServiceSampleShrinkagePanelDto
    {
        public GarmentServiceSampleShrinkagePanelDto(GarmentServiceSampleShrinkagePanel garmentServiceSampleShrinkagePanelList)
        {
            Id = garmentServiceSampleShrinkagePanelList.Identity;
            ServiceSubconShrinkagePanelNo = garmentServiceSampleShrinkagePanelList.ServiceSampleShrinkagePanelNo;
            ServiceSubconShrinkagePanelDate = garmentServiceSampleShrinkagePanelList.ServiceSampleShrinkagePanelDate;
            Remark = garmentServiceSampleShrinkagePanelList.Remark;
            IsUsed = garmentServiceSampleShrinkagePanelList.IsUsed;
            QtyPacking = garmentServiceSampleShrinkagePanelList.QtyPacking;
            UomUnit = garmentServiceSampleShrinkagePanelList.UomUnit;
            NettWeight = garmentServiceSampleShrinkagePanelList.NettWeight;
            GrossWeight = garmentServiceSampleShrinkagePanelList.GrossWeight;
            Items = new List<GarmentServiceSampleShrinkagePanelItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset ServiceSubconShrinkagePanelDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public virtual List<GarmentServiceSampleShrinkagePanelItemDto> Items { get; internal set; }
    }
}
