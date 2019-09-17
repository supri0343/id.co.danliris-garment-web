using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSewingIns
{
    public class GarmentSewingInItem : AggregateRoot<GarmentSewingInItem, GarmentSewingInItemReadModel>
    {
        public Guid SewingInId { get; private set; }
        public Guid LoadingItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Color { get; private set; }
        public double RemainingQuantity { get; private set; }

        public GarmentSewingInItem(Guid identity, Guid sewingInId, Guid loadingItemId, ProductId productId, string productCode, string productName, string designColor, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, double remainingQuantity) : base(identity)
        {
            Identity = identity;
            SewingInId = sewingInId;
            LoadingItemId = loadingItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            RemainingQuantity = remainingQuantity;

            ReadModel = new GarmentSewingInItemReadModel(identity)
            {
                SewingInId = SewingInId,
                LoadingItemId = LoadingItemId,
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
                RemainingQuantity = RemainingQuantity
            };

            ReadModel.AddDomainEvent(new OnGarmentSewingInPlaced(Identity));
        }

        public GarmentSewingInItem(GarmentSewingInItemReadModel readModel) : base(readModel)
        {
            SewingInId = readModel.SewingInId;
            LoadingItemId = readModel.LoadingItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
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

        protected override GarmentSewingInItem GetEntity()
        {
            return this;
        }
    }
}