using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Commands
{
    public class UpdateDatesGarmentSubconFinishingInCommand : ICommand<int>
    {
        public UpdateDatesGarmentSubconFinishingInCommand(List<string> ids, DateTimeOffset date, string subconType)
        {
            Identities = ids;
            Date = date;
            SubconType = subconType;
        }

        public List<string> Identities { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public string SubconType { get; private set; }
    }
}
