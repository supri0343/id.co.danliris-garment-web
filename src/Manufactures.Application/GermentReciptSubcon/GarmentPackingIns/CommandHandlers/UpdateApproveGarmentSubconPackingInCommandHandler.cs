using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPackingIns.CommandHandlers
{
    public class UpdateApproveGarmentSubconPackingInCommandHandler : ICommandHandler<UpdateApproveGarmentSubconPackingInCommand, int>
    {
        private readonly IGarmentSubconPackingInRepository _garmentPackingInRepository;
        private readonly IStorage _storage;

        public UpdateApproveGarmentSubconPackingInCommandHandler(IStorage storage)
        {
            _garmentPackingInRepository = storage.GetRepository<IGarmentSubconPackingInRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateApproveGarmentSubconPackingInCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Packings = _garmentPackingInRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSubconPackingIn(a)).ToList();

            foreach (var model in Packings)
            {
                model.setApproved(request.Approved);
                model.Modify();
                await _garmentPackingInRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
