using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Commands
{
    public class RemoveGarmentSubconFinishingOutCommand : ICommand<GarmentSubconFinishingOut>
    {
        public RemoveGarmentSubconFinishingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
