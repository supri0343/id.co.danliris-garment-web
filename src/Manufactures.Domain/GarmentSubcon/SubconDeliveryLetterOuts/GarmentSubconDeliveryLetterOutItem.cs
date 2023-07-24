﻿using Infrastructure.Domain;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;
using Manufactures.Domain.Events.GarmentSubcon;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts
{
    public class GarmentSubconDeliveryLetterOutItem : AggregateRoot<GarmentSubconDeliveryLetterOutItem, GarmentSubconDeliveryLetterOutItemReadModel>
    {
        

        public Guid SubconDeliveryLetterOutId { get; private set; }
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

        #region Cutting
        public string RONo { get; private set; }
        public Guid SubconId { get; private set; }
        public string POSerialNumber { get; private set; }
        public string SubconNo { get; private set; }

        #endregion
        //public UomId UomSatuanId { get; private set; }
        public string UomSatuanUnit { get; private set; }
        public int QtyPacking { get; private set; }
       
        //ShrinkagePanel
        public double SmallQuantity { get; private set; }
        public string SmallUomUnit { get; private set; }
        public int UENId { get; private set; }
        public string UENNo { get; private set; }
        public GarmentSubconDeliveryLetterOutItem(Guid identity, Guid subconDeliveryLetterOutId, int uENItemId, ProductId productId, string productCode, string productName, string productRemark, string designColor, double quantity, UomId uomId, string uomUnit, UomId uomOutId, string uomOutUnit, string fabricType, Guid subconId, string roNo, string poSerialNumber, string subconNo/*, UomId uomSatuanId*/, string uomSatuanUnit, int qtyPacking,double smallQty,string smallUom,int uenId,string uenNo) : base(identity)
        {
            Identity = identity;
            SubconDeliveryLetterOutId = subconDeliveryLetterOutId;
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
            SubconId = subconId;
            RONo = roNo;
            POSerialNumber = poSerialNumber;
            SubconNo = subconNo;
            //UomSatuanId = uomSatuanId;
            UomSatuanUnit = uomSatuanUnit;
            QtyPacking = qtyPacking;
            SmallQuantity = smallQty;
            SmallUomUnit = smallUom;
            UENId = uenId;
            UENNo = uenNo;
            ReadModel = new GarmentSubconDeliveryLetterOutItemReadModel(Identity)
            {
                SubconDeliveryLetterOutId = SubconDeliveryLetterOutId,
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
                SubconId = SubconId,
                RONo = RONo,
                POSerialNumber = POSerialNumber,
                SubconNo = SubconNo,
                //UomSatuanId = UomSatuanId.Value,
                UomSatuanUnit = UomSatuanUnit,
                QtyPacking = QtyPacking,
                SmallQuantity = SmallQuantity,
                SmallUomUnit = SmallUomUnit,
                UENId = UENId,
                UENNo = UENNo,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconDeliveryLetterOutPlaced(Identity));
        }

        public GarmentSubconDeliveryLetterOutItem(GarmentSubconDeliveryLetterOutItemReadModel readModel) : base(readModel)
        {
            UENItemId = readModel.UENItemId;
            SubconDeliveryLetterOutId = readModel.SubconDeliveryLetterOutId;
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
            POSerialNumber = readModel.POSerialNumber;
            RONo = readModel.RONo;
            SubconId = readModel.SubconId;
            SubconNo = readModel.SubconNo;
            //UomSatuanId = new UomId(readModel.UomSatuanId);
            UomSatuanUnit = readModel.UomSatuanUnit;
            QtyPacking = readModel.QtyPacking;
            SmallQuantity = readModel.SmallQuantity;
            SmallUomUnit = readModel.SmallUomUnit;
            UENId = readModel.UENId;
            UENNo = readModel.UENNo;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconDeliveryLetterOutItem GetEntity()
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

        public void SetSmallQty(double smallQty)
        {
            if (this.SmallQuantity != smallQty)
            {
                this.SmallQuantity = smallQty;
                ReadModel.SmallQuantity = smallQty;
            }
        }
    }
}
