using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories
{
    public interface IGarmentSubconPreparingRepository : IAggregateRepository<GarmentSubconPreparing, GarmentSubconPreparingReadModel>
    {
        IQueryable<GarmentSubconPreparingReadModel> Read(string order, List<string> select, string filter);
        IQueryable<GarmentSubconPreparingReadModel> ReadOptimized(string order, string filter, string keyword);
        IQueryable<object> ReadExecute(IQueryable<GarmentSubconPreparingReadModel> model, string keyword);
        bool RoChecking(IEnumerable<string> roList, string buyerCode);
    }
}