using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Commands
{
    public class RemoveGarmentSubconSewingOutCommand : ICommand<GarmentSubconSewingOut>
    {
        public RemoveGarmentSubconSewingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
