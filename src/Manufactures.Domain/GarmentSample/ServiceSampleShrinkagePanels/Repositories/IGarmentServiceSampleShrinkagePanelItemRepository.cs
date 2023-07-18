using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories
{
    public interface IGarmentServiceSampleShrinkagePanelItemRepository : IAggregateRepository<GarmentServiceSampleShrinkagePanelItem, GarmentServiceSampleShrinkagePanelItemReadModel>
    {
        IQueryable<GarmentServiceSampleShrinkagePanelItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
