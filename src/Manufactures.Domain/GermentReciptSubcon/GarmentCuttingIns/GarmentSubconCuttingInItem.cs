using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns
{
    public class GarmentSubconCuttingInItem : AggregateRoot<GarmentSubconCuttingInItem, GarmentSubconCuttingInItemReadModel>
    {
        public Guid CutInId { get; private set; }
        public Guid PreparingId { get; private set; }
        public int UENId { get; private set; }
        public string UENNo { get; private set; }
        public Guid SewingOutId { get; private set; }
        public string SewingOutNo { get; private set; }
		public string UId { get; private set; }
		public GarmentSubconCuttingInItem(Guid identity, Guid cutInId, Guid preparingId, int uENId, string uENNo, Guid sewingOutId, string sewingOutNo) : base(identity)
        {
            //MarkTransient();

            Identity = identity;
            CutInId = cutInId;
            PreparingId = preparingId;
            UENId = uENId;
            UENNo = uENNo;
            SewingOutId = sewingOutId;
            SewingOutNo = sewingOutNo;

            ReadModel = new GarmentSubconCuttingInItemReadModel(identity)
            {
                CutInId = CutInId,
                PreparingId = PreparingId,
                UENId = UENId,
                UENNo = UENNo,
                SewingOutId= SewingOutId,
                SewingOutNo= SewingOutNo
            };

            ReadModel.AddDomainEvent(new OnGarmentCuttingInPlaced(Identity));
        }

        public GarmentSubconCuttingInItem(GarmentSubconCuttingInItemReadModel readModel) : base(readModel)
        {
            CutInId = readModel.CutInId;
            PreparingId = readModel.PreparingId;
            UENId = readModel.UENId;
            UENNo = readModel.UENNo;
            SewingOutId = readModel.SewingOutId;
            SewingOutNo = readModel.SewingOutNo;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCuttingInItem GetEntity()
        {
            return this;
        }
    }
}
