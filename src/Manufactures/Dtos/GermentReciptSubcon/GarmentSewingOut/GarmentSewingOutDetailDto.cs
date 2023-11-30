using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GermentSewingOut
{
    public class GarmentSewingOutDetailDto : BaseDto
    {
        public GarmentSewingOutDetailDto(GarmentSubconSewingOutDetail garmentSewingOutDetail)
        {
            Id = garmentSewingOutDetail.Identity;
            SewingOutItemId = garmentSewingOutDetail.SewingOutItemId;
            Size = new SizeValueObject(garmentSewingOutDetail.SizeId.Value, garmentSewingOutDetail.SizeName);
            Quantity = garmentSewingOutDetail.Quantity;
            Uom = new Uom(garmentSewingOutDetail.UomId.Value, garmentSewingOutDetail.UomUnit);
            RealQtyOut = garmentSewingOutDetail.RealQtyOut;

        }

        public Guid Id { get; set; }
        public Guid SewingOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public double RealQtyOut { get; set; }
    }
}
