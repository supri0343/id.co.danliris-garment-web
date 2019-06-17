using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Manufactures.Dtos;
using System;

namespace Manufactures.ViewModels.GarmentPreparings
{
    public class CreateViewModelMapper
    {
        public GarmentPreparing Map(CreateViewModel viewModel)
        {
            return new GarmentPreparing(Guid.NewGuid(), viewModel.UENId, viewModel.UENNo, new UnitDepartmentId(viewModel.UnitId), viewModel.ProcessDate,
               viewModel.RONo, viewModel.Article, viewModel.IsCuttingIn);
        }

        public GarmentPreparingItem MapItem(GarmentPreparingItemDto viewModel, Guid headerId)
        {
            return new GarmentPreparingItem(Guid.NewGuid(), viewModel.UENItemId, new ProductId(viewModel.Product.Id), viewModel.DesignColor, viewModel.Quantity, new UomId(viewModel.Uom.Id), viewModel.FabricType, viewModel.RemainingQuantity, viewModel.BasicPrice, headerId);
        }
    }
}