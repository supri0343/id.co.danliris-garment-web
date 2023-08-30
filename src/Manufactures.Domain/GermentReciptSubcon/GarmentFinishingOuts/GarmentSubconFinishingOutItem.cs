using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts
{
    public class GarmentSubconFinishingOutItem : AggregateRoot<GarmentSubconFinishingOutItem, GarmentReceiptSubconFinishingOutItemReadModel>
    {
        public Guid FinishingOutId { get; private set; }
        public Guid FinishingInId { get; private set; }
        public Guid FinishingInItemId { get; private set; }
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
        public double RealQtyOut { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }

        public GarmentSubconFinishingOutItem(Guid identity, Guid finishingOutId, Guid finishingInId, Guid finishingInItemId, ProductId productId, string productCode, string productName, string designColor, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, double realQtyOut, double basicPrice, double price) : base(identity)
        {
            //MarkTransient();

            Identity = identity;
            FinishingOutId = finishingOutId;
            FinishingInId = finishingInId;
            FinishingInItemId = finishingInItemId;
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
            RealQtyOut = realQtyOut;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentReceiptSubconFinishingOutItemReadModel(identity)
            {
                FinishingOutId = FinishingOutId,
                FinishingInId = FinishingInId,
                FinishingInItemId = FinishingInItemId,
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
                RealQtyOut = RealQtyOut,
                BasicPrice = BasicPrice,
                Price = Price
            };

            ReadModel.AddDomainEvent(new OnGarmentFinishingOutPlaced(Identity));
        }

        public GarmentSubconFinishingOutItem(GarmentReceiptSubconFinishingOutItemReadModel readModel) : base(readModel)
        {
            FinishingOutId = readModel.FinishingOutId;
            FinishingInId = readModel.FinishingInId;
            FinishingInItemId = readModel.FinishingInItemId;
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
            RealQtyOut = readModel.RealQtyOut;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void SetPrice(double Price)
        {
            if (this.Price != Price)
            {
                this.Price = Price;
                ReadModel.Price = Price;
            }
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetRealQtyOut(double RealQtyOut)
        {
            if (this.RealQtyOut != RealQtyOut)
            {
                this.RealQtyOut = RealQtyOut;
                ReadModel.RealQtyOut = RealQtyOut;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconFinishingOutItem GetEntity()
        {
            return this;
        }
    }
}

