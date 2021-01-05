using System;
using System.Collections.Generic;
using Moonlay.Domain;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects
{
    public class GarmentServiceSewingItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ServiceSubconSewingId { get; set; }
        public Guid SewingInId { get; set; }
        public Guid SewingInItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public bool IsSave { get; set; }
        public double SewingInQuantity { get; set; }
        public double TotalQuantity { get; set; }

        public GarmentServiceSewingItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
