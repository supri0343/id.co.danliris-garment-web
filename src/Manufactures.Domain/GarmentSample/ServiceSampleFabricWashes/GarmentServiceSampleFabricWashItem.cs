using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes
{
    public class GarmentServiceSampleFabricWashItem : AggregateRoot<GarmentServiceSampleFabricWashItem, GarmentServiceSampleFabricWashItemReadModel>
    {
        public Guid ServiceSampleFabricWashId { get; private set; }
        public string UnitExpenditureNo { get; private set; }
        public DateTimeOffset ExpenditureDate { get; private set; }

        public UnitSenderId UnitSenderId { get; private set; }
        public string UnitSenderCode { get; private set; }
        public string UnitSenderName { get; private set; }

        public UnitRequestId UnitRequestId { get; private set; }
        public string UnitRequestCode { get; private set; }
        public string UnitRequestName { get; private set; }

        public GarmentServiceSampleFabricWashItem(Guid identity, Guid serviceSampleFabricWashId, string unitExpenditureNo, DateTimeOffset expenditureDate, UnitSenderId unitSenderId, string unitSenderCode, string unitSenderName, UnitRequestId unitRequestId, string unitRequestCode, string unitRequestName) : base(identity)
        {
            Identity = identity;
            ServiceSampleFabricWashId = serviceSampleFabricWashId;
            UnitExpenditureNo = unitExpenditureNo;
            ExpenditureDate = expenditureDate;
            UnitSenderId = unitSenderId;
            UnitSenderCode = unitSenderCode;
            UnitSenderName = unitSenderName;
            UnitRequestId = unitRequestId;
            UnitRequestCode = unitRequestCode;
            UnitRequestName = unitRequestName;

            ReadModel = new GarmentServiceSampleFabricWashItemReadModel(identity)
            {
                ServiceSampleFabricWashId = ServiceSampleFabricWashId,
                ExpenditureDate = ExpenditureDate,
                UnitExpenditureNo = UnitExpenditureNo,
                UnitSenderId = UnitSenderId.Value,
                UnitSenderCode = UnitSenderCode,
                UnitSenderName = UnitSenderName,
                UnitRequestId = UnitRequestId.Value,
                UnitRequestCode = UnitRequestCode,
                UnitRequestName = UnitRequestName
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSampleFabricWashPlaced(Identity));
        }

        public GarmentServiceSampleFabricWashItem(GarmentServiceSampleFabricWashItemReadModel readModel) : base(readModel)
        {
            ServiceSampleFabricWashId = readModel.ServiceSampleFabricWashId;
            UnitExpenditureNo = readModel.UnitExpenditureNo;
            ExpenditureDate = readModel.ExpenditureDate;
            UnitSenderId = new UnitSenderId(readModel.UnitSenderId);
            UnitSenderCode = readModel.UnitSenderCode;
            UnitSenderName = readModel.UnitSenderName;
            UnitRequestId = new UnitRequestId(readModel.UnitRequestId);
            UnitRequestCode = readModel.UnitRequestCode;
            UnitRequestName = readModel.UnitRequestName;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSampleFabricWashItem GetEntity()
        {
            return this;
        }
    }
}
