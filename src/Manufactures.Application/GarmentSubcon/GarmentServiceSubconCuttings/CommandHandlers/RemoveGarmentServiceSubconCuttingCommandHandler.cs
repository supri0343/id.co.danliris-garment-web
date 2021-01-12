using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers
{
    public class RemoveGarmentServiceSubconCuttingCommandHandler : ICommandHandler<RemoveGarmentServiceSubconCuttingCommand, GarmentServiceSubconCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconCuttingRepository _garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository _garmentServiceSubconCuttingItemRepository;

        public RemoveGarmentServiceSubconCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
        }

        public async Task<GarmentServiceSubconCutting> Handle(RemoveGarmentServiceSubconCuttingCommand request, CancellationToken cancellationToken)
        {
            var subconCutting = _garmentServiceSubconCuttingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconCutting(o)).Single();

            _garmentServiceSubconCuttingItemRepository.Find(o => o.ServiceSubconCuttingId == subconCutting.Identity).ForEach(async subconCuttingItem =>
            {
                subconCuttingItem.Remove();
                await _garmentServiceSubconCuttingItemRepository.Update(subconCuttingItem);
            });

            subconCutting.Remove();
            await _garmentServiceSubconCuttingRepository.Update(subconCutting);

            _storage.Save();

            return subconCutting;
        }
    }
}