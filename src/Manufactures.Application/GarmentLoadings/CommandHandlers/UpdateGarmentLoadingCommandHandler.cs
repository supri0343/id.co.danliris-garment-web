using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentLoadings.CommandHandlers
{
    public class UpdateGarmentLoadingCommandHandler : ICommandHandler<UpdateGarmentLoadingCommand, GarmentLoading>
    {
        private readonly IStorage _storage;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public UpdateGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        public async Task<GarmentLoading> Handle(UpdateGarmentLoadingCommand request, CancellationToken cancellationToken)
        {
            var loading = _garmentLoadingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentLoading(o)).Single();

            Dictionary<Guid, double> sewingDOItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentLoadingItemRepository.Find(o => o.LoadingId == loading.Identity).ForEach(async loadingItem =>
            {

                loadingItem.Modify();

                var item = request.Items.Where(o => o.Id == loadingItem.Identity).Single();

                var diffSewingDOQuantity = loadingItem.Quantity - item.Quantity;

                if (sewingDOItemToBeUpdated.ContainsKey(loadingItem.SewingDOItemId))
                {
                    sewingDOItemToBeUpdated[loadingItem.SewingDOItemId] += diffSewingDOQuantity;
                }
                else
                {
                    sewingDOItemToBeUpdated.Add(loadingItem.SewingDOItemId, diffSewingDOQuantity);
                }

                await _garmentLoadingItemRepository.Update(loadingItem);
            });

            foreach (var sewingDOItem in sewingDOItemToBeUpdated)
            {
                var garmentSewingDOItem = _garmentSewingDOItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingDOItem(s)).Single();
                garmentSewingDOItem.setRemainingQuantity(garmentSewingDOItem.RemainingQuantity + sewingDOItem.Value);
                garmentSewingDOItem.Modify();

                await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
            }

            loading.Modify();
            await _garmentLoadingRepository.Update(loading);

            _storage.Save();

            return loading;
        }
    }
}
