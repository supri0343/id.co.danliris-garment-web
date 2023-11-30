using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.ReadModels;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentPackingOut.Repositories
{
    public class GarmentSubconPackingOutItemRepository : AggregateRepostory<GarmentSubconPackingOutItem, GarmentSubconPackingOutItemReadModel>, IGarmentSubconPackingOutItemRepository
    {
        protected override GarmentSubconPackingOutItem Map(GarmentSubconPackingOutItemReadModel readModel)
        {
            return new GarmentSubconPackingOutItem(readModel);
        }
    }
}
