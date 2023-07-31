using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Domain.Commands;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
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
        private readonly IWebApiContext _webApiContext;
        private readonly IHttpClientService _http;
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconShrinkagePanelItemRepository _garmentServiceSubconShrinkagePanelItemRepository;
        private readonly IGarmentServiceSubconShrinkagePanelDetailRepository _garmentServiceSubconShrinkagePanelDetailRepository;

        public UpdateGarmentServiceSubconShrinkagePanelCommandHandler(IStorage storage, IWebApiContext webApiContext, IHttpClientService http)
        {
            _storage = storage;
            _http = http;
            _webApiContext = webApiContext;
            _garmentServiceSubconShrinkagePanelRepository = _storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconShrinkagePanelItemRepository = _storage.GetRepository<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _garmentServiceSubconShrinkagePanelDetailRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelDetailRepository>();
        }

        public async Task<GarmentServiceSubconShrinkagePanel> Handle(UpdateGarmentServiceSubconShrinkagePanelCommand request, CancellationToken cancellationToken)
        {
            var serviceSubconShrinkagePanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconShrinkagePanel(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            List<string> listDeletedUenNo = new List<string>();
            List<string> listUsedUenNo = new List<string>();
            _garmentServiceSubconShrinkagePanelItemRepository.Find(o => o.ServiceSubconShrinkagePanelId == serviceSubconShrinkagePanel.Identity).ForEach(async subconShrinkagePanelItem =>
            {
                var item = request.Items.Where(o => o.Id == subconShrinkagePanelItem.Identity).SingleOrDefault();

                if (item==null)
                {
                    listDeletedUenNo.Add(subconShrinkagePanelItem.UnitExpenditureNo);
                    _garmentServiceSubconShrinkagePanelDetailRepository.Find(i => i.ServiceSubconShrinkagePanelItemId == subconShrinkagePanelItem.Identity).ForEach(async subconDetail =>
                    {
                        subconDetail.Remove();
                        await _garmentServiceSubconShrinkagePanelDetailRepository.Update(subconDetail);
                    });
                    subconShrinkagePanelItem.Remove();

                }
                else
                {
                    _garmentServiceSubconShrinkagePanelDetailRepository.Find(i => i.ServiceSubconShrinkagePanelItemId == subconShrinkagePanelItem.Identity).ForEach(async subconDetail =>
                    {
                        var detail = item.Details.Where(o => o.Id == subconDetail.Identity).Single();
                        if (!detail.IsSave)
                        {
                            subconDetail.Remove();
                        }
                        else
                        {
                            subconDetail.SetQuantity(detail.Quantity);
                            subconDetail.Modify();
                        }
                        await _garmentServiceSubconShrinkagePanelDetailRepository.Update(subconDetail);
                  });
                    subconShrinkagePanelItem.Modify();
                }


                await _garmentServiceSubconShrinkagePanelItemRepository.Update(subconShrinkagePanelItem);
           });

            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    listUsedUenNo.Add(item.UnitExpenditureNo);
                    GarmentServiceSubconShrinkagePanelItem garmentServiceSubconShrinkagePanelItem = new GarmentServiceSubconShrinkagePanelItem(
                        Guid.NewGuid(),
                        serviceSubconShrinkagePanel.Identity,
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
                            GarmentServiceSubconShrinkagePanelDetail garmentServiceSubconShrinkagePanelDetail = new GarmentServiceSubconShrinkagePanelDetail(
                                         Guid.NewGuid(),
                                         garmentServiceSubconShrinkagePanelItem.Identity,
                                         new ProductId(detail.Product.Id),
                                         detail.Product.Code,
                                         detail.Product.Name,
                                         detail.Product.Remark,
                                         detail.DesignColor,
                                         detail.Quantity,
                                         new UomId(detail.Uom.Id),
                                         detail.Uom.Unit
                                     );
                            await _garmentServiceSubconShrinkagePanelDetailRepository.Update(garmentServiceSubconShrinkagePanelDetail);
                        }
                    }
                    await _garmentServiceSubconShrinkagePanelItemRepository.Update(garmentServiceSubconShrinkagePanelItem);
                }
            }

            serviceSubconShrinkagePanel.SetServiceSubconShrinkagePanelDate(request.ServiceSubconShrinkagePanelDate.GetValueOrDefault());
            serviceSubconShrinkagePanel.SetRemark(request.Remark);
            serviceSubconShrinkagePanel.SetQtyPacking(request.QtyPacking);
            serviceSubconShrinkagePanel.SetUomUnit(request.UomUnit);
            serviceSubconShrinkagePanel.SetNettWeight(request.NettWeight);
            serviceSubconShrinkagePanel.SetGrossWeight(request.GrossWeight);
            serviceSubconShrinkagePanel.Modify();
            await _garmentServiceSubconShrinkagePanelRepository.Update(serviceSubconShrinkagePanel);

            //Update Is Preparing UENNo
            if (listDeletedUenNo.Count() > 0)
            {
                var joinUenNo = string.Join(",", listDeletedUenNo.Distinct());
                await PutGarmentUnitExpenditureNoteByNo(joinUenNo, false);
            }

            if (listUsedUenNo.Count() > 0)
            {
                var joinUenNo = string.Join(",", listUsedUenNo.Distinct());
                await PutGarmentUnitExpenditureNoteByNo(joinUenNo, true);
            }

            _storage.Save();

            return serviceSubconShrinkagePanel;
        }
        public async Task<string> PutGarmentUnitExpenditureNoteByNo(string uenNo, bool isPreparing)
        {
            var garmentUnitExpenditureNoteUri = PurchasingDataSettings.Endpoint + $"garment-unit-expenditure-notes/isPreparingByNo?UENNo={uenNo}&IsPreparing={isPreparing}";
            var garmentUnitExpenditureNoteResponse = await _http.GetAsync(garmentUnitExpenditureNoteUri, _webApiContext.Token);

            return garmentUnitExpenditureNoteResponse.EnsureSuccessStatusCode().ToString();
        }
    }
}
