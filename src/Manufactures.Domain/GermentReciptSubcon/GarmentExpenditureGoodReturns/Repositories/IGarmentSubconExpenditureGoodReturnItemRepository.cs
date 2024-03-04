using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Repositories
{
    public interface IGarmentSubconExpenditureGoodReturnItemRepository : IAggregateRepository<GarmentSubconExpenditureGoodReturnItem, GarmentSubconExpenditureGoodReturnItemReadModel>
    {
    }
}
