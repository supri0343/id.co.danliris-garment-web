using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentPreparings.ValueObjects
{
    public class GarmentPreparingItemValueObject : ValueObject
    {
        public GarmentPreparingItemValueObject()
        {

        }


        public GarmentPreparingItemValueObject(Guid id, int preparingId, int uenItemId, ProductId productId, string designColor, double quantity, UomId uomId, string fabricType, double remainingQuantity, double basicPrice, Guid garmentPreparingId)
        {
            Identity = id;
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
        }

        public Guid Identity { get; set; }
        public int PreparingId { get; set; }
        public int UENItemId { get; set; }
        public ProductId ProductId { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public UomId UomId { get; set; }
        public string FabricType { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public Guid GarmentPreparingId { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}