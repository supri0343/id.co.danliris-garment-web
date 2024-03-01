using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentExpenditureGoodReturns.CommandHandlers
{
    public class RemoveGarmentSubconExpenditureGoodReturnCommandHandler : ICommandHandler<RemoveSubconGarmentExpenditureGoodReturnCommand, GarmentSubconExpenditureGoodReturn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconExpenditureGoodReturnRepository _garmentExpenditureGoodReturnRepository;
        private readonly IGarmentSubconExpenditureGoodReturnItemRepository _garmentExpenditureGoodReturnItemRepository;
        private readonly IGarmentSubconFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentSubconPackingOutItemRepository _garmentExpenditureGoodItemRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;

        public RemoveGarmentSubconExpenditureGoodReturnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentSubconExpenditureGoodReturnRepository>();
            _garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentSubconExpenditureGoodReturnItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentSubconFinishedGoodStockRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentSubconPackingOutItemRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentSubconExpenditureGoodReturn> Handle(RemoveSubconGarmentExpenditureGoodReturnCommand request, CancellationToken cancellationToken)
        {
            var ExpenditureGoodReturn = _garmentExpenditureGoodReturnRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconExpenditureGoodReturn(o)).Single();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == ExpenditureGoodReturn.UnitId && new GarmentComodityId(a.ComodityId) == ExpenditureGoodReturn.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            Dictionary<Guid, double> finStockToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<Guid, double> exGoodToBeUpdated = new Dictionary<Guid, double>();

            _garmentExpenditureGoodReturnItemRepository.Find(o => o.ReturId == ExpenditureGoodReturn.Identity).ForEach(async expenditureReturnItem =>
            {
                if (finStockToBeUpdated.ContainsKey(expenditureReturnItem.FinishedGoodStockId))
                {
                    finStockToBeUpdated[expenditureReturnItem.FinishedGoodStockId] += expenditureReturnItem.Quantity;
                }
                else
                {
                    finStockToBeUpdated.Add(expenditureReturnItem.FinishedGoodStockId, expenditureReturnItem.Quantity);
                }

                if (exGoodToBeUpdated.ContainsKey(expenditureReturnItem.ExpenditureGoodItemId))
                {
                    exGoodToBeUpdated[expenditureReturnItem.ExpenditureGoodItemId] += expenditureReturnItem.Quantity;
                }
                else
                {
                    exGoodToBeUpdated.Add(expenditureReturnItem.ExpenditureGoodItemId, expenditureReturnItem.Quantity);
                }

                expenditureReturnItem.Remove();
                await _garmentExpenditureGoodReturnItemRepository.Update(expenditureReturnItem);
            });

            foreach (var finStock in finStockToBeUpdated)
            {
                var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == finStock.Key).Select(s => new GarmentSubconFinishedGoodStock(s)).Single();
                var qty = garmentFinishingGoodStockItem.Quantity - finStock.Value;
                garmentFinishingGoodStockItem.SetQuantity(qty);
                garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                garmentFinishingGoodStockItem.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);
            }

            foreach (var exGood in exGoodToBeUpdated)
            {
                var garmentExpenditureGoodItem = _garmentExpenditureGoodItemRepository.Query.Where(x => x.Identity == exGood.Key).Select(s => new GarmentSubconPackingOutItem(s)).Single();
                var qty = garmentExpenditureGoodItem.ReturQuantity - exGood.Value;
                garmentExpenditureGoodItem.SetReturQuantity(qty);
                garmentExpenditureGoodItem.Modify();

                await _garmentExpenditureGoodItemRepository.Update(garmentExpenditureGoodItem);
            }

            ExpenditureGoodReturn.Remove();
            await _garmentExpenditureGoodReturnRepository.Update(ExpenditureGoodReturn);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI RETUR BARANG JADI - TERIMA SUBCON", "Delete Retur Barang Jadi - Terima Subcon - " + ExpenditureGoodReturn.ReturNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return ExpenditureGoodReturn;
        }
    }
}