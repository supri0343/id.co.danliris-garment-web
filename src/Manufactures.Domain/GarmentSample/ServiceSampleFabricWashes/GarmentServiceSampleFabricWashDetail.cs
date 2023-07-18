using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Manufactures.Domain.Events.GarmentSample;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes
{
    public class GarmentServiceSampleFabricWashDetail : AggregateRoot<GarmentServiceSampleFabricWashDetail, GarmentServiceSampleFabricWashDetailReadModel>
    {
        public Guid ServiceSampleFabricWashItemId { get; private set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string ProductRemark { get; private set; }

        public string DesignColor { get; private set; }
        public decimal Quantity { get; private set; }

        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }

        public GarmentServiceSampleFabricWashDetail(Guid identity, Guid serviceSampleFabricWashItemId, ProductId productId, string productCode, string productName, string productRemark, string designColor, decimal quantity, UomId uomId, string uomUnit) : base(identity)
        {
            ServiceSampleFabricWashItemId = serviceSampleFabricWashItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ProductRemark = productRemark;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;

            ReadModel = new GarmentServiceSampleFabricWashDetailReadModel(identity)
            {
                ServiceSampleFabricWashItemId = ServiceSampleFabricWashItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                ProductRemark = ProductRemark,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
            };
            ReadModel.AddDomainEvent(new OnGarmentServiceSampleFabricWashPlaced(Identity));
        }

        public GarmentServiceSampleFabricWashDetail(GarmentServiceSampleFabricWashDetailReadModel readModel) : base(readModel)
        {
            ServiceSampleFabricWashItemId = readModel.ServiceSampleFabricWashItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            ProductRemark = readModel.ProductRemark;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
        }

        public void SetQuantity(decimal Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetProductRemark(string ProductRemark)
        {
            if (this.ProductRemark != ProductRemark)
            {
                this.ProductRemark = ProductRemark;
                ReadModel.ProductRemark = ProductRemark;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleFabricWashDetail GetEntity()
        {
            return this;
        }
    }
}
