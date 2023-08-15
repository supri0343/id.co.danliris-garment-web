using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ValueObjects
{
    public class GarmentServiceSampleShrinkagePanelItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public string UnitExpenditureNo { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public UnitSender UnitSender { get; set; }
        public UnitRequest UnitRequest { get; set; }
        public List<GarmentServiceSampleShrinkagePanelDetailValueObject> Details { get; set; }

        public GarmentServiceSampleShrinkagePanelItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
