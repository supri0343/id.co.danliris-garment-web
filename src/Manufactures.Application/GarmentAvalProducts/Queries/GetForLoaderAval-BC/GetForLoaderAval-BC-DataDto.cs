using System;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentAvalProducts.Queries.GetForLoaderAval_BC
{
    public class GetForLoaderAval_BCDto
    {
        public GetForLoaderAval_BCDto()
        {

        }
        public Guid preparingId { get;  set; }
        public Guid preparingItemId { get;  set; }
        public Product Product { get; set; }
        //public int ProductId { get; set; }
        //public string ProductCode { get; set; }
        //public string ProductName { get; set; }

        public string DesignColor { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public Uom Uom { get; set; }
        public string bcno { get; set; }
        public DateTime? bcdate { get; set; }
        public string poSerialNumber { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        public string bctype { get;  set; }
        public string article { get; set; }
        //public int UomId { get; set; }
        //public string UomUnit { get; set; }
        //public Guid preparingId { get; set; }

        public GetForLoaderAval_BCDto(GetForLoaderAval_BCDto getForLoaderAval_BCDto)
        {
            preparingId = getForLoaderAval_BCDto.preparingId;
            preparingItemId = getForLoaderAval_BCDto.preparingItemId;
            //Product = new Product(getForLoaderAval_BCDto.ProductId, getForLoaderAval_BCDto.ProductName, getForLoaderAval_BCDto.ProductCode);
            DesignColor = getForLoaderAval_BCDto.DesignColor;
            RemainingQuantity = getForLoaderAval_BCDto.RemainingQuantity;
            BasicPrice = getForLoaderAval_BCDto.BasicPrice;
            bcno = getForLoaderAval_BCDto.bcno;
            bcdate = getForLoaderAval_BCDto.bcdate;
            poSerialNumber = getForLoaderAval_BCDto.poSerialNumber;
            ProcessDate = getForLoaderAval_BCDto.ProcessDate;
            bctype = getForLoaderAval_BCDto.bctype;
            article = getForLoaderAval_BCDto.article;
            //Uom = new Uom(getForLoaderAval_BCDto.UomId, getForLoaderAval_BCDtok.UomUnit);
        }
    }
}
