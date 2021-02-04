using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers
{
    public class PlaceGarmentServiceSubconSewingCommandHandler : ICommandHandler<PlaceGarmentServiceSubconSewingCommand, GarmentServiceSubconSewing>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;
        private readonly IGarmentServiceSubconSewingItemRepository _garmentServiceSubconSewingItemRepository;

        public PlaceGarmentServiceSubconSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconSewingRepository = storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconSewingItemRepository = storage.GetRepository<IGarmentServiceSubconSewingItemRepository>();
        }

        public async Task<GarmentServiceSubconSewing> Handle(PlaceGarmentServiceSubconSewingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentServiceSubconSewing garmentServiceSubconSewing = new GarmentServiceSubconSewing(
                Guid.NewGuid(),
                GenerateServiceSubconSewingNo(request),
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.ServiceSubconSewingDate.GetValueOrDefault(),
                request.IsUsed
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSubconSewingItem garmentServiceSubconSewingItem = new GarmentServiceSubconSewingItem(
                    Guid.NewGuid(),
                    garmentServiceSubconSewing.Identity,
                    item.RONo,
                    item.Article,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name
                );
                item.Id = garmentServiceSubconSewingItem.Identity;
                    
                foreach(var detail in item.Details)
                {
                    GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail = new GarmentServiceSubconSewingDetail(
                        new Guid(),
                        item.Id,
                        detail.SewingInId,
                        detail.SewingInItemId,
                        new ProductId(detail.Product.Id),
                        detail.Product.Code,
                        detail.Product.Name,
                        detail.DesignColor,
                        detail.Quantity,
                        new UomId(detail.Uom.Id),
                        detail.Uom.Unit
                    );
                }

                    await _garmentServiceSubconSewingItemRepository.Update(garmentServiceSubconSewingItem);
            }

            await _garmentServiceSubconSewingRepository.Update(garmentServiceSubconSewing);

            _storage.Save();

            return garmentServiceSubconSewing;
        }

        private string GenerateServiceSubconSewingNo(PlaceGarmentServiceSubconSewingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJS{year}{month}";

            var lastServiceSubconSewingNo = _garmentServiceSubconSewingRepository.Query.Where(w => w.ServiceSubconSewingNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSubconSewingNo)
                .Select(s => int.Parse(s.ServiceSubconSewingNo.Replace(prefix, "")))
                .FirstOrDefault();
            var ServiceSubconSewingNo = $"{prefix}{(lastServiceSubconSewingNo + 1).ToString("D4")}";

            return ServiceSubconSewingNo;
        }
    }
}
