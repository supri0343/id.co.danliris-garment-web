using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentPreparings.Repositories
{
    public class GarmentSubconPreparingRepository : AggregateRepostory<GarmentSubconPreparing, GarmentSubconPreparingReadModel>, IGarmentSubconPreparingRepository
    {
        public IQueryable<GarmentSubconPreparingReadModel> Read(string order, List<string> select, string filter)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconPreparingReadModel>.Filter(Query, FilterDictionary);

            return data;
        }
		protected override GarmentSubconPreparing Map(GarmentSubconPreparingReadModel readModel)
        {
            return new GarmentSubconPreparing(readModel);
        }

        public IQueryable<GarmentSubconPreparingReadModel> ReadOptimized(string order, string filter, string keyword)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconPreparingReadModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconPreparingReadModel>.Order(data, OrderDictionary);


            return data;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSubconPreparingReadModel> query, string keyword) {
            var newQuery = query.Select(x => new {
                Id = x.Identity,
                LastModifiedDate = x.ModifiedDate ?? x.CreatedDate,
                LastModifiedBy = x.ModifiedBy ?? x.CreatedBy,
                UENId = x.UENId,
                UENNo = x.UENNo,
                Unit = new { 
                    Id = x.UnitId, 
                    Name = x.UnitName,
                    Code = x.UnitCode 
                },
                ProcessDate = x.ProcessDate,
                RONo = x.RONo,
                Article = x.Article,
                IsCuttingIn = x.IsCuttingIn,
                CreatedBy = x.CreatedBy,
                ProductOwner = new { 
                    Id = x.ProductOwnerId, 
                    Code = x.ProductOwnerCode, 
                    Name = x.ProductOwnerName
                },
                TotalQuantity = x.GarmentPreparingItem.Sum(b => b.Quantity),
                Items = x.GarmentPreparingItem.Select(y => new {
                    Id = y.Identity,
                    LastModifiedDate = y.ModifiedDate ?? y.CreatedDate,
                    LastModifiedBy = y.ModifiedBy ?? y.CreatedBy,
                    UENItemId = y.UENItemId,
                    Product = new {
                        Id = y.ProductId, 
                        Name = y.ProductName, 
                        Code = y.ProductCode
                    },
                    DesignColor = y.DesignColor,
                    Quantity = y.Quantity,
                    Uom = new {
                        Id = y.UomId, 
                        Unit = y.UomUnit
                    },
                    FabricType = y.FabricType,
                    RemainingQuantity = y.RemainingQuantity,
                    BasicPrice = y.BasicPrice,
                    GarmentPreparingId = y.GarmentSubconPreparingId,
                    ROSource = y.ROSource,
                    BeacukaiNo = y.BeacukaiNo,
                    BeacukaiType = y.BeacukaiType,
                    BeacukaiDate = y.BeacukaiDate
                }).OrderBy(y => y.Id)
            });
            
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                newQuery = newQuery.Where(d => d.UENNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || d.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || d.Unit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || d.Article != null && d.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || d.Items.Any(e => e.Product.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            }
            return newQuery;
        }

        public bool RoChecking(IEnumerable<string> roList, string buyerCode)
        {
            var data = Query.Where(x => roList.Contains(x.RONo) && buyerCode.Contains(x.ProductOwnerCode)).ToList();
            return data.Count > 0;
        }
    }
}