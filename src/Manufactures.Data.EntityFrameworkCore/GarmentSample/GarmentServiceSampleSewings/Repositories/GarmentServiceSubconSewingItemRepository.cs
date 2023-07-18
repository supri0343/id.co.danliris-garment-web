using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleSewings.Repositories
{
    public class GarmentServiceSampleSewingItemRepository : AggregateRepostory<GarmentServiceSampleSewingItem, GarmentServiceSampleSewingItemReadModel>, IGarmentServiceSampleSewingItemRepository
    {
        IQueryable<GarmentServiceSampleSewingItemReadModel> IGarmentServiceSampleSewingItemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleSewingItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo"
            };

            data = QueryHelper<GarmentServiceSampleSewingItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleSewingItemReadModel>.Order(data, OrderDictionary);

            return data;
        }
        protected override GarmentServiceSampleSewingItem Map(GarmentServiceSampleSewingItemReadModel readModel)
        {
            return new GarmentServiceSampleSewingItem(readModel);
        }
    }
}
