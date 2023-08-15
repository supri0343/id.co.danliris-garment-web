using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories
{
    public interface IGarmentSubconFinishingInItemRepository : IAggregateRepository<GarmentSubconFinishingInItem, GarmentSubconFinishingInItemReadModel>
    {
    }
}
