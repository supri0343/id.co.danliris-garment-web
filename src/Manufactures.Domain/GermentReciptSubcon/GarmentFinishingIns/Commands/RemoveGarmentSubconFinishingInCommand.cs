using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Commands
{
    public class RemoveGarmentSubconFinishingInCommand : ICommand<GarmentSubconFinishingIn>
    {
        public RemoveGarmentSubconFinishingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
