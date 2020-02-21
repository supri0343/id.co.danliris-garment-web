using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentFinishedGoodStockDto : BaseDto
    {
        public GarmentFinishedGoodStockDto(GarmentFinishedGoodStock garmentFinishedGoodStock)
        {
            Id = garmentFinishedGoodStock.Identity;
            FinishedGoodStockNo = garmentFinishedGoodStock.FinishedGoodStockNo;
            RONo = garmentFinishedGoodStock.RONo;
            Article = garmentFinishedGoodStock.Article;
            Unit = new UnitDepartment(garmentFinishedGoodStock.UnitId.Value, garmentFinishedGoodStock.UnitCode, garmentFinishedGoodStock.UnitName);
            Comodity = new GarmentComodity(garmentFinishedGoodStock.ComodityId.Value, garmentFinishedGoodStock.ComodityCode, garmentFinishedGoodStock.ComodityName);
            Size = new SizeValueObject(garmentFinishedGoodStock.SizeId.Value, garmentFinishedGoodStock.SizeName);
            Quantity = garmentFinishedGoodStock.Quantity;
            Uom = new Uom(garmentFinishedGoodStock.UomId.Value, garmentFinishedGoodStock.UomUnit);
            BasicPrice = garmentFinishedGoodStock.BasicPrice;
            Price = garmentFinishedGoodStock.Price;
        }
        public Guid Id { get; internal set; }
        public string FinishedGoodStockNo { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public SizeValueObject Size { get; internal set; }
        public double Quantity { get; internal set; }
        public Uom Uom { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
    }
}
