using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentLoadingIn
{
    public class GarmentLoadingListDto : BaseDto
    {
        public GarmentLoadingListDto(GarmentSubconLoadingIn garmentLoading)
        {
            Id = garmentLoading.Identity;
            LoadingNo = garmentLoading.LoadingNo;
            CuttingOutNo = garmentLoading.CuttingOutNo;
            RONo = garmentLoading.RONo;
            Article = garmentLoading.Article;
            Unit = new UnitDepartment(garmentLoading.UnitId.Value, garmentLoading.UnitCode, garmentLoading.UnitName);
            UnitFrom = new UnitDepartment(garmentLoading.UnitFromId.Value, garmentLoading.UnitFromCode, garmentLoading.UnitFromName);
            Comodity = new GarmentComodity (garmentLoading.ComodityId.Value, garmentLoading.ComodityCode, garmentLoading.ComodityName);
            LoadingDate = garmentLoading.LoadingDate;
            CreatedBy = garmentLoading.AuditTrail.CreatedBy;
            IsApproved = garmentLoading.IsApproved;
        }

        public Guid Id { get; internal set; }
        public string LoadingNo { get; internal set; }
        public string CuttingOutNo { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset LoadingDate { get; internal set; }

        public double TotalRemainingQuantity { get; set; }
        public double TotalLoadingQuantity { get; set; }
        public List<string> Products { get; set; }
        public List<string> Colors { get; set; }
        public bool IsApproved { get; internal set; }
    }
}
