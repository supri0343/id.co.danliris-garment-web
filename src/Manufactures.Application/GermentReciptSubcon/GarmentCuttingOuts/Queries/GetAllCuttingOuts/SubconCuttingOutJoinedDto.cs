using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GermentReciptSubcon.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class SubconCuttingOutJoinedDto
    {
        public GarmentSubconCuttingOutListDto cuttingOutList { get; set; }
        public GarmentSubconCuttingOutItemDto cuttingOutItem { get; set; }
        public GarmentSubconCuttingOutDetailDto cuttingOutDetail { get; set; }
    }
}
