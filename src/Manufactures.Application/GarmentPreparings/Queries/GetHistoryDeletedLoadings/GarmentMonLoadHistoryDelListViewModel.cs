using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeletedLoadings
{
  public  class GarmentMonLoadHistoryDelListViewModel
    {
        public List<GarmentMonLoadHistoryDelDto> garmentMonitorings { get; set; }
        public int count { get; set; }
    }
}
