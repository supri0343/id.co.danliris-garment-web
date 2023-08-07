using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Commands
{
    public class RemoveGarmentSubconLoadingOutCommand : ICommand<GarmentSubconLoadingOut>
    {
        public RemoveGarmentSubconLoadingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
