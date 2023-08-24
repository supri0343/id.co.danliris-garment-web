using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetMonitoringWithCreatedUTC
{
    public class GetMonitoringWithCreatedUTCQueryHandler : IQueryHandler<GetMonitoringWithCreatedUTCQuery, GarmentMonitoringCuttingOutWithUTCViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;

        public GetMonitoringWithCreatedUTCQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
        }

        public async Task<GarmentMonitoringCuttingOutWithUTCViewModel> Handle(GetMonitoringWithCreatedUTCQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);

            var cuttingOutQuery = _garmentCuttingOutRepository.Query
                .Where(co => co.CuttingOutDate.AddHours(7).Date >= dateFrom.Date && co.CuttingOutDate.AddHours(7).Date <= dateTo.Date && co.UnitId == (request.unit != 0 ? request.unit : co.UnitId));

            var selectedQuery = cuttingOutQuery.Select(co => new GarmentMonitoringCuttingOutWithUTCDto
            {
                Id = co.Identity,
                CutOutNo = co.CutOutNo,
                CuttingOutType = co.CuttingOutType,
                UnitFrom = new UnitDepartment(co.UnitFromId, co.UnitFromCode, co.UnitFromName),
                CuttingOutDate = co.CuttingOutDate.AddHours(7),
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                Comodity = new GarmentComodity(co.ComodityId, co.ComodityCode, co.ComodityName),
                CreatedDate = co.CreatedDate
            }).ToList();

            foreach (var co in selectedQuery)
            {
                co.Items = _garmentCuttingOutItemRepository.Query.Where(x => x.CutOutId == co.Id).OrderBy(x => x.Identity).Select(coi => new GarmentCuttingOutItemDto
                {
                    Id = coi.Identity,
                    CutOutId = coi.CutOutId,
                    CuttingInId = coi.CuttingInId,
                    CuttingInDetailId = coi.CuttingInDetailId,
                    Product = new Product(coi.ProductId, coi.ProductCode, coi.ProductName),
                    DesignColor = coi.DesignColor,
                    TotalCuttingOut = coi.TotalCuttingOut,
                }).ToList();

                foreach (var coi in co.Items)
                {
                    coi.Details = _garmentCuttingOutDetailRepository.Query.Where(x => x.CutOutItemId == coi.Id).OrderBy(x => x.Identity).Select(cod => new GarmentCuttingOutDetailDto
                    {
                        Id = cod.Identity,
                        CutOutItemId = cod.CutOutItemId,
                        Size = new SizeValueObject(cod.SizeId, cod.SizeName),
                        CuttingOutQuantity = cod.CuttingOutQuantity,
                        CuttingOutUom = new Uom(cod.CuttingOutUomId, cod.CuttingOutUomUnit),
                        Color = cod.Color,
                        RemainingQuantity = cod.RemainingQuantity,
                        BasicPrice = cod.BasicPrice,
                        Price = cod.Price,
                    }).ToList();
                }

                co.TotalCuttingOutQuantity = co.Items.Sum(i => i.Details.Sum(d => d.CuttingOutQuantity));
                co.TotalRemainingQuantity = co.Items.Sum(i => i.Details.Sum(d => d.RemainingQuantity));
            }

            await Task.Yield();

            return new GarmentMonitoringCuttingOutWithUTCViewModel
            {
                data = selectedQuery,
                count = selectedQuery.Count(),
            };

        }

    }

}
