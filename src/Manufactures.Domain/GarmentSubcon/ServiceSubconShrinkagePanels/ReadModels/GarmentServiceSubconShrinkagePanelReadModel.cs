using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels
{
    public class GarmentServiceSubconShrinkagePanelReadModel : ReadModelBase
    {
        public GarmentServiceSubconShrinkagePanelReadModel(Guid identity) : base(identity)
        {
        }
        public string ServiceSubconShrinkagePanelNo { get; internal set; }
        public DateTimeOffset ServiceSubconShrinkagePanelDate { get; internal set; }
        public bool IsUsed { get; internal set; }

        public virtual List<GarmentServiceSubconShrinkagePanelItemReadModel> GarmentServiceSubconShrinkagePanelItem { get; internal set; }
    }
}
