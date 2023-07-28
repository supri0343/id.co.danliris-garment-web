using FluentValidation;
using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.Commands
{
    public class UpdateDatesGarmentSubconPreparingCommand : ICommand<int>
    {
        public UpdateDatesGarmentSubconPreparingCommand(List<string> ids, DateTimeOffset date)
        {
            Identities = ids;
            Date = date;
        }

        public List<string> Identities { get; private set; }
        public DateTimeOffset Date { get; private set; }
    }

}
