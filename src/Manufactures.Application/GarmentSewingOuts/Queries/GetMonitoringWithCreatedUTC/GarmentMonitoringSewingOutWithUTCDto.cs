using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSewingOuts.Queries.GetMonitoringWithCreatedUTC
{
    public class GarmentMonitoringSewingOutWithUTCDto
    {
        public Guid Id { get; set; }
        public string SewingOutNo { get; set; }
        public string SewingTo { get; set; }

        public UnitDepartment Unit { get; set; }
        public DateTimeOffset SewingOutDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment UnitTo { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public double TotalQuantity { get; set; }

        public List<Item> Items { get; set; }

        public class Item
        {
            public double Quantity { get; set; }
            public double RemainingQuantity { get; set; }
        }

    }
}
