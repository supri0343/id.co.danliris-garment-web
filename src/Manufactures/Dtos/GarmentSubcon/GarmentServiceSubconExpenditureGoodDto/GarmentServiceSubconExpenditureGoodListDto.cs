using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconExpenditureGoodDto
{
    public class GarmentServiceSubconExpenditureGoodListDto : BaseDto
    {
        public GarmentServiceSubconExpenditureGoodListDto(GarmentServiceSubconExpenditureGood serviceSubconExpenditureGood  )
        {
            Id = serviceSubconExpenditureGood.Identity;
            ServiceSubconExpenditureGoodNo = serviceSubconExpenditureGood.ServiceSubconExpenditureGoodNo;
            // Unit = new UnitDepartment(serviceSubconExpenditureGood.UnitId.Value, serviceSubconExpenditureGood.UnitCode, serviceSubconExpenditureGood.UnitName);
            ServiceSubconExpenditureGoodDate = serviceSubconExpenditureGood.ServiceSubconExpenditureGoodDate;
            CreatedBy = serviceSubconExpenditureGood.AuditTrail.CreatedBy;
            Buyer = new Buyer(serviceSubconExpenditureGood.BuyerId.Value, serviceSubconExpenditureGood.BuyerCode, serviceSubconExpenditureGood.BuyerName);
            IsUsed = serviceSubconExpenditureGood.IsUsed;
            NettWeight = serviceSubconExpenditureGood.NettWeight;
            GrossWeight = serviceSubconExpenditureGood.GrossWeight;
            UomUnit = serviceSubconExpenditureGood.UomUnit;
            QtyPacking = serviceSubconExpenditureGood.QtyPacking;
            Items = new List<GarmentServiceSubconExpenditureGoodItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSubconExpenditureGoodNo { get; set; }
        public UnitDepartment Unit { get; set; }
        //public string SewingTo { get; set; }
        public DateTimeOffset ServiceSubconExpenditureGoodDate { get; set; }
        public double TotalQuantity { get; set; }
        //public double TotalRemainingQuantity { get; set; }
        public bool IsUsed { get; set; }
        public double NettWeight { get;  set; }
        public double GrossWeight { get;  set; }
        public Buyer Buyer { get; set; }
        public string UomUnit { get;  set; }
        public double QtyPacking { get; set; }
        public List<GarmentServiceSubconExpenditureGoodItemDto> Items { get; set; }
    }
}
