using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ISubconInvoicePackingListReceiptItemRepositories;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Manufactures.Application.GarmentSubcon.InvoicePackingList.CommandHandlers
{
    public class RemoveGarmentSubconInvoicePackingListCommandHandler : ICommandHandler<RemoveGarmentSubconInvoicePackingListCommand, SubconInvoicePackingList>
    {
        private readonly IStorage _storage;
        private readonly ISubconInvoicePackingListRepository _subconInvoicePackingListRepository;
        private readonly ISubconInvoicePackingListItemRepository _subconInvoicePackingListItemRepository;
        private readonly ISubconInvoicePackingListReceiptItemRepository _subconInvoicePackingListReceiptItemRepository;

        public RemoveGarmentSubconInvoicePackingListCommandHandler(IStorage storage)
        {
            _storage = storage;
            _subconInvoicePackingListRepository = storage.GetRepository<ISubconInvoicePackingListRepository>();
            _subconInvoicePackingListItemRepository = storage.GetRepository<ISubconInvoicePackingListItemRepository>();
            _subconInvoicePackingListReceiptItemRepository = storage.GetRepository<ISubconInvoicePackingListReceiptItemRepository>();
        }

        public async Task<SubconInvoicePackingList> Handle(RemoveGarmentSubconInvoicePackingListCommand request, CancellationToken cancellationToken)
        {
            var subconContract = _subconInvoicePackingListRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new SubconInvoicePackingList(o)).Single();
            // NOT BC 262 & 27 IN
            _subconInvoicePackingListItemRepository.Find(o => o.InvoicePackingListId == subconContract.Identity).ForEach(async subconContractItem =>
            {
                subconContractItem.Remove();

                await _subconInvoicePackingListItemRepository.Update(subconContractItem);
            });
            //BC 262 & 27 IN
            _subconInvoicePackingListReceiptItemRepository.Find(o => o.InvoicePackingListId == subconContract.Identity).ForEach(async subconContractReceiptItem =>
            {
                subconContractReceiptItem.Remove();

                await _subconInvoicePackingListReceiptItemRepository.Update(subconContractReceiptItem);
            });

            subconContract.Remove();
            await _subconInvoicePackingListRepository.Update(subconContract);

            _storage.Save();

            return subconContract;
        }
    }
}
