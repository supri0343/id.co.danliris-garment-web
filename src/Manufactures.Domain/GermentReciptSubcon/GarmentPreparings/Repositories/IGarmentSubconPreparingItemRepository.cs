using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories
{
    public interface IGarmentSubconPreparingItemRepository : IAggregateRepository<GarmentSubconPreparingItem, GarmentSubconPreparingItemReadModel>
    {
    }
}