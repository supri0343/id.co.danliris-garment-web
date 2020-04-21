using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries
{
	public class GarmentMonitoringProductionStockFlowDto
	{
		public GarmentMonitoringProductionStockFlowDto()
		{
		}

		public string Ro { get; internal set; }
		public string BuyerCode { get; internal set; }
		public string Article { get; internal set; }
		public string Comodity { get; internal set; }
		public double QtyOrder { get; internal set; }
		public double BeginingBalanceCuttingQty { get; internal set; }
		public double QtyCuttingIn { get; internal set; }
		public double QtyCuttingOut { get; internal set; }
		public double QtyCuttingTransfer { get; internal set; }
		public double QtyCuttingsubkon { get; internal set; }
		public double AvalCutting { get; internal set; }
		public double AvalSewing { get; internal set; }
		public double EndBalancCuttingeQty { get; internal set; }
		public double BeginingBalanceLoadingQty { get; internal set; }
		public double QtyLoading { get; internal set; }
		public double QtyLoadingTransfer { get; internal set; }
		public double QtyLoadingAdjs { get; internal set; }
		public double EndBalanceLoadingQty { get; internal set; }
		public GarmentMonitoringProductionStockFlowDto(GarmentMonitoringProductionStockFlowDto flowDto)
		{
			 
			this.Ro = flowDto.Ro;
			this.BuyerCode = flowDto.BuyerCode;
			this.Article = flowDto.Article;
			this.Comodity = flowDto.Comodity;
			this.QtyOrder = flowDto.QtyOrder;
			this.BeginingBalanceCuttingQty = flowDto.BeginingBalanceCuttingQty;
			this.QtyCuttingIn = flowDto.QtyCuttingIn;
			this.QtyCuttingOut = flowDto.QtyCuttingOut;
			this.QtyCuttingTransfer = flowDto.QtyCuttingTransfer;
			this.QtyCuttingsubkon = flowDto.QtyCuttingsubkon;
			this.AvalCutting = flowDto.AvalCutting;
			this.AvalSewing = flowDto.AvalSewing;
			this.EndBalancCuttingeQty = flowDto.EndBalancCuttingeQty;
			this.BeginingBalanceLoadingQty = flowDto.BeginingBalanceLoadingQty;
			this.QtyLoading = flowDto.QtyLoading;
			this.QtyLoadingAdjs = flowDto.QtyLoadingAdjs;
			this.QtyLoadingTransfer = flowDto.QtyLoadingTransfer;
			this.EndBalanceLoadingQty = flowDto.EndBalanceLoadingQty;
		}
	}
}
