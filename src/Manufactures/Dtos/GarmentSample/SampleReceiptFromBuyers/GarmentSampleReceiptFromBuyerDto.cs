using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleReceiptFromBuyers
{
	public class GarmentSampleReceiptFromBuyerDto : BaseDto
	{
		public GarmentSampleReceiptFromBuyerDto(GarmentSampleReceiptFromBuyer receiptFromBuyer)
		{
			Id = receiptFromBuyer.Identity;
			SaveAs = receiptFromBuyer.SaveAs;
			ReceiptDate = receiptFromBuyer.ReceiptDate;
			ReceiptNo = receiptFromBuyer.ReceiptNo;
			Items = new List<GarmentSampleReceiptFromBuyerItemDto>();
		}

		public Guid Id { get; internal set; }
		public string SaveAs { get; internal set; }
		public DateTimeOffset ReceiptDate { get; internal set; }
		public string ReceiptNo { get; internal set; }
		public virtual List<GarmentSampleReceiptFromBuyerItemDto> Items { get; internal set; }
	}
}