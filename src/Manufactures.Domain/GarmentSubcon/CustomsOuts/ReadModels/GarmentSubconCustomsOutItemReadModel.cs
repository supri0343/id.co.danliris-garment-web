using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels
{
    public class GarmentSubconCustomsOutItemReadModel : ReadModelBase
    {
        public GarmentSubconCustomsOutItemReadModel(Guid identity) : base(identity)
        {
        }
        public string SubconDLOuttNo { get; internal set; }
        public Guid SubconDLOuttId { get; internal set; }
        public double Quantity { get; internal set; }
    }
}
