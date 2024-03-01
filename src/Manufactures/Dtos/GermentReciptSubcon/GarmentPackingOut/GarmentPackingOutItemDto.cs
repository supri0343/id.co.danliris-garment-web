using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentPackingOut
{
    public class GarmentPackingOutItemDto : BaseDto
    {
        public GarmentPackingOutItemDto(GarmentSubconPackingOutItem garmentSubconPackingOutItem)
        {
            Id = garmentSubconPackingOutItem.Identity;
            PackingOutId = garmentSubconPackingOutItem.PackingOutId;
            FinishedGoodStockId = garmentSubconPackingOutItem.FinishedGoodStockId;
            Size = new SizeValueObject(garmentSubconPackingOutItem.SizeId.Value, garmentSubconPackingOutItem.SizeName);
            Quantity = garmentSubconPackingOutItem.Quantity;
            Uom = new Uom(garmentSubconPackingOutItem.UomId.Value, garmentSubconPackingOutItem.UomUnit);
            Description = garmentSubconPackingOutItem.Description;
            BasicPrice = garmentSubconPackingOutItem.BasicPrice;
            Price = garmentSubconPackingOutItem.Price;
            ReturQuantity = garmentSubconPackingOutItem.ReturQuantity;
        }

        public Guid Id { get; set; }
        public Guid PackingOutId { get; set; }
        public Guid FinishedGoodStockId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public double ReturQuantity { get; set; }
        public Uom Uom { get; set; }
        public string Description { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
