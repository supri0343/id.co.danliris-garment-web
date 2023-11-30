using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentReceiptSubcon
{
    public class OnGarmentSubconPackingInPlaced : IGarmentSubconPackingInEvent
    {
        public OnGarmentSubconPackingInPlaced(Guid identity)
        {
            OnGarmentSubconPackingInId = identity;
        }
        public Guid OnGarmentSubconPackingInId { get; }
    }
}

