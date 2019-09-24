using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Infrastructure.Domain;
using Manufactures.Domain.Events;

namespace Manufactures.Domain.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingOutDetail : AggregateRoot<GarmentSubconCuttingOutDetail, GarmentCuttingOutDetailReadModel>
    {
        public Guid CutOutItemId { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double CuttingOutQuantity { get; private set; }
        public UomId CuttingOutUomId { get; private set; }
        public string CuttingOutUomUnit { get; private set; }
        public string Color { get; private set; }
        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public double IndirectPrice { get; private set; }
        public double OTL1 { get; private set; }
        public double OTL2 { get; private set; }
        public string Remark { get; private set; }


        public void SetCuttingOutQuantity(double CuttingOutQuantity)
        {
            if (this.CuttingOutQuantity != CuttingOutQuantity)
            {
                this.CuttingOutQuantity = CuttingOutQuantity;
                ReadModel.CuttingOutQuantity = CuttingOutQuantity;
            }
        }

        public void SetRemainingQuantity(double RemainingQuantity)
        {
            if (this.RemainingQuantity != RemainingQuantity)
            {
                this.RemainingQuantity = RemainingQuantity;
                ReadModel.RemainingQuantity = RemainingQuantity;
            }
        }

        public void SetColor(string Color)
        {
            if (this.Color != Color)
            {
                this.Color = Color;
                ReadModel.Color = Color;
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

        public GarmentSubconCuttingOutDetail(Guid identity, Guid cutOutItemId, SizeId sizeId, string sizeName, string color, double remainingQuantity, double cuttingOutQuantity, UomId cuttingOutUomId, string cuttingOutUomUnit, double otl1, double otl2, double basicPrice, double indirectPrice, string remark) : base(identity)
        {
            //MarkTransient();

            CutOutItemId = cutOutItemId;
            Color = color;
            SizeId = sizeId;
            SizeName = sizeName;
            RemainingQuantity = remainingQuantity;
            CuttingOutQuantity = cuttingOutQuantity;
            CuttingOutUomId = cuttingOutUomId;
            CuttingOutUomUnit = cuttingOutUomUnit;
            BasicPrice = basicPrice;
            IndirectPrice = indirectPrice;
            OTL1 = otl1;
            OTL2 = otl2;
            Remark = remark;

            ReadModel = new GarmentCuttingOutDetailReadModel(Identity)
            {
                CutOutItemId = CutOutItemId,
                Color = Color,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                RemainingQuantity = RemainingQuantity,
                CuttingOutQuantity = CuttingOutQuantity,
                CuttingOutUomId = CuttingOutUomId.Value,
                CuttingOutUomUnit = CuttingOutUomUnit,
                BasicPrice = BasicPrice,
                IndirectPrice = IndirectPrice,
                OTL1 = OTL1,
                OTL2 = OTL2,
                Remark=Remark
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCuttingOutPlaced(Identity));
        }

        public GarmentSubconCuttingOutDetail(GarmentCuttingOutDetailReadModel readModel) : base(readModel)
        {
            CutOutItemId = readModel.CutOutItemId;
            Color = readModel.Color;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            RemainingQuantity = readModel.RemainingQuantity;
            CuttingOutQuantity = readModel.CuttingOutQuantity;
            CuttingOutUomId = new UomId(readModel.CuttingOutUomId);
            CuttingOutUomUnit = CuttingOutUomUnit;
            BasicPrice = BasicPrice;
            IndirectPrice = IndirectPrice;
            OTL1 = OTL1;
            OTL2 = OTL2;
            Remark = Remark;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCuttingOutDetail GetEntity()
        {
            return this;
        }
    }
}
