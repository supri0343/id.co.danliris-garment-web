using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers
{
    public class PlaceGarmentSubconContractCommandHandler : ICommandHandler<PlaceGarmentSubconContractCommand, GarmentSubconContract>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;

        public PlaceGarmentSubconContractCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
        }

        public async Task<GarmentSubconContract> Handle(PlaceGarmentSubconContractCommand request, CancellationToken cancellationToken)
        {
            Guid SubconContractId = Guid.NewGuid();

            GarmentSubconContract garmentSubconContract = new GarmentSubconContract(
                SubconContractId,
                request.ContractType,
                request.ContractNo,
                request.AgreementNo,
                new SupplierId(request.Supplier.Id),
                request.Supplier.Code,
                request.Supplier.Name,
                request.JobType,
                request.BPJNo,
                request.FinishedGoodType,
                request.Quantity,
                request.DueDate
            );

            await _garmentSubconContractRepository.Update(garmentSubconContract);
            _storage.Save();

            return garmentSubconContract;
        }
    }
}
