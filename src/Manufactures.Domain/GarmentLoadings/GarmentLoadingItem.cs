using Infrastructure.Domain;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events;

namespace Manufactures.Domain.GarmentLoadings
{
    public class GarmentLoadingItem : AggregateRoot<GarmentLoadingItem, GarmentLoadingItemReadModel>
    {
        public Guid LoadingId { get; internal set; }
        public Guid SewingDOItemId { get; internal set; }
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
        public double BasicPrice { get; internal set; }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public GarmentLoadingItem(Guid identity, Guid loadingId, Guid sewingDOItemId, SizeId sizeId, string sizeName, ProductId productId, string productCode, string productName, string designColor, double quantity, double remainingQuantity, double basicPrice, UomId uomId, string uomUnit, string color) : base(identity)
        {
            LoadingId = loadingId;
            SewingDOItemId = sewingDOItemId;
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
            BasicPrice = basicPrice;

            ReadModel = new GarmentLoadingItemReadModel(Identity)
            {
                LoadingId = loadingId,
                SewingDOItemId = SewingDOItemId,
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
                BasicPrice = BasicPrice
            };

            ReadModel.AddDomainEvent(new OnGarmentLoadingPlaced(Identity));
        }

        public GarmentLoadingItem(GarmentLoadingItemReadModel readModel) : base(readModel)
        {
            LoadingId = readModel.LoadingId;
            SewingDOItemId = readModel.SewingDOItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductCode;
            DesignColor = readModel.DesignColor;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId( readModel.UomId);
            UomUnit = readModel.UomUnit;
            Color = readModel.Color;
            RemainingQuantity = readModel.RemainingQuantity;
            BasicPrice = readModel.BasicPrice;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentLoadingItem GetEntity()
        {
            return this;
        }
    }
}
