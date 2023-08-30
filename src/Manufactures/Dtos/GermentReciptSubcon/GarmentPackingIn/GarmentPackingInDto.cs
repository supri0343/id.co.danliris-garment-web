using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentPackingIn
{
    public class GarmentPackingInDto : BaseDto
    {
        public GarmentPackingInDto(GarmentSubconPackingIn garmentPackingIn)
        {
            Id = garmentPackingIn.Identity;
            PackingInNo = garmentPackingIn.PackingInNo;
            RONo = garmentPackingIn.RONo;
            Article = garmentPackingIn.Article;
            Unit = new UnitDepartment(garmentPackingIn.UnitId.Value, garmentPackingIn.UnitCode, garmentPackingIn.UnitName);
            UnitFrom = new UnitDepartment(garmentPackingIn.UnitFromId.Value, garmentPackingIn.UnitFromCode, garmentPackingIn.UnitFromName);
            Comodity = new GarmentComodity(garmentPackingIn.ComodityId.Value, garmentPackingIn.ComodityCode, garmentPackingIn.ComodityName);
            PackingInDate = garmentPackingIn.PackingInDate;
            PackingFrom = garmentPackingIn.PackingFrom;
            Comodity = new GarmentComodity(garmentPackingIn.ComodityId.Value, garmentPackingIn.ComodityCode, garmentPackingIn.ComodityName);
            IsApproved = garmentPackingIn.IsApproved;
            Items = new List<GarmentPackingInItemDto>();
        }

        public Guid Id { get; internal set; }
        public string PackingInNo { get; internal set; }
        public string PackingFrom { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset PackingInDate { get; internal set; }
        public bool IsApproved { get; internal set; }

        public virtual List<GarmentPackingInItemDto> Items { get; internal set; }
    }
}
