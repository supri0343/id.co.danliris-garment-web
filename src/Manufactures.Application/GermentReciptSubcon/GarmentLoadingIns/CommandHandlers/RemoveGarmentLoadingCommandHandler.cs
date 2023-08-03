using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
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

        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        public RemoveGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentSubconLoadingInRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
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
                var garmentCuttingOutDetail = _garmentCuttingOutDetailRepository.Query.Where(x => x.Identity == cuttingOutDetail.Key).Select(s => new GarmentSubconCuttingOutDetail(s)).Single();
                garmentCuttingOutDetail.SetRemainingQuantity(garmentCuttingOutDetail.RemainingQuantity + cuttingOutDetail.Value);
                garmentCuttingOutDetail.Modify();

                await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);
            }

            loading.Remove();
            await _garmentLoadingRepository.Update(loading);

            _storage.Save();

            return loading;
        }
    }
}
