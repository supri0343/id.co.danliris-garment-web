using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingIns.Repositories
{
    public class GarmentSubconSewingInRepository : AggregateRepostory<GarmentSubconSewingIn, GarmentSubconSewingInReadModel>, IGarmentSubconSewingInRepository
    {
        public IQueryable<GarmentSubconSewingInReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconSewingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SewingInNo",
                "Article",
                "RONo",
                "UnitCode",
                "UnitFromCode"
            };

            data = QueryHelper<GarmentSubconSewingInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconSewingInReadModel>.Order(data, OrderDictionary);

            return data;
        }

        public IQueryable<GarmentSubconSewingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;
            var buyerCode = string.Empty;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);

            if (FilterDictionary.ContainsKey("BuyerCode"))
            {
                buyerCode = FilterDictionary.FirstOrDefault(k => k.Key == "BuyerCode").Value.ToString();
                FilterDictionary.Remove("BuyerCode");
            }

            data = QueryHelper<GarmentSubconSewingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };

            data = QueryHelper<GarmentSubconSewingInReadModel>.Search(data, SearchAttributes, keyword);
            
            if (!string.IsNullOrEmpty(buyerCode))
            {
                var preparings = storageContext.Set<GarmentPreparingReadModel>();
                var roNo = preparings.Where(x => x.BuyerCode == buyerCode)
                    .Select(s => s.RONo).Distinct().ToList();

                data = data.Where(x => roNo.Contains(x.RONo));
            }

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconSewingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSubconSewingInReadModel> query)
        {
            var newQuery = query.Select(x => new {
                Id = x.Identity,
                SewingInNo = x.SewingInNo,
                SewingFrom = x.SewingFrom,
                LoadingOutId = x.LoadingOutId,
                LoadingOutNo = x.LoadingOutNo,
                RONo = x.RONo,
                Article = x.Article,
                Unit = new
                {
                    Id = x.UnitId,
                    Code = x.UnitCode,
                    Name = x.UnitName
                },
                UnitFrom = new
                {
                    Id = x.UnitFromId,
                    Code = x.UnitFromCode,
                    Name = x.UnitFromName
                },
                Comodity = new
                {
                    Id = x.ComodityId,
                    Code = x.ComodityCode,
                    Name = x.ComodityName
                },
                SewingInDate = x.SewingInDate,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastModifiedBy = x.ModifiedBy,
                LastModifiedDate = x.ModifiedDate,
                Items = x.GarmentSewingInItem.Select(y => new {
                    Id = y.Identity,
                    Product = new
                    {
                        Id = y.ProductId,
                        Code = y.ProductCode,
                        Name = y.ProductName
                    },
                    DesignColor = y.DesignColor,
                    Size = new
                    {
                        Id = y.SizeId,
                        Size = y.SizeName
                    },
                    Quantity = y.Quantity,
                    Uom = new
                    {
                        Id = y.UomId,
                        Unit = y.UomUnit
                    },
                    Color = y.Color,
                    RemainingQuantity = y.RemainingQuantity,
                    BasicPrice = y.BasicPrice,
                    SewingOutItemId = y.SewingOutItemId,
                    SewingOutDetailId = y.SewingOutDetailId,
                    LoadingOutItemId = y.LoadingOutItemId,
                    FinishingOutItemId = y.FinishingOutItemId,
                    FinishingOutDetailId = y.FinishingOutDetailId,
                    SewingInId = y.SewingInId,
                    Price = y.Price,
                    CreatedBy = y.CreatedBy,
                    CreatedDate = y.CreatedDate,
                    LastModifiedBy = y.ModifiedBy,
                    LastModifiedDate = y.ModifiedDate,
                })
            });

            return newQuery;
        }
        protected override GarmentSubconSewingIn Map(GarmentSubconSewingInReadModel readModel)
        {
            return new GarmentSubconSewingIn(readModel);
        }

    }
}