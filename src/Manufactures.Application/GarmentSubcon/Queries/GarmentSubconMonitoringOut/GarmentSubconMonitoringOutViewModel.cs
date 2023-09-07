using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconMonitoringOut
{
    public class GarmentSubconMonitoringOutViewModel
    {
        public List<GarmentSubconMonitoringOutDto> garmentSubconMonitoringOutDtosIn { get; set; }
        public List<GarmentSubconMonitoringOutDto> garmentSubconMonitoringOutDtosOut { get; set; }
        public int count { get; set; }
    }
}
