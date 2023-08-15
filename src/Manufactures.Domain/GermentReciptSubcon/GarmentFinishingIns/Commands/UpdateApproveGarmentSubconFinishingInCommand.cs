using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Commands
{
    public class UpdateApproveGarmentSubconFinishingInCommand : ICommand<int>
    {
        public UpdateApproveGarmentSubconFinishingInCommand(List<string> ids, bool approved)
        {
            Identities = ids;
            Approved = approved;
        }

        public List<string> Identities { get; private set; }
        public bool Approved { get; private set; }
    }
}
