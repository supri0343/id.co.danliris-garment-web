using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Moonlay;

namespace Manufactures.Application.GermentReciptSubcon.GarmentCuttingOuts.CommandHandlers
{
    public class RemoveGarmentSubconCuttingOutCommandHandler : ICommandHandler<RemoveGarmentSubconCuttingOutCommand, GarmentSubconCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingOutRepository _garmentSubconCuttingOutRepository;
        private readonly IGarmentSubconCuttingOutItemRepository _garmentSubconCuttingOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentSubconCuttingOutDetailRepository;
        private readonly IGarmentSubconCuttingInDetailRepository _garmentSubconCuttingInDetailRepository;
        //private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        //private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public RemoveGarmentSubconCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentSubconCuttingOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentSubconCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
            _garmentSubconCuttingInDetailRepository = storage.GetRepository<IGarmentSubconCuttingInDetailRepository>();
            //_garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            //_garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        public async Task<GarmentSubconCuttingOut> Handle(RemoveGarmentSubconCuttingOutCommand request, CancellationToken cancellationToken)
        {
            var cutOut = _garmentSubconCuttingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCuttingOut(o)).Single();
            //var sewingDO = _garmentSewingDORepository.Query.Where(o => o.CuttingOutId == request.Identity).Select(o => new GarmentSewingDO(o)).Single();

            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();

            _garmentSubconCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).ForEach(async cutOutItem =>
            {
                _garmentSubconCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).ForEach(async cutOutDetail =>
                {
                    if (cuttingInDetailToBeUpdated.ContainsKey(cutOutItem.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[cutOutItem.CuttingInDetailId] += cutOutDetail.CuttingOutQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(cutOutItem.CuttingInDetailId, cutOutDetail.CuttingOutQuantity);
                    }

                    cutOutDetail.Remove();
                    await _garmentSubconCuttingOutDetailRepository.Update(cutOutDetail);
                });

                cutOutItem.Remove();
                await _garmentSubconCuttingOutItemRepository.Update(cutOutItem);
            });

            foreach (var cuttingInItem in cuttingInDetailToBeUpdated)
            {
                var garmentSubconCuttingInDetail = _garmentSubconCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInItem.Key).Select(s => new GarmentSubconCuttingInDetail(s)).Single();
                garmentSubconCuttingInDetail.SetRemainingQuantity(garmentSubconCuttingInDetail.RemainingQuantity + cuttingInItem.Value);
                garmentSubconCuttingInDetail.Modify();
                await _garmentSubconCuttingInDetailRepository.Update(garmentSubconCuttingInDetail);
            }

            cutOut.Remove();
            await _garmentSubconCuttingOutRepository.Update(cutOut);

            _storage.Save();

            return cutOut;
        }
    }
}