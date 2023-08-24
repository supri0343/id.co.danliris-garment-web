using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetMonitoringWithCreatedUTC
{
    public class GarmentMonitoringFinishingOutWithUTCDto
    {
        public Guid Id { get; set; }
        public string FinishingOutNo { get; set; }
        public string FinishingTo { get; set; }

        public UnitDepartment Unit { get; set; }
        public DateTimeOffset FinishingOutDate { get; set; }
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
