using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentServiceSampleFabricWashPlaced : IGarmentServiceSampleFabricWashEvent
    {
        public OnGarmentServiceSampleFabricWashPlaced(Guid garmentServiceSampleFabricWashId)
        {
            GarmentServiceSampleFabricWashId = garmentServiceSampleFabricWashId;
        }

        public Guid GarmentServiceSampleFabricWashId { get; }
    }
}
