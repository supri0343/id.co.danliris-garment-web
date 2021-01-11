using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconSewingItemDto : BaseDto
    {
        public GarmentServiceSubconSewingItemDto(GarmentServiceSubconSewingItem garmentServiceSubconSewingItem)
        {
            Id = garmentServiceSubconSewingItem.Identity;
            ServiceSubconSewingId = garmentServiceSubconSewingItem.ServiceSubconSewingId;
            SewingInId = garmentServiceSubconSewingItem.SewingInId;
            SewingInItemId = garmentServiceSubconSewingItem.SewingInItemId;
            Product = new Product(garmentServiceSubconSewingItem.ProductId.Value, garmentServiceSubconSewingItem.ProductCode, garmentServiceSubconSewingItem.ProductName);
            Size = new SizeValueObject(garmentServiceSubconSewingItem.SizeId.Value, garmentServiceSubconSewingItem.SizeName);
            DesignColor = garmentServiceSubconSewingItem.DesignColor;
            Quantity = garmentServiceSubconSewingItem.Quantity;
            Uom = new Uom(garmentServiceSubconSewingItem.UomId.Value, garmentServiceSubconSewingItem.UomUnit);
            Color = garmentServiceSubconSewingItem.Color;
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconSewingId { get; set; }
        public Guid SewingInId { get; set; }
        public Guid SewingInItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
    }
}
