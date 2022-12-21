using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts
{
    public class GarmentAvalProductItem : AggregateRoot<GarmentAvalProductItem, GarmentAvalProductItemReadModel>
    {
        public Guid APId { get; private set; }
        public GarmentPreparingId PreparingId { get; private set; }
        public GarmentPreparingItemId PreparingItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public double BasicPrice { get; private set; }
        public bool IsReceived { get; private set; }
        public string BCNo { get; private set; }
        public DateTime? BCDate { get; private set; }
        public string POSerialNumber { get; private set; }
        public string BCType { get; private set; }


        public GarmentAvalProductItem(Guid identity, Guid apId, GarmentPreparingId preparingId, GarmentPreparingItemId preparingItemId, ProductId productId, string productCode, string productName, string designColor, double quantity, UomId uomId, string uomUnit, double basicPrice, bool isReceived, string bcno, DateTime? bcdate, string poSerialNumber,string bctype) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            APId = apId;
            PreparingId = preparingId;
            PreparingItemId = preparingItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            BasicPrice = basicPrice;
            IsReceived = isReceived;
            BCNo = bcno;
            BCDate = bcdate;
            POSerialNumber = poSerialNumber;
            BCType = bctype;

            ReadModel = new GarmentAvalProductItemReadModel(Identity)
            {
                APId = APId,
                PreparingId = PreparingId.Value,
                PreparingItemId = PreparingItemId.Value,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                BasicPrice=BasicPrice,
                IsReceived=IsReceived,
                BCNo = BCNo,
                BCDate = BCDate,
                POSerialNumber = POSerialNumber,
                BCType = BCType,
        };
            ReadModel.AddDomainEvent(new OnGarmentAvalProductPlaced(this.Identity));
        }

        public GarmentAvalProductItem(GarmentAvalProductItemReadModel readModel) : base(readModel)
        {
            APId = ReadModel.APId;
            PreparingId = new GarmentPreparingId(ReadModel.PreparingId);
            PreparingItemId = new GarmentPreparingItemId(ReadModel.PreparingItemId);
            ProductId = new ProductId(ReadModel.ProductId);
            ProductCode = ReadModel.ProductCode;
            ProductName = ReadModel.ProductName;
            DesignColor = ReadModel.DesignColor;
            Quantity = ReadModel.Quantity;
            UomId = new UomId(ReadModel.UomId);
            UomUnit = ReadModel.UomUnit;
            BasicPrice = ReadModel.BasicPrice;
            IsReceived = ReadModel.IsReceived;
            BCNo = ReadModel.BCNo;
            BCDate = ReadModel.BCDate;
            POSerialNumber = ReadModel.POSerialNumber;
            BCType = ReadModel.BCType;
        }

        public void setPreparingId(GarmentPreparingId newPreparingId)
        {
            Validator.ThrowIfNull(() => newPreparingId);

            if(newPreparingId != PreparingId)
            {
                PreparingId = newPreparingId;
                ReadModel.PreparingId = newPreparingId.Value;
            }
        }

        public void setPreparingItemId(GarmentPreparingItemId newPreparingItemId)
        {
            Validator.ThrowIfNull(() => newPreparingItemId);

            if (newPreparingItemId != PreparingItemId)
            {
                PreparingItemId = newPreparingItemId;
                ReadModel.PreparingItemId = newPreparingItemId.Value;
            }
        }

        public void setProductId(ProductId newProductId)
        {
            Validator.ThrowIfNull(() => newProductId);

            if (newProductId != ProductId)
            {
                ProductId = newProductId;
                ReadModel.ProductId = newProductId.Value;
            }
        }

        public void setProductCode(string newProductCode)
        {
            Validator.ThrowIfNullOrEmpty(() => newProductCode);

            if (newProductCode != ProductCode)
            {
                ProductCode = newProductCode;
                ReadModel.ProductCode = newProductCode;
            }
        }

        public void setProductName(string newProductName)
        {
            Validator.ThrowIfNullOrEmpty(() => newProductName);

            if (newProductName != ProductName)
            {
                ProductName = newProductName;
                ReadModel.ProductName = newProductName;
            }
        }

        public void setDesignColor(string newDesignColor)
        {
            Validator.ThrowIfNullOrEmpty(() => newDesignColor);

            if (newDesignColor != DesignColor)
            {
                DesignColor = newDesignColor;
                ReadModel.DesignColor = newDesignColor;
            }
        }

        public void setQuantity(double newQuantity)
        {
            Validator.ThrowWhenTrue(() => newQuantity < 0);


            if (newQuantity != Quantity)
            {
                Quantity = newQuantity;
                ReadModel.Quantity = newQuantity;
            }
        }

        public void setUomId(UomId newUomId)
        {
            Validator.ThrowIfNull(() => newUomId);

            if(newUomId != UomId)
            {
                UomId = newUomId;
                ReadModel.UomId = newUomId.Value;
            }
        }

        public void setUomUnit(string newUomUnit)
        {
            Validator.ThrowIfNullOrEmpty(() => newUomUnit);

            if (newUomUnit != UomUnit)
            {
                UomUnit = newUomUnit;
                ReadModel.UomUnit = newUomUnit;
            }
        }

        public void SetIsReceived(bool IsReceived)
        {
            if (this.IsReceived != IsReceived)
            {
                this.IsReceived = IsReceived;
                ReadModel.IsReceived = IsReceived;
            }
        }

        public void SetBCNo(string newBCNo)
        {
            //Validator.ThrowIfNullOrEmpty(() => newBCNo);
            if (newBCNo != BCNo)
            {
                BCNo = newBCNo;
                ReadModel.BCNo = newBCNo;
            }
        }

        public void SetBCDate(DateTime newBCDate)
        {
            if (newBCDate != BCDate)
            {
                BCDate = newBCDate;
                ReadModel.BCDate = newBCDate;
            }
        }

        public void SetPOSerialNumber(string newPOSerialNumber)
        {
            //Validator.ThrowIfNullOrEmpty(() => newPOSerialNumber);
            if (newPOSerialNumber != POSerialNumber)
            {
                POSerialNumber = newPOSerialNumber;
                ReadModel.POSerialNumber = newPOSerialNumber;
            }
        }

        public void SetBCType(string newBCType)
        {
            //Validator.ThrowIfNullOrEmpty(() => newBCNo);
            if (newBCType != BCType)
            {
                BCType = newBCType;
                ReadModel.BCType = newBCType;
            }
        }

        public void SetDeleted()
        {
            MarkModified();
        }

        protected override GarmentAvalProductItem GetEntity()
        {
            return this;
        }
    }
}