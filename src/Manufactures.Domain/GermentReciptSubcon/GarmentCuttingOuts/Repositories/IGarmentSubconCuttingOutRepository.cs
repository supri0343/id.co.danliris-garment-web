using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories
{
    public interface IGarmentSubconCuttingOutRepository : IAggregateRepository<GarmentSubconCuttingOut, GarmentSubconCuttingOutReadModel>
    {
        IQueryable<GarmentSubconCuttingOutReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentSubconCuttingOutReadModel> query);
        IQueryable<GarmentSubconCuttingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
    
}
