using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSampleExpenditureGoods.Commands
{
    public class UpdateIsReceivedGarmentSampleExpenditureGoodCommand : ICommand<GarmentSampleExpenditureGood>
    {
        public UpdateIsReceivedGarmentSampleExpenditureGoodCommand(Guid id, bool isReceived)
        {
            Identity = id;
            IsReceived =isReceived;
        }

        public Guid Identity { get; private set; }
        public bool IsReceived { get; private set; }
    }
}
