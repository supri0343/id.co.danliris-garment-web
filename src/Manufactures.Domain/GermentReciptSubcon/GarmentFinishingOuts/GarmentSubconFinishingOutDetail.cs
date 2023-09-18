using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts
{
    public class GarmentSubconFinishingOutDetail : AggregateRoot<GarmentSubconFinishingOutDetail, GarmentReceiptSubconFinishingOutDetailReadModel>
    {
        public Guid FinishingOutItemId { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public double RealQtyOut { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetSizeId(SizeId SizeId)
        {
            if (this.SizeId != SizeId)
            {
                this.SizeId = SizeId;
                ReadModel.SizeId = SizeId.Value;
            }
        }

        public void SetSizeName(string SizeName)
        {
            if (this.SizeName != SizeName)
            {
                this.SizeName = SizeName;
                ReadModel.SizeName = SizeName;
            }
        }

        public void SetRealQtyOut(double RealQtyOut)
        {
            if (this.RealQtyOut != RealQtyOut)
            {
                this.RealQtyOut = RealQtyOut;
                ReadModel.RealQtyOut = RealQtyOut;
            }
        }
        public GarmentSubconFinishingOutDetail(Guid identity, Guid sewingOutItemId, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, double realQtyOut) : base(identity)
        {
            //MarkTransient();

            FinishingOutItemId = sewingOutItemId;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            RealQtyOut = realQtyOut;
            ReadModel = new GarmentReceiptSubconFinishingOutDetailReadModel(Identity)
            {
                FinishingOutItemId = FinishingOutItemId,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                RealQtyOut = RealQtyOut
            };

            ReadModel.AddDomainEvent(new OnGarmentFinishingOutPlaced(Identity));
        }

        public GarmentSubconFinishingOutDetail(GarmentReceiptSubconFinishingOutDetailReadModel readModel) : base(readModel)
        {
            FinishingOutItemId = readModel.FinishingOutItemId;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            RealQtyOut = readModel.RealQtyOut;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconFinishingOutDetail GetEntity()
        {
            return this;
        }
    }
}