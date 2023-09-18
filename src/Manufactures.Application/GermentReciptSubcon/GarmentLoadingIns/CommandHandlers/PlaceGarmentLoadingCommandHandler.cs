using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
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
        private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttinOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;

        private readonly IGarmentSubconCuttingInDetailRepository _garmentCuttingInDetailRepository;
        public PlaceGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentSubconLoadingInRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();
            //_garmentCuttinOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentCuttinOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();

            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSubconCuttingInDetailRepository>();
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
            Dictionary<Guid, double> CutOutDetailToBeUpdated2 = new Dictionary<Guid, double>();
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
                else
                {
                    if (CutOutDetailToBeUpdated2.ContainsKey(item.CuttingOutDetailId))
                    {
                        CutOutDetailToBeUpdated2[item.CuttingOutDetailId] += item.Quantity;
                    }
                    else
                    {
                        CutOutDetailToBeUpdated2.Add(item.CuttingOutDetailId, item.Quantity);
                    }
                }
            }

            //If item Saved
            foreach (var cuttingOutDetail in CutOutDetailToBeUpdated)
            {
                //Update Real Qty Cutting Out
                var garmentCuttingOutDetail = _garmentCuttingOutDetailRepository.Query.Where(x => x.Identity == cuttingOutDetail.Key).Select(s => new GarmentSubconCuttingOutDetail(s)).Single();

                double diffQty = garmentCuttingOutDetail.CuttingOutQuantity - cuttingOutDetail.Value;
                garmentCuttingOutDetail.SetRealOutQuantity(cuttingOutDetail.Value);
                garmentCuttingOutDetail.Modify();

                await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);

                //Update RemainingQty Cutting In If CuttingOut Qty is different with Real Out Qty
                if(diffQty > 0)
                {
                    var garmentCuttingOutItem = _garmentCuttinOutItemRepository.Query.Where(x => x.Identity == garmentCuttingOutDetail.CutOutItemId).Select(s => new GarmentSubconCuttingOutItem(s)).Single();
                    garmentCuttingOutItem.SetRealOutQuantity(garmentCuttingOutItem.RealQtyOut + cuttingOutDetail.Value);
                    garmentCuttingOutItem.Modify();

                    await _garmentCuttinOutItemRepository.Update(garmentCuttingOutItem);

                    var garmenCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == garmentCuttingOutItem.CuttingInDetailId).Select(s => new GarmentSubconCuttingInDetail(s)).Single();

                    garmenCuttingInDetail.SetRemainingQuantity(garmenCuttingInDetail.RemainingQuantity + diffQty);

                    garmenCuttingInDetail.Modify();

                    await _garmentCuttingInDetailRepository.Update(garmenCuttingInDetail);
                }
            }

            //If item Not Saved
            foreach (var cuttingOutDetail in CutOutDetailToBeUpdated2)
            {
                //Delete Cutting Out Detail
                var garmentCuttingOutDetail = _garmentCuttingOutDetailRepository.Query.Where(x => x.Identity == cuttingOutDetail.Key).Select(s => new GarmentSubconCuttingOutDetail(s)).Single();

                double diffQty = garmentCuttingOutDetail.CuttingOutQuantity;

                //Update RemainingQty Cutting In If CuttingOut Qty is different with Real Out Qty
                if (diffQty > 0)
                {
                    var garmentCuttingOutItem = _garmentCuttinOutItemRepository.Query.Where(x => x.Identity == garmentCuttingOutDetail.CutOutItemId).Select(s => new GarmentSubconCuttingOutItem(s)).Single();
                    
                    if(garmentCuttingOutItem.TotalCuttingOut - diffQty == 0)
                    {
                        garmentCuttingOutItem.Remove();
                    }
                    else
                    {
                        garmentCuttingOutItem.SetTotalCuttingOutQuantity(garmentCuttingOutItem.TotalCuttingOut - cuttingOutDetail.Value);
                        garmentCuttingOutItem.Modify();
                    }

                    await _garmentCuttinOutItemRepository.Update(garmentCuttingOutItem);

                    var garmenCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == garmentCuttingOutItem.CuttingInDetailId).Select(s => new GarmentSubconCuttingInDetail(s)).Single();

                    garmenCuttingInDetail.SetRemainingQuantity(garmenCuttingInDetail.RemainingQuantity + diffQty);

                    garmenCuttingInDetail.Modify();

                    await _garmentCuttingInDetailRepository.Update(garmenCuttingInDetail);
                }

                garmentCuttingOutDetail.Remove();

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
