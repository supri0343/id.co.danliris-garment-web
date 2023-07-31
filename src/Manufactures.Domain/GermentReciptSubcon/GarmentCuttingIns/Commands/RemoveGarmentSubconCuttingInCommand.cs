using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Commands
{
    public class RemoveGarmentSubconCuttingInCommand : ICommand<GarmentSubconCuttingIn>
    {
        public RemoveGarmentSubconCuttingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
