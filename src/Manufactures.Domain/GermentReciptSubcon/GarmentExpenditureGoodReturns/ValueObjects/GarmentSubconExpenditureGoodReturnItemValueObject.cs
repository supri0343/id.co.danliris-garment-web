using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.ValueObjects
{
    public class GarmentSubconExpenditureGoodReturnItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ReturId { get; set; }
        public Guid ExpenditureGoodId { get; set; }
        public Guid ExpenditureGoodItemId { get; set; }
        public Guid FinishedGoodStockId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Description { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
        public bool isSave { get; set; }
        public double StockQuantity { get; set; }

        public GarmentSubconExpenditureGoodReturnItemValueObject()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
