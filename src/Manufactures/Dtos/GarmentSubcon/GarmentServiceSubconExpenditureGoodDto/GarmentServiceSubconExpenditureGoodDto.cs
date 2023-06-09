using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconExpenditureGoodDto
{
    public class GarmentServiceSubconExpenditureGoodDto
    {
        public GarmentServiceSubconExpenditureGoodDto(GarmentServiceSubconExpenditureGood serviceSubconExpenditureGood)
        {
            Id = serviceSubconExpenditureGood.Identity;
            ServiceSubconExpenditureGoodNo = serviceSubconExpenditureGood.ServiceSubconExpenditureGoodNo;
            //
            ServiceSubconExpenditureGoodDate = serviceSubconExpenditureGood.ServiceSubconExpenditureGoodDate;
            IsUsed = serviceSubconExpenditureGood.IsUsed;
            Buyer = new Buyer(serviceSubconExpenditureGood.BuyerId.Value, serviceSubconExpenditureGood.BuyerCode, serviceSubconExpenditureGood.BuyerName);
            QtyPacking = serviceSubconExpenditureGood.QtyPacking;
            UomUnit = serviceSubconExpenditureGood.UomUnit;
            NettWeight = serviceSubconExpenditureGood.NettWeight;
            GrossWeight = serviceSubconExpenditureGood.GrossWeight;
            Items = new List<GarmentServiceSubconExpenditureGoodItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSubconExpenditureGoodNo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset ServiceSubconExpenditureGoodDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public virtual List<GarmentServiceSubconExpenditureGoodItemDto> Items { get; internal set; }
    }
}
