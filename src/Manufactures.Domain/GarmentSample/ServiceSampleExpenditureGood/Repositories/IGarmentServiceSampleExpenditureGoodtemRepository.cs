using System;
using System.Linq;
using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;

namespace Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories
{
    public interface IGarmentServiceSampleExpenditureGoodtemRepository : IAggregateRepository<GarmentServiceSampleExpenditureGoodItem, GarmentServiceSampleExpenditureGoodItemReadModel>
    {
        IQueryable<GarmentServiceSampleExpenditureGoodItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
