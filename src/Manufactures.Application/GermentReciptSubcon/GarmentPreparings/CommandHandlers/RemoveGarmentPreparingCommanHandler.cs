using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPreparings.CommandHandlers
{
    public class RemoveGarmentPreparingCommandHandler : ICommandHandler<RemoveSubconGarmentPreparingCommand, GarmentSubconPreparing>
    {
        private readonly IGarmentSubconPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentSubconPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        public RemoveGarmentPreparingCommandHandler(IStorage storage)
        {
            _garmentPreparingRepository = storage.GetRepository<IGarmentSubconPreparingRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentSubconPreparingItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentSubconPreparing> Handle(RemoveSubconGarmentPreparingCommand request, CancellationToken cancellationToken)
        {
            var garmentPreparing = _garmentPreparingRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentPreparing == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            var garmentPreparingItems = _garmentPreparingItemRepository.Find(x => x.GarmentSubconPreparingId == request.Id);

            foreach (var item in garmentPreparingItems)
            {
                item.Remove();
                await _garmentPreparingItemRepository.Update(item);
            }

            garmentPreparing.Remove();

            await _garmentPreparingRepository.Update(garmentPreparing);

            _storage.Save();

            return garmentPreparing;
        }

    }
}