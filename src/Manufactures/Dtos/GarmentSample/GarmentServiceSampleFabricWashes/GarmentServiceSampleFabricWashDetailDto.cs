using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleFabricWashes
{
    public class GarmentServiceSampleFabricWashDetailDto
    {
        public GarmentServiceSampleFabricWashDetailDto(GarmentServiceSampleFabricWashDetail garmentServiceSampleFabricWashDetail)
        {
            Id = garmentServiceSampleFabricWashDetail.Identity;
            ServiceSampleFabricWashItemId = garmentServiceSampleFabricWashDetail.ServiceSampleFabricWashItemId;
            Product = new Product(garmentServiceSampleFabricWashDetail.ProductId.Value, garmentServiceSampleFabricWashDetail.ProductCode, garmentServiceSampleFabricWashDetail.ProductName, garmentServiceSampleFabricWashDetail.ProductRemark);
            DesignColor = garmentServiceSampleFabricWashDetail.DesignColor;
            Quantity = garmentServiceSampleFabricWashDetail.Quantity;
            Uom = new Uom(garmentServiceSampleFabricWashDetail.UomId.Value, garmentServiceSampleFabricWashDetail.UomUnit);
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleFabricWashItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public decimal Quantity { get; set; }
        public Uom Uom { get; set; }
    }
}
