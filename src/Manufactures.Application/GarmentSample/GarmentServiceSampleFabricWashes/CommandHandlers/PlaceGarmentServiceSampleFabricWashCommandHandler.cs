using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleFabricWashes.CommandHandlers
{
    public class PlaceGarmentServiceSampleFabricWashCommandHandler : ICommandHandler<PlaceGarmentServiceSampleFabricWashCommand, GarmentServiceSampleFabricWash>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleFabricWashRepository _garmentServiceSampleFabricWashRepository;
        private readonly IGarmentServiceSampleFabricWashItemRepository _garmentServiceSampleFabricWashItemRepository;
        private readonly IGarmentServiceSampleFabricWashDetailRepository _garmentServiceSampleFabricWashDetailRepository;

        public PlaceGarmentServiceSampleFabricWashCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleFabricWashRepository = storage.GetRepository<IGarmentServiceSampleFabricWashRepository>();
            _garmentServiceSampleFabricWashItemRepository = storage.GetRepository<IGarmentServiceSampleFabricWashItemRepository>();
            _garmentServiceSampleFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSampleFabricWashDetailRepository>();
        }

        public async Task<GarmentServiceSampleFabricWash> Handle(PlaceGarmentServiceSampleFabricWashCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentServiceSampleFabricWash garmentServiceSampleFabricWash = new GarmentServiceSampleFabricWash(
                Guid.NewGuid(),
                GenerateServiceSampleFabricWashNo(request),
                request.ServiceSubconFabricWashDate.GetValueOrDefault(),
                request.Remark,
                request.IsUsed,
                request.QtyPacking,
                request.UomUnit,
                request.NettWeight,
                request.GrossWeight
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSampleFabricWashItem garmentServiceSampleFabricWashItem = new GarmentServiceSampleFabricWashItem(
                    Guid.NewGuid(),
                    garmentServiceSampleFabricWash.Identity,
                    item.UnitExpenditureNo,
                    item.ExpenditureDate,
                    new UnitSenderId(item.UnitSender.Id),
                    item.UnitSender.Code,
                    item.UnitSender.Name,
                    new UnitRequestId(item.UnitRequest.Id),
                    item.UnitRequest.Code,
                    item.UnitRequest.Name
                );

                foreach (var detail in item.Details)
                {
                    if (detail.IsSave)
                    {
                        GarmentServiceSampleFabricWashDetail garmentServiceSampleFabricWashDetail = new GarmentServiceSampleFabricWashDetail(
                                     Guid.NewGuid(),
                                     garmentServiceSampleFabricWashItem.Identity,
                                     new ProductId(detail.Product.Id),
                                     detail.Product.Code,
                                     detail.Product.Name,
                                     detail.Product.Remark,
                                     detail.DesignColor,
                                     detail.Quantity,
                                     new UomId(detail.Uom.Id),
                                     detail.Uom.Unit
                                 );
                        await _garmentServiceSampleFabricWashDetailRepository.Update(garmentServiceSampleFabricWashDetail);
                    }
                }
                await _garmentServiceSampleFabricWashItemRepository.Update(garmentServiceSampleFabricWashItem);
            }

            await _garmentServiceSampleFabricWashRepository.Update(garmentServiceSampleFabricWash);

            _storage.Save();

            return garmentServiceSampleFabricWash;
        }

        private string GenerateServiceSampleFabricWashNo(PlaceGarmentServiceSampleFabricWashCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJF{year}{month}";

            var lastServiceSampleFabricWashNo = _garmentServiceSampleFabricWashRepository.Query.Where(w => w.ServiceSampleFabricWashNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSampleFabricWashNo)
                .Select(s => int.Parse(s.ServiceSampleFabricWashNo.Substring(7, 4)))
                .FirstOrDefault();
            var ServiceSampleFabricWashNo = $"{prefix}{(lastServiceSampleFabricWashNo + 1).ToString("D4")}" + "-S";

            return ServiceSampleFabricWashNo;
        }
    }
}
