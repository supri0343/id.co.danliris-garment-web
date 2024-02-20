using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.ValueObjects
{
    public class GarmentSubconCustomsOutDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SubconCustomsOutItemId { get; set; }
        public Product Product { get; set; }
        public string ProductRemark { get; set; }
        public Uom Uom { get; set; }
        public double Quantity { get; set; }
        public GarmentSubconCustomsOutDetailValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
