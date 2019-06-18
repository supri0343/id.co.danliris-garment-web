using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentPreparings.CommandHandlers
{
    public class UpdateGarmentPreparingCommandHandler : ICommandHandler<UpdateGarmentPreparingCommand, GarmentPreparing>
    {
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        public UpdateGarmentPreparingCommandHandler(IStorage storage)
        {
            _garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentPreparing> Handle(UpdateGarmentPreparingCommand request, CancellationToken cancellaitonToken)
        {
            var garmentPreparing = _garmentPreparingRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentPreparing == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            garmentPreparing.setUENId(request.UENId);
            garmentPreparing.setUENNo(request.UENNo);
            garmentPreparing.SetUnitId(request.UnitId);
            garmentPreparing.setProcessDate(request.ProcessDate);
            garmentPreparing.setRONo(request.RONo);
            garmentPreparing.setArticle(request.Article);
            garmentPreparing.setIsCuttingIN(request.IsCuttingIn);

            var dbGarmentPreparing = _garmentPreparingItemRepository.Find(y => y.GarmentPreparingId == garmentPreparing.Identity);
            var updatedItems = request.Items.Where(x => dbGarmentPreparing.Any(y => y.GarmentPreparingId == garmentPreparing.Identity));
            var addedItems = request.Items.Where(x => !dbGarmentPreparing.Any(y => y.GarmentPreparingId == garmentPreparing.Identity));
            var deletedItems = dbGarmentPreparing.Where(x => !request.Items.Any(y => y.GarmentPreparingId == garmentPreparing.Identity));

            foreach (var item in updatedItems)
            {
                var dbItem = dbGarmentPreparing.Find(x => x.Identity == item.Identity);
                dbItem.setBasicPrice(item.BasicPrice);
                dbItem.setDesignColor(item.DesignColor);
                dbItem.setFabricType(item.FabricType);
                dbItem.setProduct(item.Product);
                dbItem.setQuantity(item.Quantity);
                dbItem.setRemainingQuantity(item.RemainingQuantity);
                dbItem.setUenItemId(item.UENItemId);
                dbItem.setUomId(item.Uom);
                await _garmentPreparingItemRepository.Update(dbItem);
            }

            addedItems.Select(x => new GarmentPreparingItem(Guid.NewGuid(), x.UENItemId, x.Product, x.DesignColor, x.Quantity, x.Uom, x.FabricType, x.RemainingQuantity, x.BasicPrice, garmentPreparing.Identity)).ToList()
                .ForEach(async x => await _garmentPreparingItemRepository.Update(x));

            foreach (var item in deletedItems)
            {
                item.SetDeleted();
                await _garmentPreparingItemRepository.Update(item);
            }


            garmentPreparing.SetModified();

            await _garmentPreparingRepository.Update(garmentPreparing);

            _storage.Save();

            return garmentPreparing;
        }
    }
}