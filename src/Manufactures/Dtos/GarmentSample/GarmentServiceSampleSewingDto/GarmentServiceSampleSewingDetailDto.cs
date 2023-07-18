using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample
{
    public class GarmentServiceSampleSewingDetailDto
    {
        public GarmentServiceSampleSewingDetailDto(GarmentServiceSampleSewingDetail garmentServiceSampleSewingDetail)
        {
            Id = garmentServiceSampleSewingDetail.Identity;
            ServiceSampleSewingItemId = garmentServiceSampleSewingDetail.ServiceSampleSewingItemId;
            SewingInId = garmentServiceSampleSewingDetail.SewingInId;
            SewingInItemId = garmentServiceSampleSewingDetail.SewingInItemId;
            Product = new Product(garmentServiceSampleSewingDetail.ProductId.Value, garmentServiceSampleSewingDetail.ProductCode, garmentServiceSampleSewingDetail.ProductName);
            DesignColor = garmentServiceSampleSewingDetail.DesignColor;
            Quantity = garmentServiceSampleSewingDetail.Quantity;
            Unit = new UnitDepartment(garmentServiceSampleSewingDetail.UnitId.Value, garmentServiceSampleSewingDetail.UnitCode, garmentServiceSampleSewingDetail.UnitName);
            Uom = new Uom(garmentServiceSampleSewingDetail.UomId.Value, garmentServiceSampleSewingDetail.UomUnit);
            Remark = garmentServiceSampleSewingDetail.Remark;
            Color = garmentServiceSampleSewingDetail.Color;
        }


        public Guid Id { get; set; }
        public Guid ServiceSampleSewingItemId { get; set; }
        public Guid SewingInId { get; set; }
        public Guid SewingInItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public UnitDepartment Unit { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Remark { get; set; }
        public string Color { get; set; }
    }
}
