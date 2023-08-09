using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Domain.Commands;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
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
    public class UpdateGarmentServiceSampleFabricWashCommandHandler : ICommandHandler<UpdateGarmentServiceSampleFabricWashCommand, GarmentServiceSampleFabricWash>
    {
        private readonly IStorage _storage;
        private readonly IWebApiContext _webApiContext;
        private readonly IHttpClientService _http;
        private readonly IGarmentServiceSampleFabricWashRepository _garmentServiceSampleFabricWashRepository;
        private readonly IGarmentServiceSampleFabricWashItemRepository _garmentServiceSampleFabricWashItemRepository;
        private readonly IGarmentServiceSampleFabricWashDetailRepository _garmentServiceSampleFabricWashDetailRepository;

        public UpdateGarmentServiceSampleFabricWashCommandHandler(IStorage storage, IWebApiContext webApiContext, IHttpClientService http)
        {
            _storage = storage;
            _http = http;
            _webApiContext = webApiContext;
            _garmentServiceSampleFabricWashRepository = _storage.GetRepository<IGarmentServiceSampleFabricWashRepository>();
            _garmentServiceSampleFabricWashItemRepository = _storage.GetRepository<IGarmentServiceSampleFabricWashItemRepository>();
            _garmentServiceSampleFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSampleFabricWashDetailRepository>();
        }

        public async Task<GarmentServiceSampleFabricWash> Handle(UpdateGarmentServiceSampleFabricWashCommand request, CancellationToken cancellationToken)
        {
            var serviceSampleFabricWash = _garmentServiceSampleFabricWashRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleFabricWash(o)).Single();

            Dictionary<Guid, double> fabricWashUpdated = new Dictionary<Guid, double>();

            List<string> listDeletedUenNo = new List<string>();
            List<string> listUsedUenNo = new List<string>();
            _garmentServiceSampleFabricWashItemRepository.Find(o => o.ServiceSampleFabricWashId == serviceSampleFabricWash.Identity).ForEach(async SampleFabricWashItem =>
            {
                var item = request.Items.Where(o => o.Id == SampleFabricWashItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    listDeletedUenNo.Add(SampleFabricWashItem.UnitExpenditureNo);
                    _garmentServiceSampleFabricWashDetailRepository.Find(i => i.ServiceSampleFabricWashItemId == SampleFabricWashItem.Identity).ForEach(async SampleFabricWashDetail =>
                    {
                        SampleFabricWashDetail.Remove();
                        await _garmentServiceSampleFabricWashDetailRepository.Update(SampleFabricWashDetail);
                    });

                    SampleFabricWashItem.Remove();
                }
                else
                {
                    _garmentServiceSampleFabricWashDetailRepository.Find(i => i.ServiceSampleFabricWashItemId == SampleFabricWashItem.Identity).ForEach(async SampleFabricWashDetail =>
                    {
                        var detail = item.Details.Where(o => o.Id == SampleFabricWashDetail.Identity).Single();
                        if (!detail.IsSave)
                        {
                            SampleFabricWashDetail.Remove();
                        }
                        else
                        {
                            SampleFabricWashDetail.SetQuantity(detail.Quantity);
                            SampleFabricWashDetail.SetProductRemark(detail.Product.Remark);
                            SampleFabricWashDetail.Modify();
                        }

                        await _garmentServiceSampleFabricWashDetailRepository.Update(SampleFabricWashDetail);
                    });

                    SampleFabricWashItem.Modify();
                }

                await _garmentServiceSampleFabricWashItemRepository.Update(SampleFabricWashItem);
            });


            serviceSampleFabricWash.SetServiceSampleFabricWashDate(request.ServiceSubconFabricWashDate.GetValueOrDefault());
            serviceSampleFabricWash.SetRemark(request.Remark);
            serviceSampleFabricWash.SetQtyPacking(request.QtyPacking);
            serviceSampleFabricWash.SetUomUnit(request.UomUnit);
            serviceSampleFabricWash.SetNettWeight(request.NettWeight);
            serviceSampleFabricWash.SetGrossWeight(request.GrossWeight);
            serviceSampleFabricWash.Modify();
            var existingItem = _garmentServiceSampleFabricWashItemRepository.Find(o => o.ServiceSampleFabricWashId == serviceSampleFabricWash.Identity);

            var newitem = request.Items.Where(x => !existingItem.Select(o => o.UnitExpenditureNo).Contains(x.UnitExpenditureNo)).ToList();
            var removeItem = existingItem.Where(x => !request.Items.Select(o => o.UnitExpenditureNo).Contains(x.UnitExpenditureNo)).ToList();

            if (newitem.Count() > 0)
            {
                foreach (var item in newitem)
                {
                    listUsedUenNo.Add(item.UnitExpenditureNo);

                    GarmentServiceSampleFabricWashItem garmentServiceSampleFabricWashItem = new GarmentServiceSampleFabricWashItem(
                        Guid.NewGuid(),
                        serviceSampleFabricWash.Identity,
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
            }

            if (removeItem.Count() > 0)
            {
                foreach (var item in removeItem)
                {
                    listDeletedUenNo.Add(item.UnitExpenditureNo);
                    _garmentServiceSampleFabricWashDetailRepository.Find(i => i.ServiceSampleFabricWashItemId == item.Identity).ForEach(async serviceSampleFabricWashDetail =>
                    {
                        serviceSampleFabricWashDetail.Remove();
                        await _garmentServiceSampleFabricWashDetailRepository.Update(serviceSampleFabricWashDetail);
                    });
                    item.Remove();
                    await _garmentServiceSampleFabricWashItemRepository.Update(item);
                }
            }

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


            await _garmentServiceSampleFabricWashRepository.Update(serviceSampleFabricWash);

            _storage.Save();

            return serviceSampleFabricWash;
        }

        public async Task<string> PutGarmentUnitExpenditureNoteByNo(string uenNo, bool isPreparing)
        {
            var garmentUnitExpenditureNoteUri = PurchasingDataSettings.Endpoint + $"garment-unit-expenditure-notes/isPreparingByNo?UENNo={uenNo}&IsPreparing={isPreparing}";
            var garmentUnitExpenditureNoteResponse = await _http.GetAsync(garmentUnitExpenditureNoteUri, _webApiContext.Token);

            return garmentUnitExpenditureNoteResponse.EnsureSuccessStatusCode().ToString();
        }
    }
}
