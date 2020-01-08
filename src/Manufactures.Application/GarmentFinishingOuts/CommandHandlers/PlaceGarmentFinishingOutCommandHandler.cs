using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Data.EntityFrameworkCore.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
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
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository= storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentFinishingOut> Handle(PlaceGarmentFinishingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();
            Guid garmentFinishingOutId = Guid.NewGuid();
            GarmentFinishingOut garmentFinishingOut = new GarmentFinishingOut(
                garmentFinishingOutId,
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

            Dictionary<GarmentFinishedGoodStock, double> finGood = new Dictionary<GarmentFinishedGoodStock, double>();

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    Guid garmentFinishingOutItemId = Guid.NewGuid();
                    GarmentFinishingOutItem garmentFinishingOutItem = new GarmentFinishingOutItem(
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
                        request.IsDifferentSize ? item.TotalQuantity : item.Quantity,
                        item.BasicPrice,
                        item.Price
                    );
                    if (request.IsDifferentSize)
                    {
                        foreach (var detail in item.Details)
                        {
                            Guid garmentFinishingOutDetailId = Guid.NewGuid();
                            GarmentFinishingOutDetail garmentFinishingOutDetail = new GarmentFinishingOutDetail(
                                garmentFinishingOutDetailId,
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

                            GarmentFinishedGoodStock finStock = new GarmentFinishedGoodStock(
                                Guid.Empty,
                                "",
                                request.RONo,
                                request.Article,
                                new UnitDepartmentId(request.Unit.Id),
                                request.Unit.Code,
                                request.Unit.Name,
                                new GarmentComodityId(request.Comodity.Id),
                                request.Comodity.Code,
                                request.Comodity.Name,
                                new SizeId(detail.Size.Id),
                                detail.Size.Size,
                                new UomId(detail.Uom.Id),
                                detail.Uom.Unit,
                                0,
                                item.BasicPrice,
                                0
                                );

                            if (finGood.ContainsKey(finStock))
                            {
                                finGood[finStock] += detail.Quantity;
                            }
                            else
                            {
                                finGood.Add(finStock, detail.Quantity);
                            }
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

                        GarmentFinishedGoodStock finStock = new GarmentFinishedGoodStock(
                                Guid.Empty,
                                "",
                                request.RONo,
                                request.Article,
                                new UnitDepartmentId(request.Unit.Id),
                                request.Unit.Code,
                                request.Unit.Name,
                                new GarmentComodityId(request.Comodity.Id),
                                request.Comodity.Code,
                                request.Comodity.Name,
                                new SizeId(item.Size.Id),
                                item.Size.Size,
                                new UomId(item.Uom.Id),
                                item.Uom.Unit,
                                0,
                                item.BasicPrice,
                                0
                                );

                        if (finGood.ContainsKey(finStock))
                        {
                            finGood[finStock] += item.Quantity;
                        }
                        else
                        {
                            finGood.Add(finStock, item.Quantity);
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

            int count = 0;
            List<GarmentFinishedGoodStock> finGoodStocks = new List<GarmentFinishedGoodStock>();
            foreach(var finGoodStock in finGood)
            {
                var garmentFinishedGoodExist= _garmentFinishedGoodStockRepository.Query.Where(
                    a=> a.RONo== request.RONo &&
                        a.Article==request.Article &&
                        a.BasicPrice== finGoodStock.Key.BasicPrice &&
                        a.UnitId==request.Unit.Id &&
                        new SizeId(a.SizeId)== finGoodStock.Key.SizeId &&
                        a.ComodityId== request.Comodity.Id &&
                        new UomId(a.UomId)== finGoodStock.Key.UomId
                    ).Select(s => new GarmentFinishedGoodStock(s)).SingleOrDefault();

                double qty = garmentFinishedGoodExist == null ? finGoodStock.Value : (finGoodStock.Value + garmentFinishedGoodExist.Quantity);

                double price = (finGoodStock.Key.BasicPrice + (double)garmentComodityPrice.Price) * qty; 
                
                if (garmentFinishedGoodExist == null)
                {
                    var now = DateTime.Now;
                    var year = now.ToString("yy");
                    var month = now.ToString("MM");
                    string finGoodNo = GenerateFinGoodNo(finGoodStock.Key);
                    var prefixNo = $"ST{request.Unit.Code.Trim()}{year}{month}";
                    int no = int.Parse(finGoodNo.Replace(prefixNo, ""));
                    finGoodNo = $"{prefixNo}{(no + count).ToString("D4")}";
                    GarmentFinishedGoodStock finishedGood = new GarmentFinishedGoodStock(
                                    Guid.NewGuid(),
                                    finGoodNo,
                                    request.RONo,
                                    request.Article,
                                    new UnitDepartmentId(request.Unit.Id),
                                    request.Unit.Code,
                                    request.Unit.Name,
                                    new GarmentComodityId(request.Comodity.Id),
                                    request.Comodity.Code,
                                    request.Comodity.Name,
                                    finGoodStock.Key.SizeId,
                                    finGoodStock.Key.SizeName,
                                    finGoodStock.Key.UomId,
                                    finGoodStock.Key.UomUnit,
                                    qty,
                                    finGoodStock.Key.BasicPrice,
                                    price
                                    );
                    count++;
                    await _garmentFinishedGoodStockRepository.Update(finishedGood);

                    var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                             a.Article == request.Article &&
                             a.BasicPrice == finishedGood.BasicPrice &&
                             a.UnitId == new UnitDepartmentId(request.Unit.Id) &&
                             a.SizeId == finishedGood.SizeId &&
                             a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                             a.UomId == finishedGood.UomId).SingleOrDefault();
                    if (stock == null)
                    {
                        finGoodStocks.Add(finishedGood);
                    }
                    else
                    {
                        finGoodStocks.Remove(stock);
                        finGoodStocks.Add(finishedGood);
                    }
                    //finGoodStocks.Add(finishedGood);
                }
                else
                {
                    garmentFinishedGoodExist.SetQuantity(qty);
                    garmentFinishedGoodExist.SetPrice(price);
                    garmentFinishedGoodExist.Modify();

                    await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodExist);
                    var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                             a.Article == request.Article &&
                             a.BasicPrice == garmentFinishedGoodExist.BasicPrice &&
                             a.UnitId == new UnitDepartmentId(request.Unit.Id) &&
                             a.SizeId == garmentFinishedGoodExist.SizeId &&
                             a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                             a.UomId == garmentFinishedGoodExist.UomId).SingleOrDefault();
                    if (stock == null)
                    {
                        finGoodStocks.Add(garmentFinishedGoodExist);
                    }
                    else
                    {
                        finGoodStocks.Remove(stock);
                        finGoodStocks.Add(garmentFinishedGoodExist);
                    }
                }
                
            }

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    if (request.IsDifferentSize)
                    {
                        foreach(var detail in item.Details)
                        {
                            var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                             a.Article == request.Article &&
                             a.BasicPrice == item.BasicPrice &&
                             a.UnitId == new UnitDepartmentId(request.Unit.Id) &&
                             a.SizeId == new SizeId(detail.Size.Id) &&
                             a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                             a.UomId == new UomId(detail.Uom.Id)).Single();

                            double price= (stock.BasicPrice + (double)garmentComodityPrice.Price) * detail.Quantity;

                            GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
                                    Guid.NewGuid(),
                                    stock.Identity,
                                    item.Id,
                                    detail.Id,
                                    Guid.Empty,
                                    Guid.Empty,
                                    "IN",
                                    stock.RONo,
                                    stock.Article,
                                    stock.UnitId,
                                    stock.UnitCode,
                                    stock.UnitName,
                                    stock.ComodityId,
                                    stock.ComodityCode,
                                    stock.ComodityName,
                                    stock.SizeId,
                                    stock.SizeName,
                                    stock.UomId,
                                    stock.UomUnit,
                                    detail.Quantity,
                                    stock.BasicPrice,
                                    price
                                );
                            await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                        }
                    }
                    else
                    {
                        var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                         a.Article == request.Article &&
                         a.BasicPrice == item.BasicPrice &&
                         a.UnitId == new UnitDepartmentId(request.Unit.Id) &&
                         a.SizeId == new SizeId(item.Size.Id) &&
                         a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                         a.UomId == new UomId(item.Uom.Id)).Single();

                        double price = (stock.BasicPrice + (double)garmentComodityPrice.Price) * item.Quantity;

                        GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
                                Guid.NewGuid(),
                                stock.Identity,
                                item.Id,
                                item.Id,
                                Guid.Empty,
                                Guid.Empty,
                                "IN",
                                stock.RONo,
                                stock.Article,
                                stock.UnitId,
                                stock.UnitCode,
                                stock.UnitName,
                                stock.ComodityId,
                                stock.ComodityCode,
                                stock.ComodityName,
                                stock.SizeId,
                                stock.SizeName,
                                stock.UomId,
                                stock.UomUnit,
                                item.Quantity,
                                stock.BasicPrice,
                                price
                            );
                        await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                    }
                }
                
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

        private string GenerateFinGoodNo(GarmentFinishedGoodStock request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"ST{request.UnitCode.Trim()}{year}{month}";

            var lastFnGoodNo = _garmentFinishedGoodStockRepository.Query.Where(w => w.FinishedGoodStockNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishedGoodStockNo)
                .Select(s => int.Parse(s.FinishedGoodStockNo.Replace(prefix, "")))
                .FirstOrDefault();
            var FinGoodNo = $"{prefix}{(lastFnGoodNo + 1).ToString("D4")}";

            return FinGoodNo;
        }

    }

    //class FinishedGood
    //{
    //    public GarmentComodity comodity;
    //    public SizeValueObject size;
    //    public UnitDepartment unit;
    //    public double basicPrice;
    //}
}