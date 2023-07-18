using System;
using Infrastructure.Domain.Commands;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands
{
    public class RemoveGarmentServiceSampleSewingCommand : ICommand<GarmentServiceSampleSewing>
    {
        public RemoveGarmentServiceSampleSewingCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
