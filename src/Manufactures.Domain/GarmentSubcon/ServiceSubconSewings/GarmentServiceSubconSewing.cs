using System;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings
{
    public class GarmentServiceSubconSewing : AggregateRoot<GarmentServiceSubconSewing, GarmentServiceSubconSewingReadModel>
    {
        public string ServiceSubconSewingNo { get; private set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset ServiceSubconSewingDate { get; private set; }
        public bool IsDifferentSize { get; set; }

        public GarmentServiceSubconSewing(Guid identity, string serviceSubconSewingNo, BuyerId buyerId, string buyerCode, string buyerName, UnitDepartmentId unitId, string unitCode, string unitName, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, DateTimeOffset serviceSubconSewingDate, bool isDifferentSize) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => rONo);

            Identity = identity;
            ServiceSubconSewingNo = serviceSubconSewingNo;
            ServiceSubconSewingDate = serviceSubconSewingDate;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            BuyerCode = buyerCode;
            BuyerId = buyerId;
            BuyerName = buyerName;
            IsDifferentSize = isDifferentSize;

            ReadModel = new GarmentServiceSubconSewingReadModel(Identity)
            {
                ServiceSubconSewingNo = ServiceSubconSewingNo,
                ServiceSubconSewingDate = ServiceSubconSewingDate,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                BuyerCode = BuyerCode,
                BuyerId = BuyerId.Value,
                BuyerName = BuyerName,
                IsDifferentSize = IsDifferentSize,
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconSewingPlaced(Identity));

        }

        public GarmentServiceSubconSewing(GarmentServiceSubconSewingReadModel readModel) : base(readModel)
        {
            ServiceSubconSewingNo = readModel.ServiceSubconSewingNo;
            ServiceSubconSewingDate = readModel.ServiceSubconSewingDate;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            BuyerCode = readModel.BuyerCode;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerName = readModel.BuyerName;
            IsDifferentSize = readModel.IsDifferentSize;
        }

        public void SetDate(DateTimeOffset ServiceSubconSewingDate)
        {
            if (this.ServiceSubconSewingDate != ServiceSubconSewingDate)
            {
                this.ServiceSubconSewingDate = ServiceSubconSewingDate;
                ReadModel.ServiceSubconSewingDate = ServiceSubconSewingDate;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconSewing GetEntity()
        {
            return this;
        }
    }
}
