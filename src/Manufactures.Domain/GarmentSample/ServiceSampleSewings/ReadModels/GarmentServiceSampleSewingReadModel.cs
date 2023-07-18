using System;
using System.Collections.Generic;
using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.GarmentSewingIns.ReadModels;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels
{
    public class GarmentServiceSampleSewingReadModel : ReadModelBase
    {
        public GarmentServiceSampleSewingReadModel(Guid identity) : base(identity)
        {
        }

        public string ServiceSampleSewingNo { get; internal set; }
        public DateTimeOffset ServiceSampleSewingDate { get; internal set; }
        public bool IsUsed { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public int QtyPacking { get; internal set; }
        public string UomUnit { get; internal set; }
        public double NettWeight { get; internal set; }
        public double GrossWeight { get; internal set; }
        public virtual List<GarmentServiceSampleSewingItemReadModel> GarmentServiceSampleSewingItem { get; internal set; }

    }
}
