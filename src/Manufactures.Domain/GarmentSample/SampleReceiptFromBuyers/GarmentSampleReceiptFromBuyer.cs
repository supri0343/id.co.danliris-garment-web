using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels;
using Moonlay;
using System; 
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers
{
	public class GarmentSampleReceiptFromBuyer : AggregateRoot<GarmentSampleReceiptFromBuyer, GarmentSampleReceiptFromBuyerReadModel>
	{
		public string ReceiptNo { get;  set; }
		public string SaveAs { get; private set; }
		public DateTimeOffset ReceiptDate { get; private set; }
		 
		public GarmentSampleReceiptFromBuyer(Guid identity,string receiptNo, string saveAs, DateTimeOffset receiptDate) : base(identity)
		{

			//MarkTransient();
			ReceiptNo = receiptNo;
			SaveAs = saveAs;
			ReceiptDate = receiptDate;
			 

			ReadModel = new GarmentSampleReceiptFromBuyerReadModel(Identity)
			{
				ReceiptNo = receiptNo,
				SaveAs = saveAs,
				ReceiptDate = receiptDate 
			};

			ReadModel.AddDomainEvent(new OnGarmentSampleReceiptFromBuyerPlaced(Identity));
		}

		public GarmentSampleReceiptFromBuyer(GarmentSampleReceiptFromBuyerReadModel readModel) : base(readModel)
		{
			ReceiptNo = readModel.ReceiptNo;
			SaveAs = readModel.SaveAs;
			ReceiptDate = readModel.ReceiptDate;
		}

		 

		public void Modify()
		{
			MarkModified();
		}
		public void SetReceiptDate(DateTimeOffset ReceiptDate)
		{
			if (this.ReceiptDate != ReceiptDate)
			{
				this.ReceiptDate = ReceiptDate;
				ReadModel.ReceiptDate = ReceiptDate;
			}
		}
		protected override GarmentSampleReceiptFromBuyer GetEntity()
		{
			return this;
		}
	}
}
