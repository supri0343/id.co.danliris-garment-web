using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Commands
{
    public class UpdateApproveGarmentSubconSewingInsCommand : ICommand<int>
    {
        public UpdateApproveGarmentSubconSewingInsCommand(List<string> ids, bool approved)
        {
            Identities = ids;
            Approved = approved;
        }

        public List<string> Identities { get; private set; }
        public bool Approved { get; private set; }
    }
}
