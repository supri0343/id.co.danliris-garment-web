using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    public class UpdateGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<UpdateGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutDetailRepository _garmentSubconDeliveryLetterOutDetailRepository;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentServiceSubconCuttingRepository _garmentSubconCuttingRepository;
        private readonly IGarmentServiceSubconSewingRepository _garmentSubconSewingRepository;
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconExpenditureGoodRepository _garmentServiceSubconExpenditureGoodRepository;

        //SampleSubcon
        private readonly IGarmentServiceSampleCuttingRepository _garmentSubconSampleCuttingRepository;
        private readonly IGarmentServiceSampleSewingRepository _garmentSubconSampleSewingRepository;
        private readonly IGarmentServiceSampleShrinkagePanelRepository _garmentServiceSubconSampleShrinkagePanelRepository;
        private readonly IGarmentServiceSampleFabricWashRepository _garmentServiceSubconSampleFabricWashRepository;
        private readonly IGarmentServiceSampleExpenditureGoodRepository _garmentServiceSubconSampleExpenditureGoodRepository;

        public UpdateGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = _storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = _storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentSubconDeliveryLetterOutDetailRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutDetailRepository>();
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentSubconSewingRepository = storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentServiceSubconExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodRepository>();
            //Sample
            _garmentSubconSampleCuttingRepository = storage.GetRepository<IGarmentServiceSampleCuttingRepository>();
            _garmentSubconSampleSewingRepository = storage.GetRepository<IGarmentServiceSampleSewingRepository>();
            _garmentServiceSubconSampleShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelRepository>();
            _garmentServiceSubconSampleFabricWashRepository = storage.GetRepository<IGarmentServiceSampleFabricWashRepository>();
            _garmentServiceSubconSampleExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodRepository>();
        }

        public async Task<GarmentSubconDeliveryLetterOut> Handle(UpdateGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {

            if (request.ItemsAcc != null)
            {
                foreach (var itemAcc in request.ItemsAcc)
                {
                    if (itemAcc.Id != Guid.Empty || itemAcc.Quantity > 0)
                    {
                        request.Items.Add(itemAcc);
                    }
                }
            }
            var subconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconDeliveryLetterOut(o)).Single();

            //if(subconDeliveryLetterOut.SubconCategory == "SUBCON CUTTING SEWING")
            //{

            //    //subconDeliveryLetterOut.SetEPOItemId(request.EPOItemId);
            //    //subconDeliveryLetterOut.SetPONo(request.PONo);

            //    _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDeliveryLetterOutItem =>
            //    {
            //        var item = request.Items.Where(o => o.Id == subconDeliveryLetterOutItem.Identity).Single();

            //        subconDeliveryLetterOutItem.SetQuantity(item.Quantity);
            //        subconDeliveryLetterOutItem.Modify();

            //        await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDeliveryLetterOutItem);
            //    });
            //}
            //else
            //{
            _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDLItem =>
            {
                var item = request.Items.Where(o => o.Id == subconDLItem.Identity).SingleOrDefault();
                if (item.Quantity == 0)
                {
                    subconDLItem.Remove();
                }
                if (item == null)
                {
                    //New Query
                    if (subconDeliveryLetterOut.OrderType == "SAMPLE")
                    {
                        switch (subconDeliveryLetterOut.SubconCategory)
                        {
                            case "SUBCON JASA KOMPONEN":
                                var subconCutting = _garmentSubconSampleCuttingRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSampleCutting(s)).Single();
                                subconCutting.SetIsUsed(false);
                                subconCutting.Modify();

                                await _garmentSubconSampleCuttingRepository.Update(subconCutting);
                                break;
                            case "SUBCON JASA GARMENT WASH":
                                var subconSewing = _garmentSubconSampleSewingRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSampleSewing(s)).Single();
                                subconSewing.SetIsUsed(false);
                                subconSewing.Modify();

                                await _garmentSubconSampleSewingRepository.Update(subconSewing);
                                break;
                            case "SUBCON BB SHRINKAGE/PANEL":
                                var subconPanel = _garmentServiceSubconSampleShrinkagePanelRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSampleShrinkagePanel(s)).Single();
                                subconPanel.SetIsUsed(false);
                                subconPanel.Modify();

                                await _garmentServiceSubconSampleShrinkagePanelRepository.Update(subconPanel);
                                break;
                            case "SUBCON BB FABRIC WASH/PRINT":
                                var subconFabric = _garmentServiceSubconSampleFabricWashRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSampleFabricWash(s)).Single();
                                subconFabric.SetIsUsed(false);
                                subconFabric.Modify();

                                await _garmentServiceSubconSampleFabricWashRepository.Update(subconFabric);
                                break;
                            case "SUBCON JASA BARANG JADI":
                                var subconExpenditureGood = _garmentServiceSubconSampleExpenditureGoodRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSampleExpenditureGood(s)).Single();
                                subconExpenditureGood.SetIsUsed(false);
                                subconExpenditureGood.Modify();

                                await _garmentServiceSubconSampleExpenditureGoodRepository.Update(subconExpenditureGood);
                                break;
                        }
                    }
                    else
                    {
                        switch (subconDeliveryLetterOut.SubconCategory)
                        {
                            case "SUBCON SEWING":
                                var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                                subconCuttingOut.SetIsUsed(false);
                                subconCuttingOut.Modify();

                                await _garmentCuttingOutRepository.Update(subconCuttingOut);
                                break;
                            case "SUBCON JASA KOMPONEN":
                                var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                                subconCutting.SetIsUsed(false);
                                subconCutting.Modify();

                                await _garmentSubconCuttingRepository.Update(subconCutting);
                                break;
                            case "SUBCON JASA GARMENT WASH":
                                var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                                subconSewing.SetIsUsed(false);
                                subconSewing.Modify();

                                await _garmentSubconSewingRepository.Update(subconSewing);
                                break;
                            case "SUBCON BB SHRINKAGE/PANEL":
                                var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                                subconPanel.SetIsUsed(false);
                                subconPanel.Modify();

                                await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                                break;
                            case "SUBCON BB FABRIC WASH/PRINT":
                                var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                                subconFabric.SetIsUsed(false);
                                subconFabric.Modify();

                                await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                                break;
                            case "SUBCON JASA BARANG JADI":
                                var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconExpenditureGood(s)).Single();
                                subconExpenditureGood.SetIsUsed(false);
                                subconExpenditureGood.Modify();

                                await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);
                                break;
                        }
                    }
                    #region OldQuery
                    //if (subconDeliveryLetterOut.SubconCategory == "SUBCON SEWING")
                    //{
                    //    var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                    //    subconCuttingOut.SetIsUsed(false);
                    //    subconCuttingOut.Modify();

                    //    await _garmentCuttingOutRepository.Update(subconCuttingOut);
                    //}
                    //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA KOMPONEN")
                    //{
                    //    var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                    //    subconCutting.SetIsUsed(false);
                    //    subconCutting.Modify();

                    //    await _garmentSubconCuttingRepository.Update(subconCutting);
                    //}
                    //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA GARMENT WASH")
                    //{
                    //    var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                    //    subconSewing.SetIsUsed(false);
                    //    subconSewing.Modify();

                    //    await _garmentSubconSewingRepository.Update(subconSewing);
                    //}
                    //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB SHRINKAGE/PANEL")
                    //{
                    //    var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                    //    subconPanel.SetIsUsed(false);
                    //    subconPanel.Modify();

                    //    await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                    //}
                    //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB FABRIC WASH/PRINT")
                    //{
                    //    var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                    //    subconFabric.SetIsUsed(false);
                    //    subconFabric.Modify();

                    //    await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                    //}
                    //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA BARANG JADI")
                    //{
                    //    var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconExpenditureGood(s)).Single();
                    //    subconExpenditureGood.SetIsUsed(false);
                    //    subconExpenditureGood.Modify();

                    //    await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);
                    //}
                    #endregion

                    _garmentSubconDeliveryLetterOutDetailRepository.Find(x => x.SubconDeliveryLetterOutItemId == subconDLItem.Identity).ForEach(async subconDLDetail =>
                {
                    subconDLDetail.Remove();
                    subconDLDetail.Modify();

                    await _garmentSubconDeliveryLetterOutDetailRepository.Update(subconDLDetail);
                });

                    subconDLItem.Remove();
                }
                else
                {
                    if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB SHRINKAGE/PANEL")
                    {
                        subconDLItem.SetSmallQty(item.SmallQuantity);

                    }
                    else if (subconDeliveryLetterOut.SubconCategory == "SUBCON SEWING" || subconDeliveryLetterOut.SubconCategory == "SUBCON JASA KOMPONEN")
                    {
                        _garmentSubconDeliveryLetterOutDetailRepository.Find(x => x.SubconDeliveryLetterOutItemId == subconDLItem.Identity).ForEach(async subconDLDetail =>
                        {
                            var detail = item.Details.Where(o => o.Id == subconDLDetail.Identity).Single();

                            if (detail.Quantity > 0)
                            {
                                subconDLDetail.SetQuantity(detail.Quantity);
                                subconDLDetail.Modify();
                            }
                            else
                            {
                                subconDLDetail.Remove();
                            }

                            await _garmentSubconDeliveryLetterOutDetailRepository.Update(subconDLDetail);
                        });
                    }
                    else if (subconDeliveryLetterOut.SubconCategory == "SUBCON CUTTING SEWING")
                    {
                        if (item.Quantity > 0)
                        {
                            subconDLItem.SetQuantity(item.Quantity);
                            subconDLItem.Modify();
                        }
                    }
                }
                await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDLItem);
            });

            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem = new GarmentSubconDeliveryLetterOutItem(
                    Guid.NewGuid(),
                    subconDeliveryLetterOut.Identity,
                    item.UENItemId,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.ProductRemark,
                    item.DesignColor,
                    item.Quantity,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    new UomId(item.UomOut.Id),
                    item.UomOut.Unit,
                    item.FabricType,
                    item.SubconId,
                    item.RONo,
                    item.POSerialNumber,
                    item.SubconNo,
                    //new UomId(item.UomSatuan.Id),
                    item.UomSatuanUnit,
                    item.QtyPacking,
                    item.SmallQuantity,
                    item.SmallUomUnit,
                    item.UENId,
                    item.UENNo
                    );


                    if (item.Details != null)
                    {
                        foreach (var detail in item.Details)
                        {
                            GarmentSubconDeliveryLetterOutDetail garmentSubconDeliveryLetterOutDetail = new GarmentSubconDeliveryLetterOutDetail(
                                   Guid.NewGuid(),
                                   garmentSubconDeliveryLetterOutItem.Identity,
                                   detail.UENItemId,
                                   new ProductId(detail.Product.Id),
                                   detail.Product.Code,
                                   detail.Product.Name,
                                   detail.ProductRemark,
                                   detail.DesignColor,
                                   detail.Quantity,
                                   new UomId(detail.Uom.Id),
                                   detail.Uom.Unit,
                                   new UomId(detail.UomOut.Id),
                                   detail.UomOut.Unit,
                                   detail.FabricType,
                                   detail.UENId,
                                   detail.UENNo
                            );
                            await _garmentSubconDeliveryLetterOutDetailRepository.Update(garmentSubconDeliveryLetterOutDetail);
                        }
                    }
                    //New Query
                    if (request.OrderType == "SAMPLE")
                    {
                        switch (request.SubconCategory)
                        {
                            case "SUBCON SEWING":
                                var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                                subconCuttingOut.SetIsUsed(true);
                                subconCuttingOut.Modify();

                                await _garmentCuttingOutRepository.Update(subconCuttingOut);
                                break;
                            case "SUBCON JASA KOMPONEN":
                                var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                                subconCutting.SetIsUsed(true);
                                subconCutting.Modify();

                                await _garmentSubconCuttingRepository.Update(subconCutting);
                                break;
                            case "SUBCON JASA GARMENT WASH":
                                var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                                subconSewing.SetIsUsed(true);
                                subconSewing.Modify();

                                await _garmentSubconSewingRepository.Update(subconSewing);
                                break;
                            case "SUBCON BB SHRINKAGE/PANEL":
                                var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                                subconPanel.SetIsUsed(true);
                                subconPanel.Modify();

                                await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                                break;
                            case "SUBCON BB FABRIC WASH/PRINT":
                                var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                                subconFabric.SetIsUsed(true);
                                subconFabric.Modify();

                                await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                                break;
                            case "SUBCON JASA BARANG JADI":
                                var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconExpenditureGood(s)).Single();
                                subconExpenditureGood.SetIsUsed(true);
                                subconExpenditureGood.Modify();

                                await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);
                                break;
                        }
                    }
                    else
                    {
                        switch (request.SubconCategory)
                        {
                            case "SUBCON JASA KOMPONEN":
                                var subconCutting = _garmentSubconSampleCuttingRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSampleCutting(s)).Single();
                                subconCutting.SetIsUsed(true);
                                subconCutting.Modify();

                                await _garmentSubconSampleCuttingRepository.Update(subconCutting);
                                break;
                            case "SUBCON JASA GARMENT WASH":
                                var subconSewing = _garmentSubconSampleSewingRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSampleSewing(s)).Single();
                                subconSewing.SetIsUsed(true);
                                subconSewing.Modify();

                                await _garmentSubconSampleSewingRepository.Update(subconSewing);
                                break;
                            case "SUBCON BB SHRINKAGE/PANEL":
                                var subconPanel = _garmentServiceSubconSampleShrinkagePanelRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSampleShrinkagePanel(s)).Single();
                                subconPanel.SetIsUsed(true);
                                subconPanel.Modify();

                                await _garmentServiceSubconSampleShrinkagePanelRepository.Update(subconPanel);
                                break;
                            case "SUBCON BB FABRIC WASH/PRINT":
                                var subconFabric = _garmentServiceSubconSampleFabricWashRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSampleFabricWash(s)).Single();
                                subconFabric.SetIsUsed(true);
                                subconFabric.Modify();

                                await _garmentServiceSubconSampleFabricWashRepository.Update(subconFabric);
                                break;
                            case "SUBCON JASA BARANG JADI":
                                var subconExpenditureGood = _garmentServiceSubconSampleExpenditureGoodRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSampleExpenditureGood(s)).Single();
                                subconExpenditureGood.SetIsUsed(true);
                                subconExpenditureGood.Modify();

                                await _garmentServiceSubconSampleExpenditureGoodRepository.Update(subconExpenditureGood);
                                break;
                        }
                    }
                    #region OldQuery
                    //if (request.SubconCategory == "SUBCON SEWING")
                    //    {
                    //        var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                    //        subconCuttingOut.SetIsUsed(true);
                    //        subconCuttingOut.Modify();

                    //        await _garmentCuttingOutRepository.Update(subconCuttingOut);
                    //    }
                    //    else if (request.SubconCategory == "SUBCON JASA KOMPONEN")
                    //    {
                    //        var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                    //        subconCutting.SetIsUsed(true);
                    //        subconCutting.Modify();

                    //        await _garmentSubconCuttingRepository.Update(subconCutting);
                    //    }
                    //    else if (request.SubconCategory == "SUBCON JASA GARMENT WASH")
                    //    {
                    //        var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                    //        subconSewing.SetIsUsed(true);
                    //        subconSewing.Modify();

                    //        await _garmentSubconSewingRepository.Update(subconSewing);
                    //    }
                    //    else if (request.SubconCategory == "SUBCON BB SHRINKAGE/PANEL")
                    //    {
                    //        var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                    //        subconPanel.SetIsUsed(true);
                    //        subconPanel.Modify();

                    //        await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                    //    }
                    //    else if (request.SubconCategory == "SUBCON BB FABRIC WASH/PRINT")
                    //    {
                    //        var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                    //        subconFabric.SetIsUsed(true);
                    //        subconFabric.Modify();

                    //        await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                    //    }
                    //    else if (request.SubconCategory == "SUBCON JASA BARANG JADI")
                    //    {
                    //        var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconExpenditureGood(s)).Single();
                    //        subconExpenditureGood.SetIsUsed(true);
                    //        subconExpenditureGood.Modify();

                    //        await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);
                    //    }
                    #endregion
                    await _garmentSubconDeliveryLetterOutItemRepository.Update(garmentSubconDeliveryLetterOutItem);
                }
                else
                {
                    if (item.Details != null)
                    {
                        foreach (var detail in item.Details)
                        {
                            if (detail.Id == Guid.Empty && detail.Quantity > 0)
                            {
                                GarmentSubconDeliveryLetterOutDetail garmentSubconDeliveryLetterOutDetail = new GarmentSubconDeliveryLetterOutDetail(
                                  Guid.NewGuid(),
                                  item.Id,
                                  detail.UENItemId,
                                  new ProductId(detail.Product.Id),
                                  detail.Product.Code,
                                  detail.Product.Name,
                                  detail.ProductRemark,
                                  detail.DesignColor,
                                  detail.Quantity,
                                  new UomId(detail.Uom.Id),
                                  detail.Uom.Unit,
                                  new UomId(detail.UomOut.Id),
                                  detail.UomOut.Unit,
                                  detail.FabricType,
                                  detail.UENId,
                                  detail.UENNo
                                );
                                await _garmentSubconDeliveryLetterOutDetailRepository.Update(garmentSubconDeliveryLetterOutDetail);
                            }

                        }
                    }
                }
            }


            subconDeliveryLetterOut.SetDate(request.DLDate.GetValueOrDefault());
            subconDeliveryLetterOut.SetRemark(request.Remark);


            subconDeliveryLetterOut.Modify();

            await _garmentSubconDeliveryLetterOutRepository.Update(subconDeliveryLetterOut);

            _storage.Save();

            return subconDeliveryLetterOut;
        }
        }
    }

