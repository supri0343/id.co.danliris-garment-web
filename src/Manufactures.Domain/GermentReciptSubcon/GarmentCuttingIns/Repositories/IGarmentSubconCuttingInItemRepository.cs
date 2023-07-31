using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories
{
    public interface IGarmentSubconCuttingInItemRepository : IAggregateRepository<GarmentSubconCuttingInItem, GarmentSubconCuttingInItemReadModel>
    {
    }
}
