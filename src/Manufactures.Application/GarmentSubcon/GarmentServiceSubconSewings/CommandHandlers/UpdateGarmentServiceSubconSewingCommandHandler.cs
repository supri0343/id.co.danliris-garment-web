using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers
{
    public class UpdateGarmentServiceSubconSewingCommandHandler : ICommandHandler<UpdateGarmentServiceSubconSewingCommand, GarmentServiceSubconSewing>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;
        private readonly IGarmentServiceSubconSewingItemRepository _garmentServiceSubconSewingItemRepository;
        private readonly IGarmentServiceSubconSewingDetailRepository _garmentServiceSubconSewingDetailRepository;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public UpdateGarmentServiceSubconSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconSewingRepository = _storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconSewingItemRepository = _storage.GetRepository<IGarmentServiceSubconSewingItemRepository>();
            _garmentServiceSubconSewingDetailRepository = _storage.GetRepository<IGarmentServiceSubconSewingDetailRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        public async Task<GarmentServiceSubconSewing> Handle(UpdateGarmentServiceSubconSewingCommand request, CancellationToken cancellationToken)
        {
            var serviceSubconSewing = _garmentServiceSubconSewingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconSewing(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentServiceSubconSewingItemRepository.Find(o => o.ServiceSubconSewingId == serviceSubconSewing.Identity).ForEach(async subconSewingItem =>
            {
                var item = request.Items.Where(o => o.Id == subconSewingItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    _garmentServiceSubconSewingDetailRepository.Find(i => i.ServiceSubconSewingItemId == subconSewingItem.Identity).ForEach(async subconDetail =>
                    {
                        subconDetail.Remove();
                        await _garmentServiceSubconSewingDetailRepository.Update(subconDetail);
                    });
                    subconSewingItem.Remove();

                }
                else
                {
                    _garmentServiceSubconSewingDetailRepository.Find(i => i.ServiceSubconSewingItemId == subconSewingItem.Identity).ForEach(async subconDetail =>
                    {
                        var detail = item.Details.Where(o => o.Id == subconDetail.Identity).SingleOrDefault();
                        if (detail == null)
                        {
                            subconDetail.Remove();
                        }
                        else
                        {
                            subconDetail.SetQuantity(detail.Quantity);
                            subconDetail.SetUomUnit(detail.Uom.Unit);
                            subconDetail.Modify();
                        }
                        await _garmentServiceSubconSewingDetailRepository.Update(subconDetail);
                    });
                    subconSewingItem.Modify();
                }


                await _garmentServiceSubconSewingItemRepository.Update(subconSewingItem);
            });

            //Old Query
            //foreach (var item in request.Items)
            //{
            //    if (item.Id == Guid.Empty)
            //    {
            //        GarmentServiceSubconSewingItem subconSewingItem = new GarmentServiceSubconSewingItem(
            //            Guid.NewGuid(),
            //            serviceSubconSewing.Identity,
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
            //                GarmentServiceSubconSewingDetail subconDetail = new GarmentServiceSubconSewingDetail(
            //                             Guid.NewGuid(),
            //                             subconSewingItem.Identity,
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
            //                await _garmentServiceSubconSewingDetailRepository.Update(subconDetail);
            //            }
            //        }
            //        await _garmentServiceSubconSewingItemRepository.Update(subconSewingItem);
            //    }
            //}


            //New Query
            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentServiceSubconSewingItem garmentServiceSubconSewingItem = new GarmentServiceSubconSewingItem(
                    Guid.NewGuid(),
                    serviceSubconSewing.Identity,
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
                    //item.Id = garmentServiceSubconSewingItem.Identity;

                    var SewingIn = _garmentSewingInRepository.Query.Where(x => x.RONo == item.RONo).OrderBy(a => a.CreatedDate).ToList();
                    List<GarmentServiceSubconSewingDetail> SewingInDetails = new List<GarmentServiceSubconSewingDetail>();

                    foreach (var sewIn in SewingIn)
                    {
                        var SewingInItems = _garmentSewingInItemRepository.Query.Where(x => x.SewingInId == sewIn.Identity).OrderBy(a => a.CreatedDate).ToList();
                        foreach (var sewInItem in SewingInItems)
                        {
                            var subconSewingDetails = _garmentServiceSubconSewingDetailRepository.Query.Where(o => o.SewingInItemId == sewInItem.Identity);
                            if (subconSewingDetails != null)
                            {
                                double qty = (double)sewInItem.Quantity;
                                foreach (var subconSewingDetail in subconSewingDetails.ToList())
                                {
                                    qty -= subconSewingDetail.Quantity;
                                }
                                if (qty > 0)
                                {
                                    SewingInDetails.Add(new GarmentServiceSubconSewingDetail
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
                                        ""

                                    ));
                                }
                            }
                        }
                    }

                    foreach (var detail in item.Details)
                    {
                        if (detail.IsSave)
                        {
                            var sewInDetail = SewingInDetails.Where(y => y.DesignColor == detail.DesignColor && y.UnitId == new UnitDepartmentId(detail.Unit.Id)).ToList();
                            var qty = detail.Quantity;
                            if (sewInDetail.ToArray().Count() != 0)
                            {
                                foreach (var d in sewInDetail)
                                {
                                    var qtyRemains = d.Quantity - qty;
                                    if (qtyRemains >= 0)
                                    {
                                        GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail = new GarmentServiceSubconSewingDetail(
                                            Guid.NewGuid(),
                                            garmentServiceSubconSewingItem.Identity,
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
                                        await _garmentServiceSubconSewingDetailRepository.Update(garmentServiceSubconSewingDetail);
                                        break;
                                    }
                                    else if (qtyRemains < 0)
                                    {
                                        qty -= d.Quantity;
                                        GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail = new GarmentServiceSubconSewingDetail(
                                            Guid.NewGuid(),
                                            garmentServiceSubconSewingItem.Identity,
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
                                        await _garmentServiceSubconSewingDetailRepository.Update(garmentServiceSubconSewingDetail);
                                    }
                                }
                            }
                        }
                    }
                    await _garmentServiceSubconSewingItemRepository.Update(garmentServiceSubconSewingItem);
                }
                    
            }

            serviceSubconSewing.SetDate(request.ServiceSubconSewingDate.GetValueOrDefault());
            serviceSubconSewing.SetBuyerId(new BuyerId(request.Buyer.Id));
            serviceSubconSewing.SetBuyerCode(request.Buyer.Code);
            serviceSubconSewing.SetBuyerName(request.Buyer.Name);
            serviceSubconSewing.SetQtyPacking(request.QtyPacking);
            serviceSubconSewing.SetUomUnit(request.UomUnit);
            serviceSubconSewing.Modify();
            await _garmentServiceSubconSewingRepository.Update(serviceSubconSewing);

            _storage.Save();

            return serviceSubconSewing;
        }
    }
}