using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories
{
    public interface IGarmentSubconReprocessRepository : IAggregateRepository<GarmentSubconReprocess, GarmentSubconReprocessReadModel>
    {
        IQueryable<GarmentSubconReprocessReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
