using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentLoadingOut
{
    public class GarmentLoadingOutItemDto : BaseDto
    {
        public GarmentLoadingOutItemDto(GarmentSubconLoadingOutItem garmentLoadingItem)
        {
            Id = garmentLoadingItem.Identity;
            Product = new Product(garmentLoadingItem.ProductId.Value, garmentLoadingItem.ProductCode, garmentLoadingItem.ProductName);
            DesignColor = garmentLoadingItem.DesignColor;
            Size= new SizeValueObject(garmentLoadingItem.SizeId.Value, garmentLoadingItem.SizeName);
            Quantity = garmentLoadingItem.Quantity;
            Uom = new Uom(garmentLoadingItem.UomId.Value, garmentLoadingItem.UomUnit);
            Color = garmentLoadingItem.Color;
            RealQtyOut = garmentLoadingItem.RealQtyOut;
            BasicPrice = garmentLoadingItem.BasicPrice;
            LoadingInItemId = garmentLoadingItem.LoadingInItemId;
            LoadingOutId = garmentLoadingItem.LoadingOutId;
            Price = garmentLoadingItem.Price;

        }

        public Guid Id { get; set; }
        public Guid LoadingInItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RealQtyOut { get; set; }
        public double BasicPrice { get; set; }
        public Guid LoadingOutId { get; set; }
        public double Price { get; set; }
    }
}
