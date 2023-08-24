using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Manufactures.Application.GarmentLoadings.Queries.GetMonitoringWithCreatedUTC.GarmentMonitoringLoadingListDto;

namespace Manufactures.Application.GarmentLoadings.Queries.GetMonitoringWithCreatedUTC
{
    public class GetMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetMonitoringWithCreatedUTCQuery, GarmentMonitoringLoadingListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;
        public GetMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
        }

        public async Task<GarmentMonitoringLoadingListViewModel> Handle(GetMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var LoadingQuery = _garmentLoadingRepository.Query
                .Where(co => co.LoadingDate.AddHours(7).Date >= dateFrom.Date && co.LoadingDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = LoadingQuery.Select(co => new GarmentMonitoringLoadingListDto
            {
                Id = co.Identity,
                LoadingNo = co.LoadingNo,
                UnitFrom = new UnitDepartment(co.UnitFromId, co.UnitFromCode, co.UnitFromName),
                LoadingDate = co.LoadingDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                Comodity = new GarmentComodity(co.ComodityId, co.ComodityCode, co.ComodityName),
                CreatedDate = co.CreatedDate
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentLoadingItemRepository.Query.Where(x => x.LoadingId == co.Id).OrderBy(x => x.Identity).Select(coi => new Item
                {
                    Quantity = coi.Quantity,
                    RemainingQuantity = coi.RemainingQuantity,
                }).ToList();

                co.TotalLoadingQuantity = co.Items.Sum(i => i.Quantity);
                co.TotalRemainingQuantity = co.Items.Sum(i => i.RemainingQuantity);
            }

            await Task.Yield();

            return new GarmentMonitoringLoadingListViewModel
            {
                data = selectedQuery,
                count = selectedQuery.Count(),
            };

        }

    }

}
