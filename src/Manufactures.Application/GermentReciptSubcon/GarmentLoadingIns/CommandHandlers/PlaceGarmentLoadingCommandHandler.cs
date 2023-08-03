using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentLoadings.CommandHandlers
{
    public class PlaceGarmentLoadingCommandHandler : ICommandHandler<PlaceGarmentSubconLoadingInCommand, GarmentSubconLoadingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconLoadingInRepository _garmentLoadingRepository;
        private readonly IGarmentSubconLoadingInItemRepository _garmentLoadingItemRepository;

        //private readonly IGarmentSubconCuttingOutRepository _garmentCuttinOutRepository;
        //private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttinOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        public PlaceGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentSubconLoadingInRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();
            //_garmentCuttinOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            //_garmentCuttinOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
        }

        public async Task<GarmentSubconLoadingIn> Handle(PlaceGarmentSubconLoadingInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentSubconLoadingIn garmentLoading = new GarmentSubconLoadingIn(
                Guid.NewGuid(),
                GenerateLoadingNo(request),
                request.CuttingOutId,
                request.CuttingOutNo,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.LoadingDate,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                false
            );

            Dictionary<Guid, double> CutOutDetailToBeUpdated = new Dictionary<Guid, double>();
            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentSubconLoadingInItem garmentLoadingItem = new GarmentSubconLoadingInItem(
                        Guid.NewGuid(),
                        garmentLoading.Identity,
                        item.CuttingOutDetailId,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        item.Quantity,
                        item.Quantity,
                        item.BasicPrice,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        item.Price
                    );

                    if (CutOutDetailToBeUpdated.ContainsKey(item.CuttingOutDetailId))
                    {
                        CutOutDetailToBeUpdated[item.CuttingOutDetailId] += item.Quantity;
                    }
                    else
                    {
                        CutOutDetailToBeUpdated.Add(item.CuttingOutDetailId, item.Quantity);
                    }

                    await _garmentLoadingItemRepository.Update(garmentLoadingItem);

                }
            }

            foreach (var cuttingOutDetail in CutOutDetailToBeUpdated)
            {
                var garmentCuttingOutDetail = _garmentCuttingOutDetailRepository.Query.Where(x => x.Identity == cuttingOutDetail.Key).Select(s => new GarmentSubconCuttingOutDetail(s)).Single();
                garmentCuttingOutDetail.SetRemainingQuantity(garmentCuttingOutDetail.RemainingQuantity - cuttingOutDetail.Value);
                garmentCuttingOutDetail.Modify();

                await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);
            }

            await _garmentLoadingRepository.Update(garmentLoading);
            _storage.Save();

            return garmentLoading;
        }

        private string GenerateLoadingNo(PlaceGarmentSubconLoadingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"LD{unitcode}{year}{month}";

            var lastLoadingNo = _garmentLoadingRepository.Query.Where(w => w.LoadingNo.StartsWith(prefix))
                .OrderByDescending(o => o.LoadingNo)
                .Select(s => int.Parse(s.LoadingNo.Replace(prefix, "")))
                .FirstOrDefault();
            var loadingNo = $"{prefix}{(lastLoadingNo + 1).ToString("D4")}";

            return loadingNo;
        }

    }
}
