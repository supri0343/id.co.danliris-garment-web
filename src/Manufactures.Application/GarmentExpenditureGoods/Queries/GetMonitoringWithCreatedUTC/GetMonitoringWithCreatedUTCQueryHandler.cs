using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Manufactures.Application.GarmentExpenditureGoods.Queries.GetMonitoringWithCreatedUTC.GarmentMonitoringExpenditureGoodWithUTCDto;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetMonitoringWithCreatedUTC
{
    public class GetMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetMonitoringWithCreatedUTCQuery, GarmentMonitoringExpenditureGoodWithUTCViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;

        public GetMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodRepository = _storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = _storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
        }

        public async Task<GarmentMonitoringExpenditureGoodWithUTCViewModel> Handle(GetMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var date = _garmentExpenditureGoodRepository.Query.Select(x => x.ExpenditureDate.AddHours(7));

            var Query = _garmentExpenditureGoodRepository.Query
                .Where(co => co.ExpenditureDate.AddHours(7).Date >= dateFrom.Date && co.ExpenditureDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = Query.Select(co => new GarmentMonitoringExpenditureGoodWithUTCDto
            {
                Id = co.Identity,
                ExpenditureGoodNo = co.ExpenditureGoodNo,
                ExpenditureType = co.ExpenditureType,
                ExpenditureDate = co.ExpenditureDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                CreatedDate = co.CreatedDate,
                Invoice = co.Invoice,
                Buyer = new Buyer(co.BuyerId,co.BuyerCode,co.BuyerName),
                Description = co.Description
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentExpenditureGoodItemRepository.Query.Where(x => x.ExpenditureGoodId == co.Id).OrderBy(x => x.Identity).Select(coi => new Item
                {
                    Quantity = coi.Quantity
                }).ToList();

                co.TotalQuantity = co.Items.Sum(i => i.Quantity);
            }

            await Task.Yield();

            return new GarmentMonitoringExpenditureGoodWithUTCViewModel
            {
                data = selectedQuery,
                count = selectedQuery.Count(),
            };

        }

    }

}
