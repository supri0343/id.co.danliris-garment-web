using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentFinishingOut
{
    public class GarmentFinishingOutDetailDto : BaseDto
    {
        public GarmentFinishingOutDetailDto(GarmentSubconFinishingOutDetail garmentFinishingOutDetail)
        {
            Id = garmentFinishingOutDetail.Identity;
            FinishingOutItemId = garmentFinishingOutDetail.FinishingOutItemId;
            Size = new SizeValueObject(garmentFinishingOutDetail.SizeId.Value, garmentFinishingOutDetail.SizeName);
            Quantity = garmentFinishingOutDetail.Quantity;
            Uom = new Uom(garmentFinishingOutDetail.UomId.Value, garmentFinishingOutDetail.UomUnit);
            RealQtyOut =  garmentFinishingOutDetail.RealQtyOut;
        }

        public Guid Id { get; set; }
        public Guid FinishingOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public double RealQtyOut { get; set; }
        public Uom Uom { get; set; }
    }
}