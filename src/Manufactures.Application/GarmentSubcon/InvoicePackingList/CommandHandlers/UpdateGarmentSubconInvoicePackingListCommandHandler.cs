using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Domain.Commands;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ISubconInvoicePackingListReceiptItemRepositories;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconInvoicePackingListReceiptItemModel;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Manufactures.Application.GarmentSubcon.InvoicePackingList.CommandHandlers
{
    public class UpdateGarmentSubconInvoicePackingListCommandHandler : ICommandHandler<UpdateGarmentSubconInvoicePackingListCommand, SubconInvoicePackingList>
    {
        private readonly IStorage _storage;
        private readonly IWebApiContext _webApiContext;
        private readonly IHttpClientService _http;
        private readonly ISubconInvoicePackingListRepository _subconInvoicePackingListRepository;
        private readonly ISubconInvoicePackingListItemRepository _subconInvoicePackingListItemRepository;
        private readonly ISubconInvoicePackingListReceiptItemRepository _subconInvoicePackingListReceiptItemRepository;

        public UpdateGarmentSubconInvoicePackingListCommandHandler(IStorage storage, IWebApiContext webApiContext, IHttpClientService http)
        {
            _storage = storage;
            _http = http;
            _webApiContext = webApiContext;
            _subconInvoicePackingListRepository = storage.GetRepository<ISubconInvoicePackingListRepository>();
            _subconInvoicePackingListItemRepository = storage.GetRepository<ISubconInvoicePackingListItemRepository>();
            _subconInvoicePackingListReceiptItemRepository = storage.GetRepository<ISubconInvoicePackingListReceiptItemRepository>();
        }
        public async Task<SubconInvoicePackingList> Handle(UpdateGarmentSubconInvoicePackingListCommand request, CancellationToken cancellationToken)
        {
            var invoicePackingList = _subconInvoicePackingListRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new SubconInvoicePackingList(o)).Single();

            List<string> UpdateIsSubconWithPO = new List<string>();
            List<string> UpdateIsSubconNonPO = new List<string>();
            List<string> DeletedIsSubconWithPO = new List<string>();
            List<string> DeletedSubconNonPO = new List<string>();
            if (invoicePackingList.BCType == "BC 2.6.2" || invoicePackingList.BCType == "BC 2.7 IN")
            {
                _subconInvoicePackingListReceiptItemRepository.Find(o => o.InvoicePackingListId == invoicePackingList.Identity).ForEach(async invoicePackingListReceiptItem =>
                {
                    var item = request.Items.Where(o => o.Id == invoicePackingListReceiptItem.Identity).SingleOrDefault();
                    if (item == null)
                    {
                        if (invoicePackingList.POType == "DENGAN PO")
                        {
                            DeletedIsSubconWithPO.Add(invoicePackingListReceiptItem.DLNo);
                        }
                        else
                        {
                            DeletedSubconNonPO.Add(invoicePackingListReceiptItem.DLNo);
                        }

                        invoicePackingListReceiptItem.Remove();
                    }

                    await _subconInvoicePackingListReceiptItemRepository.Update(invoicePackingListReceiptItem);
                });

                foreach(var item in request.Items)
                {
                    if (item.Id == Guid.Empty)
                    {
                        SubconInvoicePackingListReceiptItem subconInvoicePackingListReceiptItem = new SubconInvoicePackingListReceiptItem(
                         Guid.NewGuid(),
                         invoicePackingList.Identity,
                         item.DLNo,
                         item.DLDate,
                         new ProductId(0),
                         "-",
                         "-",
                         item.Product.Remark,
                         item.Quantity,
                         new UomId(item.Uom.Id),
                         item.Uom.Unit,
                         item.TotalPrice,
                         item.PricePerDealUnit
                       );

                        if (invoicePackingList.POType == "DENGAN PO")
                        {
                            UpdateIsSubconWithPO.Add(item.DLNo);
                        }
                        else
                        {
                            UpdateIsSubconNonPO.Add(item.DLNo);
                        }
                        await _subconInvoicePackingListReceiptItemRepository.Update(subconInvoicePackingListReceiptItem);
                    }
                }

                var DLListNo = "";
                if (UpdateIsSubconWithPO.Count() > 0)
                {
                    DLListNo = string.Join(",", UpdateIsSubconWithPO);
                    await SetIsSubconInvoiceGarmentDeliveryOrder(DLListNo, invoicePackingList.POType, true);
                }
                else if (UpdateIsSubconNonPO.Count() > 0)
                {
                    DLListNo = string.Join(",", UpdateIsSubconNonPO);
                    await SetIsSubconInvoiceGarmentDeliveryOrder(DLListNo, invoicePackingList.POType, true);
                }
                if(DeletedIsSubconWithPO.Count() > 0) ///* UpdateDeletedDLNo*/ 
                {
                    DLListNo = string.Join(",", DeletedIsSubconWithPO);
                    await SetIsSubconInvoiceGarmentDeliveryOrder(DLListNo, invoicePackingList.POType, false);
                }
                else if (DeletedSubconNonPO.Count() > 0)
                {
                    DLListNo = string.Join(",", DeletedSubconNonPO);
                    await SetIsSubconInvoiceGarmentDeliveryOrder(DLListNo, invoicePackingList.POType, false);
                }
            }
            else
            {
                _subconInvoicePackingListItemRepository.Find(o => o.InvoicePackingListId == invoicePackingList.Identity).ForEach(async invoicePackingListItem =>
                {
                    var item = request.Items.Where(o => o.Id == invoicePackingListItem.Identity).SingleOrDefault();
                    if (item == null)
                    {
                        invoicePackingListItem.Remove();

                        await _subconInvoicePackingListItemRepository.Update(invoicePackingListItem);
                    }

                    foreach (var itemm in request.Items)
                    {
                        if (item.Id == Guid.Empty)
                        {
                            SubconInvoicePackingListItem subconInvoicePackingListItem = new SubconInvoicePackingListItem(
                            Guid.NewGuid(),
                            invoicePackingList.Identity,
                            itemm.DLNo,
                            itemm.DLDate,
                             new ProductId(0),
                            "-",
                            "-",
                            "-",
                            "-",
                            itemm.Quantity,
                            new UomId(0),
                            "-",
                            itemm.CIF,
                            itemm.TotalPrice,
                            itemm.TotalNW,
                            itemm.TotalGW
                        );

                            await _subconInvoicePackingListItemRepository.Update(subconInvoicePackingListItem);
                        }
                    }
                  
                });
            }
            

            invoicePackingList.SetGW(request.GW);
            invoicePackingList.SetNW(request.NW);
            invoicePackingList.SetRemark(request.Remark);

            invoicePackingList.Modify();

            await _subconInvoicePackingListRepository.Update(invoicePackingList);

            _storage.Save();

            return invoicePackingList;

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
