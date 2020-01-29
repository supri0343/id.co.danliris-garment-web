using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
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
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public RemoveGarmentAdjustmentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
            _garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        public async Task<GarmentAdjustment> Handle(RemoveGarmentAdjustmentCommand request, CancellationToken cancellationToken)
        {
            var adjustment = _garmentAdjustmentRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentAdjustment(o)).Single();

            Dictionary<Guid, double> sewingDOItemToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<Guid, double> sewingInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentAdjustmentItemRepository.Find(o => o.AdjustmentId == adjustment.Identity).ForEach(async adjustmentItem =>
            {
                if (adjustment.AdjustmentType == "LOADING")
                {
                    if (sewingDOItemToBeUpdated.ContainsKey(adjustmentItem.SewingDOItemId))
                    {
                        sewingDOItemToBeUpdated[adjustmentItem.SewingDOItemId] += adjustmentItem.Quantity;
                    }
                    else
                    {
                        sewingDOItemToBeUpdated.Add(adjustmentItem.SewingDOItemId, adjustmentItem.Quantity);
                    }
                }
                else if (adjustment.AdjustmentType == "SEWING")
                {
                    if (sewingInItemToBeUpdated.ContainsKey(adjustmentItem.SewingInItemId))
                    {
                        sewingInItemToBeUpdated[adjustmentItem.SewingInItemId] += adjustmentItem.Quantity;
                    }
                    else
                    {
                        sewingInItemToBeUpdated.Add(adjustmentItem.SewingInItemId, adjustmentItem.Quantity);
                    }
                }

                adjustmentItem.Remove();

                await _garmentAdjustmentItemRepository.Update(adjustmentItem);
            });

            if (adjustment.AdjustmentType == "LOADING")
            {
                foreach (var sewingDOItem in sewingDOItemToBeUpdated)
                {
                    var garmentSewingDOItem = _garmentSewingDOItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingDOItem(s)).Single();
                    garmentSewingDOItem.setRemainingQuantity(garmentSewingDOItem.RemainingQuantity + sewingDOItem.Value);
                    garmentSewingDOItem.Modify();

                    await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
                }
            }
            else if (adjustment.AdjustmentType == "SEWING")
            {
                foreach (var sewingInItem in sewingInItemToBeUpdated)
                {
                    var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
                    garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity + sewingInItem.Value);
                    garmentSewingInItem.Modify();

                    await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                }
            }


            adjustment.Remove();
            await _garmentAdjustmentRepository.Update(adjustment);

            _storage.Save();

            return adjustment;
        }
    }
}