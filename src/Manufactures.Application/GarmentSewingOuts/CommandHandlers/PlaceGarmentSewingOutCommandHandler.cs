using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.CommandHandlers
{
    public class PlaceGarmentSewingOutCommandHandler : ICommandHandler<PlaceGarmentSewingOutCommand, GarmentSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public PlaceGarmentSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        public async Task<GarmentSewingOut> Handle(PlaceGarmentSewingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentSewingOut garmentSewingOut = new GarmentSewingOut(
                Guid.NewGuid(),
                GenerateSewOutNo(request),
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                new UnitDepartmentId(request.UnitTo.Id),
                request.UnitTo.Code,
                request.UnitTo.Name,
                request.SewingTo,
                request.SewingOutDate,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name, 
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.IsDifferentSize
            );

            Dictionary<Guid, double> sewingInItemToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentSewingOutItem garmentSewingOutItem = new GarmentSewingOutItem(
                        Guid.NewGuid(),
                        garmentSewingOut.Identity,
                        item.SewingInId,
                        item.SewingInItemId,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        item.Quantity,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color
                    );

                    foreach (var detail in item.Details)
                    {
                        GarmentSewingOutDetail garmentSewingOutDetail = new GarmentSewingOutDetail(
                            Guid.NewGuid(),
                            garmentSewingOutItem.Identity,
                            new SizeId(detail.Size.Id),
                            detail.Size.Size,
                            detail.Quantity,
                            new UomId(detail.Uom.Id),
                            detail.Uom.Unit
                        );

                        await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);

                    }

                    if (sewingInItemToBeUpdated.ContainsKey(item.SewingInItemId))
                    {
                        sewingInItemToBeUpdated[item.SewingInItemId] += item.Quantity;
                    }
                    else
                    {
                        sewingInItemToBeUpdated.Add(item.SewingInItemId, item.Quantity);
                    }

                    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                }


                
            }

            foreach (var sewInItem in sewingInItemToBeUpdated)
            {
                var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
                garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity - sewInItem.Value);
                garmentSewingInItem.Modify();

                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
            }


            await _garmentSewingOutRepository.Update(garmentSewingOut);

            _storage.Save();

            return garmentSewingOut;
        }

        private string GenerateSewOutNo(PlaceGarmentSewingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SO{request.Unit.Code.Trim()}{year}{month}";

            var lastSewOutNo = _garmentSewingOutRepository.Query.Where(w => w.SewingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingOutNo)
                .Select(s => int.Parse(s.SewingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewOutNo = $"{prefix}{(lastSewOutNo + 1).ToString("D4")}";

            return SewOutNo;
        }
    }
}