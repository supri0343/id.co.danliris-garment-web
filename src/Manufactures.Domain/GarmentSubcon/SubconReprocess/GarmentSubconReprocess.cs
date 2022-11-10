using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess
{
    public class GarmentSubconReprocess : AggregateRoot<GarmentSubconReprocess, GarmentSubconReprocessReadModel>
    {

        public string ReprocessNo { get; private set; }
        public string ReprocessType { get; private set; }
        public DateTimeOffset Date { get; private set; }

        public GarmentSubconReprocess(Guid identity, string reprocessNo, string reprocessType, DateTimeOffset date) : base(identity)
        {
            Identity = identity;
            ReprocessNo = reprocessNo;
            ReprocessType = reprocessType;
            Date = date;

            ReadModel = new GarmentSubconReprocessReadModel(Identity)
            {
                ReprocessNo=ReprocessNo,
                ReprocessType= ReprocessType,
                Date=Date
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconReprocessPlaced(Identity));
        }

        public GarmentSubconReprocess(GarmentSubconReprocessReadModel readModel) : base(readModel)
        {
            ReprocessNo = readModel.ReprocessNo;
            ReprocessType = readModel.ReprocessType;
            Date = readModel.Date;
        }

        public void SetDate(DateTimeOffset date)
        {
            if (this.Date != date)
            {
                this.Date = date;
                ReadModel.Date = date;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconReprocess GetEntity()
        {
            return this;
        }
    }
}
