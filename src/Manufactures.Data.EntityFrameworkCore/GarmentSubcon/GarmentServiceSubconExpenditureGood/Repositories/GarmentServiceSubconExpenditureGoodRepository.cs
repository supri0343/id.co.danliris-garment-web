using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.ServiceSubconExpenditureGoodss.Repositories
{
    public class GarmentServiceSubconExpenditureGoodRepository : AggregateRepostory<GarmentServiceSubconExpenditureGood, GarmentServiceSubconExpenditureGoodReadModel>, IGarmentServiceSubconExpenditureGoodRepository
    {
        IQueryable<GarmentServiceSubconExpenditureGoodReadModel> IGarmentServiceSubconExpenditureGoodRepository.Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconExpenditureGoodReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ServiceSubconExpenditureGoodNo",
                //"UnitCode",
            };

            data = QueryHelper<GarmentServiceSubconExpenditureGoodReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconExpenditureGoodReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSubconExpenditureGood Map(GarmentServiceSubconExpenditureGoodReadModel readModel)
        {
            return new GarmentServiceSubconExpenditureGood(readModel);
        }
    }
}
