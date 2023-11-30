using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories
{
    public interface IGarmentSubconLoadingInItemRepository : IAggregateRepository<GarmentSubconLoadingInItem, GarmentSubconLoadingInItemReadModel>
    {
    }
}
