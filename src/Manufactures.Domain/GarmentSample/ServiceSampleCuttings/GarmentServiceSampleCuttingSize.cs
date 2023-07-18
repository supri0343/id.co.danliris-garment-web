using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings
{
    public class GarmentServiceSampleCuttingSize : AggregateRoot<GarmentServiceSampleCuttingSize, GarmentServiceSampleCuttingSizeReadModel>
    {

        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Color { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public Guid ServiceSampleCuttingDetailId { get; private set; }
        public Guid CuttingInDetailId { get; private set; }
        public Guid CuttingInId { get; private set; }
        public GarmentServiceSampleCuttingSize(Guid identity,SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, Guid serviceSampleCuttingDetailId, Guid cuttingId, Guid cuttingInDetailId, ProductId productId, string productCode, string productName) : base(identity)
        {
            Identity = identity;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            CuttingInDetailId = cuttingInDetailId;
            ServiceSampleCuttingDetailId = serviceSampleCuttingDetailId;
            CuttingInId = cuttingId;
            ReadModel = new GarmentServiceSampleCuttingSizeReadModel(Identity)
            {
                ServiceSampleCuttingDetailId = ServiceSampleCuttingDetailId,
                Color = Color,
                CuttingInDetailId = CuttingInDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                CuttingInId = CuttingInId
            };

            ReadModel.AddDomainEvent(new OnServiceSampleCuttingPlaced(Identity));
        }

        public GarmentServiceSampleCuttingSize(GarmentServiceSampleCuttingSizeReadModel readModel) : base(readModel)
        {
            ServiceSampleCuttingDetailId = readModel.ServiceSampleCuttingDetailId;
            Color = readModel.Color;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            CuttingInDetailId = readModel.CuttingInDetailId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            CuttingInId = readModel.CuttingInId;
        }

        public void SetQuantity(double qty)
        {
            if (Quantity != qty)
            {
                Quantity = qty;
                ReadModel.Quantity = qty;

            }
        }

        public void SetSizeId(SizeId sizeId)
        {
            if (SizeId != sizeId)
            {
                SizeId = sizeId;
                ReadModel.SizeId = sizeId.Value;

            }
        }

        public void SetSizeName(string sizeName)
        {
            if (SizeName != sizeName)
            {
                SizeName = sizeName;
                ReadModel.SizeName = sizeName;

            }
        }

        public void SetProducId(ProductId productId)
        {
            if (ProductId != productId)
            {
                ProductId = productId;
                ReadModel.ProductId = productId.Value;

            }
        }

        public void SetProducName(string productName)
        {
            if (ProductName != productName)
            {
                ProductName = productName;
                ReadModel.ProductName = productName;

            }
        }

        public void SetProducCode(string productCode)
        {
            if (ProductCode != productCode)
            {
                ProductCode = productCode;
                ReadModel.ProductCode = productCode;

            }
        }

        public void SetColor(string color)
        {
            if (Color != color)
            {
                Color = color;
                ReadModel.Color = color;

            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleCuttingSize GetEntity()
        {
            return this;
        }
    }
}
