using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels
{
    public class GarmentSubconReprocessReadModel : ReadModelBase
    {
        public GarmentSubconReprocessReadModel(Guid identity) : base(identity)
        {
        }
        public string ReprocessNo { get; internal set; }
        public string ReprocessType { get; internal set; }
        public DateTimeOffset Date { get; internal set; }
        public virtual List<GarmentSubconReprocessItemReadModel> GarmentSubconReprocessItem { get; internal set; }
    }
}
