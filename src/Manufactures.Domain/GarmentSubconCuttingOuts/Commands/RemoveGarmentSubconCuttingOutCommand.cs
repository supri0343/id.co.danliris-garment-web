using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.Commands
{
    class RemoveGarmentSubconCuttingOutCommand : ICommand<GarmentSubconCuttingOut>
    {
        public RemoveGarmentSubconCuttingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
