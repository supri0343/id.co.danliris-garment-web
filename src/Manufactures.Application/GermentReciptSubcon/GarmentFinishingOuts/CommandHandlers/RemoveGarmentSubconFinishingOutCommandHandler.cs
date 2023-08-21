using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentFinishingOuts.CommandHandlers
{
    public class RemoveGarmentSubconFinishingOutCommandHandler : ICommandHandler<RemoveGarmentSubconFinishingOutCommand, GarmentSubconFinishingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentSubconFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentSubconFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentSubconFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public RemoveGarmentSubconFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentSubconFinishingOutRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSubconFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentSubconFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentSubconFinishingInItemRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSubconFinishingOut> Handle(RemoveGarmentSubconFinishingOutCommand request, CancellationToken cancellationToken)
        {
            var finishOut = _garmentFinishingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconFinishingOut(o)).Single();

            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == finishOut.UnitToId && new GarmentComodityId( a.ComodityId) == finishOut.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

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
                var garmentSewInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finInItem.Key).Select(s => new GarmentSubconFinishingInItem(s)).Single();
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
