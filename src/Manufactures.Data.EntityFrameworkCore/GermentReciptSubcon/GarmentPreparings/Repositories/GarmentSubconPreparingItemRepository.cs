using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentPreparings.Repositories
{
    public class GarmentSubconPreparingItemRepository : AggregateRepostory<GarmentSubconPreparingItem, GarmentSubconPreparingItemReadModel>, IGarmentSubconPreparingItemRepository
    {
        protected override GarmentSubconPreparingItem Map(GarmentSubconPreparingItemReadModel readModel)
        {
            return new GarmentSubconPreparingItem(readModel);
        }
    }
}