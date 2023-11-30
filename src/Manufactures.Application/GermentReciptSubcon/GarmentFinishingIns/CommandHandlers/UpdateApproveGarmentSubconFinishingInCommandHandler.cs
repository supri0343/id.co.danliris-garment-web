using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentLoadings.CommandHandlers
{
    public class UpdateApproveGarmentSubconFinishingInCommandHandler : ICommandHandler<UpdateApproveGarmentSubconFinishingInCommand, int>
    {
        private readonly IGarmentSubconFinishingInRepository _garmentFinishingInRepository;
        private readonly IStorage _storage;

        public UpdateApproveGarmentSubconFinishingInCommandHandler(IStorage storage)
        {
            _garmentFinishingInRepository = storage.GetRepository<IGarmentSubconFinishingInRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateApproveGarmentSubconFinishingInCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Loadings = _garmentFinishingInRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSubconFinishingIn(a)).ToList();

            foreach (var model in Loadings)
            {
                model.setApproved(request.Approved);
                model.Modify();
                await _garmentFinishingInRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
