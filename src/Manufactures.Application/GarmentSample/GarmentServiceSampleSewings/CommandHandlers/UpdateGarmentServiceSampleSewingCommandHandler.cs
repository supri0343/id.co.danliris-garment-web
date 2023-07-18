using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleSewings.CommandHandlers
{
    public class UpdateGarmentServiceSampleSewingCommandHandler : ICommandHandler<UpdateGarmentServiceSampleSewingCommand, GarmentServiceSampleSewing>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleSewingRepository _garmentServiceSampleSewingRepository;
        private readonly IGarmentServiceSampleSewingItemRepository _garmentServiceSampleSewingItemRepository;
        private readonly IGarmentServiceSampleSewingDetailRepository _garmentServiceSampleSewingDetailRepository;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public UpdateGarmentServiceSampleSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleSewingRepository = _storage.GetRepository<IGarmentServiceSampleSewingRepository>();
            _garmentServiceSampleSewingItemRepository = _storage.GetRepository<IGarmentServiceSampleSewingItemRepository>();
            _garmentServiceSampleSewingDetailRepository = _storage.GetRepository<IGarmentServiceSampleSewingDetailRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        public async Task<GarmentServiceSampleSewing> Handle(UpdateGarmentServiceSampleSewingCommand request, CancellationToken cancellationToken)
        {
            var serviceSampleSewing = _garmentServiceSampleSewingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleSewing(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentServiceSampleSewingItemRepository.Find(o => o.ServiceSampleSewingId == serviceSampleSewing.Identity).ForEach(async SampleSewingItem =>
            {
                var item = request.Items.Where(o => o.Id == SampleSewingItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    _garmentServiceSampleSewingDetailRepository.Find(i => i.ServiceSampleSewingItemId == SampleSewingItem.Identity).ForEach(async SampleDetail =>
                    {
                        SampleDetail.Remove();
                        await _garmentServiceSampleSewingDetailRepository.Update(SampleDetail);
                    });
                    SampleSewingItem.Remove();

                }
                else
                {
                    _garmentServiceSampleSewingDetailRepository.Find(i => i.ServiceSampleSewingItemId == SampleSewingItem.Identity).ForEach(async SampleDetail =>
                    {
                        var detail = item.Details.Where(o => o.Id == SampleDetail.Identity).SingleOrDefault();
                        if (detail == null)
                        {
                            SampleDetail.Remove();
                        }
                        else
                        {
                            SampleDetail.SetQuantity(detail.Quantity);
                            SampleDetail.SetUomUnit(detail.Uom.Unit);
                            SampleDetail.Modify();
                        }
                        await _garmentServiceSampleSewingDetailRepository.Update(SampleDetail);
                    });
                    SampleSewingItem.Modify();
                }


                await _garmentServiceSampleSewingItemRepository.Update(SampleSewingItem);
            });

            //Old Query
            //foreach (var item in request.Items)
            //{
            //    if (item.Id == Guid.Empty)
            //    {
            //        GarmentServiceSampleSewingItem SampleSewingItem = new GarmentServiceSampleSewingItem(
            //            Guid.NewGuid(),
            //            serviceSampleSewing.Identity,
            //            item.RONo,
            //            item.Article,
            //            new GarmentComodityId(item.Comodity.Id),
            //            item.Comodity.Code,
            //            item.Comodity.Name,
            //            new BuyerId(item.Buyer.Id),
            //            item.Buyer.Code,
            //            item.Buyer.Name,
            //            new UnitDepartmentId(item.Unit.Id),
            //            item.Unit.Code,
            //            item.Unit.Name
            //        );

            //        foreach (var detail in item.Details)
            //        {
            //            if (detail.IsSave)
            //            {
            //                GarmentServiceSampleSewingDetail SampleDetail = new GarmentServiceSampleSewingDetail(
            //                             Guid.NewGuid(),
            //                             SampleSewingItem.Identity,
            //                             Guid.NewGuid(),
            //                             Guid.NewGuid(),
            //                             new ProductId(detail.Product.Id),
            //                             detail.Product.Code,
            //                             detail.Product.Name,
            //                             detail.DesignColor,
            //                             detail.Quantity,
            //                             new UomId(detail.Uom.Id),
            //                             detail.Uom.Unit,
            //                             new UnitDepartmentId(detail.Unit.Id),
            //                             detail.Unit.Code,
            //                             detail.Unit.Name,
            //                             detail.Remark,
            //                             detail.Color
            //                         );
            //                await _garmentServiceSampleSewingDetailRepository.Update(SampleDetail);
            //            }
            //        }
            //        await _garmentServiceSampleSewingItemRepository.Update(SampleSewingItem);
            //    }
            //}


            //New Query
            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentServiceSampleSewingItem garmentServiceSampleSewingItem = new GarmentServiceSampleSewingItem(
                    Guid.NewGuid(),
                    serviceSampleSewing.Identity,
                    item.RONo,
                    item.Article,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name,
                    new BuyerId(item.Buyer.Id),
                    item.Buyer.Code,
                    item.Buyer.Name,
                    new UnitDepartmentId(item.Unit.Id),
                    item.Unit.Code,
                    item.Unit.Name

                );
                    //item.Id = garmentServiceSampleSewingItem.Identity;

                    var SewingIn = _garmentSewingInRepository.Query.Where(x => x.RONo == item.RONo).OrderBy(a => a.CreatedDate).ToList();
                    List<GarmentServiceSampleSewingDetail> SewingInDetails = new List<GarmentServiceSampleSewingDetail>();

                    foreach (var sewIn in SewingIn)
                    {
                        var SewingInItems = _garmentSewingInItemRepository.Query.Where(x => x.SewingInId == sewIn.Identity).OrderBy(a => a.CreatedDate).ToList();
                        foreach (var sewInItem in SewingInItems)
                        {
                            var SampleSewingDetails = _garmentServiceSampleSewingDetailRepository.Query.Where(o => o.SewingInItemId == sewInItem.Identity);
                            if (SampleSewingDetails != null)
                            {
                                double qty = (double)sewInItem.Quantity;
                                foreach (var SampleSewingDetail in SampleSewingDetails.ToList())
                                {
                                    qty -= SampleSewingDetail.Quantity;
                                }
                                if (qty > 0)
                                {
                                    SewingInDetails.Add(new GarmentServiceSampleSewingDetail
                                    (
                                        new Guid(),
                                        item.Id,
                                        sewIn.Identity,
                                        sewInItem.Identity,
                                        new ProductId(sewInItem.ProductId),
                                        sewInItem.ProductCode,
                                        sewInItem.ProductName,
                                        sewInItem.DesignColor,
                                        qty,
                                        new UomId(sewInItem.UomId),
                                        sewInItem.UomUnit,
                                        new UnitDepartmentId(sewIn.UnitId),
                                        sewIn.UnitCode,
                                        sewIn.UnitName,
                                        "",
                                        sewInItem.Color

                                    ));
                                }
                            }
                        }
                    }

                    foreach (var detail in item.Details)
                    {
                        if (detail.IsSave)
                        {
                            var sewInDetail = SewingInDetails.Where(y => y.DesignColor == detail.DesignColor && y.UnitId == new UnitDepartmentId(detail.Unit.Id) && y.Color == detail.Color).ToList();
                            var qty = detail.Quantity;
                            if (sewInDetail.ToArray().Count() != 0)
                            {
                                foreach (var d in sewInDetail)
                                {
                                    var qtyRemains = d.Quantity - qty;
                                    if (qtyRemains >= 0)
                                    {
                                        GarmentServiceSampleSewingDetail garmentServiceSampleSewingDetail = new GarmentServiceSampleSewingDetail(
                                            Guid.NewGuid(),
                                            garmentServiceSampleSewingItem.Identity,
                                            d.SewingInId,
                                            d.SewingInItemId,
                                            d.ProductId,
                                            d.ProductCode,
                                            d.ProductName,
                                            d.DesignColor,
                                            qty,
                                            d.UomId,
                                            d.UomUnit,
                                            d.UnitId,
                                            d.UnitCode,
                                            d.UnitName,
                                            detail.Remark,
                                            detail.Color
                                        );
                                        await _garmentServiceSampleSewingDetailRepository.Update(garmentServiceSampleSewingDetail);
                                        break;
                                    }
                                    else if (qtyRemains < 0)
                                    {
                                        qty -= d.Quantity;
                                        GarmentServiceSampleSewingDetail garmentServiceSampleSewingDetail = new GarmentServiceSampleSewingDetail(
                                            Guid.NewGuid(),
                                            garmentServiceSampleSewingItem.Identity,
                                            d.SewingInId,
                                            d.SewingInItemId,
                                            d.ProductId,
                                            d.ProductCode,
                                            d.ProductName,
                                            d.DesignColor,
                                            d.Quantity,
                                            d.UomId,
                                            d.UomUnit,
                                            d.UnitId,
                                            d.UnitCode,
                                            d.UnitName,
                                            detail.Remark,
                                            detail.Color
                                        );
                                        await _garmentServiceSampleSewingDetailRepository.Update(garmentServiceSampleSewingDetail);
                                    }
                                }
                            }
                        }
                    }
                    await _garmentServiceSampleSewingItemRepository.Update(garmentServiceSampleSewingItem);
                }
                    
            }

            serviceSampleSewing.SetDate(request.ServiceSampleSewingDate.GetValueOrDefault());
            serviceSampleSewing.SetBuyerId(new BuyerId(request.Buyer.Id));
            serviceSampleSewing.SetBuyerCode(request.Buyer.Code);
            serviceSampleSewing.SetBuyerName(request.Buyer.Name);
            serviceSampleSewing.SetQtyPacking(request.QtyPacking);
            serviceSampleSewing.SetUomUnit(request.UomUnit);
            serviceSampleSewing.Modify();
            await _garmentServiceSampleSewingRepository.Update(serviceSampleSewing);

            _storage.Save();

            return serviceSampleSewing;
        }
    }
}