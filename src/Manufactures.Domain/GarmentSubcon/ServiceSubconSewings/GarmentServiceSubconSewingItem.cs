using System;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings
{
    public class GarmentServiceSubconSewingItem : AggregateRoot<GarmentServiceSubconSewingItem, GarmentServiceSubconSewingItemReadModel>
    {
        public Guid ServiceSubconSewingId { get; private set; }
        public Guid SewingInId { get; private set; }
        public Guid SewingInItemId { get; private set; }
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

        public GarmentServiceSubconSewingItem(Guid identity, Guid serviceSubconSewingId, Guid sewingInId, Guid sewingInItemId, ProductId productId, string productCode, string productName, string designColor, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color) : base(identity)
        {
            Identity = identity;
            ServiceSubconSewingId = serviceSubconSewingId;
            SewingInId = sewingInId;
            SewingInItemId = sewingInItemId;
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

            ReadModel = new GarmentServiceSubconSewingItemReadModel(identity)
            {
                ServiceSubconSewingId = ServiceSubconSewingId,
                SewingInId = SewingInId,
                SewingInItemId = SewingInItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Color = Color
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconSewingPlaced(Identity));

        }

        public GarmentServiceSubconSewingItem(GarmentServiceSubconSewingItemReadModel readModel) : base(readModel)
        {
            ServiceSubconSewingId = readModel.ServiceSubconSewingId;
            SewingInId = readModel.SewingInId;
            SewingInItemId = readModel.SewingInItemId;
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
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconSewingItem GetEntity()
        {
            return this;
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
    }
}
