using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels
{
    public class GarmentSubconCuttingReadModel : ReadModelBase
    {
        public GarmentSubconCuttingReadModel(Guid identity) : base(identity)
        {
        }
        public string RONo { get; internal set; }
        public string SizeName { get; internal set; }
        public int SizeId { get; internal set; }
        public double Quantity { get; internal set; }

    }
}
