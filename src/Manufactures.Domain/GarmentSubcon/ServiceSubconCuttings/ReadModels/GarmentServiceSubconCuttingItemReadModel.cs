using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels
{
    public class GarmentServiceSubconCuttingItemReadModel : ReadModelBase
    {
        public GarmentServiceSubconCuttingItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid ServiceSubconCuttingId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }

        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public virtual GarmentServiceSubconCuttingReadModel GarmentServiceSubconCutting { get; internal set; }
    }
}
