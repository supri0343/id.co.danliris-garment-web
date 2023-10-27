﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.CommandHandlers
{
    public class RemoveGarmentServiceSubconShrinkagePanelCommandHandler : ICommandHandler<RemoveGarmentServiceSubconShrinkagePanelCommand, GarmentServiceSubconShrinkagePanel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconShrinkagePanelItemRepository _garmentServiceSubconShrinkagePanelItemRepository;
        private readonly IGarmentServiceSubconShrinkagePanelDetailRepository _garmentServiceSubconShrinkagePanelDetailRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public RemoveGarmentServiceSubconShrinkagePanelCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconShrinkagePanelItemRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _garmentServiceSubconShrinkagePanelDetailRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelDetailRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentServiceSubconShrinkagePanel> Handle(RemoveGarmentServiceSubconShrinkagePanelCommand request, CancellationToken cancellationToken)
        {
            var serviceSubconShrinkagePanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconShrinkagePanel(o)).Single();

            _garmentServiceSubconShrinkagePanelItemRepository.Find(o => o.ServiceSubconShrinkagePanelId == serviceSubconShrinkagePanel.Identity).ForEach(async serviceSubconShrinkagePanelItem =>
            {
                _garmentServiceSubconShrinkagePanelDetailRepository.Find(i => i.ServiceSubconShrinkagePanelItemId == serviceSubconShrinkagePanelItem.Identity).ForEach(async serviceSubconShrinkagePanelDetail =>
                {
                    serviceSubconShrinkagePanelDetail.Remove();
                    await _garmentServiceSubconShrinkagePanelDetailRepository.Update(serviceSubconShrinkagePanelDetail);
                });
                serviceSubconShrinkagePanelItem.Remove();
                await _garmentServiceSubconShrinkagePanelItemRepository.Update(serviceSubconShrinkagePanelItem);
            });

            serviceSubconShrinkagePanel.Remove();
            await _garmentServiceSubconShrinkagePanelRepository.Update(serviceSubconShrinkagePanel);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Delete Packing List Subcon - BB Shrinkage / Panel - " + serviceSubconShrinkagePanel.ServiceSubconShrinkagePanelNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return serviceSubconShrinkagePanel;
        }
    }
}
