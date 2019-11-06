using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns
{
    public class GarmentFinishingInItem : AggregateRoot<GarmentFinishingInItem, GarmentFinishingInItemReadModel>
    {
        public Guid FinishingInId { get; internal set; }
        public Guid SewingOutItemId { get; internal set; }
        public Guid SewingOutDetailId { get; internal set; }
        public ProductId ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public SizeId SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public UomId UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public double RemainingQuantity { get; internal set; }

        public void SetRemainingQuantity(double RemainingQuantity)
        {
            if (this.RemainingQuantity != RemainingQuantity)
            {
                this.RemainingQuantity = RemainingQuantity;
                ReadModel.RemainingQuantity = RemainingQuantity;
            }
        }

        public GarmentFinishingInItem(Guid identity, Guid finishingInId, Guid sewingOutItemId, Guid sewingOutDetailId, SizeId sizeId, string sizeName, ProductId productId, string productCode, string productName, string designColor, double quantity, double remainingQuantity, UomId uomId, string uomUnit, string color) : base(identity)
        {
            FinishingInId = finishingInId;
            SewingOutItemId = sewingOutItemId;
            SewingOutDetailId = sewingOutDetailId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productCode;
            DesignColor = designColor;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            RemainingQuantity = remainingQuantity;

            ReadModel = new GarmentFinishingInItemReadModel(Identity)
            {
                FinishingInId = FinishingInId,
                SewingOutItemId = SewingOutItemId,
                SewingOutDetailId= SewingOutDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Color = Color,
                RemainingQuantity = RemainingQuantity,
            };

            ReadModel.AddDomainEvent(new OnGarmentFinishingInPlaced(Identity));
        }

        public GarmentFinishingInItem(GarmentFinishingInItemReadModel readModel) : base(readModel)
        {
            FinishingInId = readModel.FinishingInId;
            SewingOutItemId = readModel.SewingOutItemId;
            SewingOutDetailId = readModel.SewingOutDetailId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductCode;
            DesignColor = readModel.DesignColor;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Color = readModel.Color;
            RemainingQuantity = readModel.RemainingQuantity;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentFinishingInItem GetEntity()
        {
            return this;
        }
    }
}
