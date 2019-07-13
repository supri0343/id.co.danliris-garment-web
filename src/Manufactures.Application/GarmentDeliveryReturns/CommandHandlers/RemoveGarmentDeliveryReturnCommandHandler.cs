using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentDeliveryReturns.CommandHandlers
{
    public class RemoveGarmentDeliveryReturnCommandHandler : ICommandHandler<RemoveGarmentDeliveryReturnCommand, GarmentDeliveryReturn>
    {
        private readonly IGarmentDeliveryReturnRepository _garmentDeliveryReturnRepository;
        private readonly IGarmentDeliveryReturnItemRepository _garmentDeliveryReturnItemRepository;
        private readonly IStorage _storage;

        public RemoveGarmentDeliveryReturnCommandHandler(IStorage storage)
        {
            _garmentDeliveryReturnRepository = storage.GetRepository<IGarmentDeliveryReturnRepository>();
            _garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentDeliveryReturn> Handle(RemoveGarmentDeliveryReturnCommand request, CancellationToken cancellationToken)
        {
            var garmentDeliveryReturn = _garmentDeliveryReturnRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentDeliveryReturn == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            var garmentDeliveryReturnItems = _garmentDeliveryReturnItemRepository.Find(x => x.DRId == request.Id);

            foreach (var item in garmentDeliveryReturnItems)
            {
                item.Remove();
                await _garmentDeliveryReturnItemRepository.Update(item);
            }

            garmentDeliveryReturn.Remove();

            await _garmentDeliveryReturnRepository.Update(garmentDeliveryReturn);

            _storage.Save();

            return garmentDeliveryReturn;
        }
    }
}