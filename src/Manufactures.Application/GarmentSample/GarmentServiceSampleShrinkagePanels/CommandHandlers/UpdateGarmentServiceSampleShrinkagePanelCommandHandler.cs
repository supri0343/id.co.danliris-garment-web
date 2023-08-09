using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Domain.Commands;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.CommandHandlers
{
    public class UpdateGarmentServiceSampleShrinkagePanelCommandHandler : ICommandHandler<UpdateGarmentServiceSampleShrinkagePanelCommand, GarmentServiceSampleShrinkagePanel>
    {
        private readonly IStorage _storage;
        private readonly IWebApiContext _webApiContext;
        private readonly IHttpClientService _http;
        private readonly IGarmentServiceSampleShrinkagePanelRepository _garmentServiceSampleShrinkagePanelRepository;
        private readonly IGarmentServiceSampleShrinkagePanelItemRepository _garmentServiceSampleShrinkagePanelItemRepository;
        private readonly IGarmentServiceSampleShrinkagePanelDetailRepository _garmentServiceSampleShrinkagePanelDetailRepository;

        public UpdateGarmentServiceSampleShrinkagePanelCommandHandler(IStorage storage, IWebApiContext webApiContext, IHttpClientService http)
        {
            _storage = storage;
            _http = http;
            _webApiContext = webApiContext;
            _garmentServiceSampleShrinkagePanelRepository = _storage.GetRepository<IGarmentServiceSampleShrinkagePanelRepository>();
            _garmentServiceSampleShrinkagePanelItemRepository = _storage.GetRepository<IGarmentServiceSampleShrinkagePanelItemRepository>();
            _garmentServiceSampleShrinkagePanelDetailRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelDetailRepository>();
        }

        public async Task<GarmentServiceSampleShrinkagePanel> Handle(UpdateGarmentServiceSampleShrinkagePanelCommand request, CancellationToken cancellationToken)
        {
            var serviceSampleShrinkagePanel = _garmentServiceSampleShrinkagePanelRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleShrinkagePanel(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();
            List<string> listDeletedUenNo = new List<string>();
            List<string> listUsedUenNo = new List<string>();
            _garmentServiceSampleShrinkagePanelItemRepository.Find(o => o.ServiceSampleShrinkagePanelId == serviceSampleShrinkagePanel.Identity).ForEach(async SampleShrinkagePanelItem =>
            {
                var item = request.Items.Where(o => o.Id == SampleShrinkagePanelItem.Identity).SingleOrDefault();

                if (item==null)
                {
                    listDeletedUenNo.Add(SampleShrinkagePanelItem.UnitExpenditureNo);
                    _garmentServiceSampleShrinkagePanelDetailRepository.Find(i => i.ServiceSampleShrinkagePanelItemId == SampleShrinkagePanelItem.Identity).ForEach(async SampleDetail =>
                    {
                        SampleDetail.Remove();
                        await _garmentServiceSampleShrinkagePanelDetailRepository.Update(SampleDetail);
                    });
                    SampleShrinkagePanelItem.Remove();

                }
                else
                {
                    _garmentServiceSampleShrinkagePanelDetailRepository.Find(i => i.ServiceSampleShrinkagePanelItemId == SampleShrinkagePanelItem.Identity).ForEach(async SampleDetail =>
                    {
                        var detail = item.Details.Where(o => o.Id == SampleDetail.Identity).Single();
                        if (!detail.IsSave)
                        {
                            SampleDetail.Remove();
                        }
                        else
                        {
                            SampleDetail.SetQuantity(detail.Quantity);
                            SampleDetail.Modify();
                        }
                        await _garmentServiceSampleShrinkagePanelDetailRepository.Update(SampleDetail);
                  });
                    SampleShrinkagePanelItem.Modify();
                }


                await _garmentServiceSampleShrinkagePanelItemRepository.Update(SampleShrinkagePanelItem);
           });

            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    listUsedUenNo.Add(item.UnitExpenditureNo);

                    GarmentServiceSampleShrinkagePanelItem garmentServiceSampleShrinkagePanelItem = new GarmentServiceSampleShrinkagePanelItem(
                        Guid.NewGuid(),
                        serviceSampleShrinkagePanel.Identity,
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
            }

            serviceSampleShrinkagePanel.SetServiceSampleShrinkagePanelDate(request.ServiceSubconShrinkagePanelDate.GetValueOrDefault());
            serviceSampleShrinkagePanel.SetRemark(request.Remark);
            serviceSampleShrinkagePanel.SetQtyPacking(request.QtyPacking);
            serviceSampleShrinkagePanel.SetUomUnit(request.UomUnit);
            serviceSampleShrinkagePanel.SetNettWeight(request.NettWeight);
            serviceSampleShrinkagePanel.SetGrossWeight(request.GrossWeight);
            serviceSampleShrinkagePanel.Modify();

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

            await _garmentServiceSampleShrinkagePanelRepository.Update(serviceSampleShrinkagePanel);

            _storage.Save();

            return serviceSampleShrinkagePanel;
        }

        public async Task<string> PutGarmentUnitExpenditureNoteByNo(string uenNo, bool isPreparing)
        {
            var garmentUnitExpenditureNoteUri = PurchasingDataSettings.Endpoint + $"garment-unit-expenditure-notes/isPreparingByNo?UENNo={uenNo}&IsPreparing={isPreparing}";
            var garmentUnitExpenditureNoteResponse = await _http.GetAsync(garmentUnitExpenditureNoteUri,_webApiContext.Token);

            return garmentUnitExpenditureNoteResponse.EnsureSuccessStatusCode().ToString();
        }

    }
}
