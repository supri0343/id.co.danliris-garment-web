using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.ValueObjects
{
    public class GarmentSampleRequestSpecificationValueObject : ValueObject
    {
        public GarmentSampleRequestSpecificationValueObject()
        {
        }

        public Guid Id { get; set; }
        public Guid SampleRequestId { get; set; }
        public string Inventory { get; set; }
        public string SpecificationDetail { get; set; }
        public double Quantity { get; set; }
        public string Remark { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
