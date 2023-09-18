using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentCuttingOuts.CommandHandlers
{
    public class PlaceGarmentSubconCuttingOutCommandHandler : ICommandHandler<PlaceGarmentSubconCuttingOutCommand, GarmentSubconCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingOutRepository _garmentSubconCuttingOutRepository;
        private readonly IGarmentSubconCuttingOutItemRepository _garmentSubconCuttingOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentSubconCuttingOutDetailRepository;
        private readonly IGarmentSubconCuttingInDetailRepository _garmentSubconCuttingInDetailRepository;
        //private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        //private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentSubconCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentSubconCuttingOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentSubconCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
            _garmentSubconCuttingInDetailRepository = storage.GetRepository<IGarmentSubconCuttingInDetailRepository>();
            //_garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            //_garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            _garmentComodityPriceRepository= storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSubconCuttingOut> Handle(PlaceGarmentSubconCuttingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true && item.Details.Count() > 0).ToList();

            GarmentSubconCuttingOut garmentSubconCuttingOut = new GarmentSubconCuttingOut(
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
                request.Comodity.Name,
                request.IsUsed
            );

            //GarmentSewingDO garmentSewingDO = new GarmentSewingDO(
            //    Guid.NewGuid(),
            //    GenerateSewingDONo(request),
            //    garmentCuttingOut.Identity,
            //    new UnitDepartmentId(request.UnitFrom.Id),
            //    request.UnitFrom.Code,
            //    request.UnitFrom.Name,
            //    new UnitDepartmentId(request.Unit.Id),
            //    request.Unit.Code,
            //    request.Unit.Name,
            //    request.RONo,
            //    request.Article,
            //    new GarmentComodityId(request.Comodity.Id),
            //    request.Comodity.Code,
            //    request.Comodity.Name,
            //    request.CuttingOutDate.GetValueOrDefault()
            //);

            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                GarmentSubconCuttingOutItem garmentSubconCuttingOutItem = new GarmentSubconCuttingOutItem(
                    Guid.NewGuid(),
                    item.CuttingInId,
                    item.CuttingInDetailId,
                    garmentSubconCuttingOut.Identity,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.TotalCuttingOutQuantity,
                    0
                );

                foreach (var detail in item.Details)
                {
                    GarmentSubconCuttingOutDetail garmentSubconCuttingOutDetail = new GarmentSubconCuttingOutDetail(
                        Guid.NewGuid(),
                        garmentSubconCuttingOutItem.Identity,
                        new SizeId(detail.Size.Id),
                        detail.Size.Size,
                        detail.Color.ToUpper(),
                        0,
                        detail.CuttingOutQuantity,
                        new UomId(detail.CuttingOutUom.Id),
                        detail.CuttingOutUom.Unit,
                        detail.BasicPrice,
                        detail.Price
                    );

                    if (cuttingInDetailToBeUpdated.ContainsKey(item.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[item.CuttingInDetailId] += detail.CuttingOutQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(item.CuttingInDetailId, detail.CuttingOutQuantity);
                    }

                    await _garmentSubconCuttingOutDetailRepository.Update(garmentSubconCuttingOutDetail);

                }
                await _garmentSubconCuttingOutItemRepository.Update(garmentSubconCuttingOutItem);
            }

            foreach (var cuttingInDetail in cuttingInDetailToBeUpdated)
            {
                var garmentSubconCuttingInDetail = _garmentSubconCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInDetail.Key).Select(s => new GarmentSubconCuttingInDetail(s)).Single();
                garmentSubconCuttingInDetail.SetRemainingQuantity(garmentSubconCuttingInDetail.RemainingQuantity - cuttingInDetail.Value);
                garmentSubconCuttingInDetail.Modify();

                await _garmentSubconCuttingInDetailRepository.Update(garmentSubconCuttingInDetail);
            }

            await _garmentSubconCuttingOutRepository.Update(garmentSubconCuttingOut);

            _storage.Save();

            return garmentSubconCuttingOut;
        }

        private string GenerateCutOutNo(PlaceGarmentSubconCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"CR{request.UnitFrom.Code}{year}{month}";

            var lastCutOutNo = _garmentSubconCuttingOutRepository.Query.Where(w => w.CutOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.CutOutNo)
                .Select(s => int.Parse(s.CutOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutOutNo = $"{prefix}{(lastCutOutNo + 1).ToString("D4")}";

            return CutOutNo;
        }
    }
}