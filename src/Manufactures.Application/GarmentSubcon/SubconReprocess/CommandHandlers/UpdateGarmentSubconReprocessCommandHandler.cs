using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.SubconReprocess.CommandHandlers
{
    public class UpdateGarmentSubconReprocessCommandHandler : ICommandHandler<UpdateGarmentSubconReprocessCommand, GarmentSubconReprocess>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconReprocessRepository _garmentSubconReprocessRepository;
        private readonly IGarmentSubconReprocessItemRepository _garmentSubconReprocessItemRepository;
        private readonly IGarmentSubconReprocessDetailRepository _garmentSubconReprocessDetailRepository;

        public UpdateGarmentSubconReprocessCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconReprocessRepository = storage.GetRepository<IGarmentSubconReprocessRepository>();
            _garmentSubconReprocessItemRepository = storage.GetRepository<IGarmentSubconReprocessItemRepository>();
            _garmentSubconReprocessDetailRepository = storage.GetRepository<IGarmentSubconReprocessDetailRepository>();
        }

        public async Task<GarmentSubconReprocess> Handle(UpdateGarmentSubconReprocessCommand request, CancellationToken cancellationToken)
        {
            var SubconReprocess = _garmentSubconReprocessRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconReprocess(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentSubconReprocessItemRepository.Find(o => o.ReprocessId == SubconReprocess.Identity).ForEach(async reprocessItem =>
            {
                var item = request.Items.Where(o => o.Id == reprocessItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    _garmentSubconReprocessDetailRepository.Find(i => i.ReprocessItemId == reprocessItem.Identity).ForEach(async reprocessDetail =>
                    {
                        reprocessDetail.Remove();
                        await _garmentSubconReprocessDetailRepository.Update(reprocessDetail);
                    });
                    reprocessItem.Remove();

                }
                else
                {
                    _garmentSubconReprocessDetailRepository.Find(i => i.ReprocessItemId == reprocessItem.Identity).ForEach(async reprocessDetail =>
                    {
                        var detail = item.Details.Where(o => o.Id == reprocessDetail.Identity).SingleOrDefault();
                        if (detail==null)
                        {
                            reprocessDetail.Remove();
                        }
                        else
                        {
                            reprocessDetail.SetQuantity(detail.Quantity);
                            reprocessDetail.SetRemark(detail.Remark);
                            reprocessDetail.SetColor(detail.Color);
                            reprocessDetail.Modify();
                        }
                        await _garmentSubconReprocessDetailRepository.Update(reprocessDetail);
                    });
                    reprocessItem.Modify();
                }


                await _garmentSubconReprocessItemRepository.Update(reprocessItem);
            });

            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentSubconReprocessItem subconSewingItem = new GarmentSubconReprocessItem(
                        Guid.NewGuid(),
                        SubconReprocess.Identity,
                        item.ServiceSubconSewingId,
                        item.ServiceSubconSewingNo,
                        item.ServiceSubconSewingItemId,
                        item.ServiceSubconCuttingId,
                        item.ServiceSubconCuttingNo,
                        item.ServiceSubconCuttingItemId,
                        item.RONo,
                        item.Article,
                        new GarmentComodityId(item.Comodity.Id),
                        item.Comodity.Code,
                        item.Comodity.Name,
                        new BuyerId(item.Buyer.Id),
                        item.Buyer.Code,
                        item.Buyer.Name
                    );

                    foreach (var detail in item.Details)
                    {
                        GarmentSubconReprocessDetail subconDetail = new GarmentSubconReprocessDetail(
                            Guid.NewGuid(),
                            subconSewingItem.Identity,
                            request.ReprocessType == "SUBCON JASA KOMPONEN" ? new SizeId(detail.Size.Id) : null,
                            request.ReprocessType == "SUBCON JASA KOMPONEN" ? detail.Size.Size : null,
                            detail.Quantity,
                            detail.ReprocessQuantity,
                            new UomId(detail.Uom.Id),
                            detail.Uom.Unit,
                            detail.Color,
                            detail.ServiceSubconCuttingDetailId,
                            detail.ServiceSubconCuttingSizeId,
                            detail.ServiceSubconSewingDetailId,
                            detail.DesignColor,
                            request.ReprocessType != "SUBCON JASA KOMPONEN" ? new UnitDepartmentId(detail.Unit.Id) : null,
                            request.ReprocessType != "SUBCON JASA KOMPONEN" ? detail.Unit.Code : null,
                            request.ReprocessType != "SUBCON JASA KOMPONEN" ? detail.Unit.Name : null,
                            detail.Remark
                        );
                        await _garmentSubconReprocessDetailRepository.Update(subconDetail);
                        
                    }
                    await _garmentSubconReprocessItemRepository.Update(subconSewingItem);
                }
            }

            SubconReprocess.SetDate(request.Date);
            SubconReprocess.Modify();
            await _garmentSubconReprocessRepository.Update(SubconReprocess);

            _storage.Save();

            return SubconReprocess;
        }
    }
}
