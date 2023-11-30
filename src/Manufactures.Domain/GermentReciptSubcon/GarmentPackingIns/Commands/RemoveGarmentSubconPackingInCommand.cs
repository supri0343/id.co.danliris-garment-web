using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Commands
{
    public class RemoveGarmentSubconPackingInCommand : ICommand<GarmentSubconPackingIn>
    {
        public RemoveGarmentSubconPackingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
