using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels
{
    public class GarmentServiceSampleCuttingDetailReadModel : ReadModelBase
    {
        public GarmentServiceSampleCuttingDetailReadModel(Guid identity) : base(identity)
        {
        }
        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public Guid ServiceSampleCuttingItemId { get; internal set; }

        public virtual GarmentServiceSampleCuttingItemReadModel GarmentServiceSampleCuttingItem { get; internal set; }
        public virtual List<GarmentServiceSampleCuttingSizeReadModel> GarmentServiceSampleCuttingSizes { get; internal set; }
    }
}
