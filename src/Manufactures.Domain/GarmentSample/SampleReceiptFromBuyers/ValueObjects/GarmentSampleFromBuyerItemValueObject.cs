

using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ValueObjects
{
	public class GarmentSampleFromBuyerItemValueObject : ValueObject
	{ 
			public Guid Id { get; set; }
			public Guid ReceiptId { get;  set; }
			public string InvoiceNo { get;   set; }
			public int BuyerAgentId { get;   set; }
			public string BuyerAgentCode { get;   set; }
			public string BuyerAgentName { get;   set; }
			public string RONo { get;   set; }
			public string Article { get;   set; }
			public string Description { get;   set; }
			public string Style { get;   set; }
			public int ComodityId { get;   set; }
			public string ComodityCode { get;   set; }
			public string ComodityName { get;   set; }
			public string Colour { get;   set; }
			public int SizeId { get;   set; }
			public string SizeName { get;   set; }
			public double ReceiptQuantity { get;   set; }



		protected override IEnumerable<object> GetAtomicValues()
		{
			throw new NotImplementedException();
		}
	}
}
