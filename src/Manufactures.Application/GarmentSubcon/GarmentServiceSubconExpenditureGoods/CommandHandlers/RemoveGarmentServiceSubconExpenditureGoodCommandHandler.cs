using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Commands;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconExpenditureGoods.CommandHandlers
{
    public class RemoveGarmentServiceSubconExpenditureGoodCommandHandler : ICommandHandler<RemoveGarmentServiceSubconExpenditureGoodCommand, GarmentServiceSubconExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconExpenditureGoodRepository _garmentServiceSubconExpenditureGoodRepository;
        private readonly IGarmentServiceSubconExpenditureGoodtemRepository _garmentServiceSubconExpenditureGoodItemRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public RemoveGarmentServiceSubconExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodRepository>();
            _garmentServiceSubconExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodtemRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentServiceSubconExpenditureGood> Handle(RemoveGarmentServiceSubconExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconExpenditureGood(o)).Single();

            _garmentServiceSubconExpenditureGoodItemRepository.Find(s => s.ServiceSubconExpenditureGoodId == request.Identity).ForEach(async subconExpenditureItem =>
            {
                subconExpenditureItem.Remove();
                await _garmentServiceSubconExpenditureGoodItemRepository.Update(subconExpenditureItem);
            });
         
            subconExpenditureGood.Remove();
            await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Delete Packing List Subcon - Jasa Barang Jadi - " + subconExpenditureGood.ServiceSubconExpenditureGoodNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return subconExpenditureGood;
        }
    }
}
