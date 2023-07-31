using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentCuttingIns.CommandHandlers
{
    public class PlaceGarmentCuttingInCommandHandler : ICommandHandler<PlaceGarmentSubconCuttingInCommand, GarmentSubconCuttingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentSubconCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentSubconCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSubconPreparingItemRepository _garmentPreparingItemRepository;

        public PlaceGarmentCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingInRepository = storage.GetRepository<IGarmentSubconCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentSubconCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSubconCuttingInDetailRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentSubconPreparingItemRepository>();
        }

        public async Task<GarmentSubconCuttingIn> Handle(PlaceGarmentSubconCuttingInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.Details.Where(detail => detail.IsSave).Count() > 0).ToList();

            GarmentSubconCuttingIn garmentCuttingIn = new GarmentSubconCuttingIn(
                Guid.NewGuid(),
                GenerateCutInNo(request),
                request.CuttingType,
                request.CuttingFrom,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.CuttingInDate.GetValueOrDefault(),
                request.FC
            );

            Dictionary<Guid, decimal> preparingItemToBeUpdated = new Dictionary<Guid, decimal>();

            foreach (var item in request.Items)
            {
                GarmentSubconCuttingInItem garmentCuttingInItem = new GarmentSubconCuttingInItem(
                    Guid.NewGuid(),
                    garmentCuttingIn.Identity,
                    item.PreparingId,
                    item.UENId,
                    item.UENNo,
                    item.SewingOutId,
                    item.SewingOutNo
                );

                //var garmentPreparing = _garmentPreparingRepository.Query.Where(x => x.Identity == item.PreparingId).Select(s => new GarmentPreparing(s)).Single();
                //garmentPreparing.setIsCuttingIN(true);
                //garmentPreparing.SetModified();
                //await _garmentPreparingRepository.Update(garmentPreparing);

                foreach (var detail in item.Details)
                {
                    if (detail.IsSave)
                    {
                        GarmentSubconCuttingInDetail garmentCuttingInDetail = new GarmentSubconCuttingInDetail(
                            Guid.NewGuid(),
                            garmentCuttingInItem.Identity,
                            detail.PreparingItemId,
                            Guid.Empty,
                            Guid.Empty,
                            new ProductId(detail.Product.Id),
                            detail.Product.Code,
                            detail.Product.Name,
                            detail.DesignColor,
                            detail.FabricType,
                            detail.PreparingQuantity,
                            new UomId(detail.PreparingUom.Id),
                            detail.PreparingUom.Unit,
                            detail.CuttingInQuantity,
                            new UomId(detail.CuttingInUom.Id),
                            detail.CuttingInUom.Unit,
                            detail.RemainingQuantity,
                            detail.BasicPrice,
                            detail.Price,
                            detail.FC,
                            null
                        );

                        if (preparingItemToBeUpdated.ContainsKey(detail.PreparingItemId))
                        {
                            preparingItemToBeUpdated[detail.PreparingItemId] += (decimal)detail.PreparingQuantity;
                        }
                        else
                        {
                            preparingItemToBeUpdated.Add(detail.PreparingItemId, (decimal)detail.PreparingQuantity);
                        }

                        await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                    }
                }

                await _garmentCuttingInItemRepository.Update(garmentCuttingInItem);
            }

            foreach (var preparingItem in preparingItemToBeUpdated)
            {
                var garmentPreparingItem = _garmentPreparingItemRepository.Query.Where(x => x.Identity == preparingItem.Key).Select(s => new GarmentSubconPreparingItem(s)).Single();
                garmentPreparingItem.setRemainingQuantity(Convert.ToDouble((decimal)garmentPreparingItem.RemainingQuantity - preparingItem.Value));
                garmentPreparingItem.SetModified();

                await _garmentPreparingItemRepository.Update(garmentPreparingItem);
            }

            //request.Items.Select(item => new GarmentSubconCuttingInItem(
            //        Guid.NewGuid(),
            //        garmentCuttingIn.Identity,
            //        item.PreparingId,
            //        item.UENId,
            //        item.UENNo
            //    ))
            //    .ToList()
            //    .ForEach(async item => await _garmentCuttingInItemRepository.Update(item));

            //request.Items.ForEach(item => item.Details.Select(detail => new GarmentCuttingInDetail(
            //            Guid.NewGuid(),
            //            Guid.NewGuid(),
            //            detail.PreparingItemId,
            //            new ProductId(detail.Product.Id),
            //            detail.Product.Code,
            //            detail.Product.Name,
            //            detail.DesignColor,
            //            detail.FabricType,
            //            detail.PreparingQuantity,
            //            new UomId(detail.PreparingUom.Id),
            //            detail.PreparingUom.Unit,
            //            detail.CuttingInQuantity,
            //            new UomId(detail.CuttingInUom.Id),
            //            detail.CuttingInUom.Unit,
            //            detail.RemainingQuantity,
            //            detail.BasicPrice
            //        ))
            //        .ToList()
            //        .ForEach(async detail => await _garmentCuttingInDetailRepository.Update(detail))
            //    );

            await _garmentCuttingInRepository.Update(garmentCuttingIn);

            _storage.Save();

            return garmentCuttingIn;
        }

        private string GenerateCutInNo(PlaceGarmentSubconCuttingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"DC{request.Unit.Code.Trim()}{year}{month}";

            var lastCutInNo = _garmentCuttingInRepository.Query.Where(w => w.CutInNo.StartsWith(prefix))
                .OrderByDescending(o => o.CutInNo)
                .Select(s => int.Parse(s.CutInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutInNo = $"{prefix}{(lastCutInNo + 1).ToString("D4")}";

            return CutInNo;
        }
    }
}
