using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts
{
    public class GarmentSubconDeliveryLetterOutDetail : AggregateRoot<GarmentSubconDeliveryLetterOutDetail, GarmentSubconDeliveryLetterOutDetailReadModel>
    {
        public Guid SubconDeliveryLetterOutItemId { get; private set; }
        public int UENItemId { get; private set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string ProductRemark { get; private set; }

        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }

        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public UomId UomOutId { get; private set; }
        public string UomOutUnit { get; private set; }

        public string FabricType { get; private set; }
        public int UENId { get; private set; }
        public string UENNo { get; private set; }

        public GarmentSubconDeliveryLetterOutDetail(Guid identity, Guid subconDeliveryLetterOutItemId, int uENItemId, ProductId productId, string productCode, string productName, string productRemark, string designColor, double quantity, UomId uomId, string uomUnit, UomId uomOutId, string uomOutUnit, string fabricType,int uenId,string uenNo) : base(identity)
        {
            Identity = identity;
            SubconDeliveryLetterOutItemId = subconDeliveryLetterOutItemId;
            UENItemId = uENItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ProductRemark = productRemark;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            UomOutId = uomOutId;
            UomOutUnit = uomOutUnit;
            FabricType = fabricType;
            UENId = uenId;
            UENNo = uenNo;

            ReadModel = new GarmentSubconDeliveryLetterOutDetailReadModel(Identity)
            {
                SubconDeliveryLetterOutItemId = SubconDeliveryLetterOutItemId,
                UENItemId = UENItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                UomOutId = UomOutId.Value,
                UomOutUnit = UomOutUnit,
                ProductRemark = ProductRemark,
                FabricType = FabricType,
                UENId = UENId,
                UENNo = UENNo,

        };
            ReadModel.AddDomainEvent(new OnGarmentSubconDeliveryLetterOutPlaced(Identity));
        }

        public GarmentSubconDeliveryLetterOutDetail(GarmentSubconDeliveryLetterOutDetailReadModel readModel) : base(readModel)
        {
            UENItemId = readModel.UENItemId;
            SubconDeliveryLetterOutItemId = readModel.SubconDeliveryLetterOutItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            ProductRemark = readModel.ProductRemark;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
            FabricType = readModel.FabricType;
            UomUnit = readModel.UomUnit;
            UomId = new UomId(readModel.UomId);
            UomOutId = new UomId(readModel.UomOutId);
            UomOutUnit = readModel.UomOutUnit;
            UENId = readModel.UENId;
            UENNo = readModel.UENNo;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconDeliveryLetterOutDetail GetEntity()
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
