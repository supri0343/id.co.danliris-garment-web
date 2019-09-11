using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentLoadings.CommandHandlers
{
    public class PlaceGarmentLoadingCommandHandler : ICommandHandler<PlaceGarmentLoadingCommand, GarmentLoading>
    {
        private readonly IStorage _storage;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;

        public PlaceGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
        }

        public async Task<GarmentLoading> Handle(PlaceGarmentLoadingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentLoading garmentLoading = new GarmentLoading(
                Guid.NewGuid(),
                GenerateLoadingNo(request),
                request.SewingDOId,
                request.SewingDONo,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.Comodity,
                request.LoadingDate
            );

            Dictionary<Guid, double> preparingItemToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                
                GarmentLoadingItem garmentLoadingItem = new GarmentLoadingItem(
                    Guid.NewGuid(),
                    garmentLoading.Identity,
                    item.SewingDOItemId,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.Quantity,
                    item.RemainingQuantity,
                    item.BasicPrice,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Color
                );

                await _garmentLoadingItemRepository.Update(garmentLoadingItem);
            }

            await _garmentLoadingRepository.Update(garmentLoading);

            _storage.Save();

            return garmentLoading;
        }

        private string GenerateLoadingNo(PlaceGarmentLoadingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");

            var prefix = $"LD{year}{month}{day}";

            var lastCutInNo = _garmentLoadingRepository.Query.Where(w => w.LoadingNo.StartsWith(prefix))
                .OrderByDescending(o => o.LoadingNo)
                .Select(s => int.Parse(s.LoadingNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutInNo = $"{prefix}{(lastCutInNo + 1).ToString("D4")}";

            return CutInNo;
        }
    }
}
