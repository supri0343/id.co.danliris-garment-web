using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.CommandHandlers
{
    public class RemoveGarmentServiceSampleFabricWashCommandHandler : ICommandHandler<RemoveGarmentServiceSampleFabricWashCommand, GarmentServiceSampleFabricWash>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleFabricWashRepository _garmentServiceSampleFabricWashRepository;
        private readonly IGarmentServiceSampleFabricWashItemRepository _garmentServiceSampleFabricWashItemRepository;
        private readonly IGarmentServiceSampleFabricWashDetailRepository _garmentServiceSampleFabricWashDetailRepository;

        public RemoveGarmentServiceSampleFabricWashCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleFabricWashRepository = storage.GetRepository<IGarmentServiceSampleFabricWashRepository>();
            _garmentServiceSampleFabricWashItemRepository = storage.GetRepository<IGarmentServiceSampleFabricWashItemRepository>();
            _garmentServiceSampleFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSampleFabricWashDetailRepository>();
        }

        public async Task<GarmentServiceSampleFabricWash> Handle(RemoveGarmentServiceSampleFabricWashCommand request, CancellationToken cancellationToken)
        {
            var serviceSampleFabricWash = _garmentServiceSampleFabricWashRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleFabricWash(o)).Single();

            _garmentServiceSampleFabricWashItemRepository.Find(o => o.ServiceSampleFabricWashId == serviceSampleFabricWash.Identity).ForEach(async serviceSampleFabricWashItem =>
            {
                _garmentServiceSampleFabricWashDetailRepository.Find(i => i.ServiceSampleFabricWashItemId == serviceSampleFabricWashItem.Identity).ForEach(async serviceSampleFabricWashDetail =>
                {
                    serviceSampleFabricWashDetail.Remove();
                    await _garmentServiceSampleFabricWashDetailRepository.Update(serviceSampleFabricWashDetail);
                });
                serviceSampleFabricWashItem.Remove();
                await _garmentServiceSampleFabricWashItemRepository.Update(serviceSampleFabricWashItem);
            });

            serviceSampleFabricWash.Remove();
            await _garmentServiceSampleFabricWashRepository.Update(serviceSampleFabricWash);

            _storage.Save();

            return serviceSampleFabricWash;
        }
    }
}
