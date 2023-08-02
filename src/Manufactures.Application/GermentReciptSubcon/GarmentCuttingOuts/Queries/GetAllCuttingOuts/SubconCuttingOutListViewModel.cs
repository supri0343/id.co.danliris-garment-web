using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GermentReciptSubcon.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class SubconCuttingOutListViewModel
    {
        public List<GarmentSubconCuttingOutListDto> data { get; set; }
        public int total { get; set; }
        public double totalQty { get; internal set; }
    }
}
