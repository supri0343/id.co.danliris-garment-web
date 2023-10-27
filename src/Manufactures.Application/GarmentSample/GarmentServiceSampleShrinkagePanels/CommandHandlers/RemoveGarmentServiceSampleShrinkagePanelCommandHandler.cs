using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.CommandHandlers
{
    public class RemoveGarmentServiceSampleShrinkagePanelCommandHandler : ICommandHandler<RemoveGarmentServiceSampleShrinkagePanelCommand, GarmentServiceSampleShrinkagePanel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleShrinkagePanelRepository _garmentServiceSampleShrinkagePanelRepository;
        private readonly IGarmentServiceSampleShrinkagePanelItemRepository _garmentServiceSampleShrinkagePanelItemRepository;
        private readonly IGarmentServiceSampleShrinkagePanelDetailRepository _garmentServiceSampleShrinkagePanelDetailRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public RemoveGarmentServiceSampleShrinkagePanelCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelRepository>();
            _garmentServiceSampleShrinkagePanelItemRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelItemRepository>();
            _garmentServiceSampleShrinkagePanelDetailRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelDetailRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentServiceSampleShrinkagePanel> Handle(RemoveGarmentServiceSampleShrinkagePanelCommand request, CancellationToken cancellationToken)
        {
            var serviceSampleShrinkagePanel = _garmentServiceSampleShrinkagePanelRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleShrinkagePanel(o)).Single();

            _garmentServiceSampleShrinkagePanelItemRepository.Find(o => o.ServiceSampleShrinkagePanelId == serviceSampleShrinkagePanel.Identity).ForEach(async serviceSampleShrinkagePanelItem =>
            {
                _garmentServiceSampleShrinkagePanelDetailRepository.Find(i => i.ServiceSampleShrinkagePanelItemId == serviceSampleShrinkagePanelItem.Identity).ForEach(async serviceSampleShrinkagePanelDetail =>
                {
                    serviceSampleShrinkagePanelDetail.Remove();
                    await _garmentServiceSampleShrinkagePanelDetailRepository.Update(serviceSampleShrinkagePanelDetail);
                });
                serviceSampleShrinkagePanelItem.Remove();
                await _garmentServiceSampleShrinkagePanelItemRepository.Update(serviceSampleShrinkagePanelItem);
            });

            serviceSampleShrinkagePanel.Remove();
            await _garmentServiceSampleShrinkagePanelRepository.Update(serviceSampleShrinkagePanel);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Delete Packing List Subcon Sample - BB Shrinkage / Panel - " + serviceSampleShrinkagePanel.ServiceSampleShrinkagePanelNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return serviceSampleShrinkagePanel;
        }
    }
}
