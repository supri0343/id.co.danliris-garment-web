using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingIns.Repositories
{
    public class GarmentSubconFinishingInItemRepository : AggregateRepostory<GarmentSubconFinishingInItem, GarmentSubconFinishingInItemReadModel>, IGarmentSubconFinishingInItemRepository
    {
        protected override GarmentSubconFinishingInItem Map(GarmentSubconFinishingInItemReadModel readModel)
        {
            return new GarmentSubconFinishingInItem(readModel);
        }
    }
}
