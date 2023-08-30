using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentPackingIns.Repositories
{
    public class GarmentSubconPackingInItemRepository : AggregateRepostory<GarmentSubconPackingInItem, GarmentSubconPackingInItemReadModel>, IGarmentSubconPackingInItemRepository
    {
        protected override GarmentSubconPackingInItem Map(GarmentSubconPackingInItemReadModel readModel)
        {
            return new GarmentSubconPackingInItem(readModel);
        }
    }
}