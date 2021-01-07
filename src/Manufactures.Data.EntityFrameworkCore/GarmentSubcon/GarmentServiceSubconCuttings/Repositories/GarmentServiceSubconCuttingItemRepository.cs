using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Repositories
{
    public class GarmentServiceSubconCuttingItemRepository : AggregateRepostory<GarmentServiceSubconCuttingItem, GarmentServiceSubconCuttingItemReadModel>, IGarmentServiceSubconCuttingItemRepository
    {
        protected override GarmentServiceSubconCuttingItem Map(GarmentServiceSubconCuttingItemReadModel readModel)
        {
            return new GarmentServiceSubconCuttingItem(readModel);
        }
    }
}

