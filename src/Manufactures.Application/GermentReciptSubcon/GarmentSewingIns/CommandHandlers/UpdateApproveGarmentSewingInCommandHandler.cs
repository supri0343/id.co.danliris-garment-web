using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentSewingIns.CommandHandlers
{
    public class UpdateApproveGarmentSewingInCommandHandler : ICommandHandler<UpdateApproveGarmentSubconSewingInsCommand, int>
    {
        private readonly IGarmentSubconSewingInRepository _garmentSewingRepository;
        private readonly IStorage _storage;

        public UpdateApproveGarmentSewingInCommandHandler(IStorage storage)
        {
            _garmentSewingRepository = storage.GetRepository<IGarmentSubconSewingInRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateApproveGarmentSubconSewingInsCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Loadings = _garmentSewingRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSubconSewingIn(a)).ToList();

            foreach (var model in Loadings)
            {
                model.setApproved(request.Approved);
                model.Modify();
                await _garmentSewingRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
