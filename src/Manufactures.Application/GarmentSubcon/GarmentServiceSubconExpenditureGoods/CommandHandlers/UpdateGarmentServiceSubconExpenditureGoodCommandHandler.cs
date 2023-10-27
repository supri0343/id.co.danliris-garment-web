using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconExpenditureGoods.CommandHandlers
{
    public class UpdateGarmentServiceSubconExpenditureGoodCommandHandler : ICommandHandler<UpdateGarmentServiceSubconExpenditureGoodCommand, GarmentServiceSubconExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconExpenditureGoodRepository _garmentServiceSubconExpenditureGoodRepository;
        private readonly IGarmentServiceSubconExpenditureGoodtemRepository _garmentServiceSubconExpenditureGoodItemRepository;

        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public UpdateGarmentServiceSubconExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodRepository>();
            _garmentServiceSubconExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodtemRepository>();

            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentServiceSubconExpenditureGood> Handle(UpdateGarmentServiceSubconExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
           var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconExpenditureGood(o)).Single();

            _garmentServiceSubconExpenditureGoodItemRepository.Find(s => s.ServiceSubconExpenditureGoodId == request.Identity).ForEach(async subconExpenditureItem =>
            {
                foreach (var item in request.Items)
                {
                    if (subconExpenditureItem.Identity == item.Id)
                    {
                        subconExpenditureItem.SetQuantity(item.Quantity);
                        subconExpenditureItem.Modify();
                        await _garmentServiceSubconExpenditureGoodItemRepository.Update(subconExpenditureItem);
                    }
                }
            });
            subconExpenditureGood.SetDate(request.ServiceSubconExpenditureGoodDate.GetValueOrDefault());
            subconExpenditureGood.SetBuyerId(new BuyerId(request.Buyer.Id));
            subconExpenditureGood.SetBuyerCode(request.Buyer.Code);
            subconExpenditureGood.SetBuyerName(request.Buyer.Name);
            subconExpenditureGood.SetQtyPacking(request.QtyPacking);
            subconExpenditureGood.SetUomUnit(request.UomUnit);
            subconExpenditureGood.Modify();
            await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Update Packing List Subcon - Jasa Barang Jadi - " + subconExpenditureGood.ServiceSubconExpenditureGoodNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return subconExpenditureGood;
        }
    }
}
