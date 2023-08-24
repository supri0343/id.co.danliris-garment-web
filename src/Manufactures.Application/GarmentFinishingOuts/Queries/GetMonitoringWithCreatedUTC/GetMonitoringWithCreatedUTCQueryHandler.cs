using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Manufactures.Application.GarmentFinishingOuts.Queries.GetMonitoringWithCreatedUTC.GarmentMonitoringFinishingOutWithUTCDto;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetMonitoringWithCreatedUTC
{
    public class GetMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetMonitoringWithCreatedUTCQuery, GarmentMonitoringFinishingOutWithUTCViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;

        public GetMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = _storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = _storage.GetRepository<IGarmentFinishingOutItemRepository>();
        }

        public async Task<GarmentMonitoringFinishingOutWithUTCViewModel> Handle(GetMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var Query = _garmentFinishingOutRepository.Query
                .Where(co => co.FinishingOutDate.AddHours(7).Date >= dateFrom.Date && co.FinishingOutDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = Query.Select(co => new GarmentMonitoringFinishingOutWithUTCDto
            {
                Id = co.Identity,
                FinishingOutNo = co.FinishingOutNo,
                FinishingTo = co.FinishingTo,
                UnitTo = new UnitDepartment(co.UnitToId, co.UnitToCode, co.UnitToName),
                FinishingOutDate = co.FinishingOutDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                CreatedDate = co.CreatedDate
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentFinishingOutItemRepository.Query.Where(x => x.FinishingOutId == co.Id).OrderBy(x => x.Identity).Select(coi => new Item
                {
                    Quantity = coi.Quantity,
                    RemainingQuantity = coi.RemainingQuantity,
                }).ToList();

                co.TotalQuantity = co.Items.Sum(i => i.Quantity);
                co.TotalRemainingQuantity = co.Items.Sum(i => i.RemainingQuantity);
            }

            await Task.Yield();

            return new GarmentMonitoringFinishingOutWithUTCViewModel
            {
                data = selectedQuery,
                count = selectedQuery.Count(),
            };

        }

    }

}
