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
    public class RemoveGarmentSubconPackingInCommandHandler : ICommandHandler<RemoveGarmentSubconPackingInCommand, GarmentSubconPackingIn>
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

        public RemoveGarmentSubconPackingInCommandHandler(IStorage storage)
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

        public async Task<GarmentSubconPackingIn> Handle(RemoveGarmentSubconPackingInCommand request, CancellationToken cancellationToken)
        {
            var packIn = _garmentPackingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconPackingIn(o)).Single();

            Dictionary<GarmentSubconFinishedGoodStock, double> finGood = new Dictionary<GarmentSubconFinishedGoodStock, double>();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == packIn.UnitId && new GarmentComodityId(a.ComodityId) == packIn.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            _garmentPackingInItemRepository.Find(o => o.PackingInId == packIn.Identity).ForEach(async packingInItem =>
            {
                switch (packIn.PackingFrom)
                {
                    case "CUTTING":
                        //Update Real Qty Cutting Out
                        var garmentCuttingOutDetail = _garmentCuttingOutDetailRepository.Query.Where(x => x.Identity == packingInItem.CuttingOutDetailId).Select(s => new GarmentSubconCuttingOutDetail(s)).Single();

                        double diffQty = garmentCuttingOutDetail.CuttingOutQuantity - packingInItem.Quantity;

                        garmentCuttingOutDetail.SetRealOutQuantity(0);
                        garmentCuttingOutDetail.Modify();

                        await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);

                        //Update RemainingQty Cutting In If CuttingOut Qty is different with Real Out Qty
                        if (diffQty > 0)
                        {
                            var garmentCuttingOutItem = _garmentCuttinOutItemRepository.Query.Where(x => x.Identity == garmentCuttingOutDetail.CutOutItemId).Select(s => new GarmentSubconCuttingOutItem(s)).Single();

                            var garmenCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == garmentCuttingOutItem.CuttingInDetailId).Select(s => new GarmentSubconCuttingInDetail(s)).Single();

                            garmenCuttingInDetail.SetRemainingQuantity(garmenCuttingInDetail.RemainingQuantity - diffQty);

                            garmenCuttingInDetail.Modify();

                            await _garmentCuttingInDetailRepository.Update(garmenCuttingInDetail);
                        }
                        break;
                    case "SEWING":
                        var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == packingInItem.SewingOutItemId).Select(s => new GarmentSubconSewingOutItem(s)).Single();

                        var garmentSewingOutDetails = _garmentSewingOutDetailRepository.Query.Where(x => x.SewingOutItemId == garmentSewingOutItem.Identity).Select(s => new GarmentSubconSewingOutDetail(s)).ToList();

                        if (garmentSewingOutDetails.Count > 0)
                        {
                            foreach (var SewingOutDetail in garmentSewingOutDetails)
                            {
                                SewingOutDetail.SetRealQtyOut(0);
                                SewingOutDetail.Modify();

                                await _garmentSewingOutDetailRepository.Update(SewingOutDetail);
                            }
                        }
                        var diffQty1 = garmentSewingOutItem.Quantity - packingInItem.Quantity;
                        garmentSewingOutItem.SetRealQtyOut(0);
                        garmentSewingOutItem.Modify();

                        await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                        //Update RemainingQty 
                        if (diffQty1 > 0)
                        {
                            var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == garmentSewingOutItem.SewingInItemId).Select(s => new GarmentSubconSewingInItem(s)).Single();

                            garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity - diffQty1);

                            garmentSewingInItem.Modify();

                            await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                        }
                        break;
                    case "FINISHING":
                        var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(x => x.Identity == packingInItem.FinishingOutItemId).Select(s => new GarmentSubconFinishingOutItem(s)).Single();

                        var garmentFinishingOutDetails = _garmentFinishingOutDetailRepository.Query.Where(x => x.FinishingOutItemId == garmentFinishingOutItem.Identity).Select(s => new GarmentSubconFinishingOutDetail(s)).ToList();

                        if (garmentFinishingOutDetails.Count > 0)
                        {
                            foreach (var FinishingOutDetail in garmentFinishingOutDetails)
                            {
                                FinishingOutDetail.SetRealQtyOut(0);
                                FinishingOutDetail.Modify();

                                await _garmentFinishingOutDetailRepository.Update(FinishingOutDetail);
                            }
                        }
                        var diffQty2 = garmentFinishingOutItem.Quantity - packingInItem.Quantity;
                        garmentFinishingOutItem.SetRealQtyOut(0);
                        garmentFinishingOutItem.Modify();

                        await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                        //Update RemainingQty 
                        if (diffQty2 > 0)
                        {
                            var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == garmentFinishingOutItem.FinishingInItemId).Select(s => new GarmentSubconFinishingInItem(s)).Single();

                            garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity - diffQty2);

                            garmentFinishingInItem.Modify();

                            await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                        }
                        break;

                       
                }

                var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                           a => a.RONo == packIn.RONo &&
                               a.Article == packIn.Article &&
                               a.BasicPrice == packingInItem.BasicPrice &&
                               new UnitDepartmentId(a.UnitId) == packIn.UnitId &&
                               new SizeId(a.SizeId) == packingInItem.SizeId &&
                               new GarmentComodityId(a.ComodityId) == packIn.ComodityId &&
                               new UomId(a.UomId) == packingInItem.UomId
                           ).Select(s => new GarmentSubconFinishedGoodStock(s)).Single();

                if (finGood.ContainsKey(garmentFinishedGoodExist))
                {
                    finGood[garmentFinishedGoodExist] += packingInItem.Quantity;
                }
                else
                {
                    finGood.Add(garmentFinishedGoodExist, packingInItem.Quantity);
                }

                packingInItem.Remove();

                await _garmentPackingInItemRepository.Update(packingInItem);
            });

            foreach (var finGoodStock in finGood)
            {
                var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                    a => a.Identity == finGoodStock.Key.Identity
                    ).Select(s => new GarmentSubconFinishedGoodStock(s)).Single();

                var qty = garmentFinishedGoodExist.Quantity - finGoodStock.Value;

                garmentFinishedGoodExist.SetQuantity(qty);
                garmentFinishedGoodExist.SetPrice((garmentFinishedGoodExist.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                garmentFinishedGoodExist.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodExist);
            }

            packIn.Remove();
            await _garmentPackingInRepository.Update(packIn);

            _storage.Save();

            return packIn;

        }
    }
}
