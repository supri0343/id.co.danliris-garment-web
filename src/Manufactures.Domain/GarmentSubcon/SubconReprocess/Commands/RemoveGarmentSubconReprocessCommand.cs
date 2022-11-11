using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands
{
    public class RemoveGarmentSubconReprocessCommand : ICommand<GarmentSubconReprocess>
    {
        public RemoveGarmentSubconReprocessCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}