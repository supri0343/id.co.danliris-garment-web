using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAdjustments.CommandHandlers
{
    public class RemoveGarmentAdjustmentCommandHandler : ICommandHandler<RemoveGarmentAdjustmentCommand, GarmentAdjustment>
    {
        private readonly IStorage _storage;
        private readonly IGarmentAdjustmentRepository _garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository _garmentAdjustmentItemRepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public RemoveGarmentAdjustmentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
            _garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        public async Task<GarmentAdjustment> Handle(RemoveGarmentAdjustmentCommand request, CancellationToken cancellationToken)
        {
            var adjustment = _garmentAdjustmentRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentAdjustment(o)).Single();

            Dictionary<Guid, double> sewingDOItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentAdjustmentItemRepository.Find(o => o.AdjustmentId == adjustment.Identity).ForEach(async adjustmentItem =>
            {
                if (sewingDOItemToBeUpdated.ContainsKey(adjustmentItem.SewingDOItemId))
                {
                    sewingDOItemToBeUpdated[adjustmentItem.SewingDOItemId] += adjustmentItem.Quantity;
                }
                else
                {
                    sewingDOItemToBeUpdated.Add(adjustmentItem.SewingDOItemId, adjustmentItem.Quantity);
                }

                adjustmentItem.Remove();

                await _garmentAdjustmentItemRepository.Update(adjustmentItem);
            });

            foreach (var sewingDOItem in sewingDOItemToBeUpdated)
            {
                var garmentSewingDOItem = _garmentSewingDOItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingDOItem(s)).Single();
                garmentSewingDOItem.setRemainingQuantity(garmentSewingDOItem.RemainingQuantity + sewingDOItem.Value);
                garmentSewingDOItem.Modify();

                await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
            }

            adjustment.Remove();
            await _garmentAdjustmentRepository.Update(adjustment);

            _storage.Save();

            return adjustment;
        }
    }
}