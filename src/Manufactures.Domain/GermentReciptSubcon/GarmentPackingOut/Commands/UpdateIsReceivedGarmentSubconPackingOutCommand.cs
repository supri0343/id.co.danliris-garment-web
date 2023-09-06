using Infrastructure.Domain.Commands;

using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut.Commands
{
    public class UpdateIsReceivedGarmentSubconPackingOutCommand : ICommand<GarmentSubconPackingOut>
    {
        public UpdateIsReceivedGarmentSubconPackingOutCommand(Guid id, bool isReceived)
        {
            Identity = id;
            IsReceived =isReceived;
        }

        public Guid Identity { get; private set; }
        public bool IsReceived { get; private set; }
    }
}
