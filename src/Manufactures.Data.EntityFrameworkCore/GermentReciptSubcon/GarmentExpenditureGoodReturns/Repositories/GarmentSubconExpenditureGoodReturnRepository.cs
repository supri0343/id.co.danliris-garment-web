using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentExpenditureGoodReturns.Repositories
{
    public class GarmentSubconExpenditureGoodReturnRepository : AggregateRepostory<GarmentSubconExpenditureGoodReturn, GarmentSubconExpenditureGoodReturnReadModel>, IGarmentSubconExpenditureGoodReturnRepository
    {
        public IQueryable<GarmentSubconExpenditureGoodReturnReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconExpenditureGoodReturnReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ReturNo",
                "ExpenditureNo",
                "URNNo",
                "ReturType",
                "Article",
                "RONo",
                "UnitCode",
                "Invoice",
                "UnitName"
            };
            data = QueryHelper<GarmentSubconExpenditureGoodReturnReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconExpenditureGoodReturnReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentSubconExpenditureGoodReturn Map(GarmentSubconExpenditureGoodReturnReadModel readModel)
        {
            return new GarmentSubconExpenditureGoodReturn(readModel);
        }
    }
}
