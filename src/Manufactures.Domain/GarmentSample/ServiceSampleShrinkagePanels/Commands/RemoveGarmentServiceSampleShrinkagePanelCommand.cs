using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands
{
    public class RemoveGarmentServiceSampleShrinkagePanelCommand : ICommand<GarmentServiceSampleShrinkagePanel>
    {
        public RemoveGarmentServiceSampleShrinkagePanelCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
