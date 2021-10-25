using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands
{
    public class PlaceGarmentServiceSubconShrinkagePanelCommand : ICommand<GarmentServiceSubconShrinkagePanel>
    {
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset? ServiceSubconShrinkagePanelDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSubconShrinkagePanelItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
    }
}
