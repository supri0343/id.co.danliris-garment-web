using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels
{
    public class GarmentSubconCustomsOutDetailReadModel : ReadModelBase
    {
        public GarmentSubconCustomsOutDetailReadModel(Guid identity) : base(identity)
        {
        }
        public Guid SubconCustomsOutItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string ProductRemark { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }

        public virtual GarmentSubconCustomsOutItemReadModel GarmentSubconCustomsOutItem { get; internal set; }
    }
}
