using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentDeliveryReturns.CommandHandlers
{
    public class UpdateGarmentDeliveryReturnCommandHandler : ICommandHandler<UpdateGarmentDeliveryReturnCommand, GarmentDeliveryReturn>
    {
        private readonly IGarmentDeliveryReturnRepository _garmentDeliveryReturnRepository;
        private readonly IGarmentDeliveryReturnItemRepository _garmentDeliveryReturnItemRepository;
        private readonly IStorage _storage;

        public UpdateGarmentDeliveryReturnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentDeliveryReturnRepository = storage.GetRepository<IGarmentDeliveryReturnRepository>();
            _garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
        }

        public async Task<GarmentDeliveryReturn> Handle(UpdateGarmentDeliveryReturnCommand request, CancellationToken cancellaitonToken)
        {
            var garmentDeliveryReturn = _garmentDeliveryReturnRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentDeliveryReturn == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            garmentDeliveryReturn.setDRNo(request.DRNo);
            garmentDeliveryReturn.setRONo(request.RONo);
            garmentDeliveryReturn.setArticle(request.Article);
            garmentDeliveryReturn.setUnitDOId(request.UnitDOId);
            garmentDeliveryReturn.setUnitDONo(request.UnitDONo);
            garmentDeliveryReturn.setUENId(request.UENId);
            garmentDeliveryReturn.setPreparingId(request.PreparingId);
            garmentDeliveryReturn.setReturnDate(request.ReturnDate);
            garmentDeliveryReturn.setReturnType(request.ReturnType);
            garmentDeliveryReturn.setUnitId(new UnitDepartmentId(request.Unit.Id));
            garmentDeliveryReturn.setUnitCode(request.Unit.Code);
            garmentDeliveryReturn.setUnitName(request.Unit.Name);
            garmentDeliveryReturn.setStorageId(new StorageId(request.Storage.Id));
            garmentDeliveryReturn.setStorageCode(request.Storage.Code);
            garmentDeliveryReturn.setStorageName(request.Storage.Name);
            garmentDeliveryReturn.setIsUsed(request.IsUsed);

            var dbGarmentDeliveryReturnItem = _garmentDeliveryReturnItemRepository.Find(y => y.DRId == garmentDeliveryReturn.Identity);
            var updatedItems = request.Items.Where(x => dbGarmentDeliveryReturnItem.Any(y => y.DRId == garmentDeliveryReturn.Identity));
            var addedItems = request.Items.Where(x => !dbGarmentDeliveryReturnItem.Any(y => y.DRId == garmentDeliveryReturn.Identity));
            var deletedItems = dbGarmentDeliveryReturnItem.Where(x => !request.Items.Any(y => y.DRId == garmentDeliveryReturn.Identity));

            foreach (var item in updatedItems)
            {
                var dbItem = dbGarmentDeliveryReturnItem.Find(x => x.Identity == item.Identity);
                dbItem.setUnitDOItemId(item.UnitDOItemId);
                dbItem.setUENItemId(item.UENItemId);
                dbItem.setPreparingItemId(item.PreparingItemId);
                dbItem.setProductId(new ProductId(item.Product.Id));
                dbItem.setProductCode(item.Product.Code);
                dbItem.setProductName(item.Product.Name);
                dbItem.setDesignColor(item.DesignColor);
                dbItem.setRONo(item.RONo);
                dbItem.setQuantity(item.Quantity);
                dbItem.setUomId(new UomId(item.Uom.Id));
                dbItem.setUomUnit(item.Uom.Unit);
                
                await _garmentDeliveryReturnItemRepository.Update(dbItem);
            }

            addedItems.Select(x => new GarmentDeliveryReturnItem(Guid.NewGuid(), garmentDeliveryReturn.Identity, x.UnitDOItemId, x.UENItemId, x.PreparingItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.RONo, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit)).ToList()
                .ForEach(async x => await _garmentDeliveryReturnItemRepository.Update(x));

            foreach (var item in deletedItems)
            {
                item.SetDeleted();
                await _garmentDeliveryReturnItemRepository.Update(item);
            }


            garmentDeliveryReturn.SetModified();

            await _garmentDeliveryReturnRepository.Update(garmentDeliveryReturn);

            _storage.Save();

            return garmentDeliveryReturn;
        }
    }
}