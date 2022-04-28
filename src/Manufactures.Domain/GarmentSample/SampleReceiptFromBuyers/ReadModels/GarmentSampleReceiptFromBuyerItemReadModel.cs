using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels
{
	public class GarmentSampleReceiptFromBuyerItemReadModel : ReadModelBase
	{
		public GarmentSampleReceiptFromBuyerItemReadModel(Guid identity) : base(identity)
		{
		}
		public Guid ReceiptId { get; internal set; }
		public string InvoiceNo { get; internal set; }
		public int BuyerAgentId { get; internal set; }
		public string BuyerAgentCode { get; internal set; }
		public string BuyerAgentName { get; internal set; }
		public string RONo { get; internal set; }
		public string Article { get; internal set; }
		public string Description { get; internal set; }
		public string Style { get; internal set; }
		public int ComodityId { get; internal set; }
		public string ComodityCode { get; internal set; }
		public string ComodityName { get; internal set; }
		public string Colour { get; internal set; }
		public int SizeId { get; internal set; }
		public string SizeName { get; internal set; }
		public double ReceiptQuantity { get; internal set; }
		public virtual GarmentSampleReceiptFromBuyerReadModel GarmentSampleReceiptFromBuyer { get; internal set; }
	}
}