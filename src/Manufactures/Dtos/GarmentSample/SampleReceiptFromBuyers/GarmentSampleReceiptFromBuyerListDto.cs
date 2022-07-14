using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleReceiptFromBuyers
{
	public class GarmentSampleReceiptFromBuyerListDto : BaseDto
	{
		public GarmentSampleReceiptFromBuyerListDto(GarmentSampleReceiptFromBuyer receiptFromBuyer)
		{
			Id = receiptFromBuyer.Identity;
			SaveAs = receiptFromBuyer.SaveAs;
			ReceiptDate = receiptFromBuyer.ReceiptDate;
			ReceiptNo = receiptFromBuyer.ReceiptNo;
			
		}

		public Guid Id { get; internal set; }
		public string SaveAs { get; internal set; }
		public DateTimeOffset ReceiptDate { get; internal set; }
		public double TotalQuantity { get; internal set; }
		public string ReceiptNo { get; internal set; }
	}
}