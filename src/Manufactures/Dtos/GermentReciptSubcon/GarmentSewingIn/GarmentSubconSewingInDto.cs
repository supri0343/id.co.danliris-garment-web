using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentSewingIn
{
    public class GarmentSubconSewingInDto : BaseDto
    {
        public GarmentSubconSewingInDto(GarmentSubconSewingIn garmentSewingIn)
        {
            Id = garmentSewingIn.Identity;
            SewingInNo = garmentSewingIn.SewingInNo;
            SewingFrom = garmentSewingIn.SewingFrom;
            LoadingOutId = garmentSewingIn.LoadingOutId;
            LoadingOutNo = garmentSewingIn.LoadingOutNo;
            UnitFrom = new UnitDepartment(garmentSewingIn.UnitFromId.Value, garmentSewingIn.UnitFromCode, garmentSewingIn.UnitFromName);
            Unit = new UnitDepartment(garmentSewingIn.UnitId.Value, garmentSewingIn.UnitCode, garmentSewingIn.UnitName);
            RONo = garmentSewingIn.RONo;
            Article = garmentSewingIn.Article;
            Comodity = new GarmentComodity(garmentSewingIn.ComodityId.Value, garmentSewingIn.ComodityCode, garmentSewingIn.ComodityName);
            SewingInDate = garmentSewingIn.SewingInDate;
            IsApproved = garmentSewingIn.IsApproved;

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
        public bool IsApproved { get; set; }
        public List<GarmentSubconSewingInItemDto> Items { get; set; }
    }
}