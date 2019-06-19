using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalProducts.CommandHandlers
{
    public class PlaceGarmentAvalProductCommandHandler : ICommandHandler<PlaceGarmentAvalProductCommand, GarmentAvalProduct>
    {
        private readonly IGarmentAvalProductRepository _garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository _garmentAvalProductItemRepository;
        private readonly IStorage _storage;

        public PlaceGarmentAvalProductCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            _garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
        }

        public async Task<GarmentAvalProduct> Handle(PlaceGarmentAvalProductCommand request, CancellationToken cancellationToken)
        {
            var garmentAvalProduct = _garmentAvalProductRepository.Find(o =>
                                   o.RONo == request.RONo &&
                                   o.Article == request.Article &&
                                   o.AvalDate == request.AvalDate).FirstOrDefault();
            List<GarmentAvalProductItem> garmentAvalProductItem = new List<GarmentAvalProductItem>();
            if (garmentAvalProduct == null)
            {
                garmentAvalProduct = new GarmentAvalProduct(Guid.NewGuid(), request.RONo, request.Article, request.AvalDate);
                request.Items.Select(x => new GarmentAvalProductItem(Guid.NewGuid(), garmentAvalProduct.Identity, x.PreparingId, x.PreparingItemId, x.ProductId, x.DesignColor, x.Quantity, x.UomId)).ToList()
                    .ForEach(async x => await _garmentAvalProductItemRepository.Update(x));
            }

            garmentAvalProduct.SetModified();

            await _garmentAvalProductRepository.Update(garmentAvalProduct);

            _storage.Save();

            return garmentAvalProduct;

        }
    }
}