using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories
{
    public interface IGarmentServiceSampleSewingDetailRepository : IAggregateRepository<GarmentServiceSampleSewingDetail, GarmentServiceSampleSewingDetailReadModel>
    {
    }
}
