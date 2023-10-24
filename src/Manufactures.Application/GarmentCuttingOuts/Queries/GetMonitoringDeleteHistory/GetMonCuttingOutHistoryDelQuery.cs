using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetMonitoringDeleteHistory
{
    public class GetMonCuttingOutHistoryDelQuery : IQuery<GarmentMonCuttingOutHistoryDelViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string token { get; private set; }
        public int unit { get; private set; }
        public DateTime? dateFrom { get; private set; }
        public DateTime? dateTo { get; private set; }
        public GetMonCuttingOutHistoryDelQuery(string monType, DateTime? dateFrom, DateTime? dateTo)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
        }
    }
}