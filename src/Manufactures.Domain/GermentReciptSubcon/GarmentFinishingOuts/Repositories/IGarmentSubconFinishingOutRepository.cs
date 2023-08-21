using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using System.Linq;


namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories
{
    public interface IGarmentSubconFinishingOutRepository : IAggregateRepository<GarmentSubconFinishingOut, GarmentReceiptSubconFinishingOutReadModel>
    {
        IQueryable<GarmentReceiptSubconFinishingOutReadModel> Read(int page, int size, string order, string keyword, string filter);
		IQueryable<GarmentReceiptSubconFinishingOutReadModel> ReadColor(int page, int size, string order, string keyword, string filter);
        IQueryable<object> ReadExecute(IQueryable<GarmentReceiptSubconFinishingOutReadModel> model);
    }
}
