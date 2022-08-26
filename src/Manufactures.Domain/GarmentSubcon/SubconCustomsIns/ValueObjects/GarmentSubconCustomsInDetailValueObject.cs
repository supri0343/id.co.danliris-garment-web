using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ValueObjects
{
    public class GarmentSubconCustomsInDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SubconCustomsInItemId { get; set; }
        public Guid SubconCustomsOutId { get; set; }
        public string CustomsOutNo { get; set; }
        public decimal CustomsOutQty { get; set; }

        public GarmentSubconCustomsInDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
