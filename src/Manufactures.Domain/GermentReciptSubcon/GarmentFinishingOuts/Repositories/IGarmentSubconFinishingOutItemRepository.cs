using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories
{
    public interface IGarmentSubconFinishingOutItemRepository : IAggregateRepository<GarmentSubconFinishingOutItem, GarmentReceiptSubconFinishingOutItemReadModel>
    {
    }
}
