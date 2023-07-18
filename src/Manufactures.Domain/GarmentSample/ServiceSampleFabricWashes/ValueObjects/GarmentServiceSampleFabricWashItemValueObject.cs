using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ValueObjects
{
    public class GarmentServiceSampleFabricWashItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public string UnitExpenditureNo { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public UnitSender UnitSender { get; set; }
        public UnitRequest UnitRequest { get; set; }
        public List<GarmentServiceSampleFabricWashDetailValueObject> Details { get; set; }

        public GarmentServiceSampleFabricWashItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
