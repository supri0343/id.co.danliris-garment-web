using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories
{
    public interface IGarmentSubconSewingInItemRepository : IAggregateRepository<GarmentSubconSewingInItem, GarmentSubconSewingInItemReadModel>
    {
    }
}