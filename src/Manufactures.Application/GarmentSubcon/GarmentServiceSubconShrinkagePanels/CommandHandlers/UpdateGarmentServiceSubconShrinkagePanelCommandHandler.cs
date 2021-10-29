using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.CommandHandlers
{
    public class UpdateGarmentServiceSubconShrinkagePanelCommandHandler : ICommandHandler<UpdateGarmentServiceSubconShrinkagePanelCommand, GarmentServiceSubconShrinkagePanel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconShrinkagePanelItemRepository _garmentServiceSubconShrinkagePanelItemRepository;

        public UpdateGarmentServiceSubconShrinkagePanelCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconShrinkagePanelRepository = _storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconShrinkagePanelItemRepository = _storage.GetRepository<IGarmentServiceSubconShrinkagePanelItemRepository>();
        }

        public async Task<GarmentServiceSubconShrinkagePanel> Handle(UpdateGarmentServiceSubconShrinkagePanelCommand request, CancellationToken cancellationToken)
        {
            var serviceSubconShrinkagePanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconShrinkagePanel(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            serviceSubconShrinkagePanel.SetServiceSubconShrinkagePanelDate(request.ServiceSubconShrinkagePanelDate.GetValueOrDefault());
            serviceSubconShrinkagePanel.Modify();
            await _garmentServiceSubconShrinkagePanelRepository.Update(serviceSubconShrinkagePanel);

            _storage.Save();

            return serviceSubconShrinkagePanel;
        }
    }
}
