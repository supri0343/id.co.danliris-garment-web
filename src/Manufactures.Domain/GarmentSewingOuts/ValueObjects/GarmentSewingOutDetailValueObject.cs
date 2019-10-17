using Moonlay.Domain;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.ValueObjects
{
    public class GarmentSewingOutDetailValueObject : ValueObject
    {
        public Guid SewingOutItemId { get; internal set; }
        public SizeValueObject Size { get; internal set; }
        public double Quantity { get; internal set; }
        public Uom Uom { get; internal set; }
        public GarmentSewingOutDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
