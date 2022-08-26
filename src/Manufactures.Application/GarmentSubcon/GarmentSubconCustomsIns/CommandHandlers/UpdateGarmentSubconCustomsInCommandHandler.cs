using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconCustomsIns.CommandHandlers
{
    public class UpdateGarmentSubconCustomsInCommandHandler : ICommandHandler<UpdateGarmentSubconCustomsInCommand, GarmentSubconCustomsIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsInRepository _garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository _garmentSubconCustomsInItemRepository;
        private readonly IGarmentSubconCustomsInDetailRepository _garmentSubconCustomsInDetailRepository;

        public UpdateGarmentSubconCustomsInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            _garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
            _garmentSubconCustomsInDetailRepository = storage.GetRepository<IGarmentSubconCustomsInDetailRepository>();
        }

        public async Task<GarmentSubconCustomsIn> Handle(UpdateGarmentSubconCustomsInCommand request, CancellationToken cancellationToken)
        {
            var subconCustomsIn = _garmentSubconCustomsInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCustomsIn(o)).Single();

            _garmentSubconCustomsInItemRepository.Find(o => o.SubconCustomsInId == subconCustomsIn.Identity).ForEach(async subconCustomsItem =>
            {
                var item = request.Items.Where(o => o.Id == subconCustomsItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    _garmentSubconCustomsInDetailRepository.Find(o => o.SubconCustomsInItemId == subconCustomsItem.Identity).ForEach(async subconCustomsDetail =>
                    {
                        subconCustomsDetail.Remove();
                        await _garmentSubconCustomsInDetailRepository.Update(subconCustomsDetail);
                    });
                    subconCustomsItem.Remove();
                }
                else
                {
                    _garmentSubconCustomsInDetailRepository.Find(o => o.SubconCustomsInItemId == subconCustomsItem.Identity).ForEach(async subconCustomsDetail =>
                    {
                        var detail = item.Details.Where(d => d.Id == subconCustomsDetail.Identity).SingleOrDefault();
                        if (detail == null)
                        {
                            subconCustomsDetail.Remove();
                        }
                        else
                        {
                            subconCustomsDetail.Modify();
                        }

                        await _garmentSubconCustomsInDetailRepository.Update(subconCustomsDetail);
                    });
                    subconCustomsItem.Modify();
                }


                await _garmentSubconCustomsInItemRepository.Update(subconCustomsItem);
            });

            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentSubconCustomsInItem garmentSubconCustomsInItem = new GarmentSubconCustomsInItem(
                        Guid.NewGuid(),
                        subconCustomsIn.Identity,
                        new SupplierId(item.Supplier.Id),
                        item.Supplier.Code,
                        item.Supplier.Name,
                        item.DoId,
                        item.DoNo,
                        item.Quantity
                    );

                    await _garmentSubconCustomsInItemRepository.Update(garmentSubconCustomsInItem);

                    foreach (var detail in item.Details)
                    {
                        GarmentSubconCustomsInDetail garmentSubconCustomsInDetail = new GarmentSubconCustomsInDetail(
                            Guid.NewGuid(),
                            garmentSubconCustomsInItem.Identity,
                            detail.SubconCustomsOutId,
                            detail.CustomsOutNo,
                            detail.CustomsOutQty
                        );
                        await _garmentSubconCustomsInItemRepository.Update(garmentSubconCustomsInItem);
                    }
                }
            }

            subconCustomsIn.SetBcDate(request.BcDate.GetValueOrDefault());
            subconCustomsIn.SetBcNo(request.BcNo);
            subconCustomsIn.SetBcType(request.BcType);
            subconCustomsIn.SetRemark(request.Remark);
            subconCustomsIn.Modify();
            await _garmentSubconCustomsInRepository.Update(subconCustomsIn);

            _storage.Save();

            return subconCustomsIn;
        }
    }
}
