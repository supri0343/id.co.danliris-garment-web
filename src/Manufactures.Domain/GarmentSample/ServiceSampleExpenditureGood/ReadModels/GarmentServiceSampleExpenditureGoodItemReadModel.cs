using System;
using System.Collections.Generic;
using Infrastructure.Domain.ReadModels;

namespace Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ReadModels
{
    public class GarmentServiceSampleExpenditureGoodItemReadModel : ReadModelBase
    {
        public GarmentServiceSampleExpenditureGoodItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSampleExpenditureGoodId { get; internal set; }
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
        public virtual GarmentServiceSampleExpenditureGoodReadModel GarmentServiceSampleExpenditureGoodIdentity { get; internal set; }
        //public virtual List<GarmentServiceSampleExpenditureGoodDetailReadModel> GarmentServiceSampleExpenditureGoodDetail { get; internal set; }
    }
}
