using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalProducts.CommandHandlers
{
    public class RemoveGarmentAvalProductCommandHandler : ICommandHandler<RemoveGarmentAvalProductCommand, GarmentAvalProduct>
    {
        private readonly IGarmentAvalProductRepository _garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository _garmentAvalProductItemRepository;
        private readonly IStorage _storage;

        public RemoveGarmentAvalProductCommandHandler(IStorage storage)
        {
            _garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
            _garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentAvalProduct> Handle(RemoveGarmentAvalProductCommand request, CancellationToken cancellationToken)
        {
            var garmentAvalProduct = _garmentAvalProductRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentAvalProduct == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            var garmentAvalProductItems = _garmentAvalProductItemRepository.Find(x => x.APId == request.Id);

            foreach (var item in garmentAvalProductItems)
            {
                item.Remove();
                await _garmentAvalProductItemRepository.Update(item);
            }

            garmentAvalProduct.Remove();

            await _garmentAvalProductRepository.Update(garmentAvalProduct);

            _storage.Save();

            return garmentAvalProduct;
        }
    }
}