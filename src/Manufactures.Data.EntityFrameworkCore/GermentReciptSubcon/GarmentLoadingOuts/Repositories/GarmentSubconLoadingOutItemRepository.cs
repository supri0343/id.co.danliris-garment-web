using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GermentReciptSubcon.GarmentLoadingOuts.Repositories
{
    public class GarmentSubconLoadingOutItemRepository : AggregateRepostory<GarmentSubconLoadingOutItem, GarmentSubconLoadingOutItemReadModel>, IGarmentSubconLoadingOutItemRepository
    {
        protected override GarmentSubconLoadingOutItem Map(GarmentSubconLoadingOutItemReadModel readModel)
        {
            return new GarmentSubconLoadingOutItem(readModel);
        }
    }
}
