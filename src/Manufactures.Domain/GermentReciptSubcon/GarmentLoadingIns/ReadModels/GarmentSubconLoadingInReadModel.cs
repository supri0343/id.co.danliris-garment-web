using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ReadModels
{
    public class GarmentSubconLoadingInReadModel : ReadModelBase
    {
        public GarmentSubconLoadingInReadModel(Guid identity) : base(identity)
        {

        }

        public string LoadingNo { get; internal set; }
        public Guid CuttingOutId { get; internal set; }
        public string CuttingOutNo { get; internal set; }
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
        public DateTimeOffset LoadingDate { get; internal set; }
        public bool IsApproved { get; internal set; }
        public string UId { get; private set; }
		public virtual List<GarmentSubconLoadingInItemReadModel> Items { get; internal set; }



    }
}
