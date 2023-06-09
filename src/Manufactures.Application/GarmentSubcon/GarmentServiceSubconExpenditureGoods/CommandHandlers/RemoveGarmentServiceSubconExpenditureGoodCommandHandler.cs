using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Commands;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconExpenditureGoods.CommandHandlers
{
    public class RemoveGarmentServiceSubconExpenditureGoodCommandHandler : ICommandHandler<RemoveGarmentServiceSubconExpenditureGoodCommand, GarmentServiceSubconExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconExpenditureGoodRepository _garmentServiceSubconExpenditureGoodRepository;
        private readonly IGarmentServiceSubconExpenditureGoodtemRepository _garmentServiceSubconExpenditureGoodItemRepository;

        public RemoveGarmentServiceSubconExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodRepository>();
            _garmentServiceSubconExpenditureGoodItemRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodtemRepository>();
        }

        public async Task<GarmentServiceSubconExpenditureGood> Handle(RemoveGarmentServiceSubconExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconExpenditureGood(o)).Single();

            _garmentServiceSubconExpenditureGoodItemRepository.Find(s => s.ServiceSubconExpenditureGoodId == request.Identity).ForEach(async subconExpenditureItem =>
            {
                subconExpenditureItem.Remove();
                await _garmentServiceSubconExpenditureGoodItemRepository.Update(subconExpenditureItem);
            });
         
            subconExpenditureGood.Remove();
            await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);

            _storage.Save();

            return subconExpenditureGood;
        }
    }
}
