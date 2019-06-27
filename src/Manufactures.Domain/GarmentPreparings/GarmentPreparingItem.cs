using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GarmentPreparings
{
    public class GarmentPreparingItem : AggregateRoot<GarmentPreparingItem, GarmentPreparingItemReadModel>
    {
        public int UENItemId { get; private set; }
        public ProductId Product { get; private set; }
        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }
        public UomId Uom { get; private set; }
        public string FabricType { get; private set; }
        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public Guid GarmentPreparingId { get; private set; }

        public GarmentPreparingItem(Guid identity, int uenItemId, ProductId product, string designColor, double quantity, UomId uom, string fabricType, double remainingQuantity, double basicPrice, Guid garmentPreparingId) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            UENItemId = uenItemId;
            Product = product;
            DesignColor = designColor;
            Quantity = quantity;
            Uom = uom;
            FabricType = fabricType;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            GarmentPreparingId = garmentPreparingId;

            ReadModel = new GarmentPreparingItemReadModel(Identity)
            {
                UENItemId = UENItemId,
                ProductId = Product.Value,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = Uom.Value,
                FabricType = FabricType,
                RemainingQuantity = RemainingQuantity,
                BasicPrice = BasicPrice,
                GarmentPreparingId = GarmentPreparingId,
            };
            ReadModel.AddDomainEvent(new OnGarmentPreparingPlaced(this.Identity));
        }

        public GarmentPreparingItem(GarmentPreparingItemReadModel readModel) : base(readModel)
        {
            UENItemId = ReadModel.UENItemId;
            Product = new ProductId(ReadModel.ProductId);
            DesignColor = ReadModel.DesignColor;
            Quantity = ReadModel.Quantity;
            Uom = new UomId(ReadModel.UomId);
            FabricType = ReadModel.FabricType;
            RemainingQuantity = ReadModel.RemainingQuantity;
            BasicPrice = ReadModel.BasicPrice;
            GarmentPreparingId = ReadModel.GarmentPreparingId;
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

            if (newProduct != Product)
            {
                Product = newProduct;
                ReadModel.ProductId = newProduct.Value;
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

            if (newUom != Uom)
            {
                Uom = newUom;
                ReadModel.UomId = newUom.Value;
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

        public void SetDeleted()
        {
            MarkModified();
        }

        protected override GarmentPreparingItem GetEntity()
        {
            return this;
        }
    }
}