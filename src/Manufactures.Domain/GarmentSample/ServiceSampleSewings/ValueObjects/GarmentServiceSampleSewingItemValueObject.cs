using System;
using System.Collections.Generic;
using Moonlay.Domain;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings.ValueObjects
{
    public class GarmentServiceSampleSewingItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public Buyer Buyer { get; set; }
        public UnitDepartment Unit { get; set; }
        public List<GarmentServiceSampleSewingDetailValueObject> Details { get; set; }

        public GarmentServiceSampleSewingItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
