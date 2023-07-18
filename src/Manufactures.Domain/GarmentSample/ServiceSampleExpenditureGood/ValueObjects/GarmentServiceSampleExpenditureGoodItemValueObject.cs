using System;
using System.Collections.Generic;
using Moonlay.Domain;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ValueObjects
{
    public class GarmentServiceSampleExpenditureGoodItemValueObject : ValueObject
    {

        public Guid Id { get;  set; }
        public Guid FinishingGoodStockId { get; set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }

        //public BuyerId BuyerId { get; private set; }
        //public string BuyerCode { get; private set; }
        //public string BuyerName { get; private set; }
        public GarmentComodity Comodity { get; set; }
        public UnitDepartment Unit { get; set; }
        public string UomUnit { get;  set; }
        public double Quantity { get;  set; }
        public double BasicPrice { get;  set; }
        public double StockQuantity { get; set; }

        public GarmentServiceSampleExpenditureGoodItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
