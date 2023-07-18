using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels
{
    public class GarmentServiceSampleCuttingItemReadModel : ReadModelBase
    {
        public GarmentServiceSampleCuttingItemReadModel(Guid identity) : base(identity)
        {
        }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }

        public Guid ServiceSampleCuttingId { get; internal set; }
        
        public virtual GarmentServiceSampleCuttingReadModel GarmentServiceSampleCutting { get; internal set; }
        public virtual List<GarmentServiceSampleCuttingDetailReadModel> GarmentServiceSampleCuttingDetail { get; internal set; }
    }
}
