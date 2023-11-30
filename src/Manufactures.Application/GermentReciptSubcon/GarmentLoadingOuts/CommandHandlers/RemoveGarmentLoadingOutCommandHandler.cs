using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentLoadingOuts.CommandHandlers
{
    public class RemoveGarmentLoadingOutCommandHandler : ICommandHandler<RemoveGarmentSubconLoadingOutCommand, GarmentSubconLoadingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconLoadingOutRepository _garmentLoadingOutRepository;
        private readonly IGarmentSubconLoadingOutItemRepository _garmentLoadingOutItemRepository;

        private readonly IGarmentSubconLoadingInItemRepository _garmentLoadingInItemRepository;
        public RemoveGarmentLoadingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingOutRepository = storage.GetRepository<IGarmentSubconLoadingOutRepository>();
            _garmentLoadingOutItemRepository = storage.GetRepository<IGarmentSubconLoadingOutItemRepository>();

            _garmentLoadingInItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();
        }

        public async Task<GarmentSubconLoadingOut> Handle(RemoveGarmentSubconLoadingOutCommand request, CancellationToken cancellationToken)
        {
            var loading = _garmentLoadingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconLoadingOut(o)).Single();

            Dictionary<Guid, double> LoadingInToBeUpdated = new Dictionary<Guid, double>();
            _garmentLoadingOutItemRepository.Find(o => o.LoadingOutId == loading.Identity).ForEach(async loadingItem =>
            {
                if (LoadingInToBeUpdated.ContainsKey(loadingItem.LoadingInItemId))
                {
                    LoadingInToBeUpdated[loadingItem.LoadingInItemId] += loadingItem.Quantity;
                }
                else
                {
                    LoadingInToBeUpdated.Add(loadingItem.LoadingInItemId, loadingItem.Quantity);
                }

                loadingItem.Remove();

                await _garmentLoadingOutItemRepository.Update(loadingItem);
            });

            foreach (var _loadingInItem in LoadingInToBeUpdated)
            {
                var garmentLoadingInItem = _garmentLoadingInItemRepository.Query.Where(x => x.Identity == _loadingInItem.Key).Select(s => new GarmentSubconLoadingInItem(s)).Single();
                garmentLoadingInItem.SetRemainingQuantity(garmentLoadingInItem.RemainingQuantity + _loadingInItem.Value);
                garmentLoadingInItem.Modify();

                await _garmentLoadingInItemRepository.Update(garmentLoadingInItem);
            }

            loading.Remove();
            await _garmentLoadingOutRepository.Update(loading);

            _storage.Save();

            return loading;
        }
    }
}
