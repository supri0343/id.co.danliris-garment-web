using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSample.GarmentServiceSampleExpenditureGoodItemDtos;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleExpenditureGoodDtoos
{
    public class GarmentServiceSampleExpenditureGoodDto
    {
        public GarmentServiceSampleExpenditureGoodDto(GarmentServiceSampleExpenditureGood serviceSampleExpenditureGood)
        {
            Id = serviceSampleExpenditureGood.Identity;
            ServiceSampleExpenditureGoodNo = serviceSampleExpenditureGood.ServiceSampleExpenditureGoodNo;
            //
            ServiceSampleExpenditureGoodDate = serviceSampleExpenditureGood.ServiceSampleExpenditureGoodDate;
            IsUsed = serviceSampleExpenditureGood.IsUsed;
            Buyer = new Buyer(serviceSampleExpenditureGood.BuyerId.Value, serviceSampleExpenditureGood.BuyerCode, serviceSampleExpenditureGood.BuyerName);
            QtyPacking = serviceSampleExpenditureGood.QtyPacking;
            UomUnit = serviceSampleExpenditureGood.UomUnit;
            NettWeight = serviceSampleExpenditureGood.NettWeight;
            GrossWeight = serviceSampleExpenditureGood.GrossWeight;
            Items = new List<GarmentServiceSampleExpenditureGoodItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSampleExpenditureGoodNo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset ServiceSampleExpenditureGoodDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public virtual List<GarmentServiceSampleExpenditureGoodItemDto> Items { get; internal set; }
    }
}
