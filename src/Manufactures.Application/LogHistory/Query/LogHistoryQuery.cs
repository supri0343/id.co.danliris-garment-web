using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.LogHistories.Query
{
    public class LogHistoryQuery : IQuery<LogHistoryViewModel>
    {
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }
        public string token { get; private set; }

        public LogHistoryQuery(DateTime dateFrom, DateTime dateTo,string token)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.token = token;
        }
    }
}
