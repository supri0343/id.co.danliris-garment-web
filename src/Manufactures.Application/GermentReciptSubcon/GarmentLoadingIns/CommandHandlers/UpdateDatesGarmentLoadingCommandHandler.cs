using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentLoadings.CommandHandlers
{
    public class UpdateDatesGarmentLoadingCommandHandler : ICommandHandler<UpdateDatesGarmentSubconLoadingInCommand, int>
    {
        private readonly IGarmentSubconLoadingInRepository _garmentLoadingRepository;
        private readonly IStorage _storage;

        public UpdateDatesGarmentLoadingCommandHandler(IStorage storage)
        {
            _garmentLoadingRepository = storage.GetRepository<IGarmentSubconLoadingInRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateDatesGarmentSubconLoadingInCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Preparings = _garmentLoadingRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSubconLoadingIn(a)).ToList();

            foreach (var model in Preparings)
            {
                model.setDate(request.Date);
                model.Modify();
                await _garmentLoadingRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
