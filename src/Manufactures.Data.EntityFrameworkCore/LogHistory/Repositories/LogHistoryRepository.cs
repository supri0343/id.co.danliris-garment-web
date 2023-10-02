using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.LogHistories.ReadModels;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.LogHistories.Repositories
{
    public class LogHistoryRepository : AggregateRepostory<LogHistory, LogHistoryReadModel>, ILogHistoryRepository
    {
        public IQueryable<LogHistoryReadModel> Read(string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<LogHistoryReadModel>.Filter(data, FilterDictionary);


            //Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<LogHistoryReadModel>.Order(data, OrderDictionary);

            data.OrderByDescending(o => o.ModifiedDate);
            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override LogHistory Map(LogHistoryReadModel readModel)
        {
            return new LogHistory(readModel);
        }
    }
}
