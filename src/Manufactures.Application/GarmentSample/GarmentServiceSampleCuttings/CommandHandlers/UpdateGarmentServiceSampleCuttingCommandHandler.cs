using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.CommandHandlers
{
    public class UpdateGarmentServiceSampleCuttingCommandHandler : ICommandHandler<UpdateGarmentServiceSampleCuttingCommand, GarmentServiceSampleCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleCuttingRepository _garmentServiceSampleCuttingRepository;
        private readonly IGarmentServiceSampleCuttingItemRepository _garmentServiceSampleCuttingItemRepository;
        private readonly IGarmentServiceSampleCuttingDetailRepository _garmentServiceSampleCuttingDetailRepository;
        private readonly IGarmentServiceSampleCuttingSizeRepository _garmentServiceSampleCuttingSizeRepository;

        public UpdateGarmentServiceSampleCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleCuttingRepository = storage.GetRepository<IGarmentServiceSampleCuttingRepository>();
            _garmentServiceSampleCuttingItemRepository = storage.GetRepository<IGarmentServiceSampleCuttingItemRepository>();
            _garmentServiceSampleCuttingDetailRepository = storage.GetRepository<IGarmentServiceSampleCuttingDetailRepository>();
            _garmentServiceSampleCuttingSizeRepository = storage.GetRepository<IGarmentServiceSampleCuttingSizeRepository>();
        }

        public async Task<GarmentServiceSampleCutting> Handle(UpdateGarmentServiceSampleCuttingCommand request, CancellationToken cancellationToken)
        {
            var SampleCutting = _garmentServiceSampleCuttingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleCutting(o)).Single();

            _garmentServiceSampleCuttingItemRepository.Find(o => o.ServiceSampleCuttingId == SampleCutting.Identity).ForEach(async SampleCuttingItem =>
            {
                var item = request.Items.Where(o => o.Id == SampleCuttingItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    _garmentServiceSampleCuttingDetailRepository.Find(i => i.ServiceSampleCuttingItemId == SampleCuttingItem.Identity).ForEach(async SampleDetail =>
                    {
                        SampleDetail.Remove();
                        await _garmentServiceSampleCuttingDetailRepository.Update(SampleDetail);
                    });
                    SampleCuttingItem.Remove();

                }
                else
                {
                    _garmentServiceSampleCuttingDetailRepository.Find(i => i.ServiceSampleCuttingItemId == SampleCuttingItem.Identity).ForEach(async SampleDetail =>
                    {
                        var detail = item.Details.Where(o => o.Id == SampleDetail.Identity).Single();
                        if (!detail.IsSave)
                        {
                            SampleDetail.Remove();
                        }
                        else
                        {
                            SampleDetail.SetQuantity(detail.Quantity);
                            SampleDetail.SetDesignColor(detail.DesignColor);
                            SampleDetail.Modify();
                        }

                        _garmentServiceSampleCuttingSizeRepository.Find(x => x.ServiceSampleCuttingDetailId == SampleDetail.Identity).ForEach(async SampleCuttingSizes =>
                        {
                            var sizes = detail.Sizes.Where(s => s.Id == SampleCuttingSizes.Identity).Single();
                            SampleCuttingSizes.SetQuantity(sizes.Quantity);
                            SampleCuttingSizes.SetColor(sizes.Color);
                            SampleCuttingSizes.SetSizeName(sizes.Size.Size);
                            SampleCuttingSizes.Modify();

                            await _garmentServiceSampleCuttingSizeRepository.Update(SampleCuttingSizes);
                        });

                        await _garmentServiceSampleCuttingDetailRepository.Update(SampleDetail);
                    });
                    SampleCuttingItem.Modify();
                }


                await _garmentServiceSampleCuttingItemRepository.Update(SampleCuttingItem);
            });

            SampleCutting.SetDate(request.SubconDate.GetValueOrDefault());
            SampleCutting.SetBuyerId(new BuyerId(request.Buyer.Id));
            SampleCutting.SetBuyerCode(request.Buyer.Code);
            SampleCutting.SetBuyerName(request.Buyer.Name);
            SampleCutting.SetUomId(new UomId(request.Uom.Id));
            SampleCutting.SetUomUnit(request.Uom.Unit);
            SampleCutting.SetQtyPacking(request.QtyPacking);
            SampleCutting.SetNettWeight(request.NettWeight);
            SampleCutting.SetGrossWeight(request.GrossWeight);
            SampleCutting.SetRemark(request.Remark);
            SampleCutting.Modify();

            var existingItem = _garmentServiceSampleCuttingItemRepository.Find(o => o.ServiceSampleCuttingId == SampleCutting.Identity);

            var newItem = request.Items.Where(x => !existingItem.Select(o => o.RONo).Contains(x.RONo)).ToList();
            var removeItem = existingItem.Where(x => !request.Items.Select(o => o.RONo).Contains(x.RONo)).ToList();

            //var SampleCuttingDetail = _garmentServiceSampleCuttingDetailRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSampleCuttingDetail(o)).Single();

            //var existingDetail = _garmentServiceSampleCuttingDetailRepository.Find(o => o.ServiceSampleCuttingItemId == SampleCuttingDetail.Identity);
            //var newItemSize = 

            if (newItem.Count() > 0)
            {
                foreach (var item in newItem)
                {
                    GarmentServiceSampleCuttingItem garmentServiceSampleCuttingItem = new GarmentServiceSampleCuttingItem(
                        Guid.NewGuid(),
                        SampleCutting.Identity,
                        item.Article,
                        item.RONo,
                        new GarmentComodityId(item.Comodity.Id),
                        item.Comodity.Code,
                        item.Comodity.Name
                   );

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

                            foreach (var size in detail.Sizes)
                            {
                                GarmentServiceSampleCuttingSize garmentServiceSampleCuttingSize = new GarmentServiceSampleCuttingSize(
                                    Guid.NewGuid(),
                                    new SizeId(size.Size.Id),
                                    size.Size.Size,
                                    size.Quantity,
                                    new UomId(size.Uom.Id),
                                    size.Uom.Unit,
                                    size.Color,
                                    garmentServiceSampleCuttingDetail.Identity,
                                    size.CuttingInId,
                                    size.CuttingInDetailId,
                                    new ProductId(size.Product.Id),
                                    size.Product.Code,
                                    size.Product.Name
                                );

                                await _garmentServiceSampleCuttingSizeRepository.Update(garmentServiceSampleCuttingSize);
                            }

                            await _garmentServiceSampleCuttingDetailRepository.Update(garmentServiceSampleCuttingDetail);
                        }
                    }

                    await _garmentServiceSampleCuttingItemRepository.Update(garmentServiceSampleCuttingItem);
                }
            }

            if (removeItem.Count() > 0)
            {
                foreach (var item in removeItem)
                {
                    _garmentServiceSampleCuttingDetailRepository.Find(i => i.ServiceSampleCuttingItemId == item.Identity).ForEach(async SampleCuttingDetail =>
                    {
                        SampleCuttingDetail.Remove();



                        _garmentServiceSampleCuttingSizeRepository.Find(i => i.ServiceSampleCuttingDetailId == SampleCuttingDetail.Identity).ForEach(async SampleCuttingSize =>
                        {
                            SampleCuttingSize.Remove();

                            await _garmentServiceSampleCuttingSizeRepository.Update(SampleCuttingSize);
                        });

                        await _garmentServiceSampleCuttingDetailRepository.Update(SampleCuttingDetail);
                    });
                    item.Remove();
                    await _garmentServiceSampleCuttingItemRepository.Update(item);
                }
            }

            await _garmentServiceSampleCuttingRepository.Update(SampleCutting);

            _storage.Save();

            return SampleCutting;
        }
    }
}