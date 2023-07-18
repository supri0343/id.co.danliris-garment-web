using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels
{
    public class GarmentServiceSampleFabricWashItemReadModel : ReadModelBase
    {
        public GarmentServiceSampleFabricWashItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSampleFabricWashId { get; internal set; }
        public string UnitExpenditureNo { get; internal set; }
        public DateTimeOffset ExpenditureDate { get; internal set; }

        public int UnitSenderId { get; internal set; }
        public string UnitSenderCode { get; internal set; }
        public string UnitSenderName { get; internal set; }

        public int UnitRequestId { get; internal set; }
        public string UnitRequestCode { get; internal set; }
        public string UnitRequestName { get; internal set; }

        public virtual GarmentServiceSampleFabricWashReadModel GarmentServiceSampleFabricWashIdentity { get; internal set; }
        public virtual List<GarmentServiceSampleFabricWashDetailReadModel> GarmentServiceSampleFabricWashDetail { get; internal set; }
    }
}
