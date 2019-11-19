using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingIns.CommandHandlers
{
    public class PlaceGarmentSewingInCommandHandler : ICommandHandler<PlaceGarmentSewingInCommand, GarmentSewingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;

        public PlaceGarmentSewingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            //_garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
        }

        public async Task<GarmentSewingIn> Handle(PlaceGarmentSewingInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                Guid.NewGuid(),
                GenerateSewingInNo(request),
                request.SewingFrom,
                request.LoadingId,
                request.LoadingNo,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.SewingInDate.GetValueOrDefault()
            );

            foreach (var item in request.Items)
            {
                GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(
                    Guid.NewGuid(),
                    garmentSewingIn.Identity,
                    item.SewingOutItemId,
                    item.SewingOutDetailId,
                    item.LoadingItemId,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    item.Quantity,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Color,
                    item.Quantity
                );

                var garmentLoadingItem = _garmentLoadingItemRepository.Find(o => o.Identity == item.LoadingItemId).Single();

                garmentLoadingItem.SetRemainingQuantity(garmentLoadingItem.RemainingQuantity - item.Quantity);

                garmentLoadingItem.Modify();
                await _garmentLoadingItemRepository.Update(garmentLoadingItem);

                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
            }

            await _garmentSewingInRepository.Update(garmentSewingIn);

            _storage.Save();

            return garmentSewingIn;
        }

        private string GenerateSewingInNo(PlaceGarmentSewingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"DS{request.Unit.Code}{year}{month}";

            var lastSewingInNo = _garmentSewingInRepository.Query.Where(w => w.SewingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingInNo)
                .Select(s => int.Parse(s.SewingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingInNo = $"{prefix}{(lastSewingInNo + 1).ToString("D4")}";

            return SewingInNo;
        }
    }
}