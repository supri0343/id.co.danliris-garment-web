using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingIns.Repositories
{
    public class GarmentSubconFinishingInRepository : AggregateRepostory<GarmentSubconFinishingIn, GarmentSubconFinishingInReadModel>, IGarmentSubconFinishingInRepository
    {
        public IQueryable<GarmentSubconFinishingInReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconFinishingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "FinishingInNo",
                "FinishingInType",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName",
                "UnitFromCode",
                "UnitFromName",
                "Items.ProductName"
            };
            data = QueryHelper<GarmentSubconFinishingInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconFinishingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSubconFinishingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconFinishingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };
            data = QueryHelper<GarmentSubconFinishingInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconFinishingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentSubconFinishingIn Map(GarmentSubconFinishingInReadModel readModel)
        {
            return new GarmentSubconFinishingIn(readModel);
        }
    }
}
