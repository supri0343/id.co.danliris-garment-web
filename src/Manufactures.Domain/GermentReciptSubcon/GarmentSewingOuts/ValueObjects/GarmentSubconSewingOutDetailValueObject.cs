using Moonlay.Domain;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ValueObjects
{
    public class GarmentSubconSewingOutDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SewingOutItemId { get;  set; }
        public SizeValueObject Size { get;  set; }
        public double Quantity { get;  set; }
        public double RealQtyOut { get; set; }
        public Uom Uom { get;  set; }
        public GarmentSubconSewingOutDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
