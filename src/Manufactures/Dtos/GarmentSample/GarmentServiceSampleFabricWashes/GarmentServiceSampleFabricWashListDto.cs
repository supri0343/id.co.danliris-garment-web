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
            ServiceSubconFabricWashNo = garmentServiceSampleFabricWashList.ServiceSampleFabricWashNo;
            ServiceSubconFabricWashDate = garmentServiceSampleFabricWashList.ServiceSampleFabricWashDate;
            CreatedBy = garmentServiceSampleFabricWashList.AuditTrail.CreatedBy;
            IsUsed = garmentServiceSampleFabricWashList.IsUsed;
            Items = new List<GarmentServiceSampleFabricWashItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSubconFabricWashNo { get; set; }
        public DateTimeOffset ServiceSubconFabricWashDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSampleFabricWashItemDto> Items { get; set; }
    }
}
