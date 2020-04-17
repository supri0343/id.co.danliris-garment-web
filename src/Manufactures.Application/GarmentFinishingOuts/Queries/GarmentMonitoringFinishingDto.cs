using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentFinishingOuts.Queries
{
	public class GarmentMonitoringFinishingDto
	{ 
	public GarmentMonitoringFinishingDto()
	{
	}

	public string roJob { get; internal set; }
	public string article { get; internal set; }
	public string buyerCode { get; internal set; }
	public double qtyOrder { get; internal set; }
	public double stock { get; internal set; }
	public double sewingOutQtyPcs { get; internal set; }
	public double finishingOutQtyPcs { get; internal set; }
	public string uomUnit { get; internal set; }
	public string style { get; internal set; }
	public double remainQty { get; internal set; }
	public decimal price { get; internal set; }
	public GarmentMonitoringFinishingDto(GarmentMonitoringFinishingDto garmentMonitoring)
	{

		this.roJob = garmentMonitoring.roJob;
		this.article = garmentMonitoring.article;
		this.qtyOrder = garmentMonitoring.qtyOrder;
		this.stock = garmentMonitoring.stock;
		this.sewingOutQtyPcs = garmentMonitoring.sewingOutQtyPcs;
		this.finishingOutQtyPcs = garmentMonitoring.finishingOutQtyPcs;
		this.uomUnit = garmentMonitoring.uomUnit;
		this.remainQty = garmentMonitoring.remainQty;
		this.style = garmentMonitoring.style;
		this.price = garmentMonitoring.price;
		this.buyerCode = garmentMonitoring.buyerCode;

	}
}
}
