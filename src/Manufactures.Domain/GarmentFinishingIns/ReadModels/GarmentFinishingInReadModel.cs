using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns.ReadModels
{
    public class GarmentFinishingInReadModel : ReadModelBase
    {
        public GarmentFinishingInReadModel(Guid identity) : base(identity)
        {

        }
        public string FinishingInNo { get; internal set; }
        public string FinishingInType { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public Guid SewingOutId { get; internal set; }
        public string SewingOutNo { get; internal set; }
        public int UnitFromId { get; internal set; }
        public string UnitFromCode { get; internal set; }
        public string UnitFromName { get; internal set; }
        public string Article { get; internal set; }
        public string RONo { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset FinishingInDate { get; internal set; }

        public virtual List<GarmentFinishingInItemReadModel> Items { get; internal set; }

    }
}
