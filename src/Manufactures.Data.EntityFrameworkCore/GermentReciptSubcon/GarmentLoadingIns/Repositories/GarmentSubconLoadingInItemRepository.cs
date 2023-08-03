using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GermentReciptSubcon.GarmentLoadings.Repositories
{
    public class GarmentSubconLoadingInItemRepository : AggregateRepostory<GarmentSubconLoadingInItem, GarmentSubconLoadingInItemReadModel>, IGarmentSubconLoadingInItemRepository
    {
        protected override GarmentSubconLoadingInItem Map(GarmentSubconLoadingInItemReadModel readModel)
        {
            return new GarmentSubconLoadingInItem(readModel);
        }
    }
}
