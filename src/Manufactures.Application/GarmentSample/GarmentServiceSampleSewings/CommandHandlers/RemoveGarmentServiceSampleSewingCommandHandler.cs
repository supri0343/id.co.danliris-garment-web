using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleSewings.CommandHandlers
{
    public class RemoveGarmentServiceSampleSewingCommandHandler : ICommandHandler<RemoveGarmentServiceSampleSewingCommand, GarmentServiceSampleSewing>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleSewingRepository _garmentServiceSampleSewingRepository;
        private readonly IGarmentServiceSampleSewingItemRepository _garmentServiceSampleSewingItemRepository;
        private readonly IGarmentServiceSampleSewingDetailRepository _garmentServiceSampleSewingDetailRepository;

        public RemoveGarmentServiceSampleSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleSewingRepository = storage.GetRepository<IGarmentServiceSampleSewingRepository>();
            _garmentServiceSampleSewingItemRepository = storage.GetRepository<IGarmentServiceSampleSewingItemRepository>();
            _garmentServiceSampleSewingDetailRepository = storage.GetRepository<IGarmentServiceSampleSewingDetailRepository>();
        }

        public async Task<GarmentServiceSampleSewing> Handle(RemoveGarmentServiceSampleSewingCommand request, CancellationToken cancellationToken)
        {
            var serviceSampleSewing = _garmentServiceSampleSewingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleSewing(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentServiceSampleSewingItemRepository.Find(o => o.ServiceSampleSewingId == serviceSampleSewing.Identity).ForEach(async serviceSampleSewingItem =>
            {
                //if (!serviceSampleSewing.IsDifferentSize)
                //{
                //    if (sewInItemToBeUpdated.ContainsKey(serviceSampleSewingItem.SewingInItemId))
                //    {
                //        sewInItemToBeUpdated[serviceSampleSewingItem.SewingInItemId] += serviceSampleSewingItem.Quantity;
                //    }
                //    else
                //    {
                //        sewInItemToBeUpdated.Add(serviceSampleSewingItem.SewingInItemId, serviceSampleSewingItem.Quantity);
                //    }
                //}
                _garmentServiceSampleSewingDetailRepository.Find(i => i.ServiceSampleSewingItemId == serviceSampleSewingItem.Identity).ForEach(async SampleDetail =>
                {
                    SampleDetail.Remove();
                    await _garmentServiceSampleSewingDetailRepository.Update(SampleDetail);
                });
                serviceSampleSewingItem.Remove();
                await _garmentServiceSampleSewingItemRepository.Update(serviceSampleSewingItem);
            });

            serviceSampleSewing.Remove();
            await _garmentServiceSampleSewingRepository.Update(serviceSampleSewing);

            _storage.Save();

            return serviceSampleSewing;
        }
    }
}
