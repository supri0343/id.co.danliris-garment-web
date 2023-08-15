using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentFinishingIns.CommandHandlers
{
    public class RemoveGarmentSubconFinishingInCommandHandler : ICommandHandler<RemoveGarmentSubconFinishingInCommand, GarmentSubconFinishingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentSubconFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentSubconSewingOutItemRepository _garmentSewingOutItemRepository;

        public RemoveGarmentSubconFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentSubconFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentSubconFinishingInItemRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSubconSewingOutItemRepository>();
        }

        public async Task<GarmentSubconFinishingIn> Handle(RemoveGarmentSubconFinishingInCommand request, CancellationToken cancellationToken)
        {
            var finIn = _garmentFinishingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconFinishingIn(o)).Single();

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
                var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == sewingOutItem.Key).Select(s => new GarmentSubconSewingOutItem(s)).Single();
                garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity + sewingOutItem.Value);
                garmentSewingOutItem.Modify();

                await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
            }

            finIn.Remove();
            await _garmentFinishingInRepository.Update(finIn);

            _storage.Save();

            return finIn;
        }
    }
}
