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
        public DateTimeOffset ServiceSubconSewingDate { get; private set; }
        public bool IsUsed { get; internal set; }

        public GarmentServiceSubconSewing(Guid identity, string serviceSubconSewingNo, BuyerId buyerId, string buyerCode, string buyerName, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset serviceSubconSewingDate, bool isUsed) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);

            Identity = identity;
            ServiceSubconSewingNo = serviceSubconSewingNo;
            ServiceSubconSewingDate = serviceSubconSewingDate;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            BuyerCode = buyerCode;
            BuyerId = buyerId;
            BuyerName = buyerName;
            IsUsed = isUsed;

            ReadModel = new GarmentServiceSubconSewingReadModel(Identity)
            {
                ServiceSubconSewingNo = ServiceSubconSewingNo,
                ServiceSubconSewingDate = ServiceSubconSewingDate,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                BuyerCode = BuyerCode,
                BuyerId = BuyerId.Value,
                BuyerName = BuyerName,
                IsUsed = IsUsed,
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconSewingPlaced(Identity));

        }

        public GarmentServiceSubconSewing(GarmentServiceSubconSewingReadModel readModel) : base(readModel)
        {
            ServiceSubconSewingNo = readModel.ServiceSubconSewingNo;
            ServiceSubconSewingDate = readModel.ServiceSubconSewingDate;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            BuyerCode = readModel.BuyerCode;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerName = readModel.BuyerName;
            IsUsed = readModel.IsUsed;
        }

        public void SetDate(DateTimeOffset ServiceSubconSewingDate)
        {
            if (this.ServiceSubconSewingDate != ServiceSubconSewingDate)
            {
                this.ServiceSubconSewingDate = ServiceSubconSewingDate;
                ReadModel.ServiceSubconSewingDate = ServiceSubconSewingDate;
            }
        }

        public void SetIsUsed(bool isUsed)
        {
            if (isUsed != IsUsed)
            {
                IsUsed = isUsed;
                ReadModel.IsUsed = isUsed;

                MarkModified();
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
