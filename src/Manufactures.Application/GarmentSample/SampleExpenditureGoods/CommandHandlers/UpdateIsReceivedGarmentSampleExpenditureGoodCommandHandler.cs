using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSampleExpenditureGoods.Commands;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSampleExpenditureGoods.CommandHandlers
{
    public class UpdateIsReceivedGarmentSampleExpenditureGoodCommandHandler : ICommandHandler<UpdateIsReceivedGarmentSampleExpenditureGoodCommand, GarmentSampleExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleExpenditureGoodRepository _garmentSampleExpenditureGoodRepository;

        public UpdateIsReceivedGarmentSampleExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();

        }

        public async Task<GarmentSampleExpenditureGood> Handle(UpdateIsReceivedGarmentSampleExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var ExpenditureGood = _garmentSampleExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleExpenditureGood(o)).Single();

            ExpenditureGood.SetIsReceived(request.IsReceived);
            ExpenditureGood.Modify();
            await _garmentSampleExpenditureGoodRepository.Update(ExpenditureGood);

            _storage.Save();

            return ExpenditureGood;
        }
    }
}
