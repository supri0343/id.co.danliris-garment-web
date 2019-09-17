using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSewingInItemDto : BaseDto
    {
        public GarmentSewingInItemDto(GarmentSewingInItem garmentSewingInItem)
        {
            Id = garmentSewingInItem.Identity;
            SewingInId = garmentSewingInItem.SewingInId;
            LoadingItemId = garmentSewingInItem.LoadingItemId;
            Product = new Product(garmentSewingInItem.ProductId.Value, garmentSewingInItem.ProductCode, garmentSewingInItem.ProductName);
            DesignColor = garmentSewingInItem.DesignColor;
            Size = new SizeValueObject(garmentSewingInItem.SizeId.Value, garmentSewingInItem.SizeName);
            Quantity = garmentSewingInItem.Quantity;
            Uom = new Uom(garmentSewingInItem.UomId.Value, garmentSewingInItem.UomUnit);
            Color = garmentSewingInItem.Color;
            RemainingQuantity = garmentSewingInItem.RemainingQuantity;
        }

        public Guid Id { get; set; }
        public Guid SewingInId { get; internal set; }
        public Guid LoadingItemId { get; internal set; }
        public Product Product { get; internal set; }
        public string DesignColor { get; internal set; }
        public SizeValueObject Size { get; internal set; }
        public double Quantity { get; internal set; }
        public Uom Uom { get; internal set; }
        public string Color { get; internal set; }
        public double RemainingQuantity { get; internal set; }
    }
}