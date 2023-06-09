using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.ServiceSubconExpenditureGood.Repositories
{
    public class GarmentServiceSubconExpenditureGoodItemRepository : AggregateRepostory<GarmentServiceSubconExpenditureGoodItem, GarmentServiceSubconExpenditureGoodItemReadModel>, IGarmentServiceSubconExpenditureGoodtemRepository
    {
        IQueryable<GarmentServiceSubconExpenditureGoodItemReadModel> IGarmentServiceSubconExpenditureGoodtemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconExpenditureGoodItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo"
            };

            data = QueryHelper<GarmentServiceSubconExpenditureGoodItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconExpenditureGoodItemReadModel>.Order(data, OrderDictionary);

            return data;
        }
        protected override GarmentServiceSubconExpenditureGoodItem Map(GarmentServiceSubconExpenditureGoodItemReadModel readModel)
        {
            return new GarmentServiceSubconExpenditureGoodItem(readModel);
        }
    }
}
