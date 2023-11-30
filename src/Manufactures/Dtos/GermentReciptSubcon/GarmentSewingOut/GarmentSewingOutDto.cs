using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GermentSewingOut
{
    public class GarmentSewingOutDto : BaseDto
    {
        public GarmentSewingOutDto(GarmentSubconSewingOut garmentSewingOutList)
        {
            Id = garmentSewingOutList.Identity;
            SewingOutNo = garmentSewingOutList.SewingOutNo;
            UnitTo = new UnitDepartment(garmentSewingOutList.UnitToId.Value, garmentSewingOutList.UnitToCode, garmentSewingOutList.UnitToName);
            Unit = new UnitDepartment(garmentSewingOutList.UnitId.Value, garmentSewingOutList.UnitCode, garmentSewingOutList.UnitName);
            RONo = garmentSewingOutList.RONo;
            Article = garmentSewingOutList.Article;
            SewingOutDate = garmentSewingOutList.SewingOutDate;
            SewingTo = garmentSewingOutList.SewingTo;
            ProductOwner = new Buyer(garmentSewingOutList.ProductOwnerId.Value, garmentSewingOutList.ProductOwnerCode, garmentSewingOutList.ProductOwnerName);
            Comodity = new GarmentComodity(garmentSewingOutList.ComodityId.Value, garmentSewingOutList.ComodityCode, garmentSewingOutList.ComodityName);
            IsDifferentSize = garmentSewingOutList.IsDifferentSize;

            Items = new List<GarmentSewingOutItemDto>();
        }

        public Guid Id { get; internal set; }
        public string SewingOutNo { get;  set; }
        public Buyer ProductOwner { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string SewingTo { get;  set; }
        public UnitDepartment UnitTo { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public DateTimeOffset SewingOutDate { get;  set; }
        public bool IsDifferentSize { get;  set; }

        public virtual List<GarmentSewingOutItemDto> Items { get; internal set; }
    }
}
