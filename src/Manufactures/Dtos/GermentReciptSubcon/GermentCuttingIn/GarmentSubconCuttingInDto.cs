using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GermentCuttingIn
{
    public class GarmentSubconCuttingInDto : BaseDto
    {
        public GarmentSubconCuttingInDto(GarmentSubconCuttingIn garmentCuttingIn)
        {
            Id = garmentCuttingIn.Identity;

            CutInNo = garmentCuttingIn.CutInNo;
            CuttingType = garmentCuttingIn.CuttingType;
            RONo = garmentCuttingIn.RONo;
            Article = garmentCuttingIn.Article;
            Unit = new UnitDepartment(garmentCuttingIn.UnitId.Value, garmentCuttingIn.UnitCode, garmentCuttingIn.UnitName);
            CuttingInDate = garmentCuttingIn.CuttingInDate;
            FC = garmentCuttingIn.FC;
            CuttingFrom = garmentCuttingIn.CuttingFrom;

            Items = new List<GarmentSubconCuttingInItemDto>();
        }

        public Guid Id { get; set; }

        public string CutInNo { get; set; }
        public string CuttingType { get; set; }
        public string CuttingFrom { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset CuttingInDate { get; set; }
        public double FC { get; set; }
        public List<GarmentSubconCuttingInItemDto> Items { get; set; }
    }
}
