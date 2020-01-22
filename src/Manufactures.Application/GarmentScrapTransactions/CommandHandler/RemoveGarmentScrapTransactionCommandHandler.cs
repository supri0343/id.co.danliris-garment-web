using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapTransactions;
using Manufactures.Domain.GarmentScrapTransactions.Commands;
using Manufactures.Domain.GarmentScrapTransactions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapTransactions.CommandHandler
{
	public class RemoveGarmentScrapTransactionCommandHandler : ICommandHandler<RemoveGarmentScrapTransactionCommand, GarmentScrapTransaction>
	{
		private readonly IStorage _storage;
		private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;
		private readonly IGarmentScrapTransactionItemRepository _garmentScrapTransactionItemRepository;
		private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;
		private readonly IGarmentScrapDestinationRepository _garmentScrapDestinationRepository;
		private readonly IGarmentScrapStockRepository _garmentScrapStockRepository;

		public RemoveGarmentScrapTransactionCommandHandler(IStorage storage)
		{
			_storage = storage;
			_garmentScrapTransactionRepository = storage.GetRepository<IGarmentScrapTransactionRepository>();
			_garmentScrapTransactionItemRepository = storage.GetRepository<IGarmentScrapTransactionItemRepository>();
			_garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
			_garmentScrapDestinationRepository = storage.GetRepository<IGarmentScrapDestinationRepository>();
			_garmentScrapStockRepository = storage.GetRepository<IGarmentScrapStockRepository>();

		}

		public async Task<GarmentScrapTransaction> Handle(RemoveGarmentScrapTransactionCommand request, CancellationToken cancellationToken)
		{
			var scrapTransaction = _garmentScrapTransactionRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentScrapTransaction(o)).Single();
			_garmentScrapTransactionItemRepository.Find(o => o.ScrapTransactionId == scrapTransaction.Identity).ForEach(async item =>
			{
				var gStock= _garmentScrapStockRepository.Query.Where(s => s.ScrapDestinationId == scrapTransaction.ScrapDestinationId && s.ScrapClassificationId == item.ScrapClassificationId).Select(i => new GarmentScrapStock(i)).FirstOrDefault();
				gStock.SetQuantity( gStock.Quantity - item.Quantity);
				item.Remove();
				await _garmentScrapTransactionItemRepository.Update(item);
				gStock.Modify();
				await _garmentScrapStockRepository.Update(gStock);
			});
			scrapTransaction.Remove();
			await _garmentScrapTransactionRepository.Update(scrapTransaction);
			_storage.Save();

			return scrapTransaction;
		}
	}

}
