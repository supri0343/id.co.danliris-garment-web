using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GermentCuttingIn
{
    public class GarmentSubconCuttingInItemDto : BaseDto
    {
        public GarmentSubconCuttingInItemDto(GarmentSubconCuttingInItem garmentCuttingInItem)
        {
            Id = garmentCuttingInItem.Identity;
            CutInId = garmentCuttingInItem.CutInId;
            PreparingId = garmentCuttingInItem.PreparingId;
            UENId = garmentCuttingInItem.UENId;
            UENNo = garmentCuttingInItem.UENNo;
            SewingOutId = garmentCuttingInItem.SewingOutId;
            SewingOutNo = garmentCuttingInItem.SewingOutNo;

            Details = new List<GarmentSubconCuttingInDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid CutInId { get; set; }
        public Guid PreparingId { get; set; }
        public int UENId { get; set; }
        public string UENNo { get; set; }
        public Guid SewingOutId { get; set; }
        public string SewingOutNo { get; set; }
        public List<GarmentSubconCuttingInDetailDto> Details { get; set; }
    }
}
