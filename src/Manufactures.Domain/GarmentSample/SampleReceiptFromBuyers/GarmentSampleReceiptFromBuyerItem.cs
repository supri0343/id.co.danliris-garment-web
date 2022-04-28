using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers
{
	public class GarmentSampleReceiptFromBuyerItem : AggregateRoot<GarmentSampleReceiptFromBuyerItem, GarmentSampleReceiptFromBuyerItemReadModel>
	{
		public Guid ReceiptId { get;   set; }
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

		public GarmentSampleReceiptFromBuyerItem(Guid identity,Guid receiptId, string invoiceNo, int buyerAgentId, string buyerAgentCode, string buyerAgentName,  string rONo, string article, string description, string style,int comodityId, string comodityCode,string comodityName, string colour, int sizeId, string sizeName, double receiptQuantity) : base(identity)
		{


			//MarkTransient();
			ReceiptId = receiptId;
			InvoiceNo = invoiceNo;
			Identity = identity;
			BuyerAgentId = buyerAgentId;
			BuyerAgentCode = buyerAgentCode;
			BuyerAgentName = buyerAgentName;
			RONo = rONo;
			Article = article;
			Description = description;
			Style = style;
			ComodityId = comodityId;
			ComodityCode = comodityCode;
			ComodityName = comodityName;
			Colour = colour;
			SizeId = sizeId;
			SizeName = sizeName;
			ReceiptQuantity = receiptQuantity;


			ReadModel = new GarmentSampleReceiptFromBuyerItemReadModel(Identity)
			{
				InvoiceNo = invoiceNo,
				ReceiptId = receiptId,
				BuyerAgentId = buyerAgentId,
				BuyerAgentCode = buyerAgentCode,
				BuyerAgentName = buyerAgentName,
				RONo = rONo,
				Article = article,
				Description = description,
				Style = style,
				ComodityId = comodityId,
				ComodityCode = comodityCode,
				ComodityName = comodityName,
				Colour = colour,
				SizeId = sizeId,
				SizeName = sizeName,
				ReceiptQuantity = receiptQuantity
			};
			ReadModel.AddDomainEvent(new OnGarmentSampleReceiptFromBuyerPlaced(Identity));
		}
		public GarmentSampleReceiptFromBuyerItem(GarmentSampleReceiptFromBuyerItemReadModel readModel) : base(readModel)
		{
			InvoiceNo = readModel.InvoiceNo;
			Identity = readModel.Identity;
			BuyerAgentId = readModel.BuyerAgentId;
			BuyerAgentCode = readModel.BuyerAgentCode;
			BuyerAgentName = readModel.BuyerAgentName;
			RONo = readModel.RONo;
			Article = readModel.Article;
			Description = readModel.Description;
			Style = readModel.Style;
			ComodityId = readModel.ComodityId;
			ComodityCode = readModel.ComodityCode;
			ComodityName = readModel.ComodityName;
			Colour = readModel.Colour;
			SizeId = readModel.SizeId;
			SizeName = readModel.SizeName;
			ReceiptQuantity = readModel.ReceiptQuantity;
		}
		public void SetReceiptQuantity(double ReceiptQuantity)
		{
			if (this.ReceiptQuantity != ReceiptQuantity)
			{
				this.ReceiptQuantity = ReceiptQuantity;
				ReadModel.ReceiptQuantity = ReceiptQuantity;
			}
		}
		public void SetDeleted()
		{
			MarkModified();
		}
		protected override GarmentSampleReceiptFromBuyerItem GetEntity()
		{
			return this;
		}
	}
	 
}
