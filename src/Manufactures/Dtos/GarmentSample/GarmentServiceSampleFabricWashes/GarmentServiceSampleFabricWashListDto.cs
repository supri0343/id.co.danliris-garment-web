using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleFabricWashes
{
    public class GarmentServiceSampleFabricWashListDto : BaseDto
    {
        public GarmentServiceSampleFabricWashListDto(GarmentServiceSampleFabricWash garmentServiceSampleFabricWashList)
        {
            Id = garmentServiceSampleFabricWashList.Identity;
            ServiceSampleFabricWashNo = garmentServiceSampleFabricWashList.ServiceSampleFabricWashNo;
            ServiceSampleFabricWashDate = garmentServiceSampleFabricWashList.ServiceSampleFabricWashDate;
            CreatedBy = garmentServiceSampleFabricWashList.AuditTrail.CreatedBy;
            IsUsed = garmentServiceSampleFabricWashList.IsUsed;
            Items = new List<GarmentServiceSampleFabricWashItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSampleFabricWashNo { get; set; }
        public DateTimeOffset ServiceSampleFabricWashDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSampleFabricWashItemDto> Items { get; set; }
    }
}
