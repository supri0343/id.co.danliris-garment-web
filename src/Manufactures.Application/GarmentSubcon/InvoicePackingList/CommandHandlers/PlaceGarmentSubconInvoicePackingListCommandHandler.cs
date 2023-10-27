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
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Manufactures.Application.GarmentSubcon.InvoicePackingList.CommandHandlers
{
    public class PlaceGarmentSubconInvoicePackingListCommandHandler : ICommandHandler<PlaceGarmentSubconInvoicePackingListCommand, SubconInvoicePackingList>
    {
        private readonly IStorage _storage;
        private readonly IWebApiContext _webApiContext;
        private readonly IHttpClientService _http;
        private readonly ISubconInvoicePackingListRepository _subconInvoicePackingListRepository;
        private readonly ISubconInvoicePackingListItemRepository _subconInvoicePackingListItemRepository;
        private readonly ISubconInvoicePackingListReceiptItemRepository _subconInvoicePackingListReceiptItemRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;

        public PlaceGarmentSubconInvoicePackingListCommandHandler(IStorage storage, IHttpClientService http, IWebApiContext webApiContext)
        {
            _http = http;
            _storage = storage;
            _webApiContext = webApiContext;
            _subconInvoicePackingListRepository = storage.GetRepository<ISubconInvoicePackingListRepository>();
            _subconInvoicePackingListItemRepository = storage.GetRepository<ISubconInvoicePackingListItemRepository>();
            _subconInvoicePackingListReceiptItemRepository = storage.GetRepository<ISubconInvoicePackingListReceiptItemRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }
        public async Task<SubconInvoicePackingList> Handle(PlaceGarmentSubconInvoicePackingListCommand request, CancellationToken cancellationToken)
        {
            Guid InvoicePackingListId = Guid.NewGuid();
            SubconInvoicePackingList subconInvoicePackingList = new SubconInvoicePackingList(
                InvoicePackingListId,
                GenerateNo(request),
                request.BCType,
                request.Date,
                new SupplierId(request.Supplier.Id),
                request.Supplier.Code,
                request.Supplier.Name,
                request.Supplier.Address,
                request.ContractNo,
                request.NW,
                request.GW,
                request.Remark,
                request.BuyerStaff,
                request.SubconContractId,
                request.POType
                );

            List<string> UpdateIsSubconWithPO = new List<string>();
            List<string> UpdateIsSubconNonPO = new List<string>();
            foreach (var item in request.Items)
            {
                if(request.BCType == "BC 2.6.2" || request.BCType == "BC 2.7 IN")
                {
                    if(request.POType == "DENGAN PO")
                    {
                        SubconInvoicePackingListReceiptItem subconInvoicePackingListReceiptItem = new SubconInvoicePackingListReceiptItem(
                            Guid.NewGuid(),
                            InvoicePackingListId,
                            item.DLNo,
                            item.DLDate,
                            new ProductId(item.Product.Id),
                            item.Product.Code,
                            item.Product.Name,
                            item.Product.Remark,
                            item.Quantity,
                            new UomId(item.Uom.Id),
                            item.Uom.Unit,
                            item.TotalPrice,
                            item.PricePerDealUnit
                       );
                       UpdateIsSubconWithPO.Add(item.DLNo);

                       await _subconInvoicePackingListReceiptItemRepository.Update(subconInvoicePackingListReceiptItem);
                    }
                    else
                    {
                        SubconInvoicePackingListReceiptItem subconInvoicePackingListReceiptItem = new SubconInvoicePackingListReceiptItem(
                          Guid.NewGuid(),
                          InvoicePackingListId,
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
                        UpdateIsSubconNonPO.Add(item.DLNo);

                        await _subconInvoicePackingListReceiptItemRepository.Update(subconInvoicePackingListReceiptItem);
                    }
                   
                }
                else
                {
                    SubconInvoicePackingListItem subconInvoicePackingListItem = new SubconInvoicePackingListItem(
                        Guid.NewGuid(),
                        InvoicePackingListId,
                        item.DLNo,
                        item.DLDate,
                        //new ProductId(item.Product.Id),
                        //item.Product.Code,
                        //item.Product.Name,
                        //item.Product.Remark,
                        // item.DesignColor,

                        new ProductId(0),
                        "-",
                        "-",
                        "-",
                        "-",
                        item.Quantity,
                        //new UomId(item.Uom.Id),
                        //item.Uom.Unit,
                        new UomId(0),
                        "-",
                        item.CIF,
                        item.TotalPrice,
                        item.TotalNW,
                        item.TotalGW
                    );
                    await _subconInvoicePackingListItemRepository.Update(subconInvoicePackingListItem);
                }
                
            }

            var DLListNo = "";
            if (UpdateIsSubconWithPO.Count() > 0 )
            {
                DLListNo = string.Join(",", UpdateIsSubconWithPO);
                await SetIsSubconInvoiceGarmentDeliveryOrder(DLListNo, request.POType, true);
            }else if (UpdateIsSubconNonPO.Count() > 0)
            {
                DLListNo = string.Join(",", UpdateIsSubconNonPO);
                await SetIsSubconInvoiceGarmentDeliveryOrder(DLListNo, request.POType, true);
            }

            await _subconInvoicePackingListRepository.Update(subconInvoicePackingList);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Create Invoice Packing List  - " + subconInvoicePackingList.InvoiceNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);


            _storage.Save();

            return subconInvoicePackingList;
        }
        private string GenerateNo(PlaceGarmentSubconInvoicePackingListCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.BCType == "BC 2.6.1" ? "M" : "K";
            //var type = request.ContractType == "SUBCON BAHAN BAKU" ? "BB" : request.ContractType == "SUBCON CUTTING" ? "CT" : request.ContractType == "SUBCON GARMENT" ? "SG" : "JS";

            var prefix = $"IV{code}{year}{month}";

            var lastNo = _subconInvoicePackingListRepository.Query.Where(w => w.InvoiceNo.StartsWith(prefix))
                .OrderByDescending(o => o.InvoiceNo)
                .Select(s => int.Parse(s.InvoiceNo.Substring(9, 3)))
                .FirstOrDefault();
            var no = $"{prefix}{(lastNo + 1).ToString("D5")}";

            return no;
        }


        public async Task<string> SetIsSubconInvoiceGarmentDeliveryOrder(string DLNo,string POType, bool IsSubconInvoice)
        {
            var garmentPurchasingtUri = PurchasingDataSettings.Endpoint ;
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
