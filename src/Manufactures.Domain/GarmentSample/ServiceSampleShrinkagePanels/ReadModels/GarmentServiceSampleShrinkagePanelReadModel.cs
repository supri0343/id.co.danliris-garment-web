using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels
{
    public class GarmentServiceSampleShrinkagePanelReadModel : ReadModelBase
    {
        public GarmentServiceSampleShrinkagePanelReadModel(Guid identity) : base(identity)
        {
        }
        public string ServiceSampleShrinkagePanelNo { get; internal set; }
        public DateTimeOffset ServiceSampleShrinkagePanelDate { get; internal set; }
        public string Remark { get; internal set; }
        public bool IsUsed { get; internal set; }
        public int QtyPacking { get; internal set; }
        public string UomUnit { get; internal set; }
        public double NettWeight { get; internal set; }
        public double GrossWeight { get; internal set; }
        public virtual List<GarmentServiceSampleShrinkagePanelItemReadModel> GarmentServiceSampleShrinkagePanelItem { get; internal set; }
    }
}
