using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentExpenditureGoods.CommandHandlers
{
    public class PlaceGarmentExpenditureGoodCommandHandler : ICommandHandler<PlaceGarmentExpenditureGoodCommand, GarmentExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentExpenditureGood> Handle(PlaceGarmentExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

            GarmentExpenditureGood garmentExpenditureGood = new GarmentExpenditureGood(
                Guid.NewGuid(),
                GenerateExpenditureGoodNo(request),
                request.ExpenditureType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                request.ExpenditureDate,
                request.Invoice,
                request.ContractNo,
                request.Carton,
                request.Description
            );

            Dictionary<Guid, double> finStockToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(
                    Guid.NewGuid(),
                    garmentExpenditureGood.Identity,
                    item.FinishedGoodStockId,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    item.Quantity,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Description,
                    item.BasicPrice,
                    item.Price
                );

                GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
                                        Guid.NewGuid(),
                                        item.FinishedGoodStockId,
                                        Guid.Empty,
                                        Guid.Empty,
                                        garmentExpenditureGood.Identity,
                                        garmentExpenditureGoodItem.Identity,
                                        "OUT",
                                        garmentExpenditureGood.RONo,
                                        garmentExpenditureGood.Article,
                                        garmentExpenditureGood.UnitId,
                                        garmentExpenditureGood.UnitCode,
                                        garmentExpenditureGood.UnitName,
                                        garmentExpenditureGood.ComodityId,
                                        garmentExpenditureGood.ComodityCode,
                                        garmentExpenditureGood.ComodityName,
                                        garmentExpenditureGoodItem.SizeId,
                                        garmentExpenditureGoodItem.SizeName,
                                        garmentExpenditureGoodItem.UomId,
                                        garmentExpenditureGoodItem.UomUnit,
                                        garmentExpenditureGoodItem.Quantity,
                                        garmentExpenditureGoodItem.BasicPrice,
                                        garmentExpenditureGoodItem.Price
                                    );
                await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);

                if (finStockToBeUpdated.ContainsKey(item.FinishedGoodStockId))
                {
                    finStockToBeUpdated[item.FinishedGoodStockId] += item.Quantity;
                }
                else
                {
                    finStockToBeUpdated.Add(item.FinishedGoodStockId, item.Quantity);
                }

                await _garmentExpenditureGoodItemRepository.Update(garmentExpenditureGoodItem);

            }

            foreach (var finStock in finStockToBeUpdated)
            {
                var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == finStock.Key).Select(s => new GarmentFinishedGoodStock(s)).Single();
                var qty = garmentFinishingGoodStockItem.Quantity - finStock.Value;
                garmentFinishingGoodStockItem.SetQuantity(qty);
                garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                garmentFinishingGoodStockItem.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);
            }

            await _garmentExpenditureGoodRepository.Update(garmentExpenditureGood);

            _storage.Save();

            return garmentExpenditureGood;
        }

        private string GenerateExpenditureGoodNo(PlaceGarmentExpenditureGoodCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var pre = request.ExpenditureType == "EXPORT" ? "EGE" : request.ExpenditureType == "SAMPLE" ? "EGS" : "EGL";

            var prefix = $"{pre}{unitcode}{year}{month}";

            var lastExpenditureGoodNo = _garmentExpenditureGoodRepository.Query.Where(w => w.ExpenditureGoodNo.StartsWith(prefix))
                .OrderByDescending(o => o.ExpenditureGoodNo)
                .Select(s => int.Parse(s.ExpenditureGoodNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastExpenditureGoodNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}

