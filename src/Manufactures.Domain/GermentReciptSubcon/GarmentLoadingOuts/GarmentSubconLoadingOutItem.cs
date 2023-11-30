using Infrastructure.Domain;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts
{
    public class GarmentSubconLoadingOutItem : AggregateRoot<GarmentSubconLoadingOutItem, GarmentSubconLoadingOutItemReadModel>
    {
        public Guid LoadingOutId { get; internal set; }
        public Guid LoadingInItemId { get; internal set; }
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
        public double RealQtyOut { get; internal set; }
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

        public void SetRealQtyOut(double RealQtyOut)
        {
            if (this.RealQtyOut != RealQtyOut)
            {
                this.RealQtyOut = RealQtyOut;
                ReadModel.RealQtyOut = RealQtyOut;
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

        public GarmentSubconLoadingOutItem(Guid identity, Guid loadingId, Guid sewingDOItemId, SizeId sizeId, string sizeName, ProductId productId, string productCode, string productName, string designColor, double quantity, double realQtyOut, double basicPrice, UomId uomId, string uomUnit, string color, double price) : base(identity)
        {
            LoadingOutId = loadingId;
            LoadingInItemId = sewingDOItemId;
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
            RealQtyOut = realQtyOut;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentSubconLoadingOutItemReadModel(Identity)
            {
                LoadingOutId = loadingId,
                LoadingInItemId = LoadingInItemId,
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
                Price=Price
            };

            ReadModel.AddDomainEvent(new OnGarmentLoadingPlaced(Identity));
        }

        public GarmentSubconLoadingOutItem(GarmentSubconLoadingOutItemReadModel readModel) : base(readModel)
        {
            LoadingOutId = readModel.LoadingOutId;
            LoadingInItemId = readModel.LoadingInItemId;
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
            RealQtyOut = readModel.RealQtyOut;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconLoadingOutItem GetEntity()
        {
            return this;
        }
    }
}
