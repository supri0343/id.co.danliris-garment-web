using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingOuts.Repositories
{
    public class GarmentSubconSewingOutItemRepository : AggregateRepostory<GarmentSubconSewingOutItem, GarmentSubconSewingOutItemReadModel>, IGarmentSubconSewingOutItemRepository
    {
        protected override GarmentSubconSewingOutItem Map(GarmentSubconSewingOutItemReadModel readModel)
        {
            return new GarmentSubconSewingOutItem(readModel);
        }
    }
}