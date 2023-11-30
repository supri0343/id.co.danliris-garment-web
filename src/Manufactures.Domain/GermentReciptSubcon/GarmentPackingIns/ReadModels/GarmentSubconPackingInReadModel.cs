using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels
{
    public class GarmentSubconPackingInReadModel : ReadModelBase
    {
        public GarmentSubconPackingInReadModel(Guid identity) : base(identity)
        {

        }
        public string PackingInNo { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public int UnitFromId { get; internal set; }
        public string UnitFromCode { get; internal set; }
        public string UnitFromName { get; internal set; }
        public string PackingFrom { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset PackingInDate { get; internal set; }
        public bool IsApproved { get; internal set; }
        public virtual List<GarmentSubconPackingInItemReadModel> GarmentSubconPackingInItem { get; internal set; }
    }
}
