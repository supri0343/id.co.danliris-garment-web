using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.CommandHandlers
{
    public class RemoveGarmentSampleExpenditureGoodCommandHandler : ICommandHandler<RemoveGarmentSampleExpenditureGoodCommand, GarmentSampleExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleExpenditureGoodRepository _GarmentSampleExpenditureGoodRepository;
        private readonly IGarmentSampleExpenditureGoodItemRepository _GarmentSampleExpenditureGoodItemRepository;
        private readonly IGarmentSampleFinishedGoodStockRepository _GarmentSampleFinishedGoodStockRepository;
        private readonly IGarmentSampleFinishedGoodStockHistoryRepository _GarmentSampleFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public RemoveGarmentSampleExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
            _GarmentSampleExpenditureGoodItemRepository = storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();
            _GarmentSampleFinishedGoodStockRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockRepository>();
            _GarmentSampleFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSampleExpenditureGood> Handle(RemoveGarmentSampleExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var ExpenditureGood = _GarmentSampleExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleExpenditureGood(o)).Single();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == ExpenditureGood.UnitId && new GarmentComodityId(a.ComodityId) == ExpenditureGood.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            Dictionary<Guid, double> finStockToBeUpdated = new Dictionary<Guid, double>();

            _GarmentSampleExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == ExpenditureGood.Identity).ForEach(async expenditureItem =>
            {
                if (finStockToBeUpdated.ContainsKey(expenditureItem.FinishedGoodStockId))
                {
                    finStockToBeUpdated[expenditureItem.FinishedGoodStockId] += expenditureItem.Quantity;
                }
                else
                {
                    finStockToBeUpdated.Add(expenditureItem.FinishedGoodStockId, expenditureItem.Quantity);
                }

                GarmentSampleFinishedGoodStockHistory garmentSampleFinishedGoodStockHistory = _GarmentSampleFinishedGoodStockHistoryRepository.Query.Where(a => a.ExpenditureGoodItemId == expenditureItem.Identity).Select(a => new GarmentSampleFinishedGoodStockHistory(a)).Single();
                garmentSampleFinishedGoodStockHistory.Remove();
                await _GarmentSampleFinishedGoodStockHistoryRepository.Update(garmentSampleFinishedGoodStockHistory);

                expenditureItem.Remove();
                await _GarmentSampleExpenditureGoodItemRepository.Update(expenditureItem);
            });

            foreach (var finStock in finStockToBeUpdated)
            {
                var GarmentSampleFinishingGoodStockItem = _GarmentSampleFinishedGoodStockRepository.Query.Where(x => x.Identity == finStock.Key).Select(s => new GarmentSampleFinishedGoodStock(s)).Single();
                var qty = GarmentSampleFinishingGoodStockItem.Quantity + finStock.Value;
                GarmentSampleFinishingGoodStockItem.SetQuantity(qty);
                GarmentSampleFinishingGoodStockItem.SetPrice((GarmentSampleFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                GarmentSampleFinishingGoodStockItem.Modify();

                await _GarmentSampleFinishedGoodStockRepository.Update(GarmentSampleFinishingGoodStockItem);
            }

            ExpenditureGood.Remove();
            await _GarmentSampleExpenditureGoodRepository.Update(ExpenditureGood);

            _storage.Save();

            return ExpenditureGood;
        }
    }
}
