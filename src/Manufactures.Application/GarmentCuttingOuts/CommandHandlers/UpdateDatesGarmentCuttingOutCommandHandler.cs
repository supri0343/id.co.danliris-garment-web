using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingOuts.CommandHandlers
{
    public class UpdateDatesGarmentCuttingOutCommandHandler : ICommandHandler<UpdateDatesGarmentCuttingOutCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;

        public UpdateDatesGarmentCuttingOutCommandHandler(IStorage storage)
        {
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _storage = storage;
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentCuttingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var CutOuts = _garmentCuttingOutRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentCuttingOut(a)).ToList();

            foreach (var model in CutOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _garmentCuttingOutRepository.Update(model);

                //Add Log History
                LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Update Date Cutting Out - " + model.CutOutNo, DateTime.Now);
                await _logHistoryRepository.Update(logHistory);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
