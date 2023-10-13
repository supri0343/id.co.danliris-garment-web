using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingIns.CommandHandlers
{
    public class RemoveGarmentCuttingInCommandHandler : ICommandHandler<RemoveGarmentCuttingInCommand, GarmentCuttingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;


        public RemoveGarmentCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentCuttingIn> Handle(RemoveGarmentCuttingInCommand request, CancellationToken cancellationToken)
        {
            var cutIn = _garmentCuttingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentCuttingIn(o)).Single();

            Dictionary<Guid, decimal> preparingItemToBeUpdated = new Dictionary<Guid, decimal>();

            _garmentCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).ForEach(async cutInItem =>
            {
                _garmentCuttingInDetailRepository.Find(o => o.CutInItemId == cutInItem.Identity).ForEach(async cutInDetail =>
                {
                    if (preparingItemToBeUpdated.ContainsKey(cutInDetail.PreparingItemId))
                    {
                        preparingItemToBeUpdated[cutInDetail.PreparingItemId] +=(decimal)cutInDetail.PreparingQuantity;
                    }
                    else
                    {
                        preparingItemToBeUpdated.Add(cutInDetail.PreparingItemId, (decimal)cutInDetail.PreparingQuantity);
                    }

                    cutInDetail.Remove();
                    await _garmentCuttingInDetailRepository.Update(cutInDetail);
                });

                cutInItem.Remove();
                await _garmentCuttingInItemRepository.Update(cutInItem);
            });

            foreach (var preparingItem in preparingItemToBeUpdated)
            {
                var garmentPreparingItem = _garmentPreparingItemRepository.Query.Where(x => x.Identity == preparingItem.Key).Select(s => new GarmentPreparingItem(s)).Single();
                garmentPreparingItem.setRemainingQuantity(Convert.ToDouble((decimal)garmentPreparingItem.RemainingQuantity + preparingItem.Value));
                garmentPreparingItem.SetModified();
                await _garmentPreparingItemRepository.Update(garmentPreparingItem);
            }

            cutIn.Remove();
            await _garmentCuttingInRepository.Update(cutIn);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI CUTTING IN", "Delete Cutting In - " + cutIn.CutInNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return cutIn;
        }
    }
}
