using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentLoadingOuts.CommandHandlers
{
    public class PlaceGarmentLoadingOutCommandHandler : ICommandHandler<PlaceGarmentSubconLoadingOutCommand, GarmentSubconLoadingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconLoadingOutRepository _garmentLoadingOutRepository;
        private readonly IGarmentSubconLoadingOutItemRepository _garmentLoadingOutItemRepository;

        private readonly IGarmentSubconLoadingInItemRepository _garmentLoadingInItemRepository;
        public PlaceGarmentLoadingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingOutRepository = storage.GetRepository<IGarmentSubconLoadingOutRepository>();
            _garmentLoadingOutItemRepository = storage.GetRepository<IGarmentSubconLoadingOutItemRepository>();

            _garmentLoadingInItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();
        }

        public async Task<GarmentSubconLoadingOut> Handle(PlaceGarmentSubconLoadingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentSubconLoadingOut garmentLoading = new GarmentSubconLoadingOut(
                Guid.NewGuid(),
                GenerateLoadingNo(request),
                request.LoadingInId,
                request.LoadingInNo,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.LoadingOutDate,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name
            );

            Dictionary<Guid, double> LoadingInToBeUpdated = new Dictionary<Guid, double>();
            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentSubconLoadingOutItem garmentLoadingItem = new GarmentSubconLoadingOutItem(
                        Guid.NewGuid(),
                        garmentLoading.Identity,
                        item.LoadingInItemId,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        item.Quantity,
                        0,
                        item.BasicPrice,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        item.Price
                    );

                    if (LoadingInToBeUpdated.ContainsKey(item.LoadingInItemId))
                    {
                        LoadingInToBeUpdated[item.LoadingInItemId] += item.Quantity;
                    }
                    else
                    {
                        LoadingInToBeUpdated.Add(item.LoadingInItemId, item.Quantity);
                    }

                    await _garmentLoadingOutItemRepository.Update(garmentLoadingItem);

                }
            }

            foreach (var loadingInItem in LoadingInToBeUpdated)
            {
                var garmentLoadingInItem = _garmentLoadingInItemRepository.Query.Where(x => x.Identity == loadingInItem.Key).Select(s => new GarmentSubconLoadingInItem(s)).Single();
                garmentLoadingInItem.SetRemainingQuantity(garmentLoadingInItem.RemainingQuantity - loadingInItem.Value);
                garmentLoadingInItem.Modify();

                await _garmentLoadingInItemRepository.Update(garmentLoadingInItem);
            }

            await _garmentLoadingOutRepository.Update(garmentLoading);
            _storage.Save();

            return garmentLoading;
        }

        private string GenerateLoadingNo(PlaceGarmentSubconLoadingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"LO{unitcode}{year}{month}";

            var lastLoadingNo = _garmentLoadingOutRepository.Query.Where(w => w.LoadingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.LoadingOutNo)
                .Select(s => int.Parse(s.LoadingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var loadingNo = $"{prefix}{(lastLoadingNo + 1).ToString("D4")}";

            return loadingNo;
        }

    }
}
