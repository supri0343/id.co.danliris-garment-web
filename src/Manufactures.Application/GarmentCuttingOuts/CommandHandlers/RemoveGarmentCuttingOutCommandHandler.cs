using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingOuts.CommandHandlers
{
    public class RemoveGarmentCuttingOutCommandHandler : ICommandHandler<RemoveGarmentCuttingOutCommand, GarmentCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;

        public RemoveGarmentCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
        }

        public async Task<GarmentCuttingOut> Handle(RemoveGarmentCuttingOutCommand request, CancellationToken cancellationToken)
        {
            var cutOut = _garmentCuttingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentCuttingOut(o)).Single();

            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();

            _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).ForEach(async cutOutItem =>
            {
                _garmentCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).ForEach(async cutOutDetail =>
                {
                    if (cuttingInDetailToBeUpdated.ContainsKey(cutOutItem.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[cutOutItem.CuttingInDetailId] += cutOutDetail.RemainingQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(cutOutItem.CuttingInDetailId, cutOutDetail.RemainingQuantity);
                    }

                    cutOutDetail.Remove();
                    await _garmentCuttingOutDetailRepository.Update(cutOutDetail);
                });

                cutOutItem.Remove();
                await _garmentCuttingOutItemRepository.Update(cutOutItem);
            });

            foreach (var cuttingInItem in cuttingInDetailToBeUpdated)
            {
                var garmentCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInItem.Key).Select(s => new GarmentCuttingInDetail(s)).Single();
                garmentCuttingInDetail.SetRemainingQuantity(garmentCuttingInDetail.RemainingQuantity + cuttingInItem.Value);
                garmentCuttingInDetail.Modify();
                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
            }

            cutOut.Remove();
            await _garmentCuttingOutRepository.Update(cutOut);

            _storage.Save();

            return cutOut;
        }
    }
}