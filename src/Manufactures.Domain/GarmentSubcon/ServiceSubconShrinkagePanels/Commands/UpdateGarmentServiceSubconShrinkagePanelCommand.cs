using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands
{
    public class UpdateGarmentServiceSubconShrinkagePanelCommand : ICommand<GarmentServiceSubconShrinkagePanel>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset? ServiceSubconShrinkagePanelDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSubconShrinkagePanelItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
}
