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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPackingOuts.CommandHandlers
{
    public class RemoveGarmentSubconPackingOutCommandHandler : ICommandHandler<RemoveGarmentSubconPackingOutCommand, GarmentSubconPackingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconPackingOutRepository _garmentPackingOutRepository;
        private readonly IGarmentSubconPackingOutItemRepository _garmentPackingOutItemRepository;

        private readonly IGarmentSubconPackingInItemRepository _garmentPackingInItemRepository;
        private readonly IGarmentSubconFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        public RemoveGarmentSubconPackingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentPackingOutRepository = storage.GetRepository<IGarmentSubconPackingOutRepository>();
            _garmentPackingOutItemRepository = storage.GetRepository<IGarmentSubconPackingOutItemRepository>();

            _garmentPackingInItemRepository = storage.GetRepository<IGarmentSubconPackingInItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentSubconFinishedGoodStockRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSubconPackingOut> Handle(RemoveGarmentSubconPackingOutCommand request, CancellationToken cancellationToken)
        {
            var packOut = _garmentPackingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconPackingOut(o)).Single();
            
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == packOut.UnitId && new GarmentComodityId(a.ComodityId) == packOut.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();
            Dictionary<Guid, double> finStockToBeUpdated = new Dictionary<Guid, double>();
            _garmentPackingOutItemRepository.Find(o => o.PackingOutId == packOut.Identity).ForEach(async packingOutItem =>
            {

                if (finStockToBeUpdated.ContainsKey(packingOutItem.FinishedGoodStockId))
                {
                    finStockToBeUpdated[packingOutItem.FinishedGoodStockId] += packingOutItem.Quantity;
                }
                else
                {
                    finStockToBeUpdated.Add(packingOutItem.FinishedGoodStockId, packingOutItem.Quantity);
                }
                //var packingInTtem = _garmentPackingInItemRepository.Query.Where(x => x.Identity == packingOutItem.PackingInItemId).Select(o => new GarmentSubconPackingInItem(o)).Single();

                //packingInTtem.SetRemainingQuantity(packingInTtem.RemainingQuantity + packingOutItem.Quantity);
                //packingInTtem.Modify();

                //await _garmentPackingInItemRepository.Update(packingInTtem);

                packingOutItem.Remove();

                await _garmentPackingOutItemRepository.Update(packingOutItem);
            });

            foreach (var finStock in finStockToBeUpdated)
            {
                var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == finStock.Key).Select(s => new GarmentSubconFinishedGoodStock(s)).Single();
                var qty = garmentFinishingGoodStockItem.Quantity + finStock.Value;
                garmentFinishingGoodStockItem.SetQuantity(qty);
                garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                garmentFinishingGoodStockItem.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);
            }

            packOut.Remove();
            await _garmentPackingOutRepository.Update(packOut);

            _storage.Save();

            return packOut;

        }
    }
}
