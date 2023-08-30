using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Commands
{
    public class UpdateApproveGarmentSubconPackingInCommand : ICommand<int>
    {
        public UpdateApproveGarmentSubconPackingInCommand(List<string> ids, bool approved)
        {
            Identities = ids;
            Approved = approved;
        }

        public List<string> Identities { get; private set; }
        public bool Approved { get; private set; }
    }
}
