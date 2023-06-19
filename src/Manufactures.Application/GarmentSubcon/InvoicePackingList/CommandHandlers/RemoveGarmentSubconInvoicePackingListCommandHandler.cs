using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Domain.Commands;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
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
        private readonly IWebApiContext _webApiContext;
        private readonly IHttpClientService _http;
        private readonly ISubconInvoicePackingListRepository _subconInvoicePackingListRepository;
        private readonly ISubconInvoicePackingListItemRepository _subconInvoicePackingListItemRepository;
        private readonly ISubconInvoicePackingListReceiptItemRepository _subconInvoicePackingListReceiptItemRepository;

        public RemoveGarmentSubconInvoicePackingListCommandHandler(IStorage storage, IWebApiContext webApiContext, IHttpClientService http)
        {
            _storage = storage;
            _http = http;
            _webApiContext = webApiContext;
            _subconInvoicePackingListRepository = storage.GetRepository<ISubconInvoicePackingListRepository>();
            _subconInvoicePackingListItemRepository = storage.GetRepository<ISubconInvoicePackingListItemRepository>();
            _subconInvoicePackingListReceiptItemRepository = storage.GetRepository<ISubconInvoicePackingListReceiptItemRepository>();
        }

        public async Task<SubconInvoicePackingList> Handle(RemoveGarmentSubconInvoicePackingListCommand request, CancellationToken cancellationToken)
        {
            List<string> UpdateIsSubconWithPO = new List<string>();
            List<string> UpdateIsSubconNonPO = new List<string>();
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
                if (subconContract.POType == "DENGAN PO")
                {
                    UpdateIsSubconWithPO.Add(subconContractReceiptItem.DLNo);
                }
                else
                {
                    UpdateIsSubconNonPO.Add(subconContractReceiptItem.DLNo);
                }

                subconContractReceiptItem.Remove();

                await _subconInvoicePackingListReceiptItemRepository.Update(subconContractReceiptItem);
            });

            var DLListNo = "";
            if (UpdateIsSubconWithPO.Count() > 0)
            {
                DLListNo = string.Join(",", UpdateIsSubconWithPO);
                await SetIsSubconInvoiceGarmentDeliveryOrder(DLListNo, subconContract.POType, false);
            }
            else if (UpdateIsSubconNonPO.Count() > 0)
            {
                DLListNo = string.Join(",", UpdateIsSubconNonPO);
                await SetIsSubconInvoiceGarmentDeliveryOrder(DLListNo, subconContract.POType, false);
            }


            subconContract.Remove();
            await _subconInvoicePackingListRepository.Update(subconContract);

            _storage.Save();

            return subconContract;
        }

        public async Task<string> SetIsSubconInvoiceGarmentDeliveryOrder(string DLNo, string POType, bool IsSubconInvoice)
        {
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint;
            var garmentPurchasingtUriUpdate = "";
            if (POType == "TANPA PO")
            {
                garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-delivery-orders-non-po/isSubconInvoice?DONos={DLNo}&isSubconInvoice={IsSubconInvoice}";
            }
            else
            {
                garmentPurchasingtUriUpdate = garmentPurchasingtUri + $"garment-delivery-orders/isSubconInvoice?DONos={DLNo}&isSubconInvoice={IsSubconInvoice}";
            }
            var garmentPurchasingtResponse = await _http.GetAsync(garmentPurchasingtUriUpdate, _webApiContext.Token);

            return garmentPurchasingtResponse.EnsureSuccessStatusCode().ToString(); ;
        }
    }
}
