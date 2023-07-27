using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.CommandHandlers
{
    public class PlaceGarmentServiceSampleCuttingCommandHandler : ICommandHandler<PlaceGarmentServiceSampleCuttingCommand, GarmentServiceSampleCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleCuttingRepository _garmentServiceSampleCuttingRepository;
        private readonly IGarmentServiceSampleCuttingItemRepository _garmentServiceSampleCuttingItemRepository;
        private readonly IGarmentServiceSampleCuttingDetailRepository _garmentServiceSampleCuttingDetailRepository;
        private readonly IGarmentServiceSampleCuttingSizeRepository _garmentServiceSampleCuttingSizeRepository;
        private readonly IGarmentSampleCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSamplePreparingRepository _garmentPreparingRepository;

        public PlaceGarmentServiceSampleCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleCuttingRepository = storage.GetRepository<IGarmentServiceSampleCuttingRepository>();
            _garmentServiceSampleCuttingItemRepository = storage.GetRepository<IGarmentServiceSampleCuttingItemRepository>();
            _garmentServiceSampleCuttingDetailRepository= storage.GetRepository<IGarmentServiceSampleCuttingDetailRepository>();
            _garmentServiceSampleCuttingSizeRepository = storage.GetRepository<IGarmentServiceSampleCuttingSizeRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();

        }

        public async Task<GarmentServiceSampleCutting> Handle(PlaceGarmentServiceSampleCuttingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.Details.Where(detail => detail.IsSave).Count() > 0).ToList();
            var collectRoNo = _garmentPreparingRepository.RoChecking(request.Items.Select(x => x.RONo), request.Buyer.Code);
            if (!collectRoNo)
                throw new Exception("RoNo tidak sesuai dengan data pembeli");

            GarmentServiceSampleCutting garmentServiceSampleCutting = new GarmentServiceSampleCutting(
                Guid.NewGuid(),
                GenerateSampleNo(request),
                request.SubconType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.SubconDate.GetValueOrDefault(),
                request.IsUsed,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                new UomId(request.Uom.Id),
                request.Uom.Unit,
                request.QtyPacking,
                request.NettWeight,
                request.GrossWeight,
                request.Remark
            );
            foreach (var item in request.Items)
            {
                GarmentServiceSampleCuttingItem garmentServiceSampleCuttingItem = new GarmentServiceSampleCuttingItem(
                    Guid.NewGuid(),
                    garmentServiceSampleCutting.Identity,
                    item.RONo,
                    item.Article,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name
                );

                List<GarmentServiceSampleCuttingSize> cuttingInDetails = new List<GarmentServiceSampleCuttingSize>();
                var cuttingIn = _garmentCuttingInRepository.Query.Where(x => x.RONo == item.RONo).OrderBy(a => a.CreatedDate).ToList();

                foreach (var cutIn in cuttingIn)
                {
                    var cuttingInItems = _garmentCuttingInItemRepository.Query.Where(x => x.CutInId == cutIn.Identity).OrderBy(a => a.CreatedDate).ToList();
                    foreach (var cutInItem in cuttingInItems)
                    {
                        var cutInDetails = _garmentCuttingInDetailRepository.Query.Where(x => x.CutInItemId == cutInItem.Identity).OrderBy(a => a.CreatedDate).ToList();

                        foreach (var cutInDetail in cutInDetails)
                        {
                            var SampleCuttingSizes = _garmentServiceSampleCuttingSizeRepository.Query.Where(o => o.CuttingInDetailId == cutInDetail.Identity).ToList();
                            if (SampleCuttingSizes != null)
                            {
                                double qty = (double)cutInDetail.CuttingInQuantity;
                                foreach (var SampleCuttingDetail in SampleCuttingSizes)
                                {
                                    qty -= SampleCuttingDetail.Quantity;
                                }
                                if (qty > 0)
                                {
                                    cuttingInDetails.Add(new GarmentServiceSampleCuttingSize
                                    (
                                        new Guid(),
                                        new SizeId(1),
                                        "",
                                        qty,
                                        new UomId(1),
                                        "",
                                        cutInDetail.DesignColor,
                                        item.Id,
                                        cutIn.Identity,
                                        cutInDetail.Identity,
                                        new ProductId(cutInDetail.ProductId),
                                        cutInDetail.ProductCode,
                                        cutInDetail.ProductName
                                    ));
                                }

                            }
                        }
                    }
                }

                foreach (var detail in item.Details)
                {
                    if (detail.IsSave)
                    {
                        GarmentServiceSampleCuttingDetail garmentServiceSampleCuttingDetail = new GarmentServiceSampleCuttingDetail(
                                    Guid.NewGuid(),
                                    garmentServiceSampleCuttingItem.Identity,
                                    detail.DesignColor,
                                    detail.Quantity
                                );
                        var cutInDetail = cuttingInDetails.Where(y => y.Color == detail.DesignColor).ToList();
                        foreach (var size in detail.Sizes)
                        {
                            var qty = size.Quantity;
                            foreach (var d in cutInDetail)
                            {
                                if (d.Quantity > 0)
                                {
                                    var qtyRemains = d.Quantity - qty;
                                    if (qtyRemains >= 0)
                                    {
                                        GarmentServiceSampleCuttingSize garmentServiceSampleCuttingSize = new GarmentServiceSampleCuttingSize(
                                            Guid.NewGuid(),
                                            new SizeId(size.Size.Id),
                                            size.Size.Size,
                                            qty,
                                            new UomId(size.Uom.Id),
                                            size.Uom.Unit,
                                            size.Color,
                                            garmentServiceSampleCuttingDetail.Identity,
                                            d.CuttingInId,
                                            d.CuttingInDetailId,
                                            d.ProductId,
                                            d.ProductCode,
                                            d.ProductName
                                        );
                                        await _garmentServiceSampleCuttingSizeRepository.Update(garmentServiceSampleCuttingSize);
                                        d.SetQuantity(qtyRemains);
                                        break;
                                    }
                                    else if (qtyRemains < 0)
                                    {
                                        qty -= d.Quantity;
                                        GarmentServiceSampleCuttingSize garmentServiceSampleCuttingSize = new GarmentServiceSampleCuttingSize(
                                            Guid.NewGuid(),
                                            new SizeId(size.Size.Id),
                                            size.Size.Size,
                                            d.Quantity,
                                            new UomId(size.Uom.Id),
                                            size.Uom.Unit,
                                            size.Color,
                                            garmentServiceSampleCuttingDetail.Identity,
                                            d.CuttingInId,
                                            d.CuttingInDetailId,
                                            d.ProductId,
                                            d.ProductCode,
                                            d.ProductName
                                        );
                                        await _garmentServiceSampleCuttingSizeRepository.Update(garmentServiceSampleCuttingSize);
                                        d.SetQuantity(qtyRemains);
                                    }
                                }
                                
                            }
                        }
                        await _garmentServiceSampleCuttingDetailRepository.Update(garmentServiceSampleCuttingDetail);
                    }
                }

                await _garmentServiceSampleCuttingItemRepository.Update(garmentServiceSampleCuttingItem);
            }


            await _garmentServiceSampleCuttingRepository.Update(garmentServiceSampleCutting);

            _storage.Save();

            return garmentServiceSampleCutting;
        }

        private string GenerateSampleNo(PlaceGarmentServiceSampleCuttingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.SubconType == "BORDIR" ? "BR" : request.SubconType == "PRINT" ? "PR" : request.SubconType == "OTHERS" ? "OT" : "PL";

            var prefix = $"SJC{code}{year}{month}";

            var lastSampleNo = _garmentServiceSampleCuttingRepository.Query.Where(w => w.SampleNo.StartsWith(prefix))
                .OrderByDescending(o => o.SampleNo)
                .Select(s => int.Parse(s.SampleNo.Substring(9, 4)))
                .FirstOrDefault();
            var CutInNo = $"{prefix}{(lastSampleNo + 1).ToString("D4")}" + "-S";

            return CutInNo;
        }
    }

    
}