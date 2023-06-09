using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon
{
    public class OnGarmentServiceSubconExpenditureGoodPlaced : IGarmentServiceSubconExpenditureGoodEvent
    {
        public OnGarmentServiceSubconExpenditureGoodPlaced(Guid garmentServiceSubconExpenditureGoodId)
        {
            GarmentServiceSubconExpenditureGoodId = garmentServiceSubconExpenditureGoodId;
        }

        public Guid GarmentServiceSubconExpenditureGoodId { get; }
    }
}
