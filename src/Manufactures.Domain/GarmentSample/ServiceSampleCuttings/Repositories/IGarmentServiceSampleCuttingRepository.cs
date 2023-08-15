using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories
{
    public interface IGarmentServiceSampleCuttingRepository : IAggregateRepository<GarmentServiceSampleCutting, GarmentServiceSampleCuttingReadModel>
    {
        IQueryable<GarmentServiceSampleCuttingReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}