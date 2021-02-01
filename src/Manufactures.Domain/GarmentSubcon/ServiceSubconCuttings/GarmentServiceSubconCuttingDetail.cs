using Infrastructure.Domain;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events.GarmentSubcon;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings
{
    public class GarmentServiceSubconCuttingDetail : AggregateRoot<GarmentServiceSubconCuttingDetail, GarmentServiceSubconCuttingDetailReadModel>
    {

        public Guid ServiceSubconCuttingItemId { get; private set; }
        public Guid CuttingInDetailId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }

        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }

        public GarmentServiceSubconCuttingDetail(Guid identity, Guid serviceSubconCuttingItemId, Guid cuttingInDetailId, ProductId productId, string productCode, string productName, string designColor, double quantity) : base(identity)
        {
            Identity = identity;
            ServiceSubconCuttingItemId = serviceSubconCuttingItemId;
            CuttingInDetailId = cuttingInDetailId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            Quantity = quantity;

            ReadModel = new GarmentServiceSubconCuttingDetailReadModel(Identity)
            {
                ServiceSubconCuttingItemId = ServiceSubconCuttingItemId,
                CuttingInDetailId = CuttingInDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                Quantity = Quantity,
            };

            ReadModel.AddDomainEvent(new OnServiceSubconCuttingPlaced(Identity));
        }

        public GarmentServiceSubconCuttingDetail(GarmentServiceSubconCuttingDetailReadModel readModel) : base(readModel)
        {
            ServiceSubconCuttingItemId = readModel.ServiceSubconCuttingItemId;
            CuttingInDetailId = readModel.CuttingInDetailId;
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

        protected override GarmentServiceSubconCuttingDetail GetEntity()
        {
            return this;
        }
    }
}
