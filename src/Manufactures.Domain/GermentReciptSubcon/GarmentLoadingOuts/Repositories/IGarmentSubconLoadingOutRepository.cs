using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Repositories
{
    public interface IGarmentSubconLoadingOutRepository : IAggregateRepository<GarmentSubconLoadingOut, GarmentSubconLoadingOutReadModel>
    {
        IQueryable<GarmentSubconLoadingOutReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<object> ReadExecute(IQueryable<GarmentSubconLoadingOutReadModel> model);
    }
}
