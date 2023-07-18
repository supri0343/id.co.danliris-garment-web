using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories
{
    public interface IGarmentServiceSampleCuttingItemRepository : IAggregateRepository<GarmentServiceSampleCuttingItem, GarmentServiceSampleCuttingItemReadModel>
    {

        IQueryable<GarmentServiceSampleCuttingItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
