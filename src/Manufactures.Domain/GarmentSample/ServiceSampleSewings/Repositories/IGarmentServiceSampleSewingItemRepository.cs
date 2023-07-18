using System;
using System.Linq;
using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories
{
    public interface IGarmentServiceSampleSewingItemRepository : IAggregateRepository<GarmentServiceSampleSewingItem, GarmentServiceSampleSewingItemReadModel>
    {
        IQueryable<GarmentServiceSampleSewingItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
