using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories
{
    public interface IGarmentSubconCuttingOutItemRepository : IAggregateRepository<GarmentSubconCuttingOutItem, GarmentSubconCuttingOutItemReadModel>
    {
    }
}
