using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentReceiptSubcon
{
    public class OnGarmentSubconPackingOutPlaced : IGarmentSubconPackingOutEvent
    {
        public OnGarmentSubconPackingOutPlaced(Guid identity)
        {
            OnGarmentSubconPackingOutId = identity;
        }
        public Guid OnGarmentSubconPackingOutId { get; }
    }
}

