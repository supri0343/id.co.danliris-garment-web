using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAdjustments.CommandHandlers
{
    public class PlaceGarmentAdjustmentCommandHandler : ICommandHandler<PlaceGarmentAdjustmentCommand, GarmentAdjustment>
    {
        private readonly IStorage _storage;
        private readonly IGarmentAdjustmentRepository _garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository _garmentAdjustmentItemRepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public PlaceGarmentAdjustmentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
            _garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        public async Task<GarmentAdjustment> Handle(PlaceGarmentAdjustmentCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentAdjustment garmentAdjustment = new GarmentAdjustment(
                Guid.NewGuid(),
                GenerateAdjustmentNo(request),
                request.AdjustmentType,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.AdjustmentDate,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name
            );

            Dictionary<Guid, double> sewingDOItemToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentAdjustmentItem garmentAdjustmentItem = new GarmentAdjustmentItem(
                        Guid.NewGuid(),
                        garmentAdjustment.Identity,
                        item.SewingDOItemId,
                        item.SewingInItemId,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        item.Quantity,
                        item.BasicPrice,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        item.Price
                    );

                    if (sewingDOItemToBeUpdated.ContainsKey(item.SewingDOItemId))
                    {
                        sewingDOItemToBeUpdated[item.SewingDOItemId] += item.Quantity;
                    }
                    else
                    {
                        sewingDOItemToBeUpdated.Add(item.SewingDOItemId, item.Quantity);
                    }

                    await _garmentAdjustmentItemRepository.Update(garmentAdjustmentItem);
                }
            }

            foreach (var sewingDOItem in sewingDOItemToBeUpdated)
            {
                var garmentSewingDOItem = _garmentSewingDOItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingDOItem(s)).Single();
                garmentSewingDOItem.setRemainingQuantity(garmentSewingDOItem.RemainingQuantity - sewingDOItem.Value);
                garmentSewingDOItem.Modify();

                await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
            }

            await _garmentAdjustmentRepository.Update(garmentAdjustment);

            _storage.Save();

            return garmentAdjustment;
        }

        private string GenerateAdjustmentNo(PlaceGarmentAdjustmentCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"ADJ{unitcode}{year}{month}";
            if (request.AdjustmentType == "LOADING")
            {
                prefix = $"ADJL{unitcode}{year}{month}";
            }
            else if(request.AdjustmentType == "SEWING")
            {
                prefix = $"ADJS{unitcode}{year}{month}";
            }

            var lastAdjustmentNo = _garmentAdjustmentRepository.Query.Where(w => w.AdjustmentNo.StartsWith(prefix))
                .OrderByDescending(o => o.AdjustmentNo)
                .Select(s => int.Parse(s.AdjustmentNo.Replace(prefix, "")))
                .FirstOrDefault();
            var loadingNo = $"{prefix}{(lastAdjustmentNo + 1).ToString("D4")}";

            return loadingNo;
        }
    }
}
