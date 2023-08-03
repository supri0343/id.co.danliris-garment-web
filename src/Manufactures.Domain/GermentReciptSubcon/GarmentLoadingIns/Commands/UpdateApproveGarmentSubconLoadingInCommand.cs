using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Commands
{
    public class UpdateApproveGarmentSubconLoadingInCommand : ICommand<int>
    {
        public UpdateApproveGarmentSubconLoadingInCommand(List<string> ids, bool approved)
        {
            Identities = ids;
            Approved = approved;
        }

        public List<string> Identities { get; private set; }
        public bool Approved { get; private set; }
    }
}
