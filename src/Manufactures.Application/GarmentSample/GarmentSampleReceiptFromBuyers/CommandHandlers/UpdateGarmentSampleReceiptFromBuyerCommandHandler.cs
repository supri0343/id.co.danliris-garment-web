using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Commands;
using Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentSampleReceiptFromBuyers.CommandHandlers
{
	public class UpdateGarmentSampleReceiptFromBuyerCommandHandler : ICommandHandler<UpdateGarmentSampleReceiptFromBuyerCommand, GarmentSampleReceiptFromBuyer>
	{
		private readonly IStorage _storage;
		private readonly IGarmentSampleReceiptFromBuyerRepository _GarmentSampleReceiptFromBuyerRepository;
		private readonly IGarmentSampleReceiptFromBuyerItemRepository _GarmentSampleReceiptFromBuyerItemRepository;

		public UpdateGarmentSampleReceiptFromBuyerCommandHandler(IStorage storage)
		{
			_storage = storage;
			_GarmentSampleReceiptFromBuyerRepository = storage.GetRepository<IGarmentSampleReceiptFromBuyerRepository>();
			_GarmentSampleReceiptFromBuyerItemRepository = storage.GetRepository<IGarmentSampleReceiptFromBuyerItemRepository>();

		}

		public async Task<GarmentSampleReceiptFromBuyer> Handle(UpdateGarmentSampleReceiptFromBuyerCommand request, CancellationToken cancellaitonToken)
		{
			var garmentSampleReceiptFromBuyer = _GarmentSampleReceiptFromBuyerRepository.Find(o => o.Identity == request.Identity).FirstOrDefault();

			 
			garmentSampleReceiptFromBuyer.SetReceiptDate(request.ReceiptDate);
		 
			var dbGarmentReceiptFromBuyer = _GarmentSampleReceiptFromBuyerItemRepository.Find(y => y.ReceiptId == garmentSampleReceiptFromBuyer.Identity);
			var updatedItems = request.Items.Where(x => dbGarmentReceiptFromBuyer.Any(y => y.Identity == garmentSampleReceiptFromBuyer.Identity));
			var addedItems = request.Items.Where(x => !dbGarmentReceiptFromBuyer.Any(y => y.Identity == garmentSampleReceiptFromBuyer.Identity));
			var deletedItems = dbGarmentReceiptFromBuyer.Where(x => !request.Items.Any(y => y.Id == garmentSampleReceiptFromBuyer.Identity));

			foreach (var item in updatedItems)
			{
				var dbItem = dbGarmentReceiptFromBuyer.Find(x => x.Identity == item.Id);
				dbItem.SetReceiptQuantity(item.ReceiptQuantity);
				 
				await _GarmentSampleReceiptFromBuyerItemRepository.Update(dbItem);
			}

			addedItems.Select(x => new GarmentSampleReceiptFromBuyerItem(Guid.NewGuid(), garmentSampleReceiptFromBuyer.Identity, x.InvoiceNo,x.BuyerAgentId,x.BuyerAgentCode,x.BuyerAgentName,x.RONo,x.Article,x.Description,x.Style,x.ComodityId,x.ComodityCode,x.ComodityName,x.Colour,x.SizeId,x.SizeName,x.ReceiptQuantity)).ToList()
				.ForEach(async x => await _GarmentSampleReceiptFromBuyerItemRepository.Update(x));

			foreach (var item in deletedItems)
			{
				item.Remove();
				item.SetDeleted();
				await _GarmentSampleReceiptFromBuyerItemRepository.Update(item);
			}


			garmentSampleReceiptFromBuyer.Modify();

			await _GarmentSampleReceiptFromBuyerRepository.Update(garmentSampleReceiptFromBuyer);

			_storage.Save();

			return garmentSampleReceiptFromBuyer;
		}
	}
}
