using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentSubconSewingOuts.CommandHandlers
{
    public class PlaceGarmentSubconSewingOutCommandHandler : ICommandHandler<PlaceGarmentSubconSewingOutCommand, GarmentSubconSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSubconSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSubconSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSubconSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSubconSewingInItemRepository _garmentSewingInItemRepository;
        //private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentSubconSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSubconSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSubconSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSubconSewingOutDetailRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSubconSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSubconSewingInItemRepository>();
            //_garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSubconSewingOut> Handle(PlaceGarmentSubconSewingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            Guid sewingOutId = Guid.NewGuid();
            string sewingOutNo = GenerateSewOutNo(request);

            
            GarmentSubconSewingOut garmentSewingOut = new GarmentSubconSewingOut(
                sewingOutId,
                sewingOutNo,
                new BuyerId(request.ProductOwner.Id),
                request.ProductOwner.Code,
                request.ProductOwner.Name,
                new UnitDepartmentId(request.UnitTo.Id),
                request.UnitTo.Code,
                request.UnitTo.Name,
                request.SewingTo,
                request.SewingOutDate.GetValueOrDefault(),
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
                    GarmentSubconSewingOutItem garmentSewingOutItem = new GarmentSubconSewingOutItem(
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
                        request.IsDifferentSize? item.TotalQuantity : item.Quantity,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        0,
                        item.BasicPrice,
                        item.Price
                    );
                    item.Id = garmentSewingOutItem.Identity;

                    if (request.IsDifferentSize)
                    {
                        foreach (var detail in item.Details)
                        {
                            GarmentSubconSewingOutDetail garmentSewingOutDetail = new GarmentSubconSewingOutDetail(
                                Guid.NewGuid(),
                                garmentSewingOutItem.Identity,
                                new SizeId(detail.Size.Id),
                                detail.Size.Size,
                                detail.Quantity,
                                new UomId(detail.Uom.Id),
                                detail.Uom.Unit,
                                0
                            );
                            detail.Id = garmentSewingOutDetail.Identity;

                            if (sewingInItemToBeUpdated.ContainsKey(item.SewingInItemId))
                            {
                                sewingInItemToBeUpdated[item.SewingInItemId] += detail.Quantity;
                            }
                            else
                            {
                                sewingInItemToBeUpdated.Add(item.SewingInItemId, detail.Quantity);
                            }
                            await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);
                        }
                    }
                    else
                    {
                        if (sewingInItemToBeUpdated.ContainsKey(item.SewingInItemId))
                        {
                            sewingInItemToBeUpdated[item.SewingInItemId] += item.Quantity;
                        }
                        else
                        {
                            sewingInItemToBeUpdated.Add(item.SewingInItemId, item.Quantity);
                        }
                    }
                    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                }
                
            }

            foreach (var sewInItem in sewingInItemToBeUpdated)
            {
                var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewInItem.Key).Select(s => new GarmentSubconSewingInItem(s)).Single();
                garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity - sewInItem.Value);
                garmentSewingInItem.Modify();

                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
            }

            await _garmentSewingOutRepository.Update(garmentSewingOut);

            _storage.Save();

            return garmentSewingOut;
        }

        private string GenerateSewOutNo(PlaceGarmentSubconSewingOutCommand request)
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