using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentCuttingIns.CommandHandlers
{
    public class RemoveGarmentCuttingInCommandHandler : ICommandHandler<RemoveGarmentSubconCuttingInCommand, GarmentSubconCuttingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentSubconCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentSubconCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSubconPreparingItemRepository _garmentPreparingItemRepository;

        public RemoveGarmentCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingInRepository = storage.GetRepository<IGarmentSubconCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentSubconCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSubconCuttingInDetailRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentSubconPreparingItemRepository>();
        }

        public async Task<GarmentSubconCuttingIn> Handle(RemoveGarmentSubconCuttingInCommand request, CancellationToken cancellationToken)
        {
            var cutIn = _garmentCuttingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCuttingIn(o)).Single();

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
                var garmentPreparingItem = _garmentPreparingItemRepository.Query.Where(x => x.Identity == preparingItem.Key).Select(s => new GarmentSubconPreparingItem(s)).Single();
                garmentPreparingItem.setRemainingQuantity(Convert.ToDouble((decimal)garmentPreparingItem.RemainingQuantity + preparingItem.Value));
                garmentPreparingItem.SetModified();
                await _garmentPreparingItemRepository.Update(garmentPreparingItem);
            }

            cutIn.Remove();
            await _garmentCuttingInRepository.Update(cutIn);

            _storage.Save();

            return cutIn;
        }
    }
}
