using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood
{
    public class GarmentServiceSampleExpenditureGoodItem : AggregateRoot<GarmentServiceSampleExpenditureGoodItem, GarmentServiceSampleExpenditureGoodItemReadModel>
    {
        public Guid ServiceSampleExpenditureGoodId { get; private set; }
        public Guid FinishedGoodStockId { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        //public BuyerId BuyerId { get; private set; }
        //public string BuyerCode { get; private set; }
        //public string BuyerName { get; private set; }
        
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }

        public string UomUnit { get; private set; }
        public double Quantity { get; private set; }
        public double BasicPrice { get; private set; }


        public GarmentServiceSampleExpenditureGoodItem(Guid identity, Guid serviceSampleExpenditureGoodId,Guid finishedGoodStockId, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, /*BuyerId buyerId, string buyerCode, string buyerName,*/ UnitDepartmentId unitId, string unitCode, string unitName,string uomUnit,double quantity,double basicPrice) : base(identity)
        {
            Identity = identity;
            ServiceSampleExpenditureGoodId = serviceSampleExpenditureGoodId;
            FinishedGoodStockId = finishedGoodStockId;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            //BuyerCode = buyerCode;
            //BuyerId = buyerId;
            //BuyerName = buyerName;
            UnitId = unitId;
            UnitName = unitName;
            UnitCode = unitCode;
            UomUnit = uomUnit;
            Quantity = quantity;
            BasicPrice = basicPrice;

            ReadModel = new GarmentServiceSampleExpenditureGoodItemReadModel(identity)
            {
                ServiceSampleExpenditureGoodId = ServiceSampleExpenditureGoodId,
                FinishedGoodStockId = FinishedGoodStockId,
                RONo = RONo,
                Article = Article,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                //BuyerCode = BuyerCode,
                //BuyerId = BuyerId.Value,
                //BuyerName = BuyerName,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                UomUnit = UomUnit,
                Quantity = Quantity,
                BasicPrice = BasicPrice,
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSampleExpenditureGoodPlaced(Identity));

        }

        public GarmentServiceSampleExpenditureGoodItem(GarmentServiceSampleExpenditureGoodItemReadModel readModel) : base(readModel)
        {
            ServiceSampleExpenditureGoodId = readModel.ServiceSampleExpenditureGoodId;
            FinishedGoodStockId = readModel.FinishedGoodStockId;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            //BuyerCode = readModel.BuyerCode;
            //BuyerId = new BuyerId(readModel.BuyerId);
            //BuyerName = readModel.BuyerName;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            UomUnit = readModel.UomUnit;
            Quantity = readModel.Quantity;
            BasicPrice = readModel.BasicPrice;

        }

      
        public void Modify()
        {
            MarkModified();
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        protected override GarmentServiceSampleExpenditureGoodItem GetEntity()
        {
            return this;
        }
    }
}
