using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.Commands;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPackingOuts.CommandHandlers
{
    public class RemoveGarmentSubconPackingOutCommandHandler : ICommandHandler<RemoveGarmentSubconPackingOutCommand, GarmentSubconPackingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconPackingOutRepository _garmentPackingOutRepository;
        private readonly IGarmentSubconPackingOutItemRepository _garmentPackingOutItemRepository;

        private readonly IGarmentSubconPackingInItemRepository _garmentPackingInItemRepository;

        public RemoveGarmentSubconPackingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentPackingOutRepository = storage.GetRepository<IGarmentSubconPackingOutRepository>();
            _garmentPackingOutItemRepository = storage.GetRepository<IGarmentSubconPackingOutItemRepository>();

            _garmentPackingInItemRepository = storage.GetRepository<IGarmentSubconPackingInItemRepository>();
        }

        public async Task<GarmentSubconPackingOut> Handle(RemoveGarmentSubconPackingOutCommand request, CancellationToken cancellationToken)
        {
            var packOut = _garmentPackingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconPackingOut(o)).Single();

            _garmentPackingOutItemRepository.Find(o => o.PackingOutId == packOut.Identity).ForEach(async packingOutItem =>
            {
                var packingInTtem = _garmentPackingInItemRepository.Query.Where(x => x.Identity == packingOutItem.PackingInItemId).Select(o => new GarmentSubconPackingInItem(o)).Single();

                packingInTtem.SetRemainingQuantity(packingInTtem.RemainingQuantity + packingOutItem.Quantity);
                packingInTtem.Modify();

                await _garmentPackingInItemRepository.Update(packingInTtem);

                packingOutItem.Remove();

                await _garmentPackingOutItemRepository.Update(packingOutItem);
            });

            packOut.Remove();
            await _garmentPackingOutRepository.Update(packOut);

            _storage.Save();

            return packOut;

        }
    }
}
