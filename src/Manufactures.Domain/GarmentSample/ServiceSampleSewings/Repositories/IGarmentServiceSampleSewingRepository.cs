using System;
using System.Linq;
using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories
{
    public interface IGarmentServiceSampleSewingRepository : IAggregateRepository<GarmentServiceSampleSewing, GarmentServiceSampleSewingReadModel>
    {
        IQueryable<GarmentServiceSampleSewingReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
