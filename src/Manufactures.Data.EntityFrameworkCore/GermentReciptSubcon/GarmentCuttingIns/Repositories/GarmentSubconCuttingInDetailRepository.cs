using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingIns.Repositories
{
    public class GarmentSubconCuttingInDetailRepository : AggregateRepostory<GarmentSubconCuttingInDetail, GarmentSubconCuttingInDetailReadModel>, IGarmentSubconCuttingInDetailRepository
    {
        protected override GarmentSubconCuttingInDetail Map(GarmentSubconCuttingInDetailReadModel readModel)
        {
            return new GarmentSubconCuttingInDetail(readModel);
        }
    }
}
