using Infrastructure.Domain;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts
{
    public class GarmentCuttingOutDetail : AggregateRoot<GarmentCuttingOutDetail, GarmentCuttingOutDetailReadModel>
    {
        public Guid CuttingInItemId { get; private set; }
        public Guid CutOutItemId { get; private set; }

        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public string Color { get; private set; }

        public double RemainingQuantity { get; private set; }
        public double CuttingOutQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public double IndirectPrice { get; private set; }
        public double OTL1 { get; private set; }
        public double OTL2 { get; private set; }

        public UomId CuttingOutUomId { get; private set; }
        public string CuttingOutUomUnit { get; private set; }

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

        public GarmentCuttingOutDetail(Guid identity, Guid cutOutItemId, Guid cuttingInItemId, SizeId sizeId, string sizeName, string color, double remainingQuantity, double cuttingOutQuantity, UomId cuttingOutUomId, string cuttingOutUomUnit, double cuttingInQuantity, UomId cuttingInUomId, string cuttingInUomUnit, double remainingQuantity, double basicPrice) : base(identity)
        {
            //MarkTransient();

            CutInItemId = cutInItemId;
            PreparingItemId = preparingItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            FabricType = fabricType;
            PreparingQuantity = preparingQuantity;
            PreparingUomId = preparingUomId;
            PreparingUomUnit = preparingUomUnit;
            CuttingInQuantity = cuttingInQuantity;
            CuttingInUomId = cuttingInUomId;
            CuttingInUomUnit = cuttingInUomUnit;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;

            ReadModel = new GarmentCuttingOutDetailReadModel(Identity)
            {
                CutInItemId = CutInItemId,
                PreparingItemId = PreparingItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                FabricType = FabricType,
                PreparingQuantity = PreparingQuantity,
                PreparingUomId = PreparingUomId.Value,
                PreparingUomUnit = PreparingUomUnit,
                CuttingInQuantity = CuttingInQuantity,
                CuttingInUomId = CuttingInUomId.Value,
                CuttingInUomUnit = CuttingInUomUnit,
                RemainingQuantity = RemainingQuantity,
                BasicPrice = BasicPrice,
            };

            ReadModel.AddDomainEvent(new OnGarmentCuttingOutPlaced(Identity));
        }

        public GarmentCuttingOutDetail(GarmentCuttingOutDetailReadModel readModel) : base(readModel)
        {
            CutInItemId = readModel.CutInItemId;
            PreparingItemId = readModel.PreparingItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            FabricType = readModel.FabricType;
            PreparingQuantity = readModel.PreparingQuantity;
            PreparingUomId = new UomId(readModel.PreparingUomId);
            PreparingUomUnit = readModel.PreparingUomUnit;
            CuttingInQuantity = readModel.CuttingInQuantity;
            CuttingInUomId = new UomId(readModel.CuttingInUomId);
            CuttingInUomUnit = readModel.CuttingInUomUnit;
            RemainingQuantity = readModel.RemainingQuantity;
            BasicPrice = readModel.BasicPrice;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentCuttingOutDetail GetEntity()
        {
            return this;
        }
    }
}
