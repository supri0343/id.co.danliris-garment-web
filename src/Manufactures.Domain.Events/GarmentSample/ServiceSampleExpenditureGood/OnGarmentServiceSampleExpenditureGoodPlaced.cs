using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentServiceSampleExpenditureGoodPlaced : IGarmentServiceSampleExpenditureGoodEvent
    {
        public OnGarmentServiceSampleExpenditureGoodPlaced(Guid garmentServiceSampleExpenditureGoodId)
        {
            GarmentServiceSampleExpenditureGoodId = garmentServiceSampleExpenditureGoodId;
        }

        public Guid GarmentServiceSampleExpenditureGoodId { get; }
    }
}
