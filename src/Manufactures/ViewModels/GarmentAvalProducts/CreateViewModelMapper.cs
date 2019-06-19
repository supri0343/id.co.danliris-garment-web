using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Dtos;
using System;

namespace Manufactures.ViewModels.GarmentAvalProducts
{
    public class CreateViewModelMapper
    {
        public GarmentAvalProduct Map(CreateViewModel viewModel)
        {
            return new GarmentAvalProduct(Guid.NewGuid(), viewModel.RONo, viewModel.Article, viewModel.AvalDate);
        }

        public GarmentAvalProductItem MapItem(GarmentAvalProductItemDto viewModel, Guid headerId)
        {
            return new GarmentAvalProductItem(Guid.NewGuid(), headerId, new GarmentPreparingId(viewModel.PreparingId.Id), new GarmentPreparingItemId(viewModel.PreparingItemId.Id), new ProductId(viewModel.ProductId.Id), viewModel.DesignColor, viewModel.Quantity, new UomId(viewModel.UomId.Id));
        }
    }
}