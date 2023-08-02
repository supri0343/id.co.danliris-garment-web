using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentCuttingOuts.CommandHandlers
{
    public class UpdateDatesGarmentSubconCuttingOutCommandHandler : ICommandHandler<UpdateDatesGarmentSubconCuttingOutCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingOutRepository _garmentSubconCuttingOutRepository;

        public UpdateDatesGarmentSubconCuttingOutCommandHandler(IStorage storage)
        {
            _garmentSubconCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateDatesGarmentSubconCuttingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var CutOuts = _garmentSubconCuttingOutRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSubconCuttingOut(a)).ToList();

            foreach (var model in CutOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _garmentSubconCuttingOutRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
