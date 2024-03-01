using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks.Repositories
{
    public interface IGarmentSubconFinishedGoodStockRepository : IAggregateRepository<GarmentSubconFinishedGoodStock, GarmentSubconFinishedGoodStockReadModel>
    {
        IQueryable<GarmentSubconFinishedGoodStockReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSubconFinishedGoodStockReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}
