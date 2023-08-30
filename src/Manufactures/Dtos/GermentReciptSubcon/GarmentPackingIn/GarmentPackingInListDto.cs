using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentPackingIn
{
    public class GarmentPackingInListDto : BaseDto
    {
        public GarmentPackingInListDto(GarmentSubconPackingIn garmentPackingIn)
        {
            Id = garmentPackingIn.Identity;
            PackingInNo = garmentPackingIn.PackingInNo;
            RONo = garmentPackingIn.RONo;
            Article = garmentPackingIn.Article;
            Unit = new UnitDepartment(garmentPackingIn.UnitId.Value, garmentPackingIn.UnitCode, garmentPackingIn.UnitName);
            UnitFrom = new UnitDepartment(garmentPackingIn.UnitFromId.Value, garmentPackingIn.UnitFromCode, garmentPackingIn.UnitFromName);
            PackingInDate = garmentPackingIn.PackingInDate;
            PackingFrom = garmentPackingIn.PackingFrom;
            Comodity = new GarmentComodity(garmentPackingIn.ComodityId.Value, garmentPackingIn.ComodityCode, garmentPackingIn.ComodityName);
            CreatedBy = garmentPackingIn.AuditTrail.CreatedBy;
            IsApproved = garmentPackingIn.IsApproved;
        }

        public Guid Id { get; internal set; }
        public string PackingInNo { get; internal set; }
        public string PackingFrom { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public DateTimeOffset PackingInDate { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }

        public double TotalRemainingQuantity { get; set; }
        public double TotalPackingInQuantity { get; set; }
        public bool IsApproved { get; set; }
        public List<string> Products { get; set; }
    }
}
