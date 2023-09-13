using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.Commands;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPackingIns.CommandHandlers
{
    public class UpdateIsPackingListGarmentSubconPackinOutCommandHandler : ICommandHandler<UpdateIsPackingListGarmentSubconPackingOutCommand, int>
    {
        private readonly IGarmentSubconPackingOutItemRepository _garmentPackingOutItemRepository;
        private readonly IStorage _storage;

        public UpdateIsPackingListGarmentSubconPackinOutCommandHandler(IStorage storage)
        {
            _garmentPackingOutItemRepository = storage.GetRepository<IGarmentSubconPackingOutItemRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateIsPackingListGarmentSubconPackingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Packings = _garmentPackingOutItemRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSubconPackingOutItem(a)).ToList();

            foreach (var model in Packings)
            {
                model.SetIsPackingList(request.IsPackingList);
                model.Modify();
                await _garmentPackingOutItemRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
