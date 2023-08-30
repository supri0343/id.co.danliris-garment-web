using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories
{
    public interface IGarmentSubconPackingInRepository : IAggregateRepository<GarmentSubconPackingIn, GarmentSubconPackingInReadModel>
    {
        IQueryable<GarmentSubconPackingInReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentSubconPackingInReadModel> query);

        //IQueryable<GarmentSubconPackingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}
