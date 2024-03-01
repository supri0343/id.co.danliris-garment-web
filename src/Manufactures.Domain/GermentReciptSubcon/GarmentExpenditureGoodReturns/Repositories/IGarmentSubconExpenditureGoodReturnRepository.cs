using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Repositories
{
    public interface IGarmentSubconExpenditureGoodReturnRepository : IAggregateRepository<GarmentSubconExpenditureGoodReturn, GarmentSubconExpenditureGoodReturnReadModel>
    {
        IQueryable<GarmentSubconExpenditureGoodReturnReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
