using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentLoadings.Queries.GetMonitoringWithCreatedUTC
{
    public class GarmentMonitoringLoadingListDto
    {
        public Guid Id { get; set; }
        public string LoadingNo { get; set; }

        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset LoadingDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public double TotalLoadingQuantity { get; set; }

        public List<Item> Items { get; set; }

        public class Item
        {
            public double Quantity { get; set; }
            public double RemainingQuantity { get; set; }
        }
    }
}
