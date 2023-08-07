using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentLoadingOut
{
    public class GarmentLoadingOutDto : BaseDto
    {
        public GarmentLoadingOutDto(GarmentSubconLoadingOut garmentLoading)
        {
            Id = garmentLoading.Identity;
            LoadingOutNo = garmentLoading.LoadingOutNo;
            LoadingInId = garmentLoading.LoadingInId;
            LoadingInNo = garmentLoading.LoadingInNo;
            RONo = garmentLoading.RONo;
            Article = garmentLoading.Article;
            Unit = new UnitDepartment(garmentLoading.UnitId.Value, garmentLoading.UnitCode, garmentLoading.UnitName);
            UnitFrom = new UnitDepartment(garmentLoading.UnitFromId.Value, garmentLoading.UnitFromCode, garmentLoading.UnitFromName);
            Comodity = new GarmentComodity(garmentLoading.ComodityId.Value, garmentLoading.ComodityCode, garmentLoading.ComodityName);
            LoadingOutDate = garmentLoading.LoadingOutDate;
            Items = new List<GarmentLoadingOutItemDto>();
        }

        public Guid Id { get; internal set; }
        public string LoadingOutNo { get; internal set; }

        public Guid LoadingInId { get; internal set; }
        public string LoadingInNo { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset LoadingOutDate { get; internal set; }

        public virtual List<GarmentLoadingOutItemDto> Items { get; internal set; }
    }
}
