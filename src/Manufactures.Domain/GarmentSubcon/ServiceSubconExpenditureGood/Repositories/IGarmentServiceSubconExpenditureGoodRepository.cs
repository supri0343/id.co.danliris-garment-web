using System;
using System.Linq;
using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories
{
    public interface IGarmentServiceSubconExpenditureGoodRepository : IAggregateRepository<GarmentServiceSubconExpenditureGood, GarmentServiceSubconExpenditureGoodReadModel>
    {
        IQueryable<GarmentServiceSubconExpenditureGoodReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
