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
		public double QtyLoadingIn { get; internal set; }
		public double QtyLoading { get; internal set; }
		public double QtyLoadingAdjs { get; internal set; }
		public double EndBalanceLoadingQty { get; internal set; }
		public double BeginingBalanceSewingQty { get; internal set; }
		public double QtySewingIn { get; internal set; }
		public double QtySewingOut { get; internal set; }
		public double QtySewingInTransfer { get; internal set; }
		public double WipSewingOut { get; internal set; }
		public double WipFinishingOut { get; internal set; }
		public double QtySewingRetur { get; internal set; }
		public double QtySewingAdj { get; internal set; }
		public double EndBalanceSewingQty { get; internal set; }
		public double BeginingBalanceFinishingQty { get; internal set; }
		public double FinishingInQty { get; internal set; }
		public double BeginingBalanceSubconQty { get; internal set; }
		public double SubconInQty { get; internal set; }
		public double SubconOutQty { get; internal set; }
		public double EndBalanceSubconQty { get; internal set; }
		public double FinishingOutQty { get; internal set; }
		public double FinishingInTransferQty { get; internal set; }
		public double FinishingAdjQty { get; internal set; }
		public double FinishingReturQty { get; internal set; }
		public double EndBalanceFinishingQty { get; internal set; }
		public double BeginingBalanceExpenditureGood { get; internal set; }
		public double FinishingTransferExpenditure { get; internal set; }
		public double FinishingInExpenditure { get; internal set; }
		public double ExpenditureGoodRetur { get; internal set; }
		public double ExportQty { get; internal set; }
		public double OtherQty { get; internal set; }
		public double SampleQty { get; internal set; }
		public double ExpenditureGoodAdj { get; internal set; }
		public double EndBalanceExpenditureGood { get; internal set; }

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
			this.EndBalanceLoadingQty = flowDto.EndBalanceLoadingQty;
			this.BeginingBalanceSewingQty = flowDto.BeginingBalanceSewingQty;
			this.QtySewingIn = flowDto.QtySewingAdj;
			this.QtySewingOut = flowDto.QtySewingOut;
			this.QtySewingAdj = flowDto.QtySewingAdj;
			this.QtySewingInTransfer = flowDto.QtySewingInTransfer;
			this.WipSewingOut = flowDto.WipSewingOut;
			this.WipFinishingOut = flowDto.WipFinishingOut;
			this.QtySewingRetur = flowDto.QtySewingRetur;
			this.QtySewingAdj = flowDto.QtySewingAdj;
			this.EndBalanceSewingQty = flowDto.EndBalanceSewingQty;
			this.BeginingBalanceFinishingQty = flowDto.BeginingBalanceFinishingQty;
			this.FinishingInQty = flowDto.FinishingInQty;
			this.FinishingAdjQty = flowDto.FinishingAdjQty;
			this.FinishingInTransferQty = flowDto.FinishingInTransferQty;
			this.FinishingOutQty = flowDto.FinishingOutQty;
			this.FinishingReturQty = flowDto.FinishingReturQty;
			this.EndBalanceSubconQty = flowDto.EndBalanceSubconQty;
			this.EndBalanceFinishingQty = flowDto.EndBalanceFinishingQty;
			this.SubconInQty = flowDto.SubconInQty;
			this.SubconOutQty = flowDto.SubconOutQty;
			this.BeginingBalanceExpenditureGood = flowDto.BeginingBalanceExpenditureGood;
			this.ExpenditureGoodRetur = flowDto.ExpenditureGoodRetur;
			this.SampleQty = flowDto.SampleQty;
			this.ExportQty = flowDto.ExportQty;
			this.ExpenditureGoodAdj = flowDto.ExpenditureGoodAdj;
			this.EndBalanceExpenditureGood = flowDto.EndBalanceExpenditureGood;
			this.FinishingInExpenditure = flowDto.FinishingInExpenditure;
		}
		
	}
}
