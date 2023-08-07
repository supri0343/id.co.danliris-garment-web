using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Repositories
{
    public interface IGarmentSubconLoadingOutItemRepository : IAggregateRepository<GarmentSubconLoadingOutItem, GarmentSubconLoadingOutItemReadModel>
    {
    }
}
