using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentLoadingOut
{
    public class GarmentLoadingOutListDto : BaseDto
    {
        public GarmentLoadingOutListDto(GarmentSubconLoadingOut garmentLoading)
        {
            Id = garmentLoading.Identity;
            LoadingOutNo = garmentLoading.LoadingOutNo;
            LoadingInNo = garmentLoading.LoadingInNo;
            RONo = garmentLoading.RONo;
            Article = garmentLoading.Article;
            Unit = new UnitDepartment(garmentLoading.UnitId.Value, garmentLoading.UnitCode, garmentLoading.UnitName);
            UnitFrom = new UnitDepartment(garmentLoading.UnitFromId.Value, garmentLoading.UnitFromCode, garmentLoading.UnitFromName);
            Comodity = new GarmentComodity (garmentLoading.ComodityId.Value, garmentLoading.ComodityCode, garmentLoading.ComodityName);
            LoadingOutDate = garmentLoading.LoadingOutDate;
            CreatedBy = garmentLoading.AuditTrail.CreatedBy;
        }

        public Guid Id { get; internal set; }
        public string LoadingOutNo { get; internal set; }

        public string LoadingInNo { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset LoadingOutDate { get; internal set; }

        public double TotalRealQtyOut { get; set; }
        public double TotalLoadingQuantity { get; set; }
        public List<string> Products { get; set; }
        public List<string> Colors { get; set; }
    }
}
