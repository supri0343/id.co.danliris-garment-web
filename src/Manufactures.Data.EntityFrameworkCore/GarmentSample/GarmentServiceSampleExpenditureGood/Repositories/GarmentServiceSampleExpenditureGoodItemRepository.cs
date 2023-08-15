using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.ServiceSampleExpenditureGood.Repositories
{
    public class GarmentServiceSampleExpenditureGoodItemRepository : AggregateRepostory<GarmentServiceSampleExpenditureGoodItem, GarmentServiceSampleExpenditureGoodItemReadModel>, IGarmentServiceSampleExpenditureGoodtemRepository
    {
        IQueryable<GarmentServiceSampleExpenditureGoodItemReadModel> IGarmentServiceSampleExpenditureGoodtemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleExpenditureGoodItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo"
            };

            data = QueryHelper<GarmentServiceSampleExpenditureGoodItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleExpenditureGoodItemReadModel>.Order(data, OrderDictionary);

            return data;
        }
        protected override GarmentServiceSampleExpenditureGoodItem Map(GarmentServiceSampleExpenditureGoodItemReadModel readModel)
        {
            return new GarmentServiceSampleExpenditureGoodItem(readModel);
        }
    }
}
