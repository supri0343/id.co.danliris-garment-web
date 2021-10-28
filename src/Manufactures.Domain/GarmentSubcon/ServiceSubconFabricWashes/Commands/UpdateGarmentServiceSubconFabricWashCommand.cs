using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands
{
    public class UpdateGarmentServiceSubconFabricWashCommand : ICommand<GarmentServiceSubconFabricWash>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconFabricWashNo { get; set; }
        public DateTimeOffset? ServiceSubconFabricWashDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSubconFabricWashItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
}
