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
        private readonly IGarmentServiceSubconCuttingDetailRepository _garmentServiceSubconCuttingDetailRepository;

        public PlaceGarmentServiceSubconCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
            _garmentServiceSubconCuttingDetailRepository= storage.GetRepository<IGarmentServiceSubconCuttingDetailRepository>();
        }

        public async Task<GarmentServiceSubconCutting> Handle(PlaceGarmentServiceSubconCuttingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.Details.Where(detail => detail.IsSave).Count() > 0).ToList();

            GarmentServiceSubconCutting garmentServiceSubconCutting = new GarmentServiceSubconCutting(
                Guid.NewGuid(),
                GenerateSubconNo(request),
                request.SubconType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.SubconDate.GetValueOrDefault(),
                request.IsUsed
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSubconCuttingItem garmentServiceSubconCuttingItem = new GarmentServiceSubconCuttingItem(
                    Guid.NewGuid(),
                    garmentServiceSubconCutting.Identity,
                    item.CuttingInId,
                    item.RONo,
                    item.Article,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name
                );

                foreach(var detail in item.Details)
                {
                    if (detail.IsSave)
                    {
                        GarmentServiceSubconCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSubconCuttingDetail(
                            Guid.NewGuid(),
                            garmentServiceSubconCuttingItem.Identity,
                            detail.CuttingInDetailId,
                            new ProductId(detail.Product.Id),
                            detail.Product.Code,
                            detail.Product.Name,
                            detail.DesignColor,
                            detail.Quantity
                            );
                        await _garmentServiceSubconCuttingDetailRepository.Update(garmentServiceSubconCuttingDetail);
                    }
                }
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
            var code = request.SubconType == "BORDIR" ? "B" : request.SubconType == "PRINT" ? "PR" : "PL";

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