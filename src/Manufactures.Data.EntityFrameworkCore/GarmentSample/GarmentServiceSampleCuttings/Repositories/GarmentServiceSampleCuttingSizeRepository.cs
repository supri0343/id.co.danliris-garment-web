using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleCuttings.Repositories
{
    public class GarmentServiceSampleCuttingSizeRepository : AggregateRepostory<GarmentServiceSampleCuttingSize, GarmentServiceSampleCuttingSizeReadModel>, IGarmentServiceSampleCuttingSizeRepository
    {
        IQueryable<GarmentServiceSampleCuttingSizeReadModel> IGarmentServiceSampleCuttingSizeRepository.ReadSize(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleCuttingSizeReadModel>.Filter(data, FilterDictionary);


            List<string> SearchAttributes = new List<string>
            {
                "ProductCode",
                "Quantity",
            };

            data = QueryHelper<GarmentServiceSampleCuttingSizeReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleCuttingSizeReadModel>.Order(data, OrderDictionary);


            return data;
        }

        protected override GarmentServiceSampleCuttingSize Map(GarmentServiceSampleCuttingSizeReadModel readModel)
        {
            return new GarmentServiceSampleCuttingSize(readModel);
        }
    }
}