using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels
{
    public class GarmentServiceSubconCuttingDetailReadModel : ReadModelBase
    {
        public GarmentServiceSubconCuttingDetailReadModel(Guid identity) : base(identity)
        {
        }
        public Guid CuttingInDetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }

        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public Guid ServiceSubconCuttingItemId { get; internal set; }

        public virtual GarmentServiceSubconCuttingItemReadModel GarmentServiceSubconCuttingItem { get; internal set; }
    }
}
