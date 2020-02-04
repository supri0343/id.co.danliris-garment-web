using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapTransactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.Commands
{
    public class RemoveGarmentScrapSourceCommand : ICommand<GarmentScrapSource>
    {
        public RemoveGarmentScrapSourceCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
