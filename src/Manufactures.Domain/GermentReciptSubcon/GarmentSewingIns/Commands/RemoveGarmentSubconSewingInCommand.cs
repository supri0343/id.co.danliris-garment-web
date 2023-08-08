using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Commands
{
    public class RemoveGarmentSubconSewingInCommand : ICommand<GarmentSubconSewingIn>
    {
        public RemoveGarmentSubconSewingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}