using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands
{
    public class RemoveGarmentServiceSampleFabricWashCommand : ICommand<GarmentServiceSampleFabricWash>
    {
        public RemoveGarmentServiceSampleFabricWashCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
