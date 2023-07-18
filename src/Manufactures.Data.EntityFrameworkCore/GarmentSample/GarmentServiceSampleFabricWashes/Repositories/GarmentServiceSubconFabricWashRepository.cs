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

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleFabricWashs.Repositories
{
    public class GarmentServiceSampleFabricWashRepository : AggregateRepostory<GarmentServiceSampleFabricWash, GarmentServiceSampleFabricWashReadModel>, IGarmentServiceSampleFabricWashRepository
    {
        IQueryable<GarmentServiceSampleFabricWashReadModel> IGarmentServiceSampleFabricWashRepository.Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleFabricWashReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ServiceSampleFabricWashNo",
            };

            data = QueryHelper<GarmentServiceSampleFabricWashReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleFabricWashReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSampleFabricWash Map(GarmentServiceSampleFabricWashReadModel readModel)
        {
            return new GarmentServiceSampleFabricWash(readModel);
        }
    }
}
