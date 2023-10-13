using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentPreparings.CommandHandlers
{
    public class UpdateDatesGarmentPreparingCommandHandler : ICommandHandler<UpdateDatesGarmentPreparingCommand, int>
    {
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IStorage _storage;
        private readonly ILogHistoryRepository _logHistoryRepository;

        public UpdateDatesGarmentPreparingCommandHandler(IStorage storage)
        {
            _garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            _storage = storage;
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentPreparingCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Preparings = _garmentPreparingRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentPreparing(a)).ToList();

            foreach (var model in Preparings)
            {
                model.setProcessDate(request.Date);
                model.SetModified();
                await _garmentPreparingRepository.Update(model);

                //Add Log History
                LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI PREPARING", "Update Date Preparing - " + model.UENNo, DateTime.Now);
                await _logHistoryRepository.Update(logHistory);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
