using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconSewingListDto : BaseDto
    {
        public GarmentServiceSubconSewingListDto(GarmentServiceSubconSewing garmentServiceSubconSewingList)
        {
            Id = garmentServiceSubconSewingList.Identity;
            ServiceSubconSewingNo = garmentServiceSubconSewingList.ServiceSubconSewingNo;
            Unit = new UnitDepartment(garmentServiceSubconSewingList.UnitId.Value, garmentServiceSubconSewingList.UnitCode, garmentServiceSubconSewingList.UnitName);
            RONo = garmentServiceSubconSewingList.RONo;
            Article = garmentServiceSubconSewingList.Article;
            ServiceSubconSewingDate = garmentServiceSubconSewingList.ServiceSubconSewingDate;
            CreatedBy = garmentServiceSubconSewingList.AuditTrail.CreatedBy;
            Items = new List<GarmentServiceSubconSewingItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSubconSewingNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string SewingTo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset ServiceSubconSewingDate { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Products { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public List<GarmentServiceSubconSewingItemDto> Items { get; set; }
    }
}
