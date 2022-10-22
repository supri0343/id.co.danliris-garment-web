using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.GetReportAval
{
    public class GetReportAvalQuery : IQuery<GetMutationScrapListViewModel>
    {
        public string token { get; private set; }
        public string type { get; private set; }
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }

        public GetReportAvalQuery(DateTime dateFrom, DateTime dateTo, string token,string tipe)
        {
            this.type = tipe;
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.token = token;
        }
    }
}
