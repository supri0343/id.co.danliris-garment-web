using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns
{
    public class GarmentSubconCustomsInDetail : AggregateRoot<GarmentSubconCustomsInDetail, GarmentSubconCustomsInDetailReadModel>
    {
        public Guid SubconCustomsInItemId { get; private set; }
        public Guid SubconCustomsOutId { get; private set; }
        public string CustomsOutNo { get; private set; }
        public decimal CustomsOutQty { get; private set; }

        public GarmentSubconCustomsInDetail(Guid identity, Guid subconCustomsInItemId, Guid subconCustomsOutId, string customsOutNo, decimal customsOutQty) : base(identity)
        {
            Identity = identity;
            SubconCustomsInItemId = subconCustomsInItemId;
            SubconCustomsOutId = subconCustomsOutId;
            CustomsOutNo = customsOutNo;
            CustomsOutQty = customsOutQty;

            ReadModel = new GarmentSubconCustomsInDetailReadModel(Identity)
            {
                SubconCustomsInItemId = SubconCustomsInItemId,
                SubconCustomsOutId = SubconCustomsOutId,
                CustomsOutNo = CustomsOutNo,
                CustomsOutQty = CustomsOutQty
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCustomsInPlaced(Identity));
        }
        public GarmentSubconCustomsInDetail(GarmentSubconCustomsInDetailReadModel readModel) : base(readModel)
        {
            SubconCustomsInItemId = readModel.SubconCustomsInItemId;
            SubconCustomsOutId = readModel.SubconCustomsOutId;
            CustomsOutNo = readModel.CustomsOutNo;
            CustomsOutQty = readModel.CustomsOutQty;
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCustomsInDetail GetEntity()
        {
            return this;
        }
    }
}
