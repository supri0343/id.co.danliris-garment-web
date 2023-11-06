using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.LogHistories.Query
{
    public class LogHistoryQueryHandler : IQueryHandler<LogHistoryQuery, LogHistoryViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly ILogHistoryRepository _repos;

        public LogHistoryQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            _http = serviceProvider.GetService<IHttpClientService>();

            _repos = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<LogHistoryViewModel> Handle(LogHistoryQuery request, CancellationToken cancellationToken)
        {
            LogHistoryViewModel logHistoryViewModel = new LogHistoryViewModel();
            var Query = await (from a in _repos.Query
                               where a.CreatedDate.AddHours(7).Date >= request.dateFrom.Date && a.CreatedDate.AddHours(7).Date <= request.dateTo.Date
                               select new LogHistoryDto
                               {
                                   Division = a.Division,
                                   Activity = a.Activity,
                                   CreatedBy = a.CreatedBy,
                                   CreatedDate = a.CreatedDate.AddHours(7).DateTime
                               }).ToListAsync();

            logHistoryViewModel.data = Query;

            return logHistoryViewModel;
        }
    }
}
