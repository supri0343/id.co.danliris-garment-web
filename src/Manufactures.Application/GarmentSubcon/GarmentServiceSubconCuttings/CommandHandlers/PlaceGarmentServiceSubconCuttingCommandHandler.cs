using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingIns;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers
{
    public class PlaceGarmentServiceSubconCuttingCommandHandler : ICommandHandler<PlaceGarmentServiceSubconCuttingCommand, GarmentServiceSubconCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconCuttingRepository _garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository _garmentServiceSubconCuttingItemRepository;
        private readonly IGarmentServiceSubconCuttingDetailRepository _garmentServiceSubconCuttingDetailRepository;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;

        public PlaceGarmentServiceSubconCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
            _garmentServiceSubconCuttingDetailRepository= storage.GetRepository<IGarmentServiceSubconCuttingDetailRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
        }

        public async Task<GarmentServiceSubconCutting> Handle(PlaceGarmentServiceSubconCuttingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.Details.Where(detail => detail.IsSave).Count() > 0).ToList();

            GarmentServiceSubconCutting garmentServiceSubconCutting = new GarmentServiceSubconCutting(
                Guid.NewGuid(),
                GenerateSubconNo(request),
                request.SubconType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.SubconDate.GetValueOrDefault(),
                request.IsUsed
            );
            foreach (var item in request.Items)
            {
                GarmentServiceSubconCuttingItem garmentServiceSubconCuttingItem = new GarmentServiceSubconCuttingItem(
                    Guid.NewGuid(),
                    garmentServiceSubconCutting.Identity,
                    item.RONo,
                    item.Article,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name
                );

                List<GarmentServiceSubconCuttingDetail> cuttingInDetails = new List<GarmentServiceSubconCuttingDetail>();
                var cuttingIn = _garmentCuttingInRepository.Query.Where(x => x.RONo==item.RONo).OrderBy(a => a.CreatedDate).ToList();

                foreach(var cutIn in cuttingIn)
                {
                    var cuttingInItems= _garmentCuttingInItemRepository.Query.Where(x => x.CutInId == cutIn.Identity).OrderBy(a => a.CreatedDate).ToList();
                    foreach(var cutInItem in cuttingInItems)
                    {
                        var cutInDetails = _garmentCuttingInDetailRepository.Query.Where(x => x.CutInItemId == cutInItem.Identity).OrderBy(a => a.CreatedDate).ToList();

                        foreach(var cutInDetail in cutInDetails)
                        {
                            var subconCuttingDetails = _garmentServiceSubconCuttingDetailRepository.Query.Where(o => o.CuttingInDetailId == cutInDetail.Identity).ToList();
                            if (subconCuttingDetails != null)
                            {
                                double qty = (double)cutInDetail.CuttingInQuantity;
                                foreach (var subconCuttingDetail in subconCuttingDetails)
                                {
                                    qty -= subconCuttingDetail.Quantity;
                                }
                                if (qty > 0)
                                {
                                    cuttingInDetails.Add(new GarmentServiceSubconCuttingDetail
                                    (
                                        new Guid(),
                                        item.Id,
                                        cutIn.Identity,
                                        cutInDetail.Identity,
                                        new ProductId(cutInDetail.ProductId),
                                        cutInDetail.ProductCode,
                                        cutInDetail.ProductName,
                                        cutInDetail.DesignColor,
                                        qty
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
                        var cutInDetail = cuttingInDetails.Where(y => y.DesignColor == detail.DesignColor).ToList();
                        var qty = detail.Quantity;
                        foreach(var d in cutInDetail)
                        {
                            var qtyRemains = d.Quantity - qty;
                            if (qtyRemains > 0)
                            {
                                GarmentServiceSubconCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSubconCuttingDetail(
                                    Guid.NewGuid(),
                                    garmentServiceSubconCuttingItem.Identity,
                                    d.CuttingInId,
                                    d.CuttingInDetailId,
                                    d.ProductId,
                                    d.ProductCode,
                                    d.ProductName,
                                    d.DesignColor,
                                    qty
                                );
                                await _garmentServiceSubconCuttingDetailRepository.Update(garmentServiceSubconCuttingDetail);
                                break;
                            }
                            else if(qtyRemains == 0)
                            {
                                GarmentServiceSubconCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSubconCuttingDetail(
                                    Guid.NewGuid(),
                                    garmentServiceSubconCuttingItem.Identity,
                                    d.CuttingInId,
                                    d.CuttingInDetailId,
                                    d.ProductId,
                                    d.ProductCode,
                                    d.ProductName,
                                    d.DesignColor,
                                    qty
                                );
                                await _garmentServiceSubconCuttingDetailRepository.Update(garmentServiceSubconCuttingDetail);
                                break;
                            }
                            else if (qtyRemains < 0)
                            {
                                qty -= d.Quantity;
                                GarmentServiceSubconCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSubconCuttingDetail(
                                    Guid.NewGuid(),
                                    garmentServiceSubconCuttingItem.Identity,
                                    d.CuttingInId,
                                    d.CuttingInDetailId,
                                    d.ProductId,
                                    d.ProductCode,
                                    d.ProductName,
                                    d.DesignColor,
                                    d.Quantity
                                );
                                await _garmentServiceSubconCuttingDetailRepository.Update(garmentServiceSubconCuttingDetail);
                            }
                        }
                        
                    }
                }
                await _garmentServiceSubconCuttingItemRepository.Update(garmentServiceSubconCuttingItem);
            }

            await _garmentServiceSubconCuttingRepository.Update(garmentServiceSubconCutting);

            _storage.Save();

            return garmentServiceSubconCutting;
        }

        private string GenerateSubconNo(PlaceGarmentServiceSubconCuttingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.SubconType == "BORDIR" ? "B" : request.SubconType == "PRINT" ? "PR" : "PL";

            var prefix = $"SJC{code}{year}{month}";

            var lastSubconNo = _garmentServiceSubconCuttingRepository.Query.Where(w => w.SubconNo.StartsWith(prefix))
                .OrderByDescending(o => o.SubconNo)
                .Select(s => int.Parse(s.SubconNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutInNo = $"{prefix}{(lastSubconNo + 1).ToString("D4")}";

            return CutInNo;
        }
    }

    
}