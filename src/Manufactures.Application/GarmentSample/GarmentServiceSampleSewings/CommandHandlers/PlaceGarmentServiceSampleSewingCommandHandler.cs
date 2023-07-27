using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleSewings.CommandHandlers
{
    public class PlaceGarmentServiceSampleSewingCommandHandler : ICommandHandler<PlaceGarmentServiceSampleSewingCommand, GarmentServiceSampleSewing>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleSewingRepository _garmentServiceSampleSewingRepository;
        private readonly IGarmentServiceSampleSewingItemRepository _garmentServiceSampleSewingItemRepository;
        private readonly IGarmentServiceSampleSewingDetailRepository _garmentServiceSampleSewingDetailRepository;
        private readonly IGarmentSampleSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentSamplePreparingRepository _garmentPreparingRepository;

        public PlaceGarmentServiceSampleSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleSewingRepository = storage.GetRepository<IGarmentServiceSampleSewingRepository>();
            _garmentServiceSampleSewingItemRepository = storage.GetRepository<IGarmentServiceSampleSewingItemRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
            _garmentServiceSampleSewingDetailRepository= storage.GetRepository<IGarmentServiceSampleSewingDetailRepository>();
            _garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
        }

        public async Task<GarmentServiceSampleSewing> Handle(PlaceGarmentServiceSampleSewingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();
            var collectRoNo = _garmentPreparingRepository.RoChecking(request.Items.Select(x => x.RONo), request.Buyer.Code);
            if (!collectRoNo)
                throw new Exception("RoNo tidak sesuai dengan data pembeli");

            GarmentServiceSampleSewing garmentServiceSampleSewing = new GarmentServiceSampleSewing(
                Guid.NewGuid(),
                GenerateServiceSampleSewingNo(request),
                request.ServiceSubconSewingDate.GetValueOrDefault(),
                request.IsUsed,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                request.QtyPacking,
                request.UomUnit,
                request.NettWeight,
                request.GrossWeight
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSampleSewingItem garmentServiceSampleSewingItem = new GarmentServiceSampleSewingItem(
                    Guid.NewGuid(),
                    garmentServiceSampleSewing.Identity,
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
                        var sewInDetail = SewingInDetails.Where(y => y.DesignColor == detail.DesignColor && y.UnitId== new UnitDepartmentId(detail.Unit.Id) && y.Color == detail.Color).ToList();
                        var qty = detail.Quantity;
                        if(sewInDetail.ToArray().Count() != 0)
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

            await _garmentServiceSampleSewingRepository.Update(garmentServiceSampleSewing);

            _storage.Save();

            return garmentServiceSampleSewing;
        }

        private string GenerateServiceSampleSewingNo(PlaceGarmentServiceSampleSewingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJS{year}{month}";

            var lastServiceSampleSewingNo = _garmentServiceSampleSewingRepository.Query.Where(w => w.ServiceSampleSewingNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSampleSewingNo)
                .Select(s => int.Parse(s.ServiceSampleSewingNo.Substring(7,4)))
                .FirstOrDefault();
            var ServiceSampleSewingNo = $"{prefix}{(lastServiceSampleSewingNo + 1).ToString("D4")}" + "-S";

            return ServiceSampleSewingNo;
        }
    }
}
