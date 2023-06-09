using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconExpenditureGoods.CommandHandlers
{
    public class PlaceGarmentServiceSubconExpenditureGoodCommandHandler : ICommandHandler<PlaceGarmentServiceSubconExpenditureGoodCommand, GarmentServiceSubconExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconExpenditureGoodRepository _garmentServiceSubconExpenditureGoodRepository;
        private readonly IGarmentServiceSubconExpenditureGoodtemRepository _garmentServiceSubconExpenditureGoodItemRepository;

        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;


        public PlaceGarmentServiceSubconExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodRepository>();
            _garmentServiceSubconExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodtemRepository>();

            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentServiceSubconExpenditureGood> Handle(PlaceGarmentServiceSubconExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            GarmentServiceSubconExpenditureGood garmentServiceSubconExpenditureGood = new GarmentServiceSubconExpenditureGood(
                  Guid.NewGuid(),
                  GenerateServiceSubconExpenditureGoodNo(request),
                  request.ServiceSubconExpenditureGoodDate,
                  false,
                  new BuyerId(request.Buyer.Id),
                  request.Buyer.Code,
                  request.Buyer.Name,
                  request.QtyPacking,
                  request.UomUnit,
                  request.NettWeight,
                  request.GrossWeight
                );

            foreach(var item in request.Items)
            {

                GarmentServiceSubconExpenditureGoodItem garmentServiceSubconExpenditureGoodItem = new GarmentServiceSubconExpenditureGoodItem(
                      Guid.NewGuid(),
                      garmentServiceSubconExpenditureGood.Identity,
                      item.FinishingGoodStockId,
                      item.RONo,
                      item.Article,
                      new GarmentComodityId(item.Comodity.Id),
                      item.Comodity.Code,
                      item.Comodity.Name,
                      new UnitDepartmentId(item.Unit.Id),
                      item.Unit.Code,
                      item.Unit.Name,
                      item.UomUnit,
                      item.Quantity,
                      item.BasicPrice
                    );
                #region FinishingGoodStock
                ////Reduce Quantity of FinishingGoodStok
                //GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == item.Unit.Id && a.ComodityId == item.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

                //var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == item.FinishingGoodStockId).Select(s => new GarmentFinishedGoodStock(s)).Single();
                //var qty = garmentFinishingGoodStockItem.Quantity - item.Quantity;
                //garmentFinishingGoodStockItem.SetQuantity(qty);
                //garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                //garmentFinishingGoodStockItem.Modify();

                //await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);

                ////Add FinishingGoodStockHistory
                //GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
                //                        Guid.NewGuid(),
                //                        item.FinishingGoodStockId,
                //                        Guid.Empty,
                //                        Guid.Empty,
                //                        Guid.Empty,
                //                        Guid.Empty,
                //                        Guid.Empty,
                //                        Guid.Empty,
                //                        Guid.Empty,
                //                        Guid.Empty,
                //                        "SUBCON",
                //                        item.RONo,
                //                        item.Article,
                //                        new UnitDepartmentId(item.Unit.Id),
                //                        item.Unit.Code,
                //                        item.Unit.Name,
                //                       new GarmentComodityId(item.Comodity.Id),
                //                        item.Comodity.Code,
                //                        item.Comodity.Name,
                //                        garmentFinishingGoodStockItem.SizeId,
                //                        garmentFinishingGoodStockItem.SizeName,
                //                        garmentFinishingGoodStockItem.UomId,
                //                        item.UomUnit,
                //                        item.Quantity,
                //                        item.BasicPrice,
                //                        garmentFinishingGoodStockItem.Price
                //                    );
                //await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                #endregion

                await _garmentServiceSubconExpenditureGoodItemRepository.Update(garmentServiceSubconExpenditureGoodItem);
            }

            await _garmentServiceSubconExpenditureGoodRepository.Update(garmentServiceSubconExpenditureGood);

            _storage.Save();

            return garmentServiceSubconExpenditureGood;
        }

        private string GenerateServiceSubconExpenditureGoodNo(PlaceGarmentServiceSubconExpenditureGoodCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJBJ{year}{month}";

            var lastServiceSubconExpenditureGoodNo = _garmentServiceSubconExpenditureGoodRepository.Query.Where(w => w.ServiceSubconExpenditureGoodNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSubconExpenditureGoodNo)
                .Select(s => int.Parse(s.ServiceSubconExpenditureGoodNo.Replace(prefix, "")))
                .FirstOrDefault();
            var ServiceSubconExpenditureGoodNo = "";

            if (lastServiceSubconExpenditureGoodNo != null)
            {
                ServiceSubconExpenditureGoodNo = $"{prefix}{(lastServiceSubconExpenditureGoodNo + 1).ToString("D4")}";
            }
            else{
                ServiceSubconExpenditureGoodNo = $"{prefix}{(1).ToString("D4")}";
            }
            

            return ServiceSubconExpenditureGoodNo;
        }
    }
}
