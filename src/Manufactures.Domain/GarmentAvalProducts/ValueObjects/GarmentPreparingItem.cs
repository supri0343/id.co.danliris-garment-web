using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.ValueObjects
{
    public class GarmentPreparingItem : ValueObject
    {
        public GarmentPreparingItem()
        {

        }

        public GarmentPreparingItem(string preparingItemId, ProductId productId, string designColor, double quantity)
        {
            Id = preparingItemId;
            ProductId = productId;
            DesignColor = designColor;
            Quantity = quantity;
        }

        public string Id { get; }
        public ProductId ProductId { get; }
        public string DesignColor { get; }
        public double Quantity { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return ProductId;
            yield return DesignColor;
            yield return Quantity;
        }
    }
}