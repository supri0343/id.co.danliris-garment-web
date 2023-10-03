using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentFinishingIns.CommandHandlers
{
    public class UpdateDatesGarmentFinishingInCommandHandler : ICommandHandler<UpdateDatesGarmentFinishingInCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public UpdateDatesGarmentFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentFinishingInCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var FinIns = _garmentFinishingInRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentFinishingIn(a)).ToList();

            foreach (var model in FinIns)
            {
                model.setDate(request.Date);
                model.setSubconType(request.SubconType);
                model.Modify();
                await _garmentFinishingInRepository.Update(model);

                //Add Log History
                LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Update Date Finishing In - " + model.FinishingInNo, DateTime.Now);
                await _logHistoryRepository.Update(logHistory);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
