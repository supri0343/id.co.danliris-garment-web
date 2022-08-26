using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentSubconCustomsIns
{
    public class GarmentSubconCustomsInDetailDto : BaseDto
    {
        public GarmentSubconCustomsInDetailDto(GarmentSubconCustomsInDetail garmentSubconCustomsInDetail)
        {
            Id = garmentSubconCustomsInDetail.Identity;
            SubconCustomsInItemId = garmentSubconCustomsInDetail.SubconCustomsInItemId;
            SubconCustomsOutId = garmentSubconCustomsInDetail.SubconCustomsOutId;
            CustomsOutNo = garmentSubconCustomsInDetail.CustomsOutNo;
            CustomsOutQty = garmentSubconCustomsInDetail.CustomsOutQty;
        }

        public Guid Id { get; set; }
        public Guid SubconCustomsInItemId { get; set; }
        public Guid SubconCustomsOutId { get; set; }
        public string CustomsOutNo { get; set; }
        public decimal CustomsOutQty { get; set; }
    }
    
}
