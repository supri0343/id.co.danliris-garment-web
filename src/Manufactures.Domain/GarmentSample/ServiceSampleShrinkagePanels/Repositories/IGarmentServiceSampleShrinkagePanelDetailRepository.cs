using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories
{
    public interface IGarmentServiceSampleShrinkagePanelDetailRepository : IAggregateRepository<GarmentServiceSampleShrinkagePanelDetail, GarmentServiceSampleShrinkagePanelDetailReadModel>
    {
    }
}
