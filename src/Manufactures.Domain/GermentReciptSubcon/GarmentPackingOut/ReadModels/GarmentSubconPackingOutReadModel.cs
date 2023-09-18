using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut.ReadModels
{
    public class GarmentSubconPackingOutReadModel : ReadModelBase
    {
        public GarmentSubconPackingOutReadModel(Guid identity) : base(identity)
        {
        }

        public string PackingOutNo { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string PackingOutType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public int ProductOwnerId { get; internal set; }
        public string ProductOwnerCode { get; internal set; }
        public string ProductOwnerName { get; internal set; }
        public DateTimeOffset PackingOutDate { get; internal set; }
        public string Invoice { get; internal set; }
        public int PackingListId { get; internal set; }
        public string ContractNo { get; internal set; }
        public double Carton { get; internal set; }
        public string Description { get; internal set; }
        public bool IsReceived { get; internal set; }
		public string UId { get; set; }
		public virtual List<GarmentSubconPackingOutItemReadModel> Items { get; internal set; }


    }
}
