using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishedGoodStocks.Repositories
{
    public class GarmentFinishedGoodStockRepository : AggregateRepostory<GarmentFinishedGoodStock, GarmentFinishedGoodStockReadModel>, IGarmentFinishedGoodStockRepository
    {
        protected override GarmentFinishedGoodStock Map(GarmentFinishedGoodStockReadModel readModel)
        {
            return new GarmentFinishedGoodStock(readModel);
        }
    }
}