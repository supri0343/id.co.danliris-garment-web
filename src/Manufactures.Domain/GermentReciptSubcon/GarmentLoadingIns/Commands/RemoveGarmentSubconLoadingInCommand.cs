using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Commands
{
    public class RemoveGarmentSubconLoadingInCommand : ICommand<GarmentSubconLoadingIn>
    {
        public RemoveGarmentSubconLoadingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
