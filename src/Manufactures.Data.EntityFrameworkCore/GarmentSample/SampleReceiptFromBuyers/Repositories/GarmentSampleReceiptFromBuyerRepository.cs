using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.ReadModels;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleReceiptFromBuyers.Repositories
{
	public class GarmentSampleReceiptFromBuyerRepository : AggregateRepostory<GarmentSampleReceiptFromBuyer, GarmentSampleReceiptFromBuyerReadModel>, IGarmentSampleReceiptFromBuyerRepository
	{
		public IQueryable<GarmentSampleReceiptFromBuyerReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentSampleReceiptFromBuyerReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"SaveAs" ,"ReceiptNo"
				 
			};
			data = QueryHelper<GarmentSampleReceiptFromBuyerReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleReceiptFromBuyerReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		public IQueryable<GarmentSampleReceiptFromBuyerReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentSampleReceiptFromBuyerReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"InvoiceNo"
			};
			data = QueryHelper<GarmentSampleReceiptFromBuyerReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleReceiptFromBuyerReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentSampleReceiptFromBuyer Map(GarmentSampleReceiptFromBuyerReadModel readModel)
		{
			return new GarmentSampleReceiptFromBuyer(readModel);
		}
	}
}
