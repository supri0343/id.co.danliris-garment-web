using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.ValueObjects
{
    public class GarmentSubconFinishingInItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid FinishingInId { get;  set; }
        public Guid SewingOutItemId { get;  set; }
        public Guid SewingOutDetailId { get;  set; }
        public Product Product { get;  set; }
        public string DesignColor { get;  set; }
        public SizeValueObject Size { get;  set; }
        public double Quantity { get;  set; }
        public Uom Uom { get;  set; }
        public string Color { get;  set; }
        public double SewingOutRemainingQuantity { get; set; }
        public double RemainingQuantity { get; set; }
        public bool IsSave { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }

        public GarmentSubconFinishingInItemValueObject()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
