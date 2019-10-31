using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns.Repositories
{
    interface IGarmentFinishingInItemRepository : IAggregateRepository<GarmentFinishingInItem, GarmentFinishingInItemReadModel>
    {
    }
}
