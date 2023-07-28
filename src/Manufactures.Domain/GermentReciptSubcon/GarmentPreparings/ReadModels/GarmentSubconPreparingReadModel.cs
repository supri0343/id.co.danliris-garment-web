using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels
{
    public class GarmentSubconPreparingReadModel : ReadModelBase
    {
        public GarmentSubconPreparingReadModel(Guid identity) : base(identity)
        {

        }

        public int UENId {get; internal set;}
        public string UENNo { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public int ProductOwnerId { get; internal set; }
        public string ProductOwnerCode { get; internal set; }
        public string ProductOwnerName { get; internal set; }
        public DateTimeOffset? ProcessDate { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public bool IsCuttingIn { get; internal set; }
		public string UId { get; internal set; }
		public virtual List<GarmentSubconPreparingItemReadModel> GarmentPreparingItem { get; internal set; }

    }
}