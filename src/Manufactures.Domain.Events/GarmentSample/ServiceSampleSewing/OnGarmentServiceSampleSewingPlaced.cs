using System;
namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentServiceSampleSewingPlaced : IGarmentServiceSampleSewingEvent
    {
        public OnGarmentServiceSampleSewingPlaced(Guid garmentServiceSampleSewingId)
        {
            GarmentServiceSampleSewingId = garmentServiceSampleSewingId;
        }

        public Guid GarmentServiceSampleSewingId { get; }
    }
}
