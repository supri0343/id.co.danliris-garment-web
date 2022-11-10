using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.ValueObjects
{
    public class GarmentSubconReprocessItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ReprocessId { get;  set; }
        //WASH/SEWING
        public Guid ServiceSubconSewingId { get;  set; }
        public string ServiceSubconSewingNo { get;  set; }
        public Guid ServiceSubconSewingItemId { get;  set; }

        //KOMPONEN/CUTTING
        public Guid ServiceSubconCuttingId { get;  set; }
        public string ServiceSubconCuttingNo { get;  set; }
        public Guid ServiceSubconCuttingItemId { get;  set; }

        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public string Type { get; set; }
        public Buyer Buyer { get; set; }
        public List<GarmentSubconReprocessDetailValueObject> Details { get; set; }

        public GarmentSubconReprocessItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
