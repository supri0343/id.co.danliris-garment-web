using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels
{
    public class GarmentServiceSubconCuttingReadModel : ReadModelBase
    {
        public GarmentServiceSubconCuttingReadModel(Guid identity) : base(identity)
        {
        }
        public string SubconNo { get; internal set; }
        public string SubconType { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset SubconDate { get; internal set; }
        public virtual List<GarmentServiceSubconCuttingItemReadModel> GarmentServiceSubconCuttingItem { get; internal set; }
    }
}
