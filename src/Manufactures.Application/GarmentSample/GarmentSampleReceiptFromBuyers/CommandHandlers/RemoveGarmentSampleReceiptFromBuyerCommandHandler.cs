using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Commands;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Repositories;
using Manufactures.Domain.GarmentSample.SampleStocks;
using Manufactures.Domain.GarmentSample.SampleStocks.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentSampleReceiptFromBuyers.CommandHandlers
{
	public class RemoveGarmentSampleReceiptFromBuyerCommandHandler : ICommandHandler<RemoveGarmentSampleReceiptFromBuyerCommand, GarmentSampleReceiptFromBuyer>
	{
		private readonly IStorage _storage;
		private readonly IGarmentSampleReceiptFromBuyerRepository _GarmentSampleReceiptFromBuyerRepository;
		private readonly IGarmentSampleReceiptFromBuyerItemRepository _GarmentSampleReceiptFromBuyerItemRepository;
		private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
		private readonly IGarmentSampleStockRepository _GarmentSampleStockRepository;
		private readonly IGarmentSampleStockHistoryRepository _GarmentSampleStockHistoryRepository;

		public RemoveGarmentSampleReceiptFromBuyerCommandHandler(IStorage storage)
		{
			_storage = storage;
			_GarmentSampleReceiptFromBuyerRepository = storage.GetRepository<IGarmentSampleReceiptFromBuyerRepository>();
			_GarmentSampleReceiptFromBuyerItemRepository = storage.GetRepository<IGarmentSampleReceiptFromBuyerItemRepository>();
			_garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			_GarmentSampleStockRepository = storage.GetRepository<IGarmentSampleStockRepository>();
			_GarmentSampleStockHistoryRepository = storage.GetRepository<IGarmentSampleStockHistoryRepository>();
		}

		public async Task<GarmentSampleReceiptFromBuyer> Handle(RemoveGarmentSampleReceiptFromBuyerCommand request, CancellationToken cancellationToken)
		{
			var ReceiptFromBuyer = _GarmentSampleReceiptFromBuyerRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleReceiptFromBuyer(o)).Single();
			Dictionary<Guid, double> finStockToBeUpdated = new Dictionary<Guid, double>();

			_GarmentSampleReceiptFromBuyerItemRepository.Find(o => o.ReceiptId == ReceiptFromBuyer.Identity).ForEach(async expenditureItem =>
			{
				GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitCode =="SMP1" && a.ComodityCode == expenditureItem.ComodityCode).Select(s => new GarmentComodityPrice(s)).FirstOrDefault();


				if (ReceiptFromBuyer.SaveAs == "Arsip MD" )
				{
					GarmentSampleStockHistory garmentSampleStockHistory = _GarmentSampleStockHistoryRepository.Query.Where(a => a.ExpenditureGoodId == expenditureItem.Identity).Select(a => new GarmentSampleStockHistory(a)).Single();
					garmentSampleStockHistory.Remove();
					await _GarmentSampleStockHistoryRepository.Update(garmentSampleStockHistory);

					var existStock = _GarmentSampleStockRepository.Query.Where(
						a => a.RONo == expenditureItem.RONo
						&& a.Article == expenditureItem.Article
						&& a.SizeId == expenditureItem.SizeId
						&& a.ComodityCode == expenditureItem.ComodityCode
						&& a.UomUnit == "PCS"
						&& a.ArchiveType == ReceiptFromBuyer.SaveAs
						&& a.Description == expenditureItem.Description
						).Select(s => new GarmentSampleStock(s)).Single();
					var qtyStock = existStock.Quantity - expenditureItem.ReceiptQuantity;
					existStock.SetQuantity(qtyStock);
					existStock.Modify();

					await _GarmentSampleStockRepository.Update(existStock);
				}

				expenditureItem.Remove();
				await _GarmentSampleReceiptFromBuyerItemRepository.Update(expenditureItem);
			});


			ReceiptFromBuyer.Remove();
			await _GarmentSampleReceiptFromBuyerRepository.Update(ReceiptFromBuyer);

			_storage.Save();

			return ReceiptFromBuyer;
		}
	}
}
