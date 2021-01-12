using Infrastructure.Domain;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Events.GarmentSubcon;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings
{
    public class GarmentServiceSubconCuttingItem : AggregateRoot<GarmentServiceSubconCuttingItem, GarmentServiceSubconCuttingItemReadModel>
    {

        public Guid ServiceSubconCuttingId { get; private set; }
        public Guid CuttingInDetailId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }

        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }

        public GarmentServiceSubconCuttingItem(Guid identity, Guid serviceSubconCuttingId, Guid cuttingInDetailId, ProductId productId, string productCode, string productName, string designColor, double quantity) : base(identity)
        {
            Identity = identity;
            ServiceSubconCuttingId = serviceSubconCuttingId;
            CuttingInDetailId = cuttingInDetailId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            Quantity = quantity;

            ReadModel = new GarmentServiceSubconCuttingItemReadModel(Identity)
            {
                CuttingInDetailId = CuttingInDetailId,
                ServiceSubconCuttingId=ServiceSubconCuttingId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                Quantity=Quantity
            };

            ReadModel.AddDomainEvent(new OnServiceSubconCuttingPlaced(Identity));
        }

        public GarmentServiceSubconCuttingItem(GarmentServiceSubconCuttingItemReadModel readModel) : base(readModel)
        {
            CuttingInDetailId = readModel.CuttingInDetailId;
            ServiceSubconCuttingId = readModel.ServiceSubconCuttingId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconCuttingItem GetEntity()
        {
            return this;
        }
    }
}
