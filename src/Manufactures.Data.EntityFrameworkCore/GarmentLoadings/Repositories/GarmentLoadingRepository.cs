using Infrastructure.Data.EntityFrameworkCore;

using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentLoadings.Repositories
{
    public class GarmentLoadingRepository : AggregateRepostory<GarmentLoading, GarmentLoadingReadModel>, IGarmentLoadingRepository
    {
        public IQueryable<GarmentLoadingReadModel> Read(string order, List<string> select, string filter)
        {
            throw new NotImplementedException();
        }

        protected override GarmentLoading Map(GarmentLoadingReadModel readModel)
        {
            return new GarmentLoading(readModel);
        }
    }
}
