using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingSizeDto : BaseDto
    {
        public GarmentServiceSampleCuttingSizeDto(GarmentServiceSampleCuttingSize garmentServiceSampleCuttingSize)
        {
            Id = garmentServiceSampleCuttingSize.Identity;
            ServiceSampleCuttingDetailId = garmentServiceSampleCuttingSize.ServiceSampleCuttingDetailId;
            CuttingInId = garmentServiceSampleCuttingSize.CuttingInId;
            CuttingInDetailId = garmentServiceSampleCuttingSize.CuttingInDetailId;
            Product = new Product(garmentServiceSampleCuttingSize.ProductId.Value, garmentServiceSampleCuttingSize.ProductCode, garmentServiceSampleCuttingSize.ProductName);
            Color = garmentServiceSampleCuttingSize.Color;
            Quantity = garmentServiceSampleCuttingSize.Quantity;
            Size = new SizeValueObject(garmentServiceSampleCuttingSize.SizeId.Value, garmentServiceSampleCuttingSize.SizeName);
            Uom = new Uom(garmentServiceSampleCuttingSize.UomId.Value, garmentServiceSampleCuttingSize.UomUnit);
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleCuttingDetailId { get; set; }
        public SizeValueObject Size { get; internal set; }
        public double Quantity { get; internal set; }
        public Uom Uom { get; internal set; }
        public string Color { get; internal set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
    }
}