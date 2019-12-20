using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentFinishingOuts.CommandHandlers
{
    public class PlaceGarmentFinishingOutCommandHandler : ICommandHandler<PlaceGarmentFinishingOutCommand, GarmentFinishingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;

        public PlaceGarmentFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
        }

        public async Task<GarmentFinishingOut> Handle(PlaceGarmentFinishingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentFinishingOut garmentFinishingOut = new GarmentFinishingOut(
                Guid.NewGuid(),
                GenerateSewOutNo(request),
                new UnitDepartmentId(request.UnitTo.Id),
                request.UnitTo.Code,
                request.UnitTo.Name,
                request.FinishingTo,
                request.FinishingOutDate,
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

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentFinishingOutItem garmentFinishingOutItem = new GarmentFinishingOutItem(
                        Guid.NewGuid(),
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
                        request.IsDifferentSize ? item.TotalQuantity : item.Quantity,
                        item.BasicPrice,
                        item.Price
                    );
                    if (request.IsDifferentSize)
                    {
                        foreach (var detail in item.Details)
                        {
                            GarmentFinishingOutDetail garmentFinishingOutDetail = new GarmentFinishingOutDetail(
                                Guid.NewGuid(),
                                garmentFinishingOutItem.Identity,
                                new SizeId(detail.Size.Id),
                                detail.Size.Size,
                                detail.Quantity,
                                new UomId(detail.Uom.Id),
                                detail.Uom.Unit
                            );

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
                var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finInItem.Key).Select(s => new GarmentFinishingInItem(s)).Single();
                garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity - finInItem.Value);
                garmentFinishingInItem.Modify();

                await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
            }


            await _garmentFinishingOutRepository.Update(garmentFinishingOut);

            _storage.Save();

            return garmentFinishingOut;
        }

        private string GenerateSewOutNo(PlaceGarmentFinishingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"FO{request.Unit.Code.Trim()}{year}{month}";

            var lastSewOutNo = _garmentFinishingOutRepository.Query.Where(w => w.FinishingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingOutNo)
                .Select(s => int.Parse(s.FinishingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewOutNo = $"{prefix}{(lastSewOutNo + 1).ToString("D4")}";

            return SewOutNo;
        }
    }
}