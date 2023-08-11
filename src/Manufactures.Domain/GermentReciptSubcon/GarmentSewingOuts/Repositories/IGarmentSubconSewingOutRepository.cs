using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories
{
    public interface IGarmentSubconSewingOutRepository : IAggregateRepository<GarmentSubconSewingOut, GarmentSubconSewingOutReadModel>
    {
        IQueryable<GarmentSubconSewingOutReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSubconSewingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentSubconSewingOutReadModel> model);
        IQueryable ReadDynamic(string order, string search, string select, string keyword, string filter);
    }
}
