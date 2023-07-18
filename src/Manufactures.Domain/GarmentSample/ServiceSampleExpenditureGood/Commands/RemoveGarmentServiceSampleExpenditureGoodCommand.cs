using System;
using Infrastructure.Domain.Commands;

namespace Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Commands
{
    public class RemoveGarmentServiceSampleExpenditureGoodCommand : ICommand<GarmentServiceSampleExpenditureGood>
    {
        public RemoveGarmentServiceSampleExpenditureGoodCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
