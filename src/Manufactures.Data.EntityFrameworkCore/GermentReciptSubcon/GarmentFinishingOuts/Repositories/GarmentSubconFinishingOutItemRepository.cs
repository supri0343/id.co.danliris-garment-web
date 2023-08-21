using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingOuts.Repositories
{
    public class GarmentSubconFinishingOutItemRepository : AggregateRepostory<GarmentSubconFinishingOutItem, GarmentReceiptSubconFinishingOutItemReadModel>, IGarmentSubconFinishingOutItemRepository
    {
        protected override GarmentSubconFinishingOutItem Map(GarmentReceiptSubconFinishingOutItemReadModel readModel)
        {
            return new GarmentSubconFinishingOutItem(readModel);
        }
    }
}