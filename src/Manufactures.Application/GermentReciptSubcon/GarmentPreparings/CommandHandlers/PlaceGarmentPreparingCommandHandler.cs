using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.GermentReciptSubcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPreparings.CommandHandlers
{
    public class PlaceGarmentPreparingCommandHandler : ICommandHandler<PlaceSubconGarmentPreparingCommand, GarmentSubconPreparing>
    {
        private readonly IGarmentSubconPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentSubconPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        public PlaceGarmentPreparingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentSubconPreparingItemRepository>();
            _garmentPreparingRepository = storage.GetRepository<IGarmentSubconPreparingRepository>();
        }

        public async Task<GarmentSubconPreparing> Handle(PlaceSubconGarmentPreparingCommand request, CancellationToken cancellationToken)
        {
            //var garmentPreparing  = _garmentPreparingRepository.Find(o =>
            //                        o.UENId == request.UENId &&
            //                        o.UENNo == request.UENNo &&
            //                        o.UnitId == request.Unit.Id &&
            //                        o.UnitCode == request.Unit.Code &&
            //                        o.ProcessDate == request.ProcessDate &&
            //                        o.RONo == request.RONo &&
            //                        o.Article == request.Article &&
            //                        o.IsCuttingIn == request.IsCuttingIn).FirstOrDefault();
            //List<GarmentPreparingItem> garmentPreparingItem = new List<GarmentPreparingItem>();
            //if (garmentPreparing == null)
            //{
                var garmentPreparing = new GarmentSubconPreparing(Guid.NewGuid(), request.UENId, request.UENNo, new UnitDepartmentId(request.Unit.Id), request.Unit.Code, request.Unit.Name, request.ProcessDate, request.RONo,
                        request.Article, request.IsCuttingIn,new Domain.Shared.ValueObjects.BuyerId( request.Buyer.Id), request.Buyer.Code, request.Buyer.Name);
                request.Items.Select(x => new GarmentSubconPreparingItem
                (
                    Guid.NewGuid(),
                    x.UENItemId, 
                    new ProductId(x.Product.Id),
                    x.Product.Code, 
                    x.Product.Name, 
                    x.DesignColor, 
                    x.Quantity,
                    new UomId(x.Uom.Id),
                    x.Uom.Unit, 
                    x.FabricType, 
                    x.RemainingQuantity,
                    x.BasicPrice,
                    garmentPreparing.Identity,
                    x.ROSource,
                    x.BeacukaiNo,
                    x.BeacukaiDate,
                    x.BeacukaiType)).ToList()
                    .ForEach(async x => await _garmentPreparingItemRepository.Update(x));
            //}

            garmentPreparing.SetModified();

            await _garmentPreparingRepository.Update(garmentPreparing);

            _storage.Save();

            return garmentPreparing;

        }
    }
}
