using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories
{
    public interface IGarmentSubconSewingInRepository : IAggregateRepository<GarmentSubconSewingIn, GarmentSubconSewingInReadModel>
    {
        IQueryable<GarmentSubconSewingInReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<GarmentSubconSewingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
        IQueryable<object> ReadExecute(IQueryable<GarmentSubconSewingInReadModel> query);
    }
}