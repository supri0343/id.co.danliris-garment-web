using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentSewingIns.CommandHandlers
{
    public class PlaceGarmentSewingInCommandHandler : ICommandHandler<PlaceGarmentSubconSewingInCommand, GarmentSubconSewingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSubconSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentSubconLoadingOutItemRepository _garmentLoadingOutItemRepository;
        private readonly IGarmentSubconLoadingInItemRepository _garmentLoadingInItemRepository;
        //private readonly IGarmentSubconSewingOutItemRepository _garmentSewingOutItemRepository;
        //private readonly IGarmentSubconFinishingOutItemRepository _garmentFinishingOutItemRepository;

        public PlaceGarmentSewingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingInRepository = storage.GetRepository<IGarmentSubconSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSubconSewingInItemRepository>();
            _garmentLoadingOutItemRepository = storage.GetRepository<IGarmentSubconLoadingOutItemRepository>();
            _garmentLoadingInItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();

        }

        public async Task<GarmentSubconSewingIn> Handle(PlaceGarmentSubconSewingInCommand request, CancellationToken cancellationToken)
        {
            //request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentSubconSewingIn garmentSewingIn = new GarmentSubconSewingIn(
                Guid.NewGuid(),
                GenerateSewingInNo(request),
                request.SewingFrom,
                request.LoadingOutId,
                request.LoadingOutNo,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.SewingInDate.GetValueOrDefault(),
                false
            );

            foreach (var item in request.Items)
            {
                if (item.IsSave == true)
                {
                    GarmentSubconSewingInItem garmentSewingInItem = new GarmentSubconSewingInItem(
                        Guid.NewGuid(),
                        garmentSewingIn.Identity,
                        item.SewingOutItemId,
                        item.SewingOutDetailId,
                        item.LoadingOutItemId,
                        item.FinishingOutItemId,
                        item.FinishingOutDetailId,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        item.Quantity,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        item.Quantity,
                        item.BasicPrice,
                        item.Price
                    );

                    if (request.SewingFrom == "CUTTING")
                    {
                        var garmentLoadingItem = _garmentLoadingOutItemRepository.Query.Where(o => o.Identity == item.LoadingOutItemId).Select(s => new GarmentSubconLoadingOutItem(s)).Single();

                        double diffQty = garmentLoadingItem.Quantity - item.Quantity;
                        garmentLoadingItem.SetRealQtyOut(item.Quantity);

                        garmentLoadingItem.Modify();
                        await _garmentLoadingOutItemRepository.Update(garmentLoadingItem);

                        if (diffQty > 0) 
                        {
                            var garmentCuttingInItem = _garmentLoadingInItemRepository.Query.Where(x => x.Identity == garmentLoadingItem.LoadingInItemId).Select(s => new GarmentSubconLoadingInItem(s)).Single();

                            garmentCuttingInItem.SetRemainingQuantity(garmentCuttingInItem.RemainingQuantity + diffQty);

                            garmentCuttingInItem.Modify();

                            await _garmentLoadingInItemRepository.Update(garmentCuttingInItem);
                        }
                    }
                    //else if(request.SewingFrom == "SEWING")
                    //{
                    //    var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(s => s.Identity == item.SewingOutItemId).Select(s => new GarmentSewingOutItem(s)).Single();

                    //    garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity - item.Quantity);

                    //    garmentSewingOutItem.Modify();
                    //    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                    //}
                    //else if (request.SewingFrom == "FINISHING")
                    //{
                    //    var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(s => s.Identity == item.FinishingOutItemId).Select(s => new GarmentFinishingOutItem(s)).Single();

                    //    garmentFinishingOutItem.SetRemainingQuantity(garmentFinishingOutItem.RemainingQuantity - item.Quantity);

                    //    garmentFinishingOutItem.Modify();
                    //    await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                    //}

                    await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                }
                else
                {
                    // Remove sewing out when data Item not selected
                    if (request.SewingFrom == "CUTTING")
                    {
                        var garmentLoadingItem = _garmentLoadingOutItemRepository.Query.Where(o => o.Identity == item.LoadingOutItemId).Select(s => new GarmentSubconLoadingOutItem(s)).Single();

                        double diffQty = garmentLoadingItem.Quantity;
                
                        if (diffQty > 0)
                        {
                            var garmentCuttingInItem = _garmentLoadingInItemRepository.Query.Where(x => x.Identity == garmentLoadingItem.LoadingInItemId).Select(s => new GarmentSubconLoadingInItem(s)).Single();

                            garmentCuttingInItem.SetRemainingQuantity(garmentCuttingInItem.RemainingQuantity + diffQty);

                            garmentCuttingInItem.Modify();

                            await _garmentLoadingInItemRepository.Update(garmentCuttingInItem);
                        }

                        garmentLoadingItem.Remove();

                        await _garmentLoadingOutItemRepository.Update(garmentLoadingItem);
                    }
                }
            }

            await _garmentSewingInRepository.Update(garmentSewingIn);

            _storage.Save();

            return garmentSewingIn;
        }

        private string GenerateSewingInNo(PlaceGarmentSubconSewingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"SI{request.Unit.Code}{year}{month}";

            var lastSewingInNo = _garmentSewingInRepository.Query.Where(w => w.SewingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingInNo)
                .Select(s => int.Parse(s.SewingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingInNo = $"{prefix}{(lastSewingInNo + 1).ToString("D4")}";

            return SewingInNo;
        }
    }
}