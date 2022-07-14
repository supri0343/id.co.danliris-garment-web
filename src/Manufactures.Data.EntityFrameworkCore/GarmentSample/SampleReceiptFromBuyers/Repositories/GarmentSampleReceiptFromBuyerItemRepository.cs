using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleReceiptFromBuyers.Repositories
{
	public class GarmentSampleReceiptFromBuyerItemRepository : AggregateRepostory<GarmentSampleReceiptFromBuyerItem, GarmentSampleReceiptFromBuyerItemReadModel>, IGarmentSampleReceiptFromBuyerItemRepository
	{
		protected override GarmentSampleReceiptFromBuyerItem Map(GarmentSampleReceiptFromBuyerItemReadModel readModel)
		{
			return new GarmentSampleReceiptFromBuyerItem(readModel);
		}
	}
}
