using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories
{
    public interface IGarmentServiceSampleFabricWashItemRepository : IAggregateRepository<GarmentServiceSampleFabricWashItem, GarmentServiceSampleFabricWashItemReadModel>
    {
        IQueryable<GarmentServiceSampleFabricWashItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
