using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using System;

namespace Manufactures.Domain.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingRelation : AggregateRoot<GarmentSubconCuttingRelation, GarmentSubconCuttingRelationReadModel>
    {
        public Guid GarmentSubconCuttingId { get; private set; }
        public Guid GarmentCuttingOutId { get; private set; }

        public GarmentSubconCuttingRelation(Guid identity, Guid garmentSubconCuttingId, Guid garmentCuttingOutId) : base(identity)
        {
            GarmentSubconCuttingId = garmentSubconCuttingId;
            GarmentCuttingOutId = garmentCuttingOutId;

            ReadModel = new GarmentSubconCuttingRelationReadModel(Identity)
            {
                GarmentSubconCuttingId = garmentSubconCuttingId,
                GarmentCuttingOutId = garmentCuttingOutId,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCuttingRelationPlaced(Identity));
        }

        public GarmentSubconCuttingRelation(GarmentSubconCuttingRelationReadModel readModel) : base(readModel)
        {
            GarmentSubconCuttingId = readModel.GarmentSubconCuttingId;
            GarmentCuttingOutId = readModel.GarmentCuttingOutId;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCuttingRelation GetEntity()
        {
            return this;
        }
    }
}
