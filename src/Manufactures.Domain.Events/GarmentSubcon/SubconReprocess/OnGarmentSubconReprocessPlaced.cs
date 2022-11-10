using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon.SubconReprocess
{
    public class OnGarmentSubconReprocessPlaced : IGarmentSubconReprocessEvent
    {
        public OnGarmentSubconReprocessPlaced(Guid garmentSubconReprocessId)
        {
            GarmentSubconReprocessId = garmentSubconReprocessId;
        }

        public Guid GarmentSubconReprocessId { get; }
    }
}

