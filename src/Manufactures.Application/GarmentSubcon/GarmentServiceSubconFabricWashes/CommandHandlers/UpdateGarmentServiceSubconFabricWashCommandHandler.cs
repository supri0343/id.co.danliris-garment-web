using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.CommandHandlers
{
    public class UpdateGarmentServiceSubconFabricWashCommandHandler : ICommandHandler<UpdateGarmentServiceSubconFabricWashCommand, GarmentServiceSubconFabricWash>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconFabricWashItemRepository _garmentServiceSubconFabricWashItemRepository;

        public UpdateGarmentServiceSubconFabricWashCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconFabricWashRepository = _storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentServiceSubconFabricWashItemRepository = _storage.GetRepository<IGarmentServiceSubconFabricWashItemRepository>();
        }

        public async Task<GarmentServiceSubconFabricWash> Handle(UpdateGarmentServiceSubconFabricWashCommand request, CancellationToken cancellationToken)
        {
            var serviceSubconFabricWash = _garmentServiceSubconFabricWashRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconFabricWash(o)).Single();

            serviceSubconFabricWash.SetServiceSubconFabricWashDate(request.ServiceSubconFabricWashDate.GetValueOrDefault());
            serviceSubconFabricWash.SetRemark(request.Remark);
            serviceSubconFabricWash.Modify();
            await _garmentServiceSubconFabricWashRepository.Update(serviceSubconFabricWash);

            _storage.Save();

            return serviceSubconFabricWash;
        }
    }
}
