using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconReprocess.Repositories
{
    public class GarmentSubconReprocessDetailRepository : AggregateRepostory<GarmentSubconReprocessDetail, GarmentSubconReprocessDetailReadModel>, IGarmentSubconReprocessDetailRepository
    {
        protected override GarmentSubconReprocessDetail Map(GarmentSubconReprocessDetailReadModel readModel)
        {
            return new GarmentSubconReprocessDetail(readModel);
        }
    }
}
