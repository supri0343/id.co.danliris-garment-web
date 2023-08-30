using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories
{
    public interface IGarmentSubconPackingInItemRepository : IAggregateRepository<GarmentSubconPackingInItem, GarmentSubconPackingInItemReadModel>
    {
    }
}
