using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects
{
    public class GarmentServiceSubconCuttingSizeValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ServiceSubconCuttingDetailId { get; set; }
        public SizeValueObject Size { get; internal set; }
        public double Quantity { get; internal set; }
        public Uom Uom { get; internal set; }
        public string Color { get; internal set; }
        public GarmentServiceSubconCuttingSizeValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
