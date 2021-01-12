using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers
{
    public class PlaceGarmentServiceSubconCuttingCommandHandler : ICommandHandler<PlaceGarmentServiceSubconCuttingCommand, GarmentServiceSubconCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconCuttingRepository _garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository _garmentServiceSubconCuttingItemRepository;

        public PlaceGarmentServiceSubconCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
        }

        public async Task<GarmentServiceSubconCutting> Handle(PlaceGarmentServiceSubconCuttingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave).ToList();

            GarmentServiceSubconCutting garmentServiceSubconCutting = new GarmentServiceSubconCutting(
                Guid.NewGuid(),
                GenerateSubconNo(request),
                request.SubconType,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.SubconDate.GetValueOrDefault()
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSubconCuttingItem garmentServiceSubconCuttingItem = new GarmentServiceSubconCuttingItem(
                    Guid.NewGuid(),
                    garmentServiceSubconCutting.Identity,
                    item.CuttingInDetailId,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.Quantity
                );
                await _garmentServiceSubconCuttingItemRepository.Update(garmentServiceSubconCuttingItem);
            }

            await _garmentServiceSubconCuttingRepository.Update(garmentServiceSubconCutting);

            _storage.Save();

            return garmentServiceSubconCutting;
        }

        private string GenerateSubconNo(PlaceGarmentServiceSubconCuttingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.SubconType == "BORDIR" ? "B" : request.SubconType == "PRINT" ? "P" : "PL";

            var prefix = $"SJC{code}{year}{month}";

            var lastSubconNo = _garmentServiceSubconCuttingRepository.Query.Where(w => w.SubconNo.StartsWith(prefix))
                .OrderByDescending(o => o.SubconNo)
                .Select(s => int.Parse(s.SubconNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutInNo = $"{prefix}{(lastSubconNo + 1).ToString("D4")}";

            return CutInNo;
        }
    }
}