using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.CustomsOuts.Repositories
{
    public class GarmentSubconCustomsOutDetailRepository : AggregateRepostory<GarmentSubconCustomsOutDetail, GarmentSubconCustomsOutDetailReadModel>, IGarmentSubconCustomsOutDetailRepository
    {
        protected override GarmentSubconCustomsOutDetail Map(GarmentSubconCustomsOutDetailReadModel readModel)
        {
            return new GarmentSubconCustomsOutDetail(readModel);
        }
    }
}
