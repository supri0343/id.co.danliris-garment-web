using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentFinishingOuts.CommandHandlers
{
    public class PlaceGarmentSubconFinishingOutCommandHandler : ICommandHandler<PlaceGarmentSubconFinishingOutCommand, GarmentSubconFinishingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentSubconFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentSubconFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentSubconFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentSubconFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentSubconFinishingOutRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSubconFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentSubconFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentSubconFinishingInItemRepository>();
            _garmentComodityPriceRepository= storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSubconFinishingOut> Handle(PlaceGarmentSubconFinishingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.UnitTo.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();
            Guid garmentFinishingOutId = Guid.NewGuid();
            GarmentSubconFinishingOut garmentFinishingOut = new GarmentSubconFinishingOut(
                garmentFinishingOutId,
                GenerateFinOutNo(request),
                new UnitDepartmentId(request.UnitTo.Id),
                request.UnitTo.Code,
                request.UnitTo.Name,
                request.FinishingTo,
                request.FinishingOutDate.GetValueOrDefault(),
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.IsDifferentSize
            );

            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();

            Dictionary<string, double> finGood = new Dictionary<string, double>();

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    Guid garmentFinishingOutItemId = Guid.NewGuid();
                    GarmentSubconFinishingOutItem garmentFinishingOutItem = new GarmentSubconFinishingOutItem(
                        garmentFinishingOutItemId,
                        garmentFinishingOut.Identity,
                        item.FinishingInId,
                        item.FinishingInItemId,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        request.IsDifferentSize ? item.TotalQuantity : item.Quantity,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        0,
                        item.BasicPrice,
                        item.Price
                    );
                    item.Id = garmentFinishingOutItemId;
                    if (request.IsDifferentSize)
                    {
                        foreach (var detail in item.Details)
                        {
                            Guid garmentFinishingOutDetailId = Guid.NewGuid();
                            GarmentSubconFinishingOutDetail garmentFinishingOutDetail = new GarmentSubconFinishingOutDetail(
                                garmentFinishingOutDetailId,
                                garmentFinishingOutItem.Identity,
                                new SizeId(detail.Size.Id),
                                detail.Size.Size,
                                detail.Quantity,
                                new UomId(detail.Uom.Id),
                                detail.Uom.Unit,
                                0
                            );
                            detail.Id = garmentFinishingOutDetailId;
                            if (finishingInItemToBeUpdated.ContainsKey(item.FinishingInItemId))
                            {
                                finishingInItemToBeUpdated[item.FinishingInItemId] += detail.Quantity;
                            }
                            else
                            {
                                finishingInItemToBeUpdated.Add(item.FinishingInItemId, detail.Quantity);
                            }

                            await _garmentFinishingOutDetailRepository.Update(garmentFinishingOutDetail);    
                        }
                    }
                    else
                    {
                        if (finishingInItemToBeUpdated.ContainsKey(item.FinishingInItemId))
                        {
                            finishingInItemToBeUpdated[item.FinishingInItemId] += item.Quantity;
                        }
                        else
                        {
                            finishingInItemToBeUpdated.Add(item.FinishingInItemId, item.Quantity);
                        }
                    }
                    await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                }
            }

            foreach (var finInItem in finishingInItemToBeUpdated)
            {
                var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finInItem.Key).Select(s => new GarmentSubconFinishingInItem(s)).Single();
                garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity - finInItem.Value);
                garmentFinishingInItem.Modify();

                await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
            }

            await _garmentFinishingOutRepository.Update(garmentFinishingOut);

            _storage.Save();

            return garmentFinishingOut;
        }

        private string GenerateFinOutNo(PlaceGarmentSubconFinishingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"FO{request.Unit.Code.Trim()}{year}{month}";

            var lastFinOutNo = _garmentFinishingOutRepository.Query.Where(w => w.FinishingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingOutNo)
                .Select(s => int.Parse(s.FinishingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewOutNo = $"{prefix}{(lastFinOutNo + 1).ToString("D4")}";

            return SewOutNo;
        }
    }
}