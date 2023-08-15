using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingItemDto : BaseDto
    {
        public GarmentServiceSampleCuttingItemDto(GarmentServiceSampleCuttingItem garmentServiceSampleCuttingItem)
        {
            Id = garmentServiceSampleCuttingItem.Identity;
            ServiceSampleCuttingId = garmentServiceSampleCuttingItem.ServiceSampleCuttingId;
            RONo = garmentServiceSampleCuttingItem.RONo;
            Article = garmentServiceSampleCuttingItem.Article;
            Comodity = new GarmentComodity(garmentServiceSampleCuttingItem.ComodityId.Value, garmentServiceSampleCuttingItem.ComodityCode, garmentServiceSampleCuttingItem.ComodityName);
            Details = new List<GarmentServiceSampleCuttingDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleCuttingId { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public List<GarmentServiceSampleCuttingDetailDto> Details { get; set; }
    }
}