using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSample.GarmentServiceSampleExpenditureGoodItemDtos;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleExpenditureGoodListDto
{
    public class GarmentServiceSampleExpenditureGoodListDto : BaseDto
    {
        public GarmentServiceSampleExpenditureGoodListDto(GarmentServiceSampleExpenditureGood serviceSampleExpenditureGood  )
        {
            Id = serviceSampleExpenditureGood.Identity;
            ServiceSubconExpenditureGoodNo = serviceSampleExpenditureGood.ServiceSampleExpenditureGoodNo;
            // Unit = new UnitDepartment(serviceSampleExpenditureGood.UnitId.Value, serviceSampleExpenditureGood.UnitCode, serviceSampleExpenditureGood.UnitName);
            ServiceSubconExpenditureGoodDate = serviceSampleExpenditureGood.ServiceSampleExpenditureGoodDate;
            CreatedBy = serviceSampleExpenditureGood.AuditTrail.CreatedBy;
            Buyer = new Buyer(serviceSampleExpenditureGood.BuyerId.Value, serviceSampleExpenditureGood.BuyerCode, serviceSampleExpenditureGood.BuyerName);
            IsUsed = serviceSampleExpenditureGood.IsUsed;
            NettWeight = serviceSampleExpenditureGood.NettWeight;
            GrossWeight = serviceSampleExpenditureGood.GrossWeight;
            UomUnit = serviceSampleExpenditureGood.UomUnit;
            QtyPacking = serviceSampleExpenditureGood.QtyPacking;
            Items = new List<GarmentServiceSampleExpenditureGoodItemDto>();
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
        public List<GarmentServiceSampleExpenditureGoodItemDto> Items { get; set; }
    }
}
