using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ValueObjects
{
    public class GarmentSubconCuttingInItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid PreparingId { get; set; }
        public int UENId { get; set; }
        public string UENNo { get; set; }
        public Guid SewingOutId { get; set; }
        public string SewingOutNo { get; set; }
        public List<GarmentSubconCuttingInDetailValueObject> Details { get; set; }

        public GarmentSubconCuttingInItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
