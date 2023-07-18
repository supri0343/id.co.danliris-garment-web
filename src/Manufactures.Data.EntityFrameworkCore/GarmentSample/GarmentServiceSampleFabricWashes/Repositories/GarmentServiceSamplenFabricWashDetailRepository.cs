using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleFabricWashes.Repositories
{
    public class GarmentServiceSampleFabricWashDetailRepository : AggregateRepostory<GarmentServiceSampleFabricWashDetail, GarmentServiceSampleFabricWashDetailReadModel>, IGarmentServiceSampleFabricWashDetailRepository
    {
        protected override GarmentServiceSampleFabricWashDetail Map(GarmentServiceSampleFabricWashDetailReadModel readModel)
        {
            return new GarmentServiceSampleFabricWashDetail(readModel);
        }
    }
}
