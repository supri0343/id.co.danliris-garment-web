using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleFabricWashes.Repositories
{
    public class GarmentServiceSampleFabricWashItemRepository : AggregateRepostory<GarmentServiceSampleFabricWashItem, GarmentServiceSampleFabricWashItemReadModel>, IGarmentServiceSampleFabricWashItemRepository
    {
        IQueryable<GarmentServiceSampleFabricWashItemReadModel> IGarmentServiceSampleFabricWashItemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleFabricWashItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "UnitExpenditureNo"
            };

            data = QueryHelper<GarmentServiceSampleFabricWashItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleFabricWashItemReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSampleFabricWashItem Map(GarmentServiceSampleFabricWashItemReadModel readModel)
        {
            return new GarmentServiceSampleFabricWashItem(readModel);
        }
    }
}
