using Infrastructure.Domain;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ReadModels;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns
{
    public class GarmentSubconLoadingInItem : AggregateRoot<GarmentSubconLoadingInItem, GarmentSubconLoadingInItemReadModel>
    {
        public Guid LoadingId { get; internal set; }
        //public Guid SewingDOItemId { get; internal set; }
        public Guid CuttingOutDetailId { get; internal set; }
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
        public double Price { get; internal set; }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetRemainingQuantity(double RemainingQuantity)
        {
            if (this.RemainingQuantity != RemainingQuantity)
            {
                this.RemainingQuantity = RemainingQuantity;
                ReadModel.RemainingQuantity = RemainingQuantity;
            }
        }

        public void SetPrice(double Price)
        {
            if (this.Price != Price)
            {
                this.Price = Price;
                ReadModel.Price = Price;
            }
        }

        public GarmentSubconLoadingInItem(Guid identity, Guid loadingId, Guid sewingDOItemId, SizeId sizeId, string sizeName, ProductId productId, string productCode, string productName, string designColor, double quantity, double remainingQuantity, double basicPrice, UomId uomId, string uomUnit, string color, double price) : base(identity)
        {
            LoadingId = loadingId;
            CuttingOutDetailId = sewingDOItemId;
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
            Price = price;

            ReadModel = new GarmentSubconLoadingInItemReadModel(Identity)
            {
                LoadingId = loadingId,
                CuttingOutDetailId = CuttingOutDetailId,
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
                BasicPrice = BasicPrice,
                Price=Price
            };

            ReadModel.AddDomainEvent(new OnGarmentLoadingPlaced(Identity));
        }

        public GarmentSubconLoadingInItem(GarmentSubconLoadingInItemReadModel readModel) : base(readModel)
        {
            LoadingId = readModel.LoadingId;
            CuttingOutDetailId = readModel.CuttingOutDetailId;
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
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconLoadingInItem GetEntity()
        {
            return this;
        }
    }
}
