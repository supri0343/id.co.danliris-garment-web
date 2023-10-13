using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentFinishingIns.CommandHandlers
{
    public class RemoveGarmentFinishingInCommandHandler : ICommandHandler<RemoveGarmentFinishingInCommand, GarmentFinishingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public RemoveGarmentFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentFinishingIn> Handle(RemoveGarmentFinishingInCommand request, CancellationToken cancellationToken)
        {
            var finIn = _garmentFinishingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentFinishingIn(o)).Single();

            Dictionary<Guid, double> sewingOutItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentFinishingInItemRepository.Find(o => o.FinishingInId == finIn.Identity).ForEach(async finishingInItem =>
            {
                if (sewingOutItemToBeUpdated.ContainsKey(finishingInItem.SewingOutItemId))
                {
                    sewingOutItemToBeUpdated[finishingInItem.SewingOutItemId] += finishingInItem.Quantity;
                }
                else
                {
                    sewingOutItemToBeUpdated.Add(finishingInItem.SewingOutItemId, finishingInItem.Quantity);
                }

                finishingInItem.Remove();

                await _garmentFinishingInItemRepository.Update(finishingInItem);
            });

            foreach (var sewingOutItem in sewingOutItemToBeUpdated)
            {
                var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == sewingOutItem.Key).Select(s => new GarmentSewingOutItem(s)).Single();
                garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity + sewingOutItem.Value);
                garmentSewingOutItem.Modify();

                await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
            }

            finIn.Remove();
            await _garmentFinishingInRepository.Update(finIn);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI FINISHING IN", "Delete Finishing In - " + finIn.FinishingInNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return finIn;
        }
    }
}
