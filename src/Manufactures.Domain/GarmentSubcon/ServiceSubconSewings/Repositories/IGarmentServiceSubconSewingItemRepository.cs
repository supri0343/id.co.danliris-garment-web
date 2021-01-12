using System;
using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories
{
    public interface IGarmentServiceSubconSewingItemRepository : IAggregateRepository<GarmentServiceSubconSewingItem, GarmentServiceSubconSewingItemReadModel>
    {
    }
}
