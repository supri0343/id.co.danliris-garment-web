using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentAvalProducts.ValueObjects
{
    public class GarmentAvalProductItemValueObject : ValueObject
    {
        public GarmentAvalProductItemValueObject()
        {

        }

        public GarmentAvalProductItemValueObject(Guid id, Guid apId, GarmentPreparingId preparingId, GarmentPreparingItemId preparingItemId, ProductId productId, string designColor, double quantity, UomId uomId)
        {

        }

        public Guid Identity { get; set; }
        public Guid APId { get; set; }
        public GarmentPreparingId PreparingId { get; set; }
        public GarmentPreparingItemId PreparingItemId { get; set; }
        public ProductId ProductId { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public UomId UomId { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}