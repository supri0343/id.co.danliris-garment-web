using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels
{
    public class GarmentServiceSampleFabricWashReadModel : ReadModelBase
    {
        public GarmentServiceSampleFabricWashReadModel(Guid identity) : base(identity)
        {
        }
        public string ServiceSampleFabricWashNo { get; internal set; }
        public DateTimeOffset ServiceSampleFabricWashDate { get; internal set; }
        public string Remark { get; internal set; }
        public bool IsUsed { get; internal set; }
        public int QtyPacking { get; internal set; }
        public string UomUnit { get; internal set; }
        public double NettWeight { get; internal set; }
        public double GrossWeight { get; internal set; }

        public virtual List<GarmentServiceSampleFabricWashItemReadModel> GarmentServiceSampleFabricWashItem { get; internal set; }
    }
}
