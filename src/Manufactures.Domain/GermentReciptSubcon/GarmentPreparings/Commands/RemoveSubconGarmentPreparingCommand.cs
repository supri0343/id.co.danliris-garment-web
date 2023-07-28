using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.Commands
{
    public class RemoveSubconGarmentPreparingCommand : ICommand<GarmentSubconPreparing>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}