using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentPackingIns.Repositories
{
    public class GarmentSubconPackingInsRepository : AggregateRepostory<GarmentSubconPackingIn, GarmentSubconPackingInReadModel>, IGarmentSubconPackingInRepository
    {
        public IQueryable<GarmentSubconPackingInReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconPackingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                //"FinishingOutNo",
                //"UnitCode",
                //"UnitToCode",
                "RONo",
                //"Article",
                //"garmentPackingInItem.ProductCode",
                //"garmentPackingInItem.Color",
                //"FinishingTo"
            };

            data = QueryHelper<GarmentSubconPackingInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconPackingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSubconPackingInReadModel> query)
        {
            var newQuery = query.Select(garmentPackingInList => new
            {
                Id = garmentPackingInList.Identity,
                PackingInNo = garmentPackingInList.PackingInNo,
                UnitFrom = new
                {
                    Id = garmentPackingInList.UnitFromId,
                    Code = garmentPackingInList.UnitFromCode,
                    Name = garmentPackingInList.UnitFromName
                },
                Unit = new
                {
                    Id = garmentPackingInList.UnitId,
                    Code = garmentPackingInList.UnitCode,
                    Name = garmentPackingInList.UnitName
                },
                RONo = garmentPackingInList.RONo,
                Article = garmentPackingInList.Article,
                PackingInDate = garmentPackingInList.PackingInDate,
                PackingFrom = garmentPackingInList.PackingFrom,
                Comodity = new
                {
                    Id = garmentPackingInList.ComodityId,
                    Code = garmentPackingInList.ComodityCode,
                    Name = garmentPackingInList.ComodityName
                },

                Items = garmentPackingInList.GarmentSubconPackingInItem.Select(garmentPackingInItem => new {
                    Id = garmentPackingInItem.Identity,
                    PackingInId = garmentPackingInItem.PackingInId,
                    CuttingOutItemId = garmentPackingInItem.CuttingOutItemId,
                    CuttingOutDetailId = garmentPackingInItem.CuttingOutDetailId,
                    SewingOutItemId = garmentPackingInItem.SewingOutItemId,
                    SewingOutDetailId = garmentPackingInItem.SewingOutDetailId,
                    FinishingOutItemId = garmentPackingInItem.FinishingOutItemId,
                    FinishingOutDetailId = garmentPackingInItem.FinishingOutDetailId,
                    Product = new
                    {
                        Id = garmentPackingInItem.ProductId,
                        Code = garmentPackingInItem.ProductCode,
                        Name = garmentPackingInItem.ProductName
                    },
                    Size = new
                    {
                        Id = garmentPackingInItem.SizeId,
                        Size = garmentPackingInItem.SizeName,
                    },
                    DesignColor = garmentPackingInItem.DesignColor,
                    Quantity = garmentPackingInItem.Quantity,
                    Uom = new {
                        Id = garmentPackingInItem.UomId,
                        Unit = garmentPackingInItem.UomUnit
                    },
                    Color = garmentPackingInItem.Color,
                    RemainingQuantity = garmentPackingInItem.RemainingQuantity,
                    BasicPrice = garmentPackingInItem.BasicPrice,
                    Price = garmentPackingInItem.Price,
                })

            });
            return newQuery;
        }

        protected override GarmentSubconPackingIn Map(GarmentSubconPackingInReadModel readModel)
        {
            return new GarmentSubconPackingIn(readModel);
        }
    }
}