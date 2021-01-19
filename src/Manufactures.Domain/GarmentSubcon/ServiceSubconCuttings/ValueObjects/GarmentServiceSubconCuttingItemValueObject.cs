using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects
{
    public class GarmentServiceSubconCuttingItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ServiceSubconCuttingId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }

        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public double CuttingInQuantity { get; set; }
        public bool IsSave { get; set; }
        public GarmentServiceSubconCuttingItemValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
