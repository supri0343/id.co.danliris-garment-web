using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories
{
    public interface IGarmentSubconReprocessItemRepository : IAggregateRepository<GarmentSubconReprocessItem, GarmentSubconReprocessItemReadModel>
    {
        IQueryable<GarmentSubconReprocessItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
