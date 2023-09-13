using Infrastructure.Domain.Commands;

using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut.Commands
{
    public class UpdateIsPackingListGarmentSubconPackingOutCommand : ICommand<int>
    {
        public UpdateIsPackingListGarmentSubconPackingOutCommand(List<string> ids, bool isPackingList)
        {
            Identities = ids;
            IsPackingList = isPackingList;
        }

        public List<string> Identities { get; private set; }
        public bool IsPackingList { get; private set; }
    }
}
