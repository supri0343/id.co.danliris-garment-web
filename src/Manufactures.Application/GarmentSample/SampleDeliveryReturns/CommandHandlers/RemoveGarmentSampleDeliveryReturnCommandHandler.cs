using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleDeliveryReturns.CommandHandlers
{
    public class RemoveGarmentSampleDeliveryReturnCommandHandler : ICommandHandler<RemoveGarmentSampleDeliveryReturnCommand, GarmentSampleDeliveryReturn>
    {
        private readonly IGarmentSampleDeliveryReturnRepository _garmentSampleDeliveryReturnRepository;
        private readonly IGarmentSampleDeliveryReturnItemRepository _garmentSampleDeliveryReturnItemRepository;
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;
        private readonly IStorage _storage;
        //----------
        private readonly ILogHistoryRepository _logHistoryRepository;
        //-------

        public RemoveGarmentSampleDeliveryReturnCommandHandler(IStorage storage)
        {
            _garmentSampleDeliveryReturnRepository = storage.GetRepository<IGarmentSampleDeliveryReturnRepository>();
            _garmentSampleDeliveryReturnItemRepository = storage.GetRepository<IGarmentSampleDeliveryReturnItemRepository>();
            _garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            _storage = storage;
            //------------
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
            //------------
        }

        public async Task<GarmentSampleDeliveryReturn> Handle(RemoveGarmentSampleDeliveryReturnCommand request, CancellationToken cancellationToken)
        {
            var garmentSampleDeliveryReturn = _garmentSampleDeliveryReturnRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentSampleDeliveryReturn == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            var garmentSampleDeliveryReturnItems = _garmentSampleDeliveryReturnItemRepository.Find(x => x.DRId == request.Id);

            foreach (var item in garmentSampleDeliveryReturnItems)
            {
                item.Remove();
                await _garmentSampleDeliveryReturnItemRepository.Update(item);

                if (item.ProductName == "FABRIC")
                {
                    var garmentSamplePreparingItem = _garmentSamplePreparingItemRepository.Find(o => o.Identity == Guid.Parse(item.PreparingItemId)).Single();

                    garmentSamplePreparingItem.setRemainingQuantityZeroValue(garmentSamplePreparingItem.RemainingQuantity + item.Quantity);

                    garmentSamplePreparingItem.SetModified();
                    await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItem);
                }
            }

            garmentSampleDeliveryReturn.Remove();

            await _garmentSampleDeliveryReturnRepository.Update(garmentSampleDeliveryReturn);
            //disini
            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI DELIVERY RETURN SAMPLE", "Delete Delivery Return Sample - " + garmentSampleDeliveryReturn.DRNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);
            //-----------

            _storage.Save();

            return garmentSampleDeliveryReturn;
        }
    }
}
