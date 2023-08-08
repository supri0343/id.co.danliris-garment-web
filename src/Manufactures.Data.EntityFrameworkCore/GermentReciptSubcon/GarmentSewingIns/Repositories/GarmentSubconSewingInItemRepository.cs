using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingIns.Repositories
{
    public class GarmentSubconSewingInItemRepository : AggregateRepostory<GarmentSubconSewingInItem, GarmentSubconSewingInItemReadModel>, IGarmentSubconSewingInItemRepository
    {
        protected override GarmentSubconSewingInItem Map(GarmentSubconSewingInItemReadModel readModel)
        {
            return new GarmentSubconSewingInItem(readModel);
        }
    }
}