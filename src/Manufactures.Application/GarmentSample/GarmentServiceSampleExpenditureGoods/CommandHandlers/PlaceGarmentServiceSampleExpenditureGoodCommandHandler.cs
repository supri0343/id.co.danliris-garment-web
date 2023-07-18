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
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleExpenditureGoods.CommandHandlers
{
    public class PlaceGarmentServiceSampleExpenditureGoodCommandHandler : ICommandHandler<PlaceGarmentServiceSampleExpenditureGoodCommand, GarmentServiceSampleExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleExpenditureGoodRepository _garmentServiceSampleExpenditureGoodRepository;
        private readonly IGarmentServiceSampleExpenditureGoodtemRepository _garmentServiceSampleExpenditureGoodItemRepository;

        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;


        public PlaceGarmentServiceSampleExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodRepository>();
            _garmentServiceSampleExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodtemRepository>();

            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentServiceSampleExpenditureGood> Handle(PlaceGarmentServiceSampleExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            GarmentServiceSampleExpenditureGood garmentServiceSampleExpenditureGood = new GarmentServiceSampleExpenditureGood(
                  Guid.NewGuid(),
                  GenerateServiceSampleExpenditureGoodNo(request),
                  request.ServiceSampleExpenditureGoodDate,
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

                GarmentServiceSampleExpenditureGoodItem garmentServiceSampleExpenditureGoodItem = new GarmentServiceSampleExpenditureGoodItem(
                      Guid.NewGuid(),
                      garmentServiceSampleExpenditureGood.Identity,
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
                //                        "Sample",
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

                await _garmentServiceSampleExpenditureGoodItemRepository.Update(garmentServiceSampleExpenditureGoodItem);
            }

            await _garmentServiceSampleExpenditureGoodRepository.Update(garmentServiceSampleExpenditureGood);

            _storage.Save();

            return garmentServiceSampleExpenditureGood;
        }

        private string GenerateServiceSampleExpenditureGoodNo(PlaceGarmentServiceSampleExpenditureGoodCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJBJ{year}{month}";

            var lastServiceSampleExpenditureGoodNo = _garmentServiceSampleExpenditureGoodRepository.Query.Where(w => w.ServiceSampleExpenditureGoodNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSampleExpenditureGoodNo)
                .Select(s => int.Parse(s.ServiceSampleExpenditureGoodNo.Replace(prefix, "")))
                .FirstOrDefault();
            var ServiceSampleExpenditureGoodNo = "";

            if (lastServiceSampleExpenditureGoodNo != null)
            {
                ServiceSampleExpenditureGoodNo = $"{prefix}{(lastServiceSampleExpenditureGoodNo + 1).ToString("D4")}" + "-S";
            }
            else{
                ServiceSampleExpenditureGoodNo = $"{prefix}{(1).ToString("D4")}" + "-S";
            }
            

            return ServiceSampleExpenditureGoodNo;
        }
    }
}
