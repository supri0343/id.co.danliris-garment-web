using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentPreparings.CommandHandlers
{
    public class RemoveGarmentPreparingCommandHandler : ICommandHandler<RemoveGarmentPreparingCommand, GarmentPreparing>
    {
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        private readonly ILogHistoryRepository _logHistoryRepository;

        public RemoveGarmentPreparingCommandHandler(IStorage storage)
        {
            _garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            _storage = storage;
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentPreparing> Handle(RemoveGarmentPreparingCommand request, CancellationToken cancellationToken)
        {
            var garmentPreparing = _garmentPreparingRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentPreparing == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            var garmentPreparingItems = _garmentPreparingItemRepository.Find(x => x.GarmentPreparingId == request.Id);

            foreach (var item in garmentPreparingItems)
            {
                item.Remove();
                await _garmentPreparingItemRepository.Update(item);
            }

            garmentPreparing.Remove();

            await _garmentPreparingRepository.Update(garmentPreparing);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Delete Preparing - " + garmentPreparing.UENNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return garmentPreparing;
        }

    }
}