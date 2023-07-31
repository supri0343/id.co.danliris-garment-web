using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingIns.Repositories
{
    public class GarmentSubconCuttingInItemRepository : AggregateRepostory<GarmentSubconCuttingInItem, GarmentSubconCuttingInItemReadModel>, IGarmentSubconCuttingInItemRepository
    {
        protected override GarmentSubconCuttingInItem Map(GarmentSubconCuttingInItemReadModel readModel)
        {
            return new GarmentSubconCuttingInItem(readModel);
        }
    }
}
