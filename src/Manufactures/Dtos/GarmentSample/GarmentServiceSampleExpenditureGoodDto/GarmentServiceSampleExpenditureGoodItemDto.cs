using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSample.GarmentServiceSampleExpenditureGoodItemDtos
{
    public class GarmentServiceSampleExpenditureGoodItemDto : BaseDto
    {
        public GarmentServiceSampleExpenditureGoodItemDto(GarmentServiceSampleExpenditureGoodItem serviceSampleExpenditureGoodItem)
        {
            Id = serviceSampleExpenditureGoodItem.Identity;
            ServiceSampleExpenditureGoodId = serviceSampleExpenditureGoodItem.ServiceSampleExpenditureGoodId;
            RONo = serviceSampleExpenditureGoodItem.RONo;
            Article = serviceSampleExpenditureGoodItem.Article;
            Comodity = new GarmentComodity(serviceSampleExpenditureGoodItem.ComodityId.Value, serviceSampleExpenditureGoodItem.ComodityCode, serviceSampleExpenditureGoodItem.ComodityName);
            //Buyer = new Buyer(serviceSampleExpenditureGoodItem.BuyerId.Value, serviceSampleExpenditureGoodItem.BuyerCode, serviceSampleExpenditureGoodItem.BuyerName);
            Unit = new UnitDepartment(serviceSampleExpenditureGoodItem.UnitId.Value, serviceSampleExpenditureGoodItem.UnitCode, serviceSampleExpenditureGoodItem.UnitName);
            UomUnit = serviceSampleExpenditureGoodItem.UomUnit;
            Quantity = serviceSampleExpenditureGoodItem.Quantity;
            BasicPrice = serviceSampleExpenditureGoodItem.BasicPrice;
        }

        public Guid Id { get; set; }
        public Guid ServiceSampleExpenditureGoodId { get; set; }
        //public Buyer Buyer { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public UnitDepartment Unit { get; set; }
        public string UomUnit { get;  set; }
        public double Quantity { get;  set; }
        public double BasicPrice { get;  set; }
        //public virtual List<GarmentServiceSampleSewingDetailDto> Details { get; internal set; }
    }
}
