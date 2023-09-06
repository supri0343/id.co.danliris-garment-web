using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentPackingOut.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut.Repositories
{
    public interface IGarmentSubconPackingOutItemRepository : IAggregateRepository<GarmentSubconPackingOutItem, GarmentSubconPackingOutItemReadModel>
    {
    }
}

