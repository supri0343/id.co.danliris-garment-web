using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentLoadings.Queries
{
	public class GarmentMonitoringLoadingDto
	{
		public GarmentMonitoringLoadingDto()
		{
		}

		public Guid Id { get; internal set; }
		public string roJob { get; internal set; }
		public string article { get; internal set; }
		public double qtyOrder { get; internal set; }
		public double stock { get; internal set; }
		public double cuttingQtyPcs { get; internal set; }
		public double loadingQtyPcs { get; internal set; }
		public string uomUnit { get; internal set; }
		public string style { get; internal set; }
		public double remainQty { get; internal set; }
		public GarmentMonitoringLoadingDto(GarmentMonitoringLoadingDto garmentMonitoring)
		{
			Id = garmentMonitoring.Id;
			this.roJob = garmentMonitoring.roJob;
			this.article = garmentMonitoring.article;
			this.qtyOrder = garmentMonitoring.qtyOrder;
			this.stock = garmentMonitoring.stock;
			this.cuttingQtyPcs = garmentMonitoring.cuttingQtyPcs;
			this.loadingQtyPcs = garmentMonitoring.loadingQtyPcs;
			this.uomUnit = garmentMonitoring.uomUnit;
			this.remainQty = garmentMonitoring.remainQty;
			this.style = garmentMonitoring.style;
		}
	}
}
