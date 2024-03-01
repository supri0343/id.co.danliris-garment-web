using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Commands
{
    public class RemoveSubconGarmentExpenditureGoodReturnCommand : ICommand<GarmentSubconExpenditureGoodReturn>
    {
        public RemoveSubconGarmentExpenditureGoodReturnCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
