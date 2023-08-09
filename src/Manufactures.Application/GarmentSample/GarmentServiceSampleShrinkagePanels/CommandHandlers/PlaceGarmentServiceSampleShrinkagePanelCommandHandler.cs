using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.CommandHandlers
{
    public class PlaceGarmentServiceSampleShrinkagePanelCommandHandler : ICommandHandler<PlaceGarmentServiceSampleShrinkagePanelCommand, GarmentServiceSampleShrinkagePanel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleShrinkagePanelRepository _garmentServiceSampleShrinkagePanelRepository;
        private readonly IGarmentServiceSampleShrinkagePanelItemRepository _garmentServiceSampleShrinkagePanelItemRepository;
        private readonly IGarmentServiceSampleShrinkagePanelDetailRepository _garmentServiceSampleShrinkagePanelDetailRepository;

        public PlaceGarmentServiceSampleShrinkagePanelCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelRepository>();
            _garmentServiceSampleShrinkagePanelItemRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelItemRepository>();
            _garmentServiceSampleShrinkagePanelDetailRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelDetailRepository>();
        }

        public async Task<GarmentServiceSampleShrinkagePanel> Handle(PlaceGarmentServiceSampleShrinkagePanelCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentServiceSampleShrinkagePanel garmentServiceSampleShrinkagePanel = new GarmentServiceSampleShrinkagePanel(
                Guid.NewGuid(),
                GenerateServiceSampleShrinkagePanelNo(request),
                request.ServiceSubconShrinkagePanelDate.GetValueOrDefault(),
                request.Remark,
                request.IsUsed,
                request.QtyPacking,
                request.UomUnit,
                request.NettWeight,
                request.GrossWeight
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSampleShrinkagePanelItem garmentServiceSampleShrinkagePanelItem = new GarmentServiceSampleShrinkagePanelItem(
                    Guid.NewGuid(),
                    garmentServiceSampleShrinkagePanel.Identity,
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
                        GarmentServiceSampleShrinkagePanelDetail garmentServiceSampleShrinkagePanelDetail = new GarmentServiceSampleShrinkagePanelDetail(
                                     Guid.NewGuid(),
                                     garmentServiceSampleShrinkagePanelItem.Identity,
                                     new ProductId(detail.Product.Id),
                                     detail.Product.Code,
                                     detail.Product.Name,
                                     detail.Product.Remark,
                                     detail.DesignColor,
                                     detail.Quantity,
                                     new UomId(detail.Uom.Id),
                                     detail.Uom.Unit
                                 );
                        await _garmentServiceSampleShrinkagePanelDetailRepository.Update(garmentServiceSampleShrinkagePanelDetail);
                    }
                }
                await _garmentServiceSampleShrinkagePanelItemRepository.Update(garmentServiceSampleShrinkagePanelItem);
            }

            await _garmentServiceSampleShrinkagePanelRepository.Update(garmentServiceSampleShrinkagePanel);

            _storage.Save();

            return garmentServiceSampleShrinkagePanel;
        }

        private string GenerateServiceSampleShrinkagePanelNo(PlaceGarmentServiceSampleShrinkagePanelCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJSK{year}{month}";

            var lastServiceSampleShrinkagePanelNo = _garmentServiceSampleShrinkagePanelRepository.Query.Where(w => w.ServiceSampleShrinkagePanelNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSampleShrinkagePanelNo)
                .Select(s => int.Parse(s.ServiceSampleShrinkagePanelNo.Substring(8,4)))
                .FirstOrDefault();
            var ServiceSampleShrinkagePanelNo = $"{prefix}{(lastServiceSampleShrinkagePanelNo + 1).ToString("D4")}" + "-S";

            return ServiceSampleShrinkagePanelNo;
        }
    }
}
