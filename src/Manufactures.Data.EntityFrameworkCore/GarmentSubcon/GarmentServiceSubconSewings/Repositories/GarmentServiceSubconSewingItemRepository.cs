using System;
using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Repositories
{
    public class GarmentServiceSubconSewingItemRepository : AggregateRepostory<GarmentServiceSubconSewingItem, GarmentServiceSubconSewingItemReadModel>, IGarmentServiceSubconSewingItemRepository
    {
        protected override GarmentServiceSubconSewingItem Map(GarmentServiceSubconSewingItemReadModel readModel)
        {
            return new GarmentServiceSubconSewingItem(readModel);
        }
    }
}
