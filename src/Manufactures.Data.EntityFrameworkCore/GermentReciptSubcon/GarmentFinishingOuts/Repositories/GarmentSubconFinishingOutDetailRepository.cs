using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentFinishingOuts.Repositories
{
    public class GarmentSubconFinishingOutDetailRepository : AggregateRepostory<GarmentSubconFinishingOutDetail, GarmentReceiptSubconFinishingOutDetailReadModel>, IGarmentSubconFinishingOutDetailRepository
    {
        protected override GarmentSubconFinishingOutDetail Map(GarmentReceiptSubconFinishingOutDetailReadModel readModel)
        {
            return new GarmentSubconFinishingOutDetail(readModel);
        }
    }
}
