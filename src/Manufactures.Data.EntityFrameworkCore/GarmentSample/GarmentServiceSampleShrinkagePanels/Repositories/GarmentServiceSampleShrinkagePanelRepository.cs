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
    public class GarmentServiceSampleShrinkagePanelRepository : AggregateRepostory<GarmentServiceSampleShrinkagePanel, GarmentServiceSampleShrinkagePanelReadModel>, IGarmentServiceSampleShrinkagePanelRepository
    {
        IQueryable<GarmentServiceSampleShrinkagePanelReadModel> IGarmentServiceSampleShrinkagePanelRepository.Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSampleShrinkagePanelReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ServiceSampleShrinkagePanelNo",
            };

            data = QueryHelper<GarmentServiceSampleShrinkagePanelReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSampleShrinkagePanelReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSampleShrinkagePanel Map(GarmentServiceSampleShrinkagePanelReadModel readModel)
        {
            return new GarmentServiceSampleShrinkagePanel(readModel);
        }
    }
}
