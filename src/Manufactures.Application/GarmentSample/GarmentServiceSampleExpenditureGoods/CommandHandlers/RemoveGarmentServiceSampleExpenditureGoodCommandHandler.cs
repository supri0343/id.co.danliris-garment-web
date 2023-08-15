using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Commands;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleExpenditureGoods.CommandHandlers
{
    public class RemoveGarmentServiceSampleExpenditureGoodCommandHandler : ICommandHandler<RemoveGarmentServiceSampleExpenditureGoodCommand, GarmentServiceSampleExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleExpenditureGoodRepository _garmentServiceSampleExpenditureGoodRepository;
        private readonly IGarmentServiceSampleExpenditureGoodtemRepository _garmentServiceSampleExpenditureGoodItemRepository;

        public RemoveGarmentServiceSampleExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodRepository>();
            _garmentServiceSampleExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodtemRepository>();
        }

        public async Task<GarmentServiceSampleExpenditureGood> Handle(RemoveGarmentServiceSampleExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var SampleExpenditureGood = _garmentServiceSampleExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleExpenditureGood(o)).Single();

            _garmentServiceSampleExpenditureGoodItemRepository.Find(s => s.ServiceSampleExpenditureGoodId == request.Identity).ForEach(async SampleExpenditureItem =>
            {
                SampleExpenditureItem.Remove();
                await _garmentServiceSampleExpenditureGoodItemRepository.Update(SampleExpenditureItem);
            });
         
            SampleExpenditureGood.Remove();
            await _garmentServiceSampleExpenditureGoodRepository.Update(SampleExpenditureGood);

            _storage.Save();

            return SampleExpenditureGood;
        }
    }
}
