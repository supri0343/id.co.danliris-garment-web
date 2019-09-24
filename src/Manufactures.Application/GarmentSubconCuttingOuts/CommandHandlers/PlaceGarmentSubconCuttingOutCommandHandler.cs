using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Commands;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubconCuttingOuts.CommandHandlers
{
    public class PlaceGarmentSubconCuttingOutCommandHandler : ICommandHandler<PlaceGarmentSubconCuttingOutCommand, GarmentSubconCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        //private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        //private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public PlaceGarmentSubconCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            //_garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            //_garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        public async Task<GarmentSubconCuttingOut> Handle(PlaceGarmentSubconCuttingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true && item.Details.Count() > 0).ToList();

            GarmentSubconCuttingOut garmentCuttingOut = new GarmentSubconCuttingOut(
                Guid.NewGuid(),
                GenerateCutOutNo(request),
                request.CuttingOutType,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.CuttingOutDate.GetValueOrDefault(),
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name
            );

            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                GarmentSubconCuttingOutItem garmentCuttingOutItem = new GarmentSubconCuttingOutItem(
                    Guid.NewGuid(),
                    item.CuttingInId,
                    item.CuttingInDetailId,
                    garmentCuttingOut.Identity,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.TotalCuttingOut,
                    item.EPOId,
                    item.EPOItemId,
                    item.POSerialNumber
                );

                foreach (var detail in item.Details)
                {
                    GarmentSubconCuttingOutDetail garmentCuttingOutDetail = new GarmentSubconCuttingOutDetail(
                        Guid.NewGuid(),
                        garmentCuttingOutItem.Identity,
                        new SizeId(detail.Size.Id),
                        detail.Size.Size,
                        detail.Color,
                        detail.CuttingOutQuantity,
                        detail.CuttingOutQuantity,
                        new UomId(detail.CuttingOutUom.Id),
                        detail.CuttingOutUom.Unit,
                        detail.OTL1,
                        detail.OTL2,
                        detail.BasicPrice,
                        detail.IndirectPrice,
                        detail.Remark
                    );

                    if (cuttingInDetailToBeUpdated.ContainsKey(item.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[item.CuttingInDetailId] += detail.CuttingOutQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(item.CuttingInDetailId, detail.CuttingOutQuantity);
                    }

                    await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);

                }

                await _garmentCuttingOutItemRepository.Update(garmentCuttingOutItem);
            }

            foreach (var cuttingInDetail in cuttingInDetailToBeUpdated)
            {
                var garmentCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInDetail.Key).Select(s => new GarmentCuttingInDetail(s)).Single();
                garmentCuttingInDetail.SetRemainingQuantity(garmentCuttingInDetail.RemainingQuantity - cuttingInDetail.Value);
                garmentCuttingInDetail.Modify();

                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
            }

            await _garmentCuttingOutRepository.Update(garmentCuttingOut);

            _storage.Save();

            return garmentCuttingOut;
        }

        private string GenerateCutOutNo(PlaceGarmentSubconCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"CR{year}{month}";

            var lastCutOutNo = _garmentCuttingOutRepository.Query.Where(w => w.CutOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.CutOutNo)
                .Select(s => int.Parse(s.CutOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutOutNo = $"{prefix}{(lastCutOutNo + 1).ToString("D4")}";

            return CutOutNo;
        }

    }
}
