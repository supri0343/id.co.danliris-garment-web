using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories
{
    public interface IGarmentSubconSewingOutDetailRepository : IAggregateRepository<GarmentSubconSewingOutDetail, GarmentSubconSewingOutDetailReadModel>
    {
    }
}
