using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ValueObjects
{
    public class GarmentServiceSampleCuttingItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ServiceSampleCuttingId { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public List<GarmentServiceSampleCuttingDetailValueObject> Details { get; set; }
        public GarmentServiceSampleCuttingItemValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
