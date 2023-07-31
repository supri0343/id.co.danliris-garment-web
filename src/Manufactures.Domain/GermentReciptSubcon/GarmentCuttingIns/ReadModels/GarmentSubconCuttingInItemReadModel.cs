using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels
{
    public class GarmentSubconCuttingInItemReadModel : ReadModelBase
    {
        public GarmentSubconCuttingInItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutInId { get; internal set; }
        public Guid PreparingId { get; internal set; }
        public Guid SewingOutId { get; internal set; }
        public string SewingOutNo { get; internal set; }
        public int UENId { get; internal set; }
        public string UENNo { get; internal set; }
		public string UId { get; private set; }
		public virtual GarmentSubconCuttingInReadModel GarmentCuttingIn { get; internal set; }
        public virtual ICollection<GarmentSubconCuttingInDetailReadModel> Details { get; internal set; }
    }
}
