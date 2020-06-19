using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries
{
	public class GarmentMonitoringExpenditureGoodDto
	{
		public GarmentMonitoringExpenditureGoodDto()
		{
		}

		public string expenditureGoodNo { get; internal set; }
		public string expenditureGoodType { get; internal set; }
		public DateTimeOffset  ? expenditureDate { get; internal set; }
		public string roNo { get; internal set; }
		public string buyerArticle { get; internal set; }
		public string buyerCode { get; internal set; }
		public string colour { get; internal set; }
		public string name { get; internal set; }
		public double qty { get; internal set; }
		public string invoice { get; internal set; }
		public decimal price { get; internal set; }

		public GarmentMonitoringExpenditureGoodDto(GarmentMonitoringExpenditureGoodDto garmentMonitoring)
		{

			this.expenditureGoodNo = garmentMonitoring.expenditureGoodNo;
			this.expenditureGoodType = garmentMonitoring.expenditureGoodType;
			this.expenditureDate = garmentMonitoring.expenditureDate;
			this.roNo = garmentMonitoring.roNo;
			this.buyerArticle = garmentMonitoring.buyerArticle;
			this.colour = garmentMonitoring.colour;
			this.name = garmentMonitoring.name;
			this.qty = garmentMonitoring.qty;
			this.invoice = garmentMonitoring.invoice;
		}
	}
}
