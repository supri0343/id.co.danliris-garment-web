using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentCuttingOutDetailDto : BaseDto
    {
        public GarmentCuttingOutDetailDto(GarmentCuttingOutDetail garmentCuttingOutDetail)
        {
            Id = garmentCuttingOutDetail.Identity;
            CutOutItemId = garmentCuttingOutDetail.CutOutItemId;
            Size = new SizeValueObject(garmentCuttingOutDetail.SizeId.Value, garmentCuttingOutDetail.SizeName);
            CuttingOutQuantity = garmentCuttingOutDetail.CuttingOutQuantity;
            CuttingOutUom = new Uom(garmentCuttingOutDetail.CuttingOutUomId.Value, garmentCuttingOutDetail.CuttingOutUomUnit);
            Color = garmentCuttingOutDetail.Color;
            RemainingQuantity = garmentCuttingOutDetail.RemainingQuantity;
            BasicPrice = garmentCuttingOutDetail.BasicPrice;
            IndirectPrice = garmentCuttingOutDetail.IndirectPrice;
            OTL1 = garmentCuttingOutDetail.OTL1;
            OTL2 = garmentCuttingOutDetail.OTL2;
            
        }

        public Guid Id { get; set; }
        public Guid CutOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double CuttingOutQuantity { get; set; }
        public Uom CuttingOutUom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double IndirectPrice { get; set; }
        public double OTL1 { get; set; }
        public double OTL2 { get; set; }
       
    }
}