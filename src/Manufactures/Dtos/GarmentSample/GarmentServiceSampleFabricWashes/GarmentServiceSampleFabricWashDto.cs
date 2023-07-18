using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleFabricWashes
{
    public class GarmentServiceSampleFabricWashDto
    {
        public GarmentServiceSampleFabricWashDto(GarmentServiceSampleFabricWash garmentServiceSampleFabricWashList)
        {
            Id = garmentServiceSampleFabricWashList.Identity;
            ServiceSampleFabricWashNo = garmentServiceSampleFabricWashList.ServiceSampleFabricWashNo;
            ServiceSampleFabricWashDate = garmentServiceSampleFabricWashList.ServiceSampleFabricWashDate;
            Remark = garmentServiceSampleFabricWashList.Remark;
            IsUsed = garmentServiceSampleFabricWashList.IsUsed;
            QtyPacking = garmentServiceSampleFabricWashList.QtyPacking;
            UomUnit = garmentServiceSampleFabricWashList.UomUnit;
            NettWeight = garmentServiceSampleFabricWashList.NettWeight;
            GrossWeight = garmentServiceSampleFabricWashList.GrossWeight;
            Items = new List<GarmentServiceSampleFabricWashItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSampleFabricWashNo { get; set; }
        public DateTimeOffset ServiceSampleFabricWashDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public virtual List<GarmentServiceSampleFabricWashItemDto> Items { get; internal set; }
    }
}
