using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Manufactures.Application.GarmentSewingOuts.Queries.GetMonitoringWithCreatedUTC.GarmentMonitoringSewingOutWithUTCDto;

namespace Manufactures.Application.GarmentSewingOuts.Queries.GetMonitoringWithCreatedUTC
{
    public class GetMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetMonitoringWithCreatedUTCQuery, GarmentMonitoringSewingOutWithUTCViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;

        public GetMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = _storage.GetRepository<IGarmentSewingOutRepository>();
            _garmentSewingOutItemRepository = _storage.GetRepository<IGarmentSewingOutItemRepository>();
        }

        public async Task<GarmentMonitoringSewingOutWithUTCViewModel> Handle(GetMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var Query = _garmentSewingOutRepository.Query
                .Where(co => co.SewingOutDate.AddHours(7).Date >= dateFrom.Date && co.SewingOutDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = Query.Select(co => new GarmentMonitoringSewingOutWithUTCDto
            {
                Id = co.Identity,
                SewingOutNo = co.SewingOutNo,
                SewingTo = co.SewingTo,
                UnitTo = new UnitDepartment(co.UnitToId, co.UnitToCode, co.UnitToName),
                SewingOutDate = co.SewingOutDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                CreatedDate = co.CreatedDate
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentSewingOutItemRepository.Query.Where(x => x.SewingOutId == co.Id).OrderBy(x => x.Identity).Select(coi => new Item
                {
                    Quantity = coi.Quantity,
                    RemainingQuantity = coi.RemainingQuantity,
                }).ToList();

                co.TotalQuantity = co.Items.Sum(i => i.Quantity);
                co.TotalRemainingQuantity = co.Items.Sum(i => i.RemainingQuantity);
            }

            await Task.Yield();

            return new GarmentMonitoringSewingOutWithUTCViewModel
            {
                data = selectedQuery,
                count = selectedQuery.Count(),
            };

        }

    }

}
