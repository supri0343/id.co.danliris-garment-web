using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconExpenditureGoodItemDtos
{
    public class GarmentServiceSubconExpenditureGoodItemDto : BaseDto
    {
        public GarmentServiceSubconExpenditureGoodItemDto(GarmentServiceSubconExpenditureGoodItem serviceSubconExpenditureGoodItem)
        {
            Id = serviceSubconExpenditureGoodItem.Identity;
            ServiceSubconExpenditureGoodId = serviceSubconExpenditureGoodItem.ServiceSubconExpenditureGoodId;
            RONo = serviceSubconExpenditureGoodItem.RONo;
            Article = serviceSubconExpenditureGoodItem.Article;
            Comodity = new GarmentComodity(serviceSubconExpenditureGoodItem.ComodityId.Value, serviceSubconExpenditureGoodItem.ComodityCode, serviceSubconExpenditureGoodItem.ComodityName);
            //Buyer = new Buyer(serviceSubconExpenditureGoodItem.BuyerId.Value, serviceSubconExpenditureGoodItem.BuyerCode, serviceSubconExpenditureGoodItem.BuyerName);
            Unit = new UnitDepartment(serviceSubconExpenditureGoodItem.UnitId.Value, serviceSubconExpenditureGoodItem.UnitCode, serviceSubconExpenditureGoodItem.UnitName);
            UomUnit = serviceSubconExpenditureGoodItem.UomUnit;
            Quantity = serviceSubconExpenditureGoodItem.Quantity;
            BasicPrice = serviceSubconExpenditureGoodItem.BasicPrice;
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconExpenditureGoodId { get; set; }
        //public Buyer Buyer { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public UnitDepartment Unit { get; set; }
        public string UomUnit { get;  set; }
        public double Quantity { get;  set; }
        public double BasicPrice { get;  set; }
        //public virtual List<GarmentServiceSubconSewingDetailDto> Details { get; internal set; }
    }
}
