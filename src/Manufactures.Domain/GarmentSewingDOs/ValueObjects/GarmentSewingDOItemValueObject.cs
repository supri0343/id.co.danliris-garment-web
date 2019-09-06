using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingDOs.ValueObjects
{
    public class GarmentSewingDOItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public string SewingDOId { get; set; }
        public string CuttingOutDetailId { get; set; }
        public string CuttingOutItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public string RemainingQuantity { get; set; }
        public string BasicPrice { get; set; }

        public GarmentSewingDOItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}