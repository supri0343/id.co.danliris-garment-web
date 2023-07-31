using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels;
using System.Linq;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories
{
    public interface IGarmentSubconCuttingInRepository : IAggregateRepository<GarmentSubconCuttingIn, GarmentSubconCuttingInReadModel>
    {
        IQueryable<GarmentSubconCuttingInReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentSubconCuttingInReadModel> query);
    }
}
