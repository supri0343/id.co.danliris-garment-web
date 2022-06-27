using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Repositories
{
	public interface IGarmentSampleReceiptFromBuyerRepository : IAggregateRepository<GarmentSampleReceiptFromBuyer, GarmentSampleReceiptFromBuyerReadModel>
	{
		IQueryable<GarmentSampleReceiptFromBuyerReadModel> Read(int page, int size, string order, string keyword, string filter);

		IQueryable<GarmentSampleReceiptFromBuyerReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
	}
}