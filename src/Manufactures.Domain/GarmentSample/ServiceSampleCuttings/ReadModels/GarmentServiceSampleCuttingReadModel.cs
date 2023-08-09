using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels
{
    public class GarmentServiceSampleCuttingReadModel : ReadModelBase
    {
        public GarmentServiceSampleCuttingReadModel(Guid identity) : base(identity)
        {
        }
        public string SampleNo { get; internal set; }
        public string SampleType { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        
        public DateTimeOffset SampleDate { get; internal set; }

        public bool IsUsed { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public int QtyPacking { get; internal set; }
        public double NettWeight { get; internal set; }
        public double GrossWeight { get; internal set; }
        public string Remark { get; internal set; }
        public virtual List<GarmentServiceSampleCuttingItemReadModel> GarmentServiceSampleCuttingItem { get; internal set; }
    }
}
