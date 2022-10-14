using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories
{
    public interface IGarmentServiceSubconCuttingSizeRepository : IAggregateRepository<GarmentServiceSubconCuttingSize, GarmentServiceSubconCuttingSizeReadModel>
    {
        IQueryable<GarmentServiceSubconCuttingSizeReadModel> ReadSize(int page, int size, string order, string keyword, string filter);
    }
}
