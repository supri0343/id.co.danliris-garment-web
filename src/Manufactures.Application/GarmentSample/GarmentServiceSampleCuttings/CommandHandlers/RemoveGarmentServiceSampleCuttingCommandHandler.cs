using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.CommandHandlers
{
    public class RemoveGarmentServiceSampleCuttingCommandHandler : ICommandHandler<RemoveGarmentServiceSampleCuttingCommand, GarmentServiceSampleCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleCuttingRepository _garmentServiceSampleCuttingRepository;
        private readonly IGarmentServiceSampleCuttingItemRepository _garmentServiceSampleCuttingItemRepository;
        private readonly IGarmentServiceSampleCuttingDetailRepository _garmentServiceSampleCuttingDetailRepository;
        private readonly IGarmentServiceSampleCuttingSizeRepository _garmentServiceSampleCuttingSizeRepository;

        public RemoveGarmentServiceSampleCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleCuttingRepository = storage.GetRepository<IGarmentServiceSampleCuttingRepository>();
            _garmentServiceSampleCuttingItemRepository = storage.GetRepository<IGarmentServiceSampleCuttingItemRepository>();
            _garmentServiceSampleCuttingDetailRepository = storage.GetRepository<IGarmentServiceSampleCuttingDetailRepository>();
            _garmentServiceSampleCuttingSizeRepository = storage.GetRepository<IGarmentServiceSampleCuttingSizeRepository>();
        }

        public async Task<GarmentServiceSampleCutting> Handle(RemoveGarmentServiceSampleCuttingCommand request, CancellationToken cancellationToken)
        {
            var SampleCutting = _garmentServiceSampleCuttingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleCutting(o)).Single();

            _garmentServiceSampleCuttingItemRepository.Find(o => o.ServiceSampleCuttingId == SampleCutting.Identity).ForEach(async SampleCuttingItem =>
            {
                _garmentServiceSampleCuttingDetailRepository.Find(i => i.ServiceSampleCuttingItemId == SampleCuttingItem.Identity).ForEach(async SampleDetail =>
                    {
                        _garmentServiceSampleCuttingSizeRepository.Find(i => i.ServiceSampleCuttingDetailId == SampleDetail.Identity).ForEach(async SampleSize =>
                        {
                            SampleSize.Remove();
                            await _garmentServiceSampleCuttingSizeRepository.Update(SampleSize);
                        });
                        SampleDetail.Remove();
                        await _garmentServiceSampleCuttingDetailRepository.Update(SampleDetail);
                    });
                SampleCuttingItem.Remove();
                await _garmentServiceSampleCuttingItemRepository.Update(SampleCuttingItem);
            });

            SampleCutting.Remove();
            await _garmentServiceSampleCuttingRepository.Update(SampleCutting);

            _storage.Save();

            return SampleCutting;
        }
    }
}