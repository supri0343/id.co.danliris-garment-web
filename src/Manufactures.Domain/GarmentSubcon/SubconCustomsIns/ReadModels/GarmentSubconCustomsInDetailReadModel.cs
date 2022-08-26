using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels
{
    public class GarmentSubconCustomsInDetailReadModel : ReadModelBase
    {
        public GarmentSubconCustomsInDetailReadModel(Guid identity) : base(identity)
        {
        }

        public int SubconCustomsOutId { get; internal set; }
        public string CustomsOutNo { get; internal set; }
        public decimal CustomsOutQty { get; internal set; }

        public Guid SubconCustomsInItemId { get; internal set; }
        public virtual GarmentSubconCustomsInItemReadModel GarmentSubconCustomsInItem { get; internal set; }
    }
}
