using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories
{
    public interface IGarmentSubconFinishingOutDetailRepository : IAggregateRepository<GarmentSubconFinishingOutDetail, GarmentReceiptSubconFinishingOutDetailReadModel>
    {
    }
}