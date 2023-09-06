using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.Commands;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPackingOuts.CommandHandlers
{
    public class PlaceGarmentSubconPackingOutCommandHandler : ICommandHandler<PlaceGarmentSubconPackingOutCommand, GarmentSubconPackingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconPackingOutRepository _garmentPackingOutRepository;
        private readonly IGarmentSubconPackingOutItemRepository _garmentPackingOutItemRepository;

        private readonly IGarmentSubconPackingInItemRepository _garmentPackingInItemRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        public PlaceGarmentSubconPackingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentPackingOutRepository = storage.GetRepository<IGarmentSubconPackingOutRepository>();
            _garmentPackingOutItemRepository = storage.GetRepository<IGarmentSubconPackingOutItemRepository>();

            _garmentPackingInItemRepository = storage.GetRepository<IGarmentSubconPackingInItemRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSubconPackingOut> Handle(PlaceGarmentSubconPackingOutCommand request, CancellationToken cancellationToken)
        {
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();
            request.Items = request.Items.ToList();

            GarmentSubconPackingOut garmentPackingOut = new GarmentSubconPackingOut(
                    Guid.NewGuid(),
               GeneratePackingOutNo(request),
                request.PackingOutType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                new BuyerId(request.ProductOwner.Id),
                request.ProductOwner.Code,
                request.ProductOwner.Name,
                request.PackingOutDate,
                request.Invoice,
                request.ContractNo,
                request.Carton,
                request.Description,
                request.IsReceived,
                request.PackingListId
                );

            foreach (var item in request.Items)
            {
                if (item.isSave)
                {
                    item.Price = (item.BasicPrice + ((double)garmentComodityPrice.Price * 1)) * item.Quantity;

                    GarmentSubconPackingOutItem garmentPackingOutItem = new GarmentSubconPackingOutItem(
                        Guid.NewGuid(),
                        garmentPackingOut.Identity,
                        item.PackingInItemId,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        item.Quantity,
                        0,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Description,
                        item.BasicPrice,
                        (item.BasicPrice + (double)garmentComodityPrice.Price) * item.Quantity
                       );

                    await _garmentPackingOutItemRepository.Update(garmentPackingOutItem);

                    var packingInTtem = _garmentPackingInItemRepository.Query.Where(x => x.Identity == item.PackingInItemId).Select(o => new GarmentSubconPackingInItem(o)).Single();

                    packingInTtem.SetRemainingQuantity(packingInTtem.RemainingQuantity - item.Quantity);
                    packingInTtem.Modify();

                    await _garmentPackingInItemRepository.Update(packingInTtem);    
                }
            }

            await _garmentPackingOutRepository.Update(garmentPackingOut);

            _storage.Save();

            return garmentPackingOut;

        }

        private string GeneratePackingOutNo(PlaceGarmentSubconPackingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"PO{unitcode}{year}{month}";

            var lastPackingOutNo = _garmentPackingOutRepository.Query.Where(w => w.PackingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.PackingOutNo)
                .Select(s => int.Parse(s.PackingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastPackingOutNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}
