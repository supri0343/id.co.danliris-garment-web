using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingOuts.Repositories
{
    public class GarmentSubconSewingOutDetailRepository : AggregateRepostory<GarmentSubconSewingOutDetail, GarmentSubconSewingOutDetailReadModel>, IGarmentSubconSewingOutDetailRepository
    {
        protected override GarmentSubconSewingOutDetail Map(GarmentSubconSewingOutDetailReadModel readModel)
        {
            return new GarmentSubconSewingOutDetail(readModel);
        }
    }
}