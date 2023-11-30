using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ValueObjects
{
    public class GarmentSubconFinishingOutDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid FinishingOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public GarmentSubconFinishingOutDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
