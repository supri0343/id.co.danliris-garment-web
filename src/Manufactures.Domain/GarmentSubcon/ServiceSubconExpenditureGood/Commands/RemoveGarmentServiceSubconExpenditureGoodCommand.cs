using System;
using Infrastructure.Domain.Commands;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Commands
{
    public class RemoveGarmentServiceSubconExpenditureGoodCommand : ICommand<GarmentServiceSubconExpenditureGood>
    {
        public RemoveGarmentServiceSubconExpenditureGoodCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
