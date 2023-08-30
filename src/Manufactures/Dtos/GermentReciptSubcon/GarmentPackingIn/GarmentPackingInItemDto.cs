using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentPackingIn
{
    public class GarmentPackingInItemDto : BaseDto
    {
        public GarmentPackingInItemDto(GarmentSubconPackingInItem garmentPackingInItem)
        {
            Id = garmentPackingInItem.Identity;
            Product = new Product(garmentPackingInItem.ProductId.Value, garmentPackingInItem.ProductCode, garmentPackingInItem.ProductName);
            DesignColor = garmentPackingInItem.DesignColor;
            Size = new SizeValueObject(garmentPackingInItem.SizeId.Value, garmentPackingInItem.SizeName);
            Quantity = garmentPackingInItem.Quantity;
            Uom = new Uom(garmentPackingInItem.UomId.Value, garmentPackingInItem.UomUnit);
            Color = garmentPackingInItem.Color;
            RemainingQuantity = garmentPackingInItem.RemainingQuantity;
            SewingOutItemId = garmentPackingInItem.SewingOutItemId;
            CuttingOutItemId = garmentPackingInItem.CuttingOutItemId;
            FinishingOutItemId = garmentPackingInItem.FinishingOutItemId;
            PackingInId = garmentPackingInItem.PackingInId;
            BasicPrice = garmentPackingInItem.BasicPrice;
            Price = garmentPackingInItem.Price;
        }

        public GarmentPackingInItemDto(GarmentSubconPackingInItemReadModel garmentPackingInItemReadModel)
        {
            Id = garmentPackingInItemReadModel.Identity;
            Product = new Product(garmentPackingInItemReadModel.ProductId, garmentPackingInItemReadModel.ProductCode, garmentPackingInItemReadModel.ProductName);
            DesignColor = garmentPackingInItemReadModel.DesignColor;
            Size = new SizeValueObject(garmentPackingInItemReadModel.SizeId, garmentPackingInItemReadModel.SizeName);
            Quantity = garmentPackingInItemReadModel.Quantity;
            Uom = new Uom(garmentPackingInItemReadModel.UomId, garmentPackingInItemReadModel.UomUnit);
            Color = garmentPackingInItemReadModel.Color;
            RemainingQuantity = garmentPackingInItemReadModel.RemainingQuantity;
            SewingOutItemId = garmentPackingInItemReadModel.SewingOutItemId;
            CuttingOutItemId = garmentPackingInItemReadModel.CuttingOutItemId;
            FinishingOutItemId = garmentPackingInItemReadModel.FinishingOutItemId;
            PackingInId = garmentPackingInItemReadModel.PackingInId;
            BasicPrice = garmentPackingInItemReadModel.BasicPrice;
            Price = garmentPackingInItemReadModel.Price;
        }

        public Guid Id { get; set; }
        public Guid SewingOutItemId { get; set; }
        public Guid CuttingOutItemId { get; set; }
        public Guid FinishingOutItemId { get; set; }

        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public Guid PackingInId { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
