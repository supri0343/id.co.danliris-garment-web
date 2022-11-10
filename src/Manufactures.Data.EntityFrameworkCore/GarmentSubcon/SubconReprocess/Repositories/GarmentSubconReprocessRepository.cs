using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconReprocess.Repositories
{
    public class GarmentSubconReprocessRepository : AggregateRepostory<GarmentSubconReprocess, GarmentSubconReprocessReadModel>, IGarmentSubconReprocessRepository
    {
        IQueryable<GarmentSubconReprocessReadModel> IGarmentSubconReprocessRepository.Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconReprocessReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ReprocessNo","ReprocessType"
            };

            data = QueryHelper<GarmentSubconReprocessReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconReprocessReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentSubconReprocess Map(GarmentSubconReprocessReadModel readModel)
        {
            return new GarmentSubconReprocess(readModel);
        }
    }
}
