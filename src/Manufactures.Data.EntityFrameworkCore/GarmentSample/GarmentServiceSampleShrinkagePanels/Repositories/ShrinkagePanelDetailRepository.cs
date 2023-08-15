using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.GarmentServiceSampleShrinkagePanels.Repositories
{
    public class ShrinkagePanelDetailRepository : AggregateRepostory<GarmentServiceSampleShrinkagePanelDetail, GarmentServiceSampleShrinkagePanelDetailReadModel>, IGarmentServiceSampleShrinkagePanelDetailRepository
    {
        protected override GarmentServiceSampleShrinkagePanelDetail Map(GarmentServiceSampleShrinkagePanelDetailReadModel readModel)
        {
            return new GarmentServiceSampleShrinkagePanelDetail(readModel);
        }
    }
}
