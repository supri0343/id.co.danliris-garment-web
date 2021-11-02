using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts
{
    public class GarmentSubconCustomsOutItem : AggregateRoot<GarmentSubconCustomsOutItem, GarmentSubconCustomsOutItemReadModel>
    {

        public string SubconDLOuttNo { get; private set; }
        public Guid SubconDLOuttId { get; private set; }
        public double Quantity { get; private set; }

        public GarmentSubconCustomsOutItem(Guid identity, string subconDLOuttNo, Guid subconDLOuttId, double quantity) : base(identity)
        {
            Identity = identity;
            SubconDLOuttNo = subconDLOuttNo;
            SubconDLOuttId = subconDLOuttId;
            Quantity = quantity;
            ReadModel = new GarmentSubconCustomsOutItemReadModel(Identity)
            {
                SubconDLOuttNo = SubconDLOuttNo,
                SubconDLOuttId = SubconDLOuttId,
                Quantity = Quantity,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCustomsOutPlaced(Identity));
        }
        public GarmentSubconCustomsOutItem(GarmentSubconCustomsOutItemReadModel readModel) : base(readModel)
        {
            SubconDLOuttNo = readModel.SubconDLOuttNo;
            SubconDLOuttId = readModel.SubconDLOuttId;
            Quantity = readModel.Quantity;
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCustomsOutItem GetEntity()
        {
            return this;
        }
        
    }
}
