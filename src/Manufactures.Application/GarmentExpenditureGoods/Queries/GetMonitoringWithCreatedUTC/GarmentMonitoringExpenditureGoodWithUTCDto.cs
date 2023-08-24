using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetMonitoringWithCreatedUTC
{
    public class GarmentMonitoringExpenditureGoodWithUTCDto
    {
        public Guid Id { get; set; }
        public string ExpenditureGoodNo { get; set; }
        public string ExpenditureType { get; set; }

        public UnitDepartment Unit { get; set; }
        public Buyer Buyer { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        
        public string RONo { get; set; }
        public string Article { get; set; }
        public string Invoice { get; set; }
        public string Description { get; set; }
        public double TotalQuantity { get; set; }
        

        public List<Item> Items { get; set; }

        public class Item
        {
            public double Quantity { get; set; }
        }

    }
}
