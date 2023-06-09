using System;
using System.Collections.Generic;
using Infrastructure.Domain.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.ReadModels
{
    public class GarmentServiceSubconExpenditureGoodItemReadModel : ReadModelBase
    {
        public GarmentServiceSubconExpenditureGoodItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSubconExpenditureGoodId { get; internal set; }
        public Guid FinishedGoodStockId { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }

        //public int BuyerId { get; internal set; }
        //public string BuyerCode { get; internal set; }
        //public string BuyerName { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }

        public string UomUnit { get; internal set; }
        public double Quantity { get; internal set; }
        public double BasicPrice { get; internal set; }
        public virtual GarmentServiceSubconExpenditureGoodReadModel GarmentServiceSubconExpenditureGoodIdentity { get; internal set; }
        //public virtual List<GarmentServiceSubconExpenditureGoodDetailReadModel> GarmentServiceSubconExpenditureGoodDetail { get; internal set; }
    }
}
