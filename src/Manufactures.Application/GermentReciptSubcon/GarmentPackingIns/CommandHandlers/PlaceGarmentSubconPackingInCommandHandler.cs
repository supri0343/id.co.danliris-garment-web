using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPackingIns.CommandHandlers
{
    public class PlaceGarmentSubconPackingInCommandHandler : ICommandHandler<PlaceGarmentSubconPackingInCommand, GarmentSubconPackingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconPackingInRepository _garmentPackingInRepository;
        private readonly IGarmentSubconPackingInItemRepository _garmentPackingInItemRepository;

        //FOR CUTTING
        private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttinOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentSubconCuttingInDetailRepository _garmentCuttingInDetailRepository;

        //FROM SEWING
        private readonly IGarmentSubconSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSubconSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSubconSewingInItemRepository _garmentSewingInItemRepository;

        //FROM FINISHING
        private readonly IGarmentSubconFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentSubconFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentSubconFinishingInItemRepository _garmentFinishingInItemRepository;

        private readonly IGarmentSubconFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        public PlaceGarmentSubconPackingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentPackingInRepository = storage.GetRepository<IGarmentSubconPackingInRepository>();
            _garmentPackingInItemRepository = storage.GetRepository<IGarmentSubconPackingInItemRepository>();

            //FOR CUTTING
            _garmentCuttinOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSubconCuttingInDetailRepository>();

            //FROM SEWING
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSubconSewingOutItemRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSubconSewingInItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSubconSewingOutDetailRepository>();

            //FROM FINISHING
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentSubconFinishingOutDetailRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSubconFinishingOutItemRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentSubconFinishingInItemRepository>();

            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentSubconFinishedGoodStockRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSubconPackingIn> Handle(PlaceGarmentSubconPackingInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

            GarmentSubconPackingIn garmentPackingIn = new GarmentSubconPackingIn(
                   Guid.NewGuid(),
                   GeneratePackingInNo(request),
                   new UnitDepartmentId(request.Unit.Id),
                   request.Unit.Code,
                   request.Unit.Name,
                   new UnitDepartmentId(request.UnitFrom.Id),
                   request.UnitFrom.Code,
                   request.UnitFrom.Name,
                   request.PackingFrom,
                   request.RONo,
                   request.Article,
                   new GarmentComodityId(request.Comodity.Id),
                   request.Comodity.Code,
                   request.Comodity.Name,
                   request.PackingInDate,
                   false
                );

            Dictionary<Guid, double> OriginDataToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<string, double> finGood = new Dictionary<string, double>();
            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentSubconPackingInItem garmentPackingInItem = new GarmentSubconPackingInItem(
                        Guid.NewGuid(),
                        garmentPackingIn.Identity,
                        item.CuttingOutItemId,
                        item.CuttingOutDetailId,
                        item.SewingOutItemId,
                        item.SewingOutDetailId,
                        item.FinishingOutItemId,
                        item.FinishingOutDetailId,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        item.Quantity,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        item.Quantity,
                        item.BasicPrice,
                        item.Price
                       );

                    //Add To finishedGoodStock
                    string finStock = item.Size.Id + "~" + item.Size.Size + "~" + item.Uom.Id + "~" + item.Uom.Unit + "~" + item.BasicPrice;

                    if (finGood.ContainsKey(finStock))
                    {
                        finGood[finStock] += item.Quantity;
                    }
                    else
                    {
                        finGood.Add(finStock, item.Quantity);
                    }

                    switch (request.PackingFrom)
                    {
                        case "CUTTING":
                            //Update Real Qty Cutting Out
                            var garmentCuttingOutDetail = _garmentCuttingOutDetailRepository.Query.Where(x => x.Identity == item.CuttingOutDetailId).Select(s => new GarmentSubconCuttingOutDetail(s)).Single();

                            double diffQty = garmentCuttingOutDetail.CuttingOutQuantity - item.Quantity;
                            garmentCuttingOutDetail.SetRealOutQuantity(item.Quantity);
                            garmentCuttingOutDetail.Modify();

                            await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);

                            //Update RemainingQty Cutting In If CuttingOut Qty is different with Real Out Qty
                            if (diffQty > 0)
                            {
                                var garmentCuttingOutItem = _garmentCuttinOutItemRepository.Query.Where(x => x.Identity == garmentCuttingOutDetail.CutOutItemId).Select(s => new GarmentSubconCuttingOutItem(s)).Single();

                                var garmenCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == garmentCuttingOutItem.CuttingInDetailId).Select(s => new GarmentSubconCuttingInDetail(s)).Single();

                                garmenCuttingInDetail.SetRemainingQuantity(garmenCuttingInDetail.RemainingQuantity + diffQty);

                                garmenCuttingInDetail.Modify();

                                await _garmentCuttingInDetailRepository.Update(garmenCuttingInDetail);
                            }
                            break;
                        case "SEWING":
                            //if Sewing out have detail
                            if (item.SewingOutDetailId != Guid.Empty)
                            {
                                var garmentSewingOutDetail = _garmentSewingOutDetailRepository.Query.Where(x => x.Identity == item.SewingOutDetailId).Select(s => new GarmentSubconSewingOutDetail(s)).Single();
                                var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == garmentSewingOutDetail.SewingOutItemId).Select(s => new GarmentSubconSewingOutItem(s)).Single();

                                var diffQty1 = garmentSewingOutDetail.Quantity - item.Quantity;
                                garmentSewingOutDetail.SetRealQtyOut(item.Quantity);
                                garmentSewingOutDetail.Modify();

                                await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);

                                garmentSewingOutItem.SetRealQtyOut(garmentSewingOutItem.RealQtyOut + item.Quantity);
                                garmentSewingOutItem.Modify();
                                await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);

                                if (diffQty1 > 0)
                                {
                                    var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == garmentSewingOutItem.SewingInItemId).Select(s => new GarmentSubconSewingInItem(s)).Single();

                                    garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity + diffQty1);

                                    garmentSewingInItem.Modify();

                                    await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                                }
                            }
                            //if Sewing out didnt have detail
                            else
                            {
                                var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == item.SewingOutItemId).Select(s => new GarmentSubconSewingOutItem(s)).Single();

                                var diffQty2 = garmentSewingOutItem.Quantity - item.Quantity;
                                garmentSewingOutItem.SetRealQtyOut(item.Quantity);
                                garmentSewingOutItem.Modify();

                                await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);

                                if (diffQty2 > 0)
                                {
                                    var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == garmentSewingOutItem.SewingInItemId).Select(s => new GarmentSubconSewingInItem(s)).Single();

                                    garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity + diffQty2);

                                    garmentSewingInItem.Modify();

                                    await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                                }
                            }
                            break;
                        case "FINISHING":
                            //if FINISHING out have detail
                            if (item.FinishingOutDetailId != Guid.Empty)
                            {
                                var garmentFinishingOutDetail = _garmentFinishingOutDetailRepository.Query.Where(x => x.Identity == item.FinishingOutDetailId).Select(s => new GarmentSubconFinishingOutDetail(s)).Single();
                                var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(x => x.Identity == garmentFinishingOutDetail.FinishingOutItemId).Select(s => new GarmentSubconFinishingOutItem(s)).Single();

                                var diffQty1 = garmentFinishingOutDetail.Quantity - item.Quantity;
                                garmentFinishingOutDetail.SetRealQtyOut(item.Quantity);
                                garmentFinishingOutDetail.Modify();
                                await _garmentFinishingOutDetailRepository.Update(garmentFinishingOutDetail);

                                garmentFinishingOutItem.SetRealQtyOut(garmentFinishingOutItem.RealQtyOut + item.Quantity);
                                garmentFinishingOutItem.Modify();
                                await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);

                                if (diffQty1 > 0)
                                {
                                    var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == garmentFinishingOutItem.FinishingInItemId).Select(s => new GarmentSubconFinishingInItem(s)).Single();

                                    garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity + diffQty1);

                                    garmentFinishingInItem.Modify();

                                    await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                                }
                            }
                            //if FINISHING out didnt have detail
                            else
                            {
                                var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(x => x.Identity == item.FinishingOutItemId).Select(s => new GarmentSubconFinishingOutItem(s)).Single();

                                var diffQty2 = garmentFinishingOutItem.Quantity - item.Quantity;
                                garmentFinishingOutItem.SetRealQtyOut(item.Quantity);
                                garmentFinishingOutItem.Modify();

                                await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);

                                if (diffQty2 > 0)
                                {
                                    var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == garmentFinishingOutItem.FinishingInItemId).Select(s => new GarmentSubconFinishingInItem(s)).Single();

                                    garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity + diffQty2);

                                    garmentFinishingInItem.Modify();

                                    await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                                }

                            }
                            break;
                    }

                    await _garmentPackingInItemRepository.Update(garmentPackingInItem);
                }
                else
                {
                    switch (request.PackingFrom)
                    {
                        case "CUTTING":
                            //Update Real Qty Cutting Out
                            var garmentCuttingOutDetail = _garmentCuttingOutDetailRepository.Query.Where(x => x.Identity == item.CuttingOutDetailId).Select(s => new GarmentSubconCuttingOutDetail(s)).Single();

                            double diffQty = garmentCuttingOutDetail.CuttingOutQuantity;

                            //Update RemainingQty Cutting In If CuttingOut Qty is different with Real Out Qty
                            if (diffQty > 0)
                            {
                                var garmentCuttingOutItem = _garmentCuttinOutItemRepository.Query.Where(x => x.Identity == garmentCuttingOutDetail.CutOutItemId).Select(s => new GarmentSubconCuttingOutItem(s)).Single();

                                var garmenCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == garmentCuttingOutItem.CuttingInDetailId).Select(s => new GarmentSubconCuttingInDetail(s)).Single();

                                garmenCuttingInDetail.SetRemainingQuantity(garmenCuttingInDetail.RemainingQuantity + diffQty);

                                garmenCuttingInDetail.Modify();

                                await _garmentCuttingInDetailRepository.Update(garmenCuttingInDetail);
                            }

                            garmentCuttingOutDetail.Remove();

                            await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);
                            break;
                        case "SEWING":
                            //if Sewing out have detail
                            if (item.SewingOutDetailId != Guid.Empty)
                            {
                                var garmentSewingOutDetail = _garmentSewingOutDetailRepository.Query.Where(x => x.Identity == item.SewingOutDetailId).Select(s => new GarmentSubconSewingOutDetail(s)).Single();

                                var diffQty1 = garmentSewingOutDetail.Quantity;

                                if (diffQty1 > 0)
                                {
                                    var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == garmentSewingOutDetail.SewingOutItemId).Select(s => new GarmentSubconSewingOutItem(s)).Single();

                                    var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == garmentSewingOutItem.SewingInItemId).Select(s => new GarmentSubconSewingInItem(s)).Single();

                                    if (garmentSewingOutItem.Quantity - diffQty1 == 0)
                                    {
                                        garmentSewingOutItem.Remove();
                                    }
                                    else
                                    {
                                        garmentSewingOutItem.SetQuantity(garmentSewingOutItem.Quantity - diffQty1);
                                        garmentSewingOutItem.Modify();
                                    }

                                    garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity + diffQty1);

                                    garmentSewingInItem.Modify();


                                    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                                    await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                                }
                                garmentSewingOutDetail.Remove();

                                await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);
                            }
                            //if Sewing out didnt have detail
                            else
                            {
                                var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(o => o.Identity == item.SewingOutItemId).Select(s => new GarmentSubconSewingOutItem(s)).Single();

                                var diffQty2 = garmentSewingOutItem.Quantity;

                                if (diffQty2 > 0)
                                {
                                    var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == garmentSewingOutItem.SewingInItemId).Select(s => new GarmentSubconSewingInItem(s)).Single();

                                    garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity + diffQty2);

                                    garmentSewingInItem.Modify();

                                    await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                                }

                                garmentSewingOutItem.Remove();

                                await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                            }
                            break;
                        case "FINISHING":
                            //if FINISHING out have detail
                            if (item.FinishingOutDetailId != Guid.Empty)
                            {
                                var garmentFinishingOutDetail = _garmentFinishingOutDetailRepository.Query.Where(x => x.Identity == item.FinishingOutDetailId).Select(s => new GarmentSubconFinishingOutDetail(s)).Single();

                                var diffQty1 = garmentFinishingOutDetail.Quantity;

                                if (diffQty1 > 0)
                                {
                                    var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(x => x.Identity == garmentFinishingOutDetail.FinishingOutItemId).Select(s => new GarmentSubconFinishingOutItem(s)).Single();

                                    var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == garmentFinishingOutItem.FinishingInItemId).Select(s => new GarmentSubconFinishingInItem(s)).Single();

                                    if (garmentFinishingOutItem.Quantity - diffQty1 == 0)
                                    {
                                        garmentFinishingOutItem.Remove();
                                    }
                                    else
                                    {
                                        garmentFinishingOutItem.SetQuantity(garmentFinishingOutItem.Quantity - diffQty1);
                                        garmentFinishingOutItem.Modify();
                                    }
                                    garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity + diffQty1);

                                    garmentFinishingInItem.Modify();

                                    await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                                    await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                                }
                                garmentFinishingOutDetail.Remove();

                                await _garmentFinishingOutDetailRepository.Update(garmentFinishingOutDetail);
                            }
                            //if FINISHING out didnt have detail
                            else
                            {
                                var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(o => o.Identity == item.FinishingOutItemId).Select(s => new GarmentSubconFinishingOutItem(s)).Single();

                                var diffQty2 = garmentFinishingOutItem.Quantity;

                                if (diffQty2 > 0)
                                {
                                    var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == garmentFinishingOutItem.FinishingInItemId).Select(s => new GarmentSubconFinishingInItem(s)).Single();

                                    garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity + diffQty2);

                                    garmentFinishingInItem.Modify();

                                    await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                                }

                                garmentFinishingOutItem.Remove();

                                await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                            }
                            break;
                    }
                }
            }

            List<GarmentSubconFinishedGoodStock> finGoodStocks = new List<GarmentSubconFinishedGoodStock>();
            int count = 1;
            foreach (var finGoodStock in finGood)
            {
                int sizeId = Convert.ToInt32(finGoodStock.Key.Split("~")[0]);
                string sizeName = finGoodStock.Key.Split("~")[1];
                int uomId = Convert.ToInt32(finGoodStock.Key.Split("~")[2]);
                string uomUnit = finGoodStock.Key.Split("~")[3];
                double basicPrice = Convert.ToDouble(finGoodStock.Key.Split("~")[4]);
                var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                    a => a.RONo == request.RONo &&
                        a.Article == request.Article &&
                        a.BasicPrice == basicPrice &&
                        a.UnitId == request.Unit.Id &&
                        a.SizeId == sizeId &&
                        a.ComodityId == request.Comodity.Id &&
                        a.UomId == uomId
                    ).Select(s => new GarmentSubconFinishedGoodStock(s)).SingleOrDefault();

                double qty = garmentFinishedGoodExist == null ? finGoodStock.Value : (finGoodStock.Value + garmentFinishedGoodExist.Quantity);

                double price = (basicPrice + (double)garmentComodityPrice.Price) * qty;

                if (garmentFinishedGoodExist == null)
                {
                    var now = DateTime.Now;
                    var year = now.ToString("yy");
                    var month = now.ToString("MM");
                    var prefix = $"ST{request.Unit.Code.Trim()}{year}{month}";

                    var lastFnGoodNo = _garmentFinishedGoodStockRepository.Query.Where(w => w.FinishedGoodStockNo.StartsWith(prefix))
                    .OrderByDescending(o => o.FinishedGoodStockNo)
                    .Select(s => int.Parse(s.FinishedGoodStockNo.Replace(prefix, "")))
                    .FirstOrDefault();
                    var FinGoodNo = $"{prefix}{(lastFnGoodNo + count).ToString("D4")}";
                    GarmentSubconFinishedGoodStock finishedGood = new GarmentSubconFinishedGoodStock(
                                    Guid.NewGuid(),
                                    FinGoodNo,
                                    request.RONo,
                                    request.Article,
                                    new UnitDepartmentId(request.Unit.Id),
                                    request.Unit.Code,
                                    request.Unit.Name,
                                    new GarmentComodityId(request.Comodity.Id),
                                    request.Comodity.Code,
                                    request.Comodity.Name,
                                    new SizeId(sizeId),
                                    sizeName,
                                    new UomId(uomId),
                                    uomUnit,
                                    qty,
                                    basicPrice,
                                    price
                                    );
                    count++;
                    await _garmentFinishedGoodStockRepository.Update(finishedGood);
                    //finGoodStocks.Add(finishedGood);

                }
                else
                {
                    garmentFinishedGoodExist.SetQuantity(qty);
                    garmentFinishedGoodExist.SetPrice(price);
                    garmentFinishedGoodExist.Modify();

                    await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodExist);
                    //var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                    //         a.Article == request.Article &&
                    //         a.BasicPrice == garmentFinishedGoodExist.BasicPrice &&
                    //         a.UnitId == new UnitDepartmentId(request.Unit.Id) &&
                    //         a.SizeId == garmentFinishedGoodExist.SizeId &&
                    //         a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                    //         a.UomId == garmentFinishedGoodExist.UomId).SingleOrDefault();
                    //finGoodStocks.Add(garmentFinishedGoodExist);
                }
            }

            await _garmentPackingInRepository.Update(garmentPackingIn);

            _storage.Save();

            return garmentPackingIn;


        }

        private string GeneratePackingInNo(PlaceGarmentSubconPackingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"PI{unitcode}{year}{month}";

            var lastPackingInNo = _garmentPackingInRepository.Query.Where(w => w.PackingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.PackingInNo)
                .Select(s => int.Parse(s.PackingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastPackingInNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}
