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
    public class GarmentServiceSampleCuttingDetailRepository : AggregateRepostory<GarmentServiceSampleCuttingDetail, GarmentServiceSampleCuttingDetailReadModel>, IGarmentServiceSampleCuttingDetailRepository
    {
        IQueryable<GarmentServiceSampleCuttingDetailReadModel> IGarmentServiceSampleCuttingDetailRepository.ReadDetail(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleCuttingDetailReadModel>.Filter(data, FilterDictionary);


            List<string> SearchAttributes = new List<string>
            {
                "Quantity"
            };

            data = QueryHelper<GarmentServiceSampleCuttingDetailReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleCuttingDetailReadModel>.Order(data, OrderDictionary);


            return data;
        }

        protected override GarmentServiceSampleCuttingDetail Map(GarmentServiceSampleCuttingDetailReadModel readModel)
        {
            return new GarmentServiceSampleCuttingDetail(readModel);
        }
    }
}