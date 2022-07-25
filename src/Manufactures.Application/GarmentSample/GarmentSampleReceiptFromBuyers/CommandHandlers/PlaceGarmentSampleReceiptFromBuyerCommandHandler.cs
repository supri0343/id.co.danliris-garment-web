using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
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
	public class PlaceGarmentSampleReceiptFromBuyerCommandHandler : ICommandHandler<PlaceGarmentSampleReceiptFromBuyerCommand, GarmentSampleReceiptFromBuyer>
	{
		private readonly IStorage _storage;
		private readonly IGarmentSampleReceiptFromBuyerRepository _GarmentSampleReceiptFromBuyerRepository;
		private readonly IGarmentSampleReceiptFromBuyerItemRepository _GarmentSampleReceiptFromBuyerItemRepository;
		private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
		private readonly IGarmentSampleStockRepository _GarmentSampleStockRepository;
		private readonly IGarmentSampleStockHistoryRepository _GarmentSampleStockHistoryRepository;

		public PlaceGarmentSampleReceiptFromBuyerCommandHandler(IStorage storage)
		{
			_storage = storage;
			_GarmentSampleReceiptFromBuyerRepository = storage.GetRepository<IGarmentSampleReceiptFromBuyerRepository>();
			_GarmentSampleReceiptFromBuyerItemRepository = storage.GetRepository<IGarmentSampleReceiptFromBuyerItemRepository>();
			 _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			_GarmentSampleStockRepository = storage.GetRepository<IGarmentSampleStockRepository>();
			_GarmentSampleStockHistoryRepository = storage.GetRepository<IGarmentSampleStockHistoryRepository>();
		 
		}

		public async Task<GarmentSampleReceiptFromBuyer> Handle(PlaceGarmentSampleReceiptFromBuyerCommand request, CancellationToken cancellationToken)
		{
			request.Items = request.Items.ToList();
			GarmentSampleReceiptFromBuyer GarmentSampleReceiptFromBuyer = new GarmentSampleReceiptFromBuyer(
				Guid.NewGuid(),
				GenerateReceiptFromBuyerNo(request),
				request.SaveAs,
				request.ReceiptDate
			);

			Dictionary<string, double> finStockToBeUpdated = new Dictionary<string, double>();
			Dictionary<Guid, double> finstockQty = new Dictionary<Guid, double>();

			
				int count = 1;
				List<GarmentSampleStock> stocks = new List<GarmentSampleStock>();
				foreach (var item in request.Items)

				{
					GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitCode == "SMP1" && a.ComodityId == item.ComodityId).Select(s => new GarmentComodityPrice(s)).FirstOrDefault();

					GarmentSampleReceiptFromBuyerItem garmentSampleReceiptFromBuyerItem = new GarmentSampleReceiptFromBuyerItem(
					Guid.NewGuid(),
					GarmentSampleReceiptFromBuyer.Identity,
					item.InvoiceNo,
					item.BuyerAgentId,
					item.BuyerAgentCode,
					item.BuyerAgentName,
					item.RONo,
					item.Article,
					item.Description,
					item.Style,
					item.ComodityId,
					item.ComodityCode,
					item.ComodityName,
					item.Colour,
					item.SizeId,
					item.SizeName,
					item.ReceiptQuantity
					);
				await _GarmentSampleReceiptFromBuyerItemRepository.Update(garmentSampleReceiptFromBuyerItem);
				var uom = _GarmentSampleStockRepository.Query.Where(s => s.UomUnit == "PCS").FirstOrDefault();
				if (request.SaveAs == "Arsip MD")
				{
					var existStock = _GarmentSampleStockRepository.Query.Where(
						a => a.RONo == item.RONo
						&& a.Article == item.Article
						&& new SizeId(a.SizeId) == new SizeId(item.SizeId)
						&& a.ComodityId == item.ComodityId
						&& new UomId(a.UomId) == new UomId(uom.UomId)
						&& a.ArchiveType == request.SaveAs
						&& a.Description == item.Description
						).Select(s => new GarmentSampleStock(s)).SingleOrDefault();

					if (existStock == null)
					{
						GarmentSampleStock stock = new GarmentSampleStock(
										Guid.NewGuid(),
										GenerateStockNo(request, count),
										request.SaveAs,
										item.RONo,
										item.Article,
										new GarmentComodityId(item.ComodityId),
										item.ComodityCode,
										item.ComodityName,
										new SizeId(item.SizeId),
										item.SizeName,
										new UomId(uom.UomId),
										uom.UomUnit,
										item.ReceiptQuantity,
										item.Description
										);
						count++;
						await _GarmentSampleStockRepository.Update(stock);
						stocks.Add(stock);
					}
					else
					{
						existStock.SetQuantity(existStock.Quantity + item.ReceiptQuantity);
						existStock.Modify();

						await _GarmentSampleStockRepository.Update(existStock);
						var stock = stocks.Where(a => a.RONo == item.RONo
								&& a.Article == item.Article
								&& a.SizeId == new SizeId(item.SizeId)
								&& a.ComodityId == new GarmentComodityId(item.ComodityId)
								&& a.UomId == new UomId(uom.UomId)
								&& a.ArchiveType == request.SaveAs).SingleOrDefault();
						stocks.Add(existStock);
					}

					GarmentSampleStockHistory garmentSampleStockHistory = new GarmentSampleStockHistory(
											Guid.NewGuid(),
											GarmentSampleReceiptFromBuyer.Identity,
											garmentSampleReceiptFromBuyerItem.Identity,
											"IN",
											request.SaveAs,
											item.RONo,
											item.Article,
											new GarmentComodityId(item.ComodityId),
											item.ComodityCode,
											item.ComodityName,
											new SizeId(item.SizeId),
											item.SizeName,
											 new UomId(uom.UomId),
											uom.UomUnit,
											item.ReceiptQuantity,
											item.Description
										);
					 
					await _GarmentSampleStockHistoryRepository.Update(garmentSampleStockHistory);
					
				}
			}
				await _GarmentSampleReceiptFromBuyerRepository.Update(GarmentSampleReceiptFromBuyer);
			_storage.Save();

			return GarmentSampleReceiptFromBuyer;
		}
		

			
		
	private string GenerateStockNo(PlaceGarmentSampleReceiptFromBuyerCommand request, int count)
	{
		var now = DateTime.Now;
		var year = now.ToString("yy");
		var month = now.ToString("MM");

		var prefix = $"STA{"SMP1"}{year}{month}";

		var lastStockNo = _GarmentSampleStockRepository.Query.Where(w => w.SampleStockNo.StartsWith(prefix))
			.OrderByDescending(o => o.SampleStockNo)
			.Select(s => int.Parse(s.SampleStockNo.Replace(prefix, "")))
			.FirstOrDefault();
		var StockNo = $"{prefix}{(lastStockNo + count).ToString("D4")}";

		return StockNo;
	}
	private string GenerateReceiptFromBuyerNo(PlaceGarmentSampleReceiptFromBuyerCommand request)
		{
			var now = DateTime.Now;
			var year = now.ToString("yy");
			var month = now.ToString("MM");
			var day = now.ToString("dd");
		 
			var pre = "RFB";

			var prefix = $"{pre} {year}{month}";

			var lastReceiptFromBuyerNo = _GarmentSampleReceiptFromBuyerRepository.Query.Where(w => w.ReceiptNo.StartsWith(prefix))
				.OrderByDescending(o => o.ReceiptNo)
				.Select(s => int.Parse(s.ReceiptNo.Replace(prefix, "")))
				.FirstOrDefault();
			var finInNo = $"{prefix}{(lastReceiptFromBuyerNo + 1).ToString("D4")}";

			return finInNo;
		}

	}
}
