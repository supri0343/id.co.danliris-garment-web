using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings
{
    public class GarmentServiceSubconSewingItem : AggregateRoot<GarmentServiceSubconSewingItem, GarmentServiceSubconSewingItemReadModel>
    {
        public Guid ServiceSubconSewingId { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }

        public GarmentServiceSubconSewingItem(Guid identity, Guid serviceSubconSewingId, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName) : base(identity)
        {
            Identity = identity;
            ServiceSubconSewingId = serviceSubconSewingId;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;

            ReadModel = new GarmentServiceSubconSewingItemReadModel(identity)
            {
                ServiceSubconSewingId = ServiceSubconSewingId,
                RONo = RONo,
                Article = Article,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconSewingPlaced(Identity));

        }

        public GarmentServiceSubconSewingItem(GarmentServiceSubconSewingItemReadModel readModel) : base(readModel)
        {
            ServiceSubconSewingId = readModel.ServiceSubconSewingId;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
        }

        protected override GarmentServiceSubconSewingItem GetEntity()
        {
            throw new NotImplementedException();
        }
    }
}
