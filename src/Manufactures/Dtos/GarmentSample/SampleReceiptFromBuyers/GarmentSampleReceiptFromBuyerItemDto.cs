using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleReceiptFromBuyers
{
	public class GarmentSampleReceiptFromBuyerItemDto : BaseDto
	{
		public GarmentSampleReceiptFromBuyerItemDto(GarmentSampleReceiptFromBuyerItem receiptFromBuyer)
		{
			Id = receiptFromBuyer.Identity;
			ReceiptId = receiptFromBuyer.ReceiptId;
			InvoiceNo = receiptFromBuyer.InvoiceNo;
			BuyerAgentId = receiptFromBuyer.BuyerAgentId;
			BuyerAgentCode = receiptFromBuyer.BuyerAgentCode;
			BuyerAgentName = receiptFromBuyer.BuyerAgentName;
			RONo = receiptFromBuyer.RONo;
			Article = receiptFromBuyer.Article;
			Description = receiptFromBuyer.Description;
			Style = receiptFromBuyer.Style;
			ComodityId = receiptFromBuyer.ComodityId;
			ComodityCode = receiptFromBuyer.ComodityCode;
			ComodityName = receiptFromBuyer.ComodityName;
			Colour = receiptFromBuyer.Colour;
			SizeId = receiptFromBuyer.SizeId;
			SizeName = receiptFromBuyer.SizeName;
			ReceiptQuantity = receiptFromBuyer.ReceiptQuantity;

		}
		public Guid Id { get; set; }
		public Guid ReceiptId { get;  set; }
		public string InvoiceNo { get;  set; }
		public int BuyerAgentId { get;  set; }
		public string BuyerAgentCode { get;  set; }
		public string BuyerAgentName { get;  set; }
		public string RONo { get;  set; }
		public string Article { get;  set; }
		public string Description { get;  set; }
		public string Style { get;  set; }
		public int ComodityId { get;  set; }
		public string ComodityCode { get;  set; }
		public string ComodityName { get;  set; }
		public string Colour { get;  set; }
		public int SizeId { get;  set; }
		public string SizeName { get;  set; }
		public double ReceiptQuantity { get;  set; }
	}
}