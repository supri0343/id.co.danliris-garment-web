using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingDetailDto : BaseDto
    {
        public GarmentServiceSampleCuttingDetailDto(GarmentServiceSampleCuttingDetail garmentServiceSampleCuttingDetail)
        {
            Id = garmentServiceSampleCuttingDetail.Identity;
            ServiceSampleCuttingItemId = garmentServiceSampleCuttingDetail.ServiceSampleCuttingItemId;
            DesignColor = garmentServiceSampleCuttingDetail.DesignColor;
            Quantity = garmentServiceSampleCuttingDetail.Quantity;
            Sizes= new List<GarmentServiceSampleCuttingSizeDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleCuttingItemId { get; set; }

        public string DesignColor { get; set; }

        public double Quantity { get; set; }
        public List<GarmentServiceSampleCuttingSizeDto> Sizes { get; set; }
    }
}