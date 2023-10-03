using Infrastructure.Domain.Repositories;
using Manufactures.Domain.LogHistories.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.LogHistory.Repositories
{
    public interface ILogHistoryRepository : IAggregateRepository<LogHistory, LogHistoryReadModel>
    {
        IQueryable<LogHistoryReadModel> Read(string filter);
    }
}
