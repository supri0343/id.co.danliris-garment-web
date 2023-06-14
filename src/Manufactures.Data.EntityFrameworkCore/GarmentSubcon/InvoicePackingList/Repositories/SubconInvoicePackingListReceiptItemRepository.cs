using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ISubconInvoicePackingListReceiptItemRepositories;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.SubconInvoicePackingListReceiptItemReadModels;
using Manufactures.Domain.GarmentSubcon.SubconInvoicePackingListReceiptItemModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.InvoicePackingList.ReceiptRepositories
{
    public class SubconInvoicePackingListReceiptItemRepository : AggregateRepostory<SubconInvoicePackingListReceiptItem, SubconInvoicePackingListReceiptItemReadModel>, ISubconInvoicePackingListReceiptItemRepository
    {
        protected override SubconInvoicePackingListReceiptItem Map(SubconInvoicePackingListReceiptItemReadModel readModel)
        {
            return new SubconInvoicePackingListReceiptItem(readModel);
        }
    }
}
