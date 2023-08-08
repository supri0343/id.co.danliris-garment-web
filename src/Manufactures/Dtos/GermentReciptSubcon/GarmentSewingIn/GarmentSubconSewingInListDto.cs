using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentSewingIn
{
    public class GarmentSubconSewingInListDto : BaseDto
    {
        public GarmentSubconSewingInListDto(GarmentSubconSewingIn garmentSewingInList)
        {
            Id = garmentSewingInList.Identity;
            SewingInNo = garmentSewingInList.SewingInNo;
            SewingFrom = garmentSewingInList.SewingFrom;
            LoadingOutId = garmentSewingInList.LoadingOutId;
            LoadingOutNo = garmentSewingInList.LoadingOutNo;
            UnitFrom = new UnitDepartment(garmentSewingInList.UnitFromId.Value, garmentSewingInList.UnitFromCode, garmentSewingInList.UnitFromName);
            Unit = new UnitDepartment(garmentSewingInList.UnitId.Value, garmentSewingInList.UnitCode, garmentSewingInList.UnitName);
            RONo = garmentSewingInList.RONo;
            Article = garmentSewingInList.Article;
            Comodity = new GarmentComodity(garmentSewingInList.ComodityId.Value, garmentSewingInList.ComodityCode, garmentSewingInList.ComodityName);
            SewingInDate = garmentSewingInList.SewingInDate;
            CreatedBy = garmentSewingInList.AuditTrail.CreatedBy;
            IsApproved = garmentSewingInList.IsApproved;
            Items = new List<GarmentSubconSewingInItemDto>();
        }

        public Guid Id { get; set; }
        public string SewingInNo { get; set; }
        public string SewingFrom { get; set; }
        public Guid LoadingOutId { get; set; }
        public string LoadingOutNo { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset SewingInDate { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public List<string> Products { get; set; }
        public List<string> Colors { get; set; }
        public bool IsApproved { get; set; }
        public List<GarmentSubconSewingInItemDto> Items { get; set; }
    }
}