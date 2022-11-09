using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess
{
    public class GarmentSubconReprocessItem : AggregateRoot<GarmentSubconReprocessItem, GarmentSubconReprocessItemReadModel>
    {
        

        public Guid ReprocessId { get; private set; }
        //WASH/SEWING
        public Guid ServiceSubconSewingId { get; private set; }
        public string ServiceSubconSewingNo { get; private set; }
        public Guid ServiceSubconSewingItemId { get; private set; }

        //KOMPONEN/CUTTING
        public Guid ServiceSubconCuttingId { get; private set; }
        public string ServiceSubconCuttingNo { get; private set; }
        public Guid ServiceSubconCuttingItemId { get; private set; }

        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }
        public GarmentSubconReprocessItem(Guid identity, Guid reprocessId, Guid serviceSubconSewingId, string serviceSubconSewingNo, Guid serviceSubconSewingItemId, Guid serviceSubconCuttingId, string serviceSubconCuttingNo, Guid serviceSubconCuttingItemId, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, BuyerId buyerId, string buyerCode, string buyerName) : base(identity)
        {
            Identity = identity;
            ReprocessId = reprocessId;
            ServiceSubconSewingId = serviceSubconSewingId;
            ServiceSubconSewingNo = serviceSubconSewingNo;
            ServiceSubconSewingItemId = serviceSubconSewingItemId;
            ServiceSubconCuttingId = serviceSubconCuttingId;
            ServiceSubconCuttingNo = serviceSubconCuttingNo;
            ServiceSubconCuttingItemId = serviceSubconCuttingItemId;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;

            ReadModel = new GarmentSubconReprocessItemReadModel(Identity)
            {
                ServiceSubconSewingId = ServiceSubconSewingId,
                ServiceSubconSewingNo = ServiceSubconSewingNo,
                ServiceSubconSewingItemId = ServiceSubconSewingItemId,
                ServiceSubconCuttingId= ServiceSubconCuttingId,
                ServiceSubconCuttingNo= ServiceSubconCuttingNo,
                ServiceSubconCuttingItemId= ServiceSubconCuttingItemId,
                RONo = RONo,
                Article = Article,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                BuyerId = BuyerId.Value,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName,
                ReprocessId=ReprocessId
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconReprocessPlaced(Identity));
        }

        public GarmentSubconReprocessItem(GarmentSubconReprocessItemReadModel readModel) : base(readModel)
        {
            ServiceSubconSewingId = readModel.ServiceSubconSewingId;
            ServiceSubconSewingNo = readModel.ServiceSubconSewingNo;
            ServiceSubconSewingItemId = readModel.ServiceSubconSewingItemId;
            ServiceSubconCuttingId = readModel.ServiceSubconCuttingId;
            ServiceSubconCuttingNo = readModel.ServiceSubconCuttingNo;
            ServiceSubconCuttingItemId = readModel.ServiceSubconCuttingItemId;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityName = readModel.ComodityName;
            ComodityCode = readModel.ComodityCode;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
            ReprocessId = readModel.ReprocessId;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconReprocessItem GetEntity()
        {
            return this;
        }
    }
}
