using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.SubconInvoicePackingListReceiptItemModel;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubconReceipt
{
    public class GarmentSubconInvoicePackingListReceiptItemDto : BaseDto
    {
        public Guid Id { get; set; }
        public Guid InvoicePackingListId { get;  set; }
        public string DLNo { get;  set; }
        public DateTimeOffset DLDate { get;  set; }
        public Product Product { get;  set; }
        public double Quantity { get;  set; }
        public Uom Uom { get;  set; }
        public double TotalPrice { get; private set; }
        public double PricePerDealUnit { get; private set; }
        public GarmentSubconInvoicePackingListReceiptItemDto(SubconInvoicePackingListReceiptItem subconInvoicePackingListItem)
        {
            Id = subconInvoicePackingListItem.Identity;
            InvoicePackingListId = subconInvoicePackingListItem.InvoicePackingListId;
            DLNo = subconInvoicePackingListItem.DLNo;
            DLDate = subconInvoicePackingListItem.DLDate;
            Product = new Product(subconInvoicePackingListItem.ProductId.Value, subconInvoicePackingListItem.ProductCode, subconInvoicePackingListItem.ProductName, subconInvoicePackingListItem.ProductRemark);
            //ProductRemark = subconInvoicePackingListItem.ProductRemark;
            //DesignColor = subconInvoicePackingListItem.DesignColor;
            Quantity = subconInvoicePackingListItem.Quantity;
            Uom = new Uom(subconInvoicePackingListItem.UomId.Value, subconInvoicePackingListItem.UomUnit);
            //CIF = subconInvoicePackingListItem.CIF;
            TotalPrice = subconInvoicePackingListItem.TotalPrice;
            PricePerDealUnit = subconInvoicePackingListItem.PricePerDealUnit;
            //TotalNW = subconInvoicePackingListItem.TotalNW;
            //TotalGW = subconInvoicePackingListItem.TotalGW;

        }

    }
}
