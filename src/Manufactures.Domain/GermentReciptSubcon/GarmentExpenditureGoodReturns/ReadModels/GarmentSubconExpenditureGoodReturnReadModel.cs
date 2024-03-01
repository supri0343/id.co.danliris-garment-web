using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.ReadModels
{
    public class GarmentSubconExpenditureGoodReturnReadModel : ReadModelBase
    {
        public GarmentSubconExpenditureGoodReturnReadModel(Guid identity) : base(identity)
        {
        }

        public string ReturNo { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string ReturType { get; internal set; }
        public string PackingOutNo { get; internal set; }
        public string DONo { get; internal set; }
        public string BCNo { get; internal set; }
        public string BCType { get; internal set; }
        public string URNNo { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public DateTimeOffset ReturDate { get; internal set; }
        public string Invoice { get; internal set; }
        public string ContractNo { get; internal set; }
        public string ReturDesc { get; internal set; }
		public string UId { get; set; }
		public virtual List<GarmentSubconExpenditureGoodReturnItemReadModel> Items { get; internal set; }

    }
}
