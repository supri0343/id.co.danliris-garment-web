using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.CustomsOuts.CommandHandlers
{
    public class RemoveGarmentSubconCustomsOutCommandHandler : ICommandHandler<RemoveGarmentSubconCustomsOutCommand, GarmentSubconCustomsOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsOutRepository _garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository _garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public RemoveGarmentSubconCustomsOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            _garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }


        public async Task<GarmentSubconCustomsOut> Handle(RemoveGarmentSubconCustomsOutCommand request, CancellationToken cancellationToken)
        {
            var subconCustomsOut = _garmentSubconCustomsOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCustomsOut(o)).Single();

            _garmentSubconCustomsOutItemRepository.Find(o => o.SubconCustomsOutId == subconCustomsOut.Identity).ForEach(async subconCustomsOutItem =>
            {
                subconCustomsOutItem.Remove();

                var subconDLOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(x => x.Identity == subconCustomsOutItem.SubconDLOutId).Select(s => new GarmentSubconDeliveryLetterOut(s)).Single();
                subconDLOut.SetIsUsed(false);
                subconDLOut.Modify();
                await _garmentSubconDeliveryLetterOutRepository.Update(subconDLOut);

                await _garmentSubconCustomsOutItemRepository.Update(subconCustomsOutItem);
            });

            var listCustoOutSameContract = _garmentSubconCustomsOutRepository.Query.Where(o => o.SubconContractId == subconCustomsOut.SubconContractId).Select(o => new GarmentSubconCustomsOut(o)).ToList();

            if(listCustoOutSameContract.Count() == 1)
            {
                var subconContract = _garmentSubconContractRepository.Query.Where(x => x.Identity == subconCustomsOut.SubconContractId).Select(n => new GarmentSubconContract(n)).Single();
                subconContract.SetIsCustoms(false);
                subconContract.Modify();
                await _garmentSubconContractRepository.Update(subconContract);
            }
           
            subconCustomsOut.Remove();
            await _garmentSubconCustomsOutRepository.Update(subconCustomsOut);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "EXIM", "Delete BC Keluar Subcon - " + subconCustomsOut.CustomsOutNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return subconCustomsOut;
        }
    }
}
