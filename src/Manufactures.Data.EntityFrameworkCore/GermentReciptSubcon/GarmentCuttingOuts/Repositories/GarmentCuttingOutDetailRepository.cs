using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingOuts.Repositories
{
    public class GarmentSubconCuttingOutDetailRepository : AggregateRepostory<GarmentSubconCuttingOutDetail, GarmentSubconCuttingOutDetailReadModel>, IGarmentSubconCuttingOutDetailRepository
    {
        protected override GarmentSubconCuttingOutDetail Map(GarmentSubconCuttingOutDetailReadModel readModel)
        {
            return new GarmentSubconCuttingOutDetail(readModel);
        }
    }
}