using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings
{
    public class GarmentSubconPreparingItem : AggregateRoot<GarmentSubconPreparingItem, GarmentSubconPreparingItemReadModel>
    {
        public int UENItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string FabricType { get; private set; }
        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public Guid GarmentSubconPreparingId { get; private set; }
        public string ROSource { get; private set; }
        public string BeacukaiNo { get; private set; }
        public DateTimeOffset BeacukaiDate { get; private set; }
        public string BeacukaiType { get; private set; }
        public GarmentSubconPreparingItem(Guid identity, int uenItemId, ProductId productId, string productCode, string productName, string designColor, double quantity, UomId uomId, string uomUnit, string fabricType, double remainingQuantity, double basicPrice, Guid garmentPreparingId, string roSource,string beacukaiNo,DateTimeOffset beacukaiDate,string beacukaiType) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            UENItemId = uenItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            FabricType = fabricType;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            GarmentSubconPreparingId = garmentPreparingId;
            ROSource = roSource;
            BeacukaiNo = beacukaiNo;
            BeacukaiDate = beacukaiDate;
            BeacukaiType = beacukaiType;
            ReadModel = new GarmentSubconPreparingItemReadModel(Identity)
            {
                UENItemId = UENItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                FabricType = FabricType,
                RemainingQuantity = RemainingQuantity,
                BasicPrice = BasicPrice,
                GarmentSubconPreparingId = GarmentSubconPreparingId,
                ROSource= ROSource,
                BeacukaiNo = BeacukaiNo,
                BeacukaiDate = BeacukaiDate,
                BeacukaiType = BeacukaiType,
            };
            ReadModel.AddDomainEvent(new OnGarmentPreparingPlaced(this.Identity));
        }

        public GarmentSubconPreparingItem(GarmentSubconPreparingItemReadModel readModel) : base(readModel)
        {
            UENItemId = ReadModel.UENItemId;
            ProductId = new ProductId(ReadModel.ProductId);
            ProductCode = ReadModel.ProductCode;
            ProductName = ReadModel.ProductName;
            DesignColor = ReadModel.DesignColor;
            Quantity = ReadModel.Quantity;
            UomId = new UomId(ReadModel.UomId);
            UomUnit = ReadModel.UomUnit;
            FabricType = ReadModel.FabricType;
            RemainingQuantity = ReadModel.RemainingQuantity;
            BasicPrice = ReadModel.BasicPrice;
            GarmentSubconPreparingId = ReadModel.GarmentSubconPreparingId;
            ROSource = ReadModel.ROSource;
            BeacukaiNo = ReadModel.BeacukaiNo;
            BeacukaiDate = ReadModel.BeacukaiDate;
            BeacukaiType = ReadModel.BeacukaiType;
        }

        public void setUenItemId(int newUenItemId)
        {
            Validator.ThrowIfNull(() => newUenItemId);

            if (newUenItemId != UENItemId)
            {
                UENItemId = newUenItemId;
                ReadModel.UENItemId = newUenItemId;
            }
        }

        public void setProduct(ProductId newProduct)
        {
            Validator.ThrowIfNull(() => newProduct);

            if (newProduct != ProductId)
            {
                ProductId = newProduct;
                ReadModel.ProductId = newProduct.Value;
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
            Validator.ThrowIfNull(() => newQuantity);

            if (newQuantity != Quantity)
            {
                Quantity = newQuantity;
                ReadModel.Quantity = newQuantity;
            }
        }

        public void setUomId(UomId newUom)
        {
            Validator.ThrowIfNull(() => newUom);

            if (newUom != UomId)
            {
                UomId = newUom;
                ReadModel.UomId = newUom.Value;
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

        public void setFabricType(string newFabricType)
        {
            Validator.ThrowIfNullOrEmpty(() => newFabricType);

            if (newFabricType != FabricType)
            {
                FabricType = newFabricType;
                ReadModel.FabricType = newFabricType;
            }
        }

        public void SetModified()
        {
            MarkModified();
        }

        public void setRemainingQuantity(double newRemainingQuantity)
        {
            //Validator.ThrowIfNull(() => newRemainingQuantity);

            if (newRemainingQuantity != RemainingQuantity)
            {
                RemainingQuantity = newRemainingQuantity;
                ReadModel.RemainingQuantity = newRemainingQuantity;
            }
        }

        public void setRemainingQuantityZeroValue(double newRemainingQuantity)
        {
            if (newRemainingQuantity != RemainingQuantity)
            {
                RemainingQuantity = newRemainingQuantity;
                ReadModel.RemainingQuantity = newRemainingQuantity;
            }
        }

        public void setBasicPrice(double newBasicPrice)
        {
            Validator.ThrowIfNull(() => newBasicPrice);

            if (newBasicPrice != BasicPrice)
            {
                BasicPrice = newBasicPrice;
                ReadModel.BasicPrice = newBasicPrice;
            }
        }

        public void setBCNo(string newBCNo)
        {
            if (newBCNo != BeacukaiNo)
            {
                BeacukaiNo = newBCNo;
                ReadModel.BeacukaiNo = newBCNo;
            }
        }
        public void setBCType(DateTimeOffset newBCDate)
        {
            if (newBCDate != BeacukaiDate)
            {
                BeacukaiDate = newBCDate;
                ReadModel.BeacukaiDate = newBCDate;
            }
        }
        public void setBCType(string newBCType)
        {
            if (newBCType != BeacukaiType)
            {
                BeacukaiType = newBCType;
                ReadModel.BeacukaiType = newBCType;
            }
        }


        public void SetDeleted()
        {
            MarkModified();
        }

        protected override GarmentSubconPreparingItem GetEntity()
        {
            return this;
        }
    }
}