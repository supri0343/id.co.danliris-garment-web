using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels
{
    public class GarmentSubconLoadingOutReadModel : ReadModelBase
    {
        public GarmentSubconLoadingOutReadModel(Guid identity) : base(identity)
        {

        }

        public string LoadingOutNo { get; internal set; }
        public Guid LoadingInId { get; internal set; }
        public string LoadingInNo { get; internal set; }
        public int UnitFromId { get; internal set; }
        public string UnitFromCode { get; internal set; }
        public string UnitFromName { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityName { get; internal set; }
        public string ComodityCode { get; internal set; }
        public DateTimeOffset LoadingOutDate { get; internal set; }
        public string UId { get; private set; }
		public virtual List<GarmentSubconLoadingOutItemReadModel> Items { get; internal set; }



    }
}
