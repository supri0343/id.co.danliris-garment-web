using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.ValueObjects
{
    public class GarmentSubconReprocessDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ReprocessItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public double ReprocessQuantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public string DesignColor { get; set; }
        public string Type { get; set; }

        //KOMPONEN (CUTTING)
        public Guid ServiceSubconCuttingDetailId { get; set; }
        public Guid ServiceSubconCuttingSizeId { get; set; }

        //WASH (SEWING)
        public Guid ServiceSubconSewingDetailId { get; set; }

        public UnitDepartment Unit { get; set; }
        public string Remark { get; set; }

        public double RemQty { get; set; }
        public GarmentSubconReprocessDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }

    }
}
