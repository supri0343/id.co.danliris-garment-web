using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingOuts.Repositories
{
    public class GarmentSubconCuttingOutItemRepository : AggregateRepostory<GarmentSubconCuttingOutItem, GarmentSubconCuttingOutItemReadModel>, IGarmentSubconCuttingOutItemRepository
    {
        protected override GarmentSubconCuttingOutItem Map(GarmentSubconCuttingOutItemReadModel readModel)
        {
            return new GarmentSubconCuttingOutItem(readModel);
        }
    }
}