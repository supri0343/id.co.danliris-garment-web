using Moonlay.Domain;
using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects
{
    public class GarmentSubconDeliveryLetterOutItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SubconDeliveryLetterOutId { get; set; }
        public Guid UENItemId { get; set; }

        public Product Product { get; set; }
        public string ProductRemark { get; set; }

        public string DesignColor { get; set; }
        public double Quantity { get; set; }

        public Uom Uom { get; set; }

        public string FabricType { get; set; }
        public double ContractQuantity { get; set; }
        public GarmentSubconDeliveryLetterOutItemValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
