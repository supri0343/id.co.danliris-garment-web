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
    public class GarmentSubconReprocessItemRepository : AggregateRepostory<GarmentSubconReprocessItem, GarmentSubconReprocessItemReadModel>, IGarmentSubconReprocessItemRepository
    {
        IQueryable<GarmentSubconReprocessItemReadModel> IGarmentSubconReprocessItemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconReprocessItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo","ServiceSubconCuttingNo","ServiceSubconSewingNo"
            };

            data = QueryHelper<GarmentSubconReprocessItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconReprocessItemReadModel>.Order(data, OrderDictionary);

            return data;
        }
        protected override GarmentSubconReprocessItem Map(GarmentSubconReprocessItemReadModel readModel)
        {
            return new GarmentSubconReprocessItem(readModel);
        }
    }
}