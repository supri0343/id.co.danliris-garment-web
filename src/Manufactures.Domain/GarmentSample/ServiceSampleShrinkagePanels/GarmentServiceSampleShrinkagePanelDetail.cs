using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Manufactures.Domain.Events.GarmentSample;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels
{
    public class GarmentServiceSampleShrinkagePanelDetail : AggregateRoot<GarmentServiceSampleShrinkagePanelDetail, GarmentServiceSampleShrinkagePanelDetailReadModel>
    {
        public Guid ServiceSampleShrinkagePanelItemId { get; private set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string ProductRemark { get; private set; }

        public string DesignColor { get; private set; }
        public decimal Quantity { get; private set; }

        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }

        public GarmentServiceSampleShrinkagePanelDetail(Guid identity, Guid serviceSampleShrinkagePanelItemId, ProductId productId, string productCode, string productName, string productRemark, string designColor, decimal quantity, UomId uomId, string uomUnit) : base(identity)
        {
            ServiceSampleShrinkagePanelItemId = serviceSampleShrinkagePanelItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ProductRemark = productRemark;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;

            ReadModel = new GarmentServiceSampleShrinkagePanelDetailReadModel(identity)
            {
                ServiceSampleShrinkagePanelItemId = ServiceSampleShrinkagePanelItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                ProductRemark = ProductRemark,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
            };
            ReadModel.AddDomainEvent(new OnGarmentServiceSampleShrinkagePanelPlaced(Identity));
        }

        public GarmentServiceSampleShrinkagePanelDetail(GarmentServiceSampleShrinkagePanelDetailReadModel readModel) : base(readModel)
        {
            ServiceSampleShrinkagePanelItemId = readModel.ServiceSampleShrinkagePanelItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            ProductRemark = readModel.ProductRemark;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleShrinkagePanelDetail GetEntity()
        {
            return this;
        }

        public void SetQuantity(decimal Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
    }
}
