using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubcon.CustomsOuts.CommandHandlers
{
    public class UpdateGarmentSubconCustomsOutCommandHandler : ICommandHandler<UpdateGarmentSubconCustomsOutCommand, GarmentSubconCustomsOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsOutRepository _garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository _garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconCustomsOutDetailRepository _garmentSubconCustomsOutDetailRepository;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public UpdateGarmentSubconCustomsOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            _garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
            _garmentSubconCustomsOutDetailRepository = storage.GetRepository<IGarmentSubconCustomsOutDetailRepository>();
        }

        public async Task<GarmentSubconCustomsOut> Handle(UpdateGarmentSubconCustomsOutCommand request, CancellationToken cancellationToken)
        {
            var subconCustomsOut = _garmentSubconCustomsOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCustomsOut(o)).Single();

            
            _garmentSubconCustomsOutItemRepository.Find(o => o.SubconCustomsOutId == subconCustomsOut.Identity).ForEach(async subconDLItem =>
            {
                var item = request.Items.Where(o => o.Id == subconDLItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    var subconDLOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(x => x.Identity == subconDLItem.SubconDLOutId).Select(s => new GarmentSubconDeliveryLetterOut(s)).Single();
                    subconDLOut.SetIsUsed(false);
                    subconDLOut.Modify();
                    await _garmentSubconDeliveryLetterOutRepository.Update(subconDLOut);

                    _garmentSubconCustomsOutDetailRepository.Find(x => x.SubconCustomsOutItemId == subconDLItem.Identity).ForEach(async subconCustomsDetail =>
                    {
                        subconCustomsDetail.Remove();
                        await _garmentSubconCustomsOutDetailRepository.Update(subconCustomsDetail);
                    });

                    subconDLItem.Remove();
                }
                else
                {
                    subconDLItem.Modify();
                }


                await _garmentSubconCustomsOutItemRepository.Update(subconDLItem);
            });

            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentSubconCustomsOutItem garmentSubconCustomsOutItem = new GarmentSubconCustomsOutItem(
                        Guid.NewGuid(),
                        subconCustomsOut.Identity,
                        item.SubconDLOutNo,
                        item.SubconDLOutId,
                        item.Quantity
                    );

                    foreach (var detail in item.Details)
                    {
                        GarmentSubconCustomsOutDetail garmentSubconCustomsOutDetail = new GarmentSubconCustomsOutDetail(
                            Guid.NewGuid(),
                            garmentSubconCustomsOutItem.Identity,
                            new ProductId(detail.Product.Id),
                            detail.Product.Code,
                            detail.Product.Name,
                            detail.Product.Remark,
                            detail.Quantity,
                            new UomId(detail.Uom.Id),
                            detail.Uom.Unit
                            );

                        await _garmentSubconCustomsOutDetailRepository.Update(garmentSubconCustomsOutDetail);
                    }

                    var subconDLOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(x => x.Identity == item.SubconDLOutId).Select(s => new GarmentSubconDeliveryLetterOut(s)).Single();
                    subconDLOut.SetIsUsed(true);
                    subconDLOut.Modify();
                    await _garmentSubconDeliveryLetterOutRepository.Update(subconDLOut);

                    await _garmentSubconCustomsOutItemRepository.Update(garmentSubconCustomsOutItem);
                }
            }
            

            subconCustomsOut.SetDate(request.CustomsOutDate);
            subconCustomsOut.SetRemark(request.Remark);
            subconCustomsOut.SetSubconCategory(request.SubconCategory);
            subconCustomsOut.SetCustomsOutNo(request.CustomsOutNo);
            subconCustomsOut.SetCustomsOutType(request.CustomsOutType);

            subconCustomsOut.Modify();

            await _garmentSubconCustomsOutRepository.Update(subconCustomsOut);

            ////Add Log History
            //LogHistory logHistory = new LogHistory(new Guid(), "EXIM", "Update BC Keluar Subcon - " + subconCustomsOut.CustomsOutNo, DateTime.Now);
            //await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return subconCustomsOut;
        }
    }
}
