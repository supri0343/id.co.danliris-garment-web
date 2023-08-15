using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleSewings.Repositories
{
    public class GarmentServiceSampleSewingDetailRepository : AggregateRepostory<GarmentServiceSampleSewingDetail, GarmentServiceSampleSewingDetailReadModel>, IGarmentServiceSampleSewingDetailRepository
    {
        protected override GarmentServiceSampleSewingDetail Map(GarmentServiceSampleSewingDetailReadModel readModel)
        {
            return new GarmentServiceSampleSewingDetail(readModel);
        }
    }
}
