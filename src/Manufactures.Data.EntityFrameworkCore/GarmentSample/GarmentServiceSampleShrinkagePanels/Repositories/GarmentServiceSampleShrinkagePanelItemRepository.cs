using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleShrinkagePanels.Repositories
{
    public class GarmentServiceSampleShrinkagePanelItemRepository : AggregateRepostory<GarmentServiceSampleShrinkagePanelItem, GarmentServiceSampleShrinkagePanelItemReadModel>, IGarmentServiceSampleShrinkagePanelItemRepository
    {
        IQueryable<GarmentServiceSampleShrinkagePanelItemReadModel> IGarmentServiceSampleShrinkagePanelItemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleShrinkagePanelItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "UnitExpenditureNo"
            };

            data = QueryHelper<GarmentServiceSampleShrinkagePanelItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleShrinkagePanelItemReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSampleShrinkagePanelItem Map(GarmentServiceSampleShrinkagePanelItemReadModel readModel)
        {
            return new GarmentServiceSampleShrinkagePanelItem(readModel);
        }
    }
}
