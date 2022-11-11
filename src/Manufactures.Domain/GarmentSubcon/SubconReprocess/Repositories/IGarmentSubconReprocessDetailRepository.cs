using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories
{
    public interface IGarmentSubconReprocessDetailRepository : IAggregateRepository<GarmentSubconReprocessDetail, GarmentSubconReprocessDetailReadModel>
    {
    }
}
