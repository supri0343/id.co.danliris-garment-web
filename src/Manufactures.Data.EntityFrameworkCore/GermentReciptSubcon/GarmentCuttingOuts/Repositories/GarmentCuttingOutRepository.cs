using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingOuts.Repositories
{
    public class GarmentSubconCuttingOutRepository : AggregateRepostory<GarmentSubconCuttingOut, GarmentSubconCuttingOutReadModel>, IGarmentSubconCuttingOutRepository
    {
        public IQueryable<GarmentSubconCuttingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconCuttingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "CutOutNo",
                "UnitCode",
                "RONo",
                "Article",
            };

            data = QueryHelper<GarmentSubconCuttingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconCuttingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSubconCuttingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconCuttingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",  
            };

            data = QueryHelper<GarmentSubconCuttingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconCuttingOutReadModel>.Order(data, OrderDictionary);


            return data;
        }

        protected override GarmentSubconCuttingOut Map(GarmentSubconCuttingOutReadModel readModel)
        {
            return new GarmentSubconCuttingOut(readModel);
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSubconCuttingOutReadModel> query)
        {
            var newQuery = query.Select(x => new
            {
                Id = x.Identity,
                CutOutNo = x.CutOutNo,
                CuttingOutType = x.CuttingOutType,
                UnitFrom = new
                {
                    Id = x.UnitFromId,
                    Name = x.UnitFromName,
                    Code = x.UnitFromCode
                },
                CuttingOutDate = x.CuttingOutDate,
                RONo = x.RONo,
                Article = x.Article,
                Unit = new
                {
                    Id = x.UnitId,
                    Name = x.UnitName,
                    Code = x.UnitCode
                },
                Comodity = new
                {
                    Id = x.ComodityId,
                    Code = x.ComodityCode,
                    Name = x.ComodityName
                },

                Items = x.GarmentSubconCuttingOutItem.Select(garmentCuttingOutItem => new {
                    Id = garmentCuttingOutItem.Identity,
                    CutOutId = garmentCuttingOutItem.CutOutId,
                    CuttingInId = garmentCuttingOutItem.CuttingInId,
                    CuttingInDetailId = garmentCuttingOutItem.CuttingInDetailId,
                    Product = new
                    {
                        Id = garmentCuttingOutItem.ProductId,
                        Code = garmentCuttingOutItem.ProductCode,
                        Name = garmentCuttingOutItem.ProductName
                    },
                    DesignColor = garmentCuttingOutItem.DesignColor,
                    TotalCuttingOut = garmentCuttingOutItem.TotalCuttingOut,
                    RealQtyOut = garmentCuttingOutItem.RealQtyOut,
                    Details = garmentCuttingOutItem.GarmentSubconCuttingOutDetail.Select(garmentCuttingOutDetail => new {
                        Id = garmentCuttingOutDetail.Identity,
                        CutOutItemId = garmentCuttingOutDetail.CutOutItemId,
                        Size = new
                        {
                            Id = garmentCuttingOutDetail.SizeId,
                            Size = garmentCuttingOutDetail.SizeName
                        },
                        CuttingOutQuantity = garmentCuttingOutDetail.CuttingOutQuantity,
                        CuttingOutUom = new {
                            Id = garmentCuttingOutDetail.CuttingOutUomId,
                            Unit = garmentCuttingOutDetail.CuttingOutUomUnit
                        },
                        Color = garmentCuttingOutDetail.Color,
                        RealQtyOut = garmentCuttingOutDetail.RealQtyOut,
                        BasicPrice = garmentCuttingOutDetail.BasicPrice,
                        Price = garmentCuttingOutDetail.Price

                    })

                })

            });
            return newQuery;
        }
    }
}