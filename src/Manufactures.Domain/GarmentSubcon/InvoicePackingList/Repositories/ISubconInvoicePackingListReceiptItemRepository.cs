using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.SubconInvoicePackingListReceiptItemReadModels;
using Manufactures.Domain.GarmentSubcon.SubconInvoicePackingListReceiptItemModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.ISubconInvoicePackingListReceiptItemRepositories
{
    public interface ISubconInvoicePackingListReceiptItemRepository : IAggregateRepository<SubconInvoicePackingListReceiptItem, SubconInvoicePackingListReceiptItemReadModel>
    {
    }
}
