using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentPackingOut.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut.Repositories
{
    public interface IGarmentSubconPackingOutRepository : IAggregateRepository<GarmentSubconPackingOut, GarmentSubconPackingOutReadModel>
    {
        IQueryable<GarmentSubconPackingOutReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSubconPackingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);

        double BasicPriceByRO(string Keyword = null, string Filter = "{}");
        IQueryable<object> ReadExecute(IQueryable<GarmentSubconPackingOutReadModel> query);

        IQueryable<GarmentSubconPackingOutReadModel> ReadignoreFilter(int page, int size, string order, string keyword, string filter);
    }

}
