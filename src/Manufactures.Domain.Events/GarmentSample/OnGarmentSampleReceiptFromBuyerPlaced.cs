using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
	public class OnGarmentSampleReceiptFromBuyerPlaced : IGarmentSampleReceiptFromBuyerEvent
	{
		public OnGarmentSampleReceiptFromBuyerPlaced(Guid receiptId)
		{
			ReceiptId = receiptId;
		}
		public Guid ReceiptId { get; }
	}
}
