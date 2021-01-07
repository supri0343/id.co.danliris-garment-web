using System;
using System.Collections.Generic;
using Infrastructure.Domain.Commands;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands
{
    public class UpdateDatesGarmentServiceSubconSewingCommand : ICommand<int>
    {
        public UpdateDatesGarmentServiceSubconSewingCommand(List<string> ids, DateTimeOffset date)
        {
            Identities = ids;
            Date = date;
        }

        public List<string> Identities { get; private set; }
        public DateTimeOffset Date { get; private set; }
    }
}
