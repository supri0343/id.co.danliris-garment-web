using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories
{
    public interface IGarmentSubconFinishingInRepository : IAggregateRepository<GarmentSubconFinishingIn, GarmentSubconFinishingInReadModel>
    {
        IQueryable<GarmentSubconFinishingInReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSubconFinishingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}
