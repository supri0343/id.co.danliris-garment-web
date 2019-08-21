using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.ValueObjects
{
    public class GarmentCuttingOutItemValueObject : ValueObject
    {
        public Guid CuttingInId { get;  set; }
        public Guid CutOutId { get;  set; }
        public Product ProductId { get;  set; }
        public string DesignColor { get;  set; }
        public double TotalCuttingOut { get;  set; }
        public double RemainingQuantity { get;  set; }

        public GarmentCuttingOutItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
