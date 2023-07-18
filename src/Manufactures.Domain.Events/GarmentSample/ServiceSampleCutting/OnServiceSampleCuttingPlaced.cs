using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnServiceSampleCuttingPlaced : IServiceSampleCuttingEvent
    {
        public OnServiceSampleCuttingPlaced(Guid identity)
        {
            OnServiceSampleCuttingId = identity;
        }
        public Guid OnServiceSampleCuttingId { get; }
    }
}
