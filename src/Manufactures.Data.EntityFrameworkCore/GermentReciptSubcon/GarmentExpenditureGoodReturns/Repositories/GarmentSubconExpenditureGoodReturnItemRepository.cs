using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentExpenditureGoodReturns.Repositories
{
    public class GarmentSubconExpenditureGoodReturnItemRepository : AggregateRepostory<GarmentSubconExpenditureGoodReturnItem, GarmentSubconExpenditureGoodReturnItemReadModel>, IGarmentSubconExpenditureGoodReturnItemRepository
    {
        protected override GarmentSubconExpenditureGoodReturnItem Map(GarmentSubconExpenditureGoodReturnItemReadModel readModel)
        {
            return new GarmentSubconExpenditureGoodReturnItem(readModel);
        }
    }
}
