using Infrastructure.Domain;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Events.GarmentSample;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings
{
    public class GarmentServiceSampleCuttingItem : AggregateRoot<GarmentServiceSampleCuttingItem, GarmentServiceSampleCuttingItemReadModel>
    {

        public Guid ServiceSampleCuttingId { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }

        public GarmentServiceSampleCuttingItem(Guid identity, Guid serviceSampleCuttingId, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName) : base(identity)
        {
            Identity = identity;
            ServiceSampleCuttingId = serviceSampleCuttingId;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;

            ReadModel = new GarmentServiceSampleCuttingItemReadModel(Identity)
            {
                ServiceSampleCuttingId=ServiceSampleCuttingId,
                Article = Article,
                ComodityCode = ComodityCode,
                ComodityId = ComodityId.Value,
                ComodityName = ComodityName,
                RONo = RONo
            };

            ReadModel.AddDomainEvent(new OnServiceSampleCuttingPlaced(Identity));
        }

        public GarmentServiceSampleCuttingItem(GarmentServiceSampleCuttingItemReadModel readModel) : base(readModel)
        {
            ServiceSampleCuttingId = readModel.ServiceSampleCuttingId;
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            Article = readModel.Article;
            RONo = readModel.RONo;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleCuttingItem GetEntity()
        {
            return this;
        }
    }
}
