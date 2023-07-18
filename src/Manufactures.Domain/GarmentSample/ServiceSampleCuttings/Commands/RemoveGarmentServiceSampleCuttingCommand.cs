using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands
{
    public class RemoveGarmentServiceSampleCuttingCommand : ICommand<GarmentServiceSampleCutting>
    {
        public RemoveGarmentServiceSampleCuttingCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}