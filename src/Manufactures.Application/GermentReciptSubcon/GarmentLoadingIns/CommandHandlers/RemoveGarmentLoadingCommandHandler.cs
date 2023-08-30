using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentLoadings.CommandHandlers
{
    public class RemoveGarmentLoadingCommandHandler : ICommandHandler<RemoveGarmentSubconLoadingInCommand, GarmentSubconLoadingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconLoadingInRepository _garmentLoadingRepository;
        private readonly IGarmentSubconLoadingInItemRepository _garmentLoadingItemRepository;

        private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttinOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;

        private readonly IGarmentSubconCuttingInDetailRepository _garmentCuttingInDetailRepository;
        public RemoveGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentSubconLoadingInRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();
            _garmentCuttinOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();

            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSubconCuttingInDetailRepository>();
        }

        public async Task<GarmentSubconLoadingIn> Handle(RemoveGarmentSubconLoadingInCommand request, CancellationToken cancellationToken)
        {
            var loading = _garmentLoadingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconLoadingIn(o)).Single();

            Dictionary<Guid, double> CutOutDetailToBeUpdated = new Dictionary<Guid, double>();
            _garmentLoadingItemRepository.Find(o => o.LoadingId == loading.Identity).ForEach(async loadingItem =>
            {
                if (CutOutDetailToBeUpdated.ContainsKey(loadingItem.CuttingOutDetailId))
                {
                    CutOutDetailToBeUpdated[loadingItem.CuttingOutDetailId] += loadingItem.Quantity;
                }
                else
                {
                    CutOutDetailToBeUpdated.Add(loadingItem.CuttingOutDetailId, loadingItem.Quantity);
                }

                loadingItem.Remove();

                await _garmentLoadingItemRepository.Update(loadingItem);
            });

            foreach (var cuttingOutDetail in CutOutDetailToBeUpdated)
            {
                //Update Real Qty Cutting Out
                var garmentCuttingOutDetail = _garmentCuttingOutDetailRepository.Query.Where(x => x.Identity == cuttingOutDetail.Key).Select(s => new GarmentSubconCuttingOutDetail(s)).Single();

                double diffQty = garmentCuttingOutDetail.CuttingOutQuantity - cuttingOutDetail.Value;

                garmentCuttingOutDetail.SetRealOutQuantity(0);
                garmentCuttingOutDetail.Modify();

                await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);

                //Update RemainingQty Cutting In If CuttingOut Qty is different with Real Out Qty
                if (diffQty > 0)
                {
                    var garmentCuttingOutItem = _garmentCuttinOutItemRepository.Query.Where(x => x.Identity == garmentCuttingOutDetail.CutOutItemId).Select(s => new GarmentSubconCuttingOutItem(s)).Single();
                    garmentCuttingOutItem.SetRealOutQuantity(0);
                    garmentCuttingOutItem.Modify();

                    await _garmentCuttinOutItemRepository.Update(garmentCuttingOutItem);

                    var garmenCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == garmentCuttingOutItem.CuttingInDetailId).Select(s => new GarmentSubconCuttingInDetail(s)).Single();

                    garmenCuttingInDetail.SetRemainingQuantity(garmenCuttingInDetail.RemainingQuantity - diffQty);

                    garmenCuttingInDetail.Modify();

                    await _garmentCuttingInDetailRepository.Update(garmenCuttingInDetail);
                }
            }

            loading.Remove();
            await _garmentLoadingRepository.Update(loading);

            _storage.Save();

            return loading;
        }
    }
}
