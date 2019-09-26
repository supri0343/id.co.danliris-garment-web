using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingIns.CommandHandlers
{
    public class RemoveGarmentSewingInCommandHandler : ICommandHandler<RemoveGarmentSewingInCommand, GarmentSewingIn>
    {
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;
        private readonly IStorage _storage;

        public RemoveGarmentSewingInCommandHandler(IStorage storage)
        {
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentSewingIn> Handle(RemoveGarmentSewingInCommand request, CancellationToken cancellationToken)
        {
            var garmentSewingIn = _garmentSewingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSewingIn(o)).Single();

            if (garmentSewingIn == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Identity));

            var garmentSewingInItems = _garmentSewingInItemRepository.Find(x => x.SewingInId == request.Identity);

            foreach (var item in garmentSewingInItems)
            {
                item.Remove();

                var garmentLoadingItem = _garmentLoadingItemRepository.Find(o => o.Identity == item.LoadingItemId).Single();

                garmentLoadingItem.SetRemainingQuantity(garmentLoadingItem.RemainingQuantity + item.Quantity);

                garmentLoadingItem.Modify();
                await _garmentLoadingItemRepository.Update(garmentLoadingItem);

                await _garmentSewingInItemRepository.Update(item);
            }

            garmentSewingIn.Remove();

            await _garmentSewingInRepository.Update(garmentSewingIn);

            _storage.Save();

            return garmentSewingIn;
        }
    }
}