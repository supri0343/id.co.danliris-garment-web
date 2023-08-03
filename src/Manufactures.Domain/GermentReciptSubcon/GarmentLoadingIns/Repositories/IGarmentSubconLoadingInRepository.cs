using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories
{
    public interface IGarmentSubconLoadingInRepository : IAggregateRepository<GarmentSubconLoadingIn, GarmentSubconLoadingInReadModel>
    {
        IQueryable<GarmentSubconLoadingInReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<object> ReadExecute(IQueryable<GarmentSubconLoadingInReadModel> model);
    }
}
