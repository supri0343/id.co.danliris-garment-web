using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.ServiceSampleExpenditureGoodss.Repositories
{
    public class GarmentServiceSampleExpenditureGoodRepository : AggregateRepostory<GarmentServiceSampleExpenditureGood, GarmentServiceSampleExpenditureGoodReadModel>, IGarmentServiceSampleExpenditureGoodRepository
    {
        IQueryable<GarmentServiceSampleExpenditureGoodReadModel> IGarmentServiceSampleExpenditureGoodRepository.Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleExpenditureGoodReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ServiceSampleExpenditureGoodNo",
                //"UnitCode",
            };

            data = QueryHelper<GarmentServiceSampleExpenditureGoodReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleExpenditureGoodReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSampleExpenditureGood Map(GarmentServiceSampleExpenditureGoodReadModel readModel)
        {
            return new GarmentServiceSampleExpenditureGood(readModel);
        }
    }
}
