using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories
{
    public interface IGarmentServiceSampleFabricWashDetailRepository : IAggregateRepository<GarmentServiceSampleFabricWashDetail, GarmentServiceSampleFabricWashDetailReadModel>
    {
    }
}
