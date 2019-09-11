using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentLoadings.CommandHandlers
{
    public class RemoveGarmentLoadingCommandHandler : ICommandHandler<RemoveGarmentLoadingCommand, GarmentLoading>
    {
        private readonly IStorage _storage;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;

        public RemoveGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
        }

        public async Task<GarmentLoading> Handle(RemoveGarmentLoadingCommand request, CancellationToken cancellationToken)
        {
            var loading = _garmentLoadingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentLoading(o)).Single();

            _garmentLoadingItemRepository.Find(o => o.LoadingId == loading.Identity).ForEach(async loadingItem =>
            {
                loadingItem.Remove();
                await _garmentLoadingItemRepository.Update(loadingItem);
            });

            loading.Remove();
            await _garmentLoadingRepository.Update(loading);

            _storage.Save();

            return loading;
        }
    }
}
