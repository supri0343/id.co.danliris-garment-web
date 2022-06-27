using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels
{
	public class GarmentSampleReceiptFromBuyerReadModel : ReadModelBase
	{
		public GarmentSampleReceiptFromBuyerReadModel(Guid identity) : base(identity)
		{
		}
		public string ReceiptNo { get; internal set; }
		public string SaveAs { get; internal set; }
		public DateTimeOffset ReceiptDate { get; internal set; }
		public virtual List<GarmentSampleReceiptFromBuyerItemReadModel> Items { get; internal set; }
	}
}
