using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.ExpenditureGoodReturn
{
    public class GarmentSubconExpenditureGoodReturnDto : BaseDto
    {
        public GarmentSubconExpenditureGoodReturnDto(GarmentSubconExpenditureGoodReturn garmentExpenditureGoodReturn)
        {
            Id = garmentExpenditureGoodReturn.Identity;
            ReturNo = garmentExpenditureGoodReturn.ReturNo;
            PackingOutNo = garmentExpenditureGoodReturn.PackingOutNo;
            DONo = garmentExpenditureGoodReturn.DONo;
            URNNo = garmentExpenditureGoodReturn.URNNo;
            BCNo = garmentExpenditureGoodReturn.BCNo;
            BCType = garmentExpenditureGoodReturn.BCType;
            RONo = garmentExpenditureGoodReturn.RONo;
            Article = garmentExpenditureGoodReturn.Article;
            Unit = new UnitDepartment(garmentExpenditureGoodReturn.UnitId.Value, garmentExpenditureGoodReturn.UnitCode, garmentExpenditureGoodReturn.UnitName);
            ReturDate = garmentExpenditureGoodReturn.ReturDate;
            ReturType = garmentExpenditureGoodReturn.ReturType;
            Comodity = new GarmentComodity(garmentExpenditureGoodReturn.ComodityId.Value, garmentExpenditureGoodReturn.ComodityCode, garmentExpenditureGoodReturn.ComodityName);
            Buyer = new Buyer(garmentExpenditureGoodReturn.BuyerId.Value, garmentExpenditureGoodReturn.BuyerCode, garmentExpenditureGoodReturn.BuyerName);
            Invoice = garmentExpenditureGoodReturn.Invoice;
            ReturDesc = garmentExpenditureGoodReturn.ReturDesc;
            Items = new List<GarmentSubconExpenditureGoodReturnItemDto>();
        }
        public Guid Id { get; internal set; }
        public string ReturNo { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string ReturType { get; internal set; }
        public string PackingOutNo { get; internal set; }
        public string DONo { get; internal set; }
        public string URNNo { get; internal set; }
        public string BCNo { get; internal set; }
        public string BCType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public Buyer Buyer { get; internal set; }
        public DateTimeOffset ReturDate { get; internal set; }
        public string Invoice { get; internal set; }
        public string ReturDesc { get; internal set; }
        public virtual List<GarmentSubconExpenditureGoodReturnItemDto> Items { get; internal set; }
    }
}
