using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.ValueObjects
{
    public class GarmentCuttingOutDetailValueObject : ValueObject
    {

        public Guid CuttingInItemId { get;  set; }
        public Guid CutOutItemId { get;  set; }

        public Size Size { get;  set; }
        public string Color { get;  set; }

        public double RemainingQuantity { get;  set; }
        public double CuttingOutQuantity { get;  set; }
        public double BasicPrice { get;  set; }
        public double IndirectPrice { get;  set; }
        public double OTL1 { get;  set; }
        public double OTL2 { get;  set; }

        public Uom CuttingOutUom { get;  set; }

        public GarmentCuttingOutDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
