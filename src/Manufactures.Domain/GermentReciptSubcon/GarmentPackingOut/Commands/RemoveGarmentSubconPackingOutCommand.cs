using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut.Commands
{
    public class RemoveGarmentSubconPackingOutCommand : ICommand<GarmentSubconPackingOut>
    {
        public RemoveGarmentSubconPackingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
