using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentFinishingOut
{
    public class GarmentFinishingOutItemDto : BaseDto
    {
        public GarmentFinishingOutItemDto(GarmentSubconFinishingOutItem garmentFinishingOutItem)
        {
            Id = garmentFinishingOutItem.Identity;
            FinishingOutId = garmentFinishingOutItem.FinishingOutId;
            FinishingInId = garmentFinishingOutItem.FinishingInId;
            FinishingInItemId = garmentFinishingOutItem.FinishingInItemId;
            Product = new Product(garmentFinishingOutItem.ProductId.Value, garmentFinishingOutItem.ProductCode, garmentFinishingOutItem.ProductName);
            Size = new SizeValueObject(garmentFinishingOutItem.SizeId.Value, garmentFinishingOutItem.SizeName);
            DesignColor = garmentFinishingOutItem.DesignColor;
            Quantity = garmentFinishingOutItem.Quantity;
            Uom = new Uom(garmentFinishingOutItem.UomId.Value, garmentFinishingOutItem.UomUnit);
            Color = garmentFinishingOutItem.Color;
            RealQtyOut = garmentFinishingOutItem.RealQtyOut;
            BasicPrice = garmentFinishingOutItem.BasicPrice;
            Price = garmentFinishingOutItem.Price;

            Details = new List<GarmentFinishingOutDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid FinishingOutId { get; set; }
        public Guid FinishingInId { get; set; }
        public Guid FinishingInItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RealQtyOut { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
        public List<GarmentFinishingOutDetailDto> Details { get; set; }
    }
}
