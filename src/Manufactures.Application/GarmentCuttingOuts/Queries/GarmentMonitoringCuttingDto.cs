using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries
{
	public class GarmentMonitoringCuttingDto
	{
		public GarmentMonitoringCuttingDto()
		{
		}

		public Guid Id { get; internal set; }
		public string roJob { get; internal set; }
		public string article { get; internal set; }
		public string productCode { get; internal set; }
		public double qtyOrder { get; internal set; }
		public string style { get; internal set; }
		public double hours { get; internal set; }
		public double stock { get; internal set; }
		public double cuttingQtyMeter { get; internal set; }
		public double cuttingQtyPcs { get; internal set; }
		public double fc { get; internal set; }
		public double expenditure { get; internal set; }
		public double remainQty { get; internal set; }
		public GarmentMonitoringCuttingDto(GarmentMonitoringCuttingDto garmentMonitoringCuttingDto)
		{
			Id = garmentMonitoringCuttingDto.Id;
			this.roJob = garmentMonitoringCuttingDto.roJob;
			this.article = garmentMonitoringCuttingDto.article;
			this.qtyOrder = garmentMonitoringCuttingDto.qtyOrder;
			this.productCode = garmentMonitoringCuttingDto.productCode;
			this.style = garmentMonitoringCuttingDto.style;
			this.hours = garmentMonitoringCuttingDto.hours;
			this.cuttingQtyMeter = garmentMonitoringCuttingDto.cuttingQtyMeter;
			this.stock = garmentMonitoringCuttingDto.stock;
			this.cuttingQtyPcs = garmentMonitoringCuttingDto.cuttingQtyPcs;
			this.fc = garmentMonitoringCuttingDto.fc;
			this.expenditure = garmentMonitoringCuttingDto.expenditure;
			this.remainQty = garmentMonitoringCuttingDto.remainQty;
		}
	}
}
