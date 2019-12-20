using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentFinishingOuts.CommandHandlers
{
    public class RemoveGarmentFinishingOutCommandHandler : ICommandHandler<RemoveGarmentFinishingOutCommand, GarmentFinishingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;

        public RemoveGarmentFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
        }

        public async Task<GarmentFinishingOut> Handle(RemoveGarmentFinishingOutCommand request, CancellationToken cancellationToken)
        {
            var finishOut = _garmentFinishingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentFinishingOut(o)).Single();

            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).ForEach(async finishOutItem =>
            {
                if (finishOut.IsDifferentSize)
                {
                    _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).ForEach(async finishOutDetail =>
                    {
                        if (finishingInItemToBeUpdated.ContainsKey(finishOutItem.FinishingInItemId))
                        {
                            finishingInItemToBeUpdated[finishOutItem.FinishingInItemId] += finishOutDetail.Quantity;
                        }
                        else
                        {
                            finishingInItemToBeUpdated.Add(finishOutItem.FinishingInItemId, finishOutDetail.Quantity);
                        }

                        finishOutDetail.Remove();
                        await _garmentFinishingOutDetailRepository.Update(finishOutDetail);
                    });
                }
                else
                {
                    if (finishingInItemToBeUpdated.ContainsKey(finishOutItem.FinishingInItemId))
                    {
                        finishingInItemToBeUpdated[finishOutItem.FinishingInItemId] += finishOutItem.Quantity;
                    }
                    else
                    {
                        finishingInItemToBeUpdated.Add(finishOutItem.FinishingInItemId, finishOutItem.Quantity);
                    }
                }


                finishOutItem.Remove();
                await _garmentFinishingOutItemRepository.Update(finishOutItem);
            });

            foreach (var finInItem in finishingInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finInItem.Key).Select(s => new GarmentFinishingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + finInItem.Value);
                garmentSewInItem.Modify();
                await _garmentFinishingInItemRepository.Update(garmentSewInItem);
            }

            finishOut.Remove();
            await _garmentFinishingOutRepository.Update(finishOut);

            _storage.Save();

            return finishOut;
        }
    }
}
