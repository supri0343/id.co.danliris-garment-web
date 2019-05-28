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
        public int PreparingId { get; private set; }
        public int UENItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string FabricType { get; private set; }
        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public Guid GarmentPreparingId { get; private set; }

        public GarmentPreparingItem(Guid identity, int preparingId, int uenItemId, ProductId productId, string designColor, double quantity, UomId uomId, string fabricType, double remainingQuantity, double basicPrice, Guid garmentPreparingId) : base(identity)
        {
            Validator.ThrowIfNull(() => preparingId);
            this.MarkTransient();

            Identity = identity;
            PreparingId = preparingId;
            UENItemId = uenItemId;
            ProductId = productId;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            FabricType = fabricType;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            GarmentPreparingId = garmentPreparingId;

            ReadModel = new GarmentPreparingItemReadModel(Identity)
            {
                PreparingId = PreparingId,
                UENItemId = UENItemId,
                ProductId = ProductId.Value,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                FabricType = FabricType,
                RemainingQuantity = RemainingQuantity,
                BasicPrice = BasicPrice,
                GarmentPreparingId = GarmentPreparingId,
            };
            ReadModel.AddDomainEvent(new OnGarmentPreparingPlaced(this.Identity));
        }

        public GarmentPreparingItem(GarmentPreparingItemReadModel readModel) : base(readModel)
        {
            PreparingId = ReadModel.PreparingId;
            UENItemId = ReadModel.UENItemId;
            ProductId = new ProductId(ReadModel.ProductId);
            DesignColor = ReadModel.DesignColor;
            Quantity = ReadModel.Quantity;
            UomId = new UomId(ReadModel.UomId);
            FabricType = ReadModel.FabricType;
            RemainingQuantity = ReadModel.RemainingQuantity;
            BasicPrice = ReadModel.BasicPrice;
            GarmentPreparingId = ReadModel.GarmentPreparingId;
        }

        public void setPreparingId(int newPreparingId)
        {
            Validator.ThrowIfNull(() => newPreparingId);

            if (newPreparingId != PreparingId)
            {
                PreparingId = newPreparingId;
                ReadModel.PreparingId = newPreparingId;
            }
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

        public void setProductId(ProductId newProductId)
        {
            Validator.ThrowIfNull(() => newProductId);

            if (newProductId != ProductId)
            {
                ProductId = newProductId;
                ReadModel.ProductId = newProductId.Value;
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

        public void setUomId(UomId newUomId)
        {
            Validator.ThrowIfNull(() => newUomId);

            if (newUomId != UomId)
            {
                UomId = newUomId;
                ReadModel.UomId = newUomId.Value;
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

        public void setRemainingQuantity(double newRemainingQuantity)
        {
            Validator.ThrowIfNull(() => newRemainingQuantity);

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