using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.Commands;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPackingOuts.CommandHandlers
{
    public class PlaceGarmentSubconPackingOutCommandHandler : ICommandHandler<PlaceGarmentSubconPackingOutCommand, GarmentSubconPackingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconPackingOutRepository _garmentPackingOutRepository;
        private readonly IGarmentSubconPackingOutItemRepository _garmentPackingOutItemRepository;

        private readonly IGarmentSubconPackingInItemRepository _garmentPackingInItemRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        private readonly IGarmentSubconFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        public PlaceGarmentSubconPackingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentPackingOutRepository = storage.GetRepository<IGarmentSubconPackingOutRepository>();
            _garmentPackingOutItemRepository = storage.GetRepository<IGarmentSubconPackingOutItemRepository>();

            _garmentPackingInItemRepository = storage.GetRepository<IGarmentSubconPackingInItemRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();

            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentSubconFinishedGoodStockRepository>();
        }

        public async Task<GarmentSubconPackingOut> Handle(PlaceGarmentSubconPackingOutCommand request, CancellationToken cancellationToken)
        {
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();
            request.Items = request.Items.ToList();

            GarmentSubconPackingOut garmentPackingOut = new GarmentSubconPackingOut(
                    Guid.NewGuid(),
               GeneratePackingOutNo(request),
                request.PackingOutType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                new BuyerId(request.ProductOwner.Id),
                request.ProductOwner.Code,
                request.ProductOwner.Name,
                request.PackingOutDate,
                request.Invoice,
                request.ContractNo,
                request.Carton,
                request.Description,
                request.IsReceived,
                request.PackingListId
                );

            Dictionary<string, double> finStockToBeUpdated = new Dictionary<string, double>();
            Dictionary<Guid, double> finstockQty = new Dictionary<Guid, double>();
            foreach (var item in request.Items)
            {
                if (item.isSave)
                {

                    double StockQty = 0;
                    var garmentFinishingGoodStock = _garmentFinishedGoodStockRepository.Query.Where(x => x.SizeId == item.Size.Id && x.UomId == item.Uom.Id && x.RONo == request.RONo && x.UnitId == request.Unit.Id && x.Quantity > 0).OrderBy(a => a.CreatedDate).ToList();

                    double qty = item.Quantity;
                    foreach (var finishedGood in garmentFinishingGoodStock)
                    {
                        if (!finstockQty.ContainsKey(finishedGood.Identity))
                        {
                            finstockQty.Add(finishedGood.Identity, finishedGood.Quantity);
                        }
                        string key = finishedGood.Identity.ToString() + "~" + item.Description;
                        if (qty > 0)
                        {
                            double remainQty = finstockQty[finishedGood.Identity] - qty;
                            if (remainQty < 0)
                            {
                                qty -= finstockQty[finishedGood.Identity];
                                finStockToBeUpdated.Add(key, 0);
                                finstockQty[finishedGood.Identity] = 0;
                            }
                            else if (remainQty == 0)
                            {
                                finStockToBeUpdated.Add(key, 0);
                                finstockQty[finishedGood.Identity] = remainQty;
                                break;
                            }
                            else if (remainQty > 0)
                            {
                                finStockToBeUpdated.Add(key, remainQty);
                                finstockQty[finishedGood.Identity] = remainQty;
                                break;
                            }
                        }
                    }


                   
                }
            }

            foreach (var finStock in finStockToBeUpdated)
            {
                var keyString = finStock.Key.Split("~");

                var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == Guid.Parse(keyString[0])).Select(s => new GarmentSubconFinishedGoodStock(s)).Single();

                var item = request.Items.Where(a => new SizeId(a.Size.Id) == garmentFinishingGoodStockItem.SizeId && new UomId(a.Uom.Id) == garmentFinishingGoodStockItem.UomId && a.Description == keyString[1]).Single();

                item.Price = (item.BasicPrice + ((double)garmentComodityPrice.Price * 1)) * item.Quantity;
                var qty = garmentFinishingGoodStockItem.Quantity - finStock.Value;

                GarmentSubconPackingOutItem garmentPackingOutItem = new GarmentSubconPackingOutItem(
                    Guid.NewGuid(),
                    garmentPackingOut.Identity,
                    Guid.Empty,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    qty,
                    0,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Description,
                    item.BasicPrice,
                    (item.BasicPrice + (double)garmentComodityPrice.Price) * item.Quantity,
                    garmentFinishingGoodStockItem.Identity
                   );

                await _garmentPackingOutItemRepository.Update(garmentPackingOutItem);


                garmentFinishingGoodStockItem.SetQuantity(finStock.Value);
                garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (finStock.Value));
                garmentFinishingGoodStockItem.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);
                //var packingInTtem = _garmentPackingInItemRepository.Query.Where(x => x.Identity == item.PackingInItemId).Select(o => new GarmentSubconPackingInItem(o)).Single();

                //packingInTtem.SetRemainingQuantity(packingInTtem.RemainingQuantity - item.Quantity);
                //packingInTtem.Modify();

                //await _garmentPackingInItemRepository.Update(packingInTtem);


            }

                await _garmentPackingOutRepository.Update(garmentPackingOut);

            _storage.Save();

            return garmentPackingOut;

        }

        private string GeneratePackingOutNo(PlaceGarmentSubconPackingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"PO{unitcode}{year}{month}";

            var lastPackingOutNo = _garmentPackingOutRepository.Query.Where(w => w.PackingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.PackingOutNo)
                .Select(s => int.Parse(s.PackingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastPackingOutNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}
