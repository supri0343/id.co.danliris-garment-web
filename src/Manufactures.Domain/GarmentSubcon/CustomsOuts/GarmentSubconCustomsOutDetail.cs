using Infrastructure.Domain;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events.GarmentSubcon;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts
{
    public class GarmentSubconCustomsOutDetail : AggregateRoot<GarmentSubconCustomsOutDetail, GarmentSubconCustomsOutDetailReadModel>
    {
        public Guid SubconCustomsOutItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string ProductRemark { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }

        public GarmentSubconCustomsOutDetail(Guid identity, Guid subconCustomsOutItemId, ProductId productId, string productCode, string productName, string productRemark, double quantity, UomId uomId, string uomUnit) : base(identity)
        {
            Identity = identity;
            SubconCustomsOutItemId = subconCustomsOutItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ProductRemark = productRemark;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;

            ReadModel = new GarmentSubconCustomsOutDetailReadModel(Identity)
            {
                SubconCustomsOutItemId = SubconCustomsOutItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                ProductRemark = ProductRemark,

            };
            ReadModel.AddDomainEvent(new OnGarmentSubconDeliveryLetterOutPlaced(Identity));
        }

        public GarmentSubconCustomsOutDetail(GarmentSubconCustomsOutDetailReadModel readModel) : base(readModel)
        {
            SubconCustomsOutItemId = readModel.SubconCustomsOutItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            ProductRemark = readModel.ProductRemark;
            Quantity = readModel.Quantity;
            UomUnit = readModel.UomUnit;
            UomId = new UomId(readModel.UomId);

        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCustomsOutDetail GetEntity()
        {
            return this;
        }

    }
}
