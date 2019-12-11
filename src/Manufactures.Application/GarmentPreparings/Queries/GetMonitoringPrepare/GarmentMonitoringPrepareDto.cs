using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare
{
	public class GarmentMonitoringPrepareDto
	{
		public GarmentMonitoringPrepareDto()
		{
		}

		public Guid Id { get; internal set; }
		public string roJob { get; internal set; }
		public string article { get; internal set; }
		public string buyerCode { get; internal set; }
		public string productCode { get; internal set; }
		public string uomUnit { get; internal set; }
		public string roAsal { get; internal set; }
		public string remark { get; internal set; }
		public double stock { get; internal set; }
		public double receipt { get; internal set; }
		public double mainFabricExpenditure { get; internal set; }
		public double nonMainFabricExpenditure { get; internal set; }
		public double expenditure { get; internal set; }
		public double aval { get; internal set; }
		public double remainQty { get; internal set; }
		public GarmentMonitoringPrepareDto(GarmentMonitoringPrepareDto garmentMonitoringPrepareDto)
		{
			Id = garmentMonitoringPrepareDto.Id;
			this.roJob = garmentMonitoringPrepareDto.roJob;
			this.article = garmentMonitoringPrepareDto.article;
			this.buyerCode = garmentMonitoringPrepareDto.buyerCode;
			this.productCode = garmentMonitoringPrepareDto.productCode;
			this.uomUnit = garmentMonitoringPrepareDto.uomUnit;
			this.roAsal = garmentMonitoringPrepareDto.roAsal;
			this.remark = garmentMonitoringPrepareDto.remark;
			this.stock = garmentMonitoringPrepareDto.stock;
			this.receipt = garmentMonitoringPrepareDto.receipt;
			this.mainFabricExpenditure = garmentMonitoringPrepareDto.mainFabricExpenditure;
			this.nonMainFabricExpenditure = garmentMonitoringPrepareDto.nonMainFabricExpenditure;
			this.expenditure = garmentMonitoringPrepareDto.expenditure;
			this.aval = garmentMonitoringPrepareDto.aval;
			this.remainQty = garmentMonitoringPrepareDto.remainQty;
		}
	}
}
