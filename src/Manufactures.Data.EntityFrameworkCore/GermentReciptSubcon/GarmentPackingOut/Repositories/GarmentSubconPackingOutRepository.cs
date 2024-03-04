using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.ReadModels;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentPackingOut.Repositories
{
    public class GarmentSubconPackingOutRepository : AggregateRepostory<GarmentSubconPackingOut, GarmentSubconPackingOutReadModel>, IGarmentSubconPackingOutRepository
    {
        public IQueryable<GarmentSubconPackingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconPackingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "PackingOutNo",
                "PackingOutType",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName",
                "ContractNo",
                "Invoice",
                //"BuyerName"
            };
            data = QueryHelper<GarmentSubconPackingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconPackingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSubconPackingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconPackingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };
            data = QueryHelper<GarmentSubconPackingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconPackingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public double BasicPriceByRO(string Keyword = null, string Filter = "{}")
        {
            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            long unitId = 0;
            bool hasUnitFilter = FilterDictionary.ContainsKey("UnitId") && long.TryParse(FilterDictionary["UnitId"], out unitId);
            bool hasRONoFilter = FilterDictionary.ContainsKey("RONo");
            string RONo = hasRONoFilter ? (FilterDictionary["RONo"] ?? "").Trim() : "";

            var dataHeader = Query.Where(a => a.RONo == RONo && a.UnitId == unitId).Include(a => a.Items);

            double priceTotal = 0;
            double qtyTotal = 0;

            foreach (var data in dataHeader)
            {
                priceTotal += data.Items.Sum(a => a.Price);
                qtyTotal += data.Items.Sum(a => a.Quantity);
            }

            double basicPrice = priceTotal / qtyTotal;

            return basicPrice;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSubconPackingOutReadModel> query) {
            var newQuery = query.Select(packingOut => new
            {
                Id = packingOut.Identity,
                PackingOutNo = packingOut.PackingOutNo,
                RONo = packingOut.RONo,
                Article = packingOut.Article,
                Unit = new
                {
                    Id = packingOut.UnitId,
                    Code = packingOut.UnitCode,
                    Name = packingOut.UnitName
                },
                PackingOutDate = packingOut.PackingOutDate,
                PackingOutType = packingOut.PackingOutType,
                Comodity = new
                {
                    Id = packingOut.ComodityId,
                    Code = packingOut.ComodityCode,
                    Name = packingOut.ComodityName
                },
                ProductOwner = new
                {
                    Id = packingOut.ProductOwnerId,
                    Code = packingOut.ProductOwnerCode,
                    Name = packingOut.ProductOwnerName
                },
                Invoice = packingOut.Invoice,
                ContractNo = packingOut.ContractNo,
                Carton = packingOut.Carton,
                Description = packingOut.Description,
                IsReceived = packingOut.IsReceived,
                PackingListId = packingOut.PackingListId,

                Items = packingOut.Items.Select(packingOutItem => new {
                    Id = packingOutItem.Identity,
                    PackingOutId = packingOutItem.PackingOutId,
                    PackingInItemId = packingOutItem.PackingInItemId,
                    Size = new
                    {
                        Id = packingOutItem.SizeId,
                        Size = packingOutItem.SizeName,
                    },
                    Quantity = packingOutItem.Quantity,
                    Uom = new
                    {
                        Id = packingOutItem.UomId,
                        Unit = packingOutItem.UomUnit
                    },
                    Description = packingOutItem.Description,
                    BasicPrice = packingOutItem.BasicPrice,
                    Price = packingOutItem.Price,
                    ReturQuantity = packingOutItem.ReturQuantity,
                    FinishedGoodStockId = packingOutItem.FinishedGoodStockId,
                    //IsPackingList = packingOutItem.IsPackingList,

                })

            });
            return newQuery;
        }

        protected override GarmentSubconPackingOut Map(GarmentSubconPackingOutReadModel readModel)
        {
            return new GarmentSubconPackingOut(readModel);
        }

        public IQueryable<GarmentSubconPackingOutReadModel> ReadignoreFilter(int page, int size, string order, string keyword, string filter)
        {
            var data = Query.IgnoreQueryFilters().Where(eg=>(eg.Deleted == true && eg.DeletedBy == "L") || (eg.Deleted == false) );

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconPackingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "PackingOutNo",
                "PackingOutType",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName",
                "ContractNo",
                "Invoice",
                "BuyerName"
            };
            data = QueryHelper<GarmentSubconPackingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconPackingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }
    }
}
