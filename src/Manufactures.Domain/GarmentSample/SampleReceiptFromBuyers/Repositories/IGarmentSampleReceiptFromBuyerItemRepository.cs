using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Repositories
{
	public interface IGarmentSampleReceiptFromBuyerItemRepository : IAggregateRepository<GarmentSampleReceiptFromBuyerItem, GarmentSampleReceiptFromBuyerItemReadModel>
	{
	}
}
