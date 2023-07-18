using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentServiceSampleShrinkagePanelPlaced : IGarmentServiceSampleShrinkagePanelEvent
    {
        public OnGarmentServiceSampleShrinkagePanelPlaced(Guid garmentServiceSampleShrinkagePanelId)
        {
            GarmentServiceSampleShrinkagePanelId = garmentServiceSampleShrinkagePanelId;
        }

        public Guid GarmentServiceSampleShrinkagePanelId { get; }
    }
}
