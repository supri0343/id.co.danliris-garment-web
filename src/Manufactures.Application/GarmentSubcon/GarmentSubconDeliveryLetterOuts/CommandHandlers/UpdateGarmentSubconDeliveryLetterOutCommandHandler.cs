using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    class UpdateGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<UpdateGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;

        public UpdateGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = _storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = _storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
        }

        public async Task<GarmentSubconDeliveryLetterOut> Handle(UpdateGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {
            var subconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconDeliveryLetterOut(o)).Single();

            _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDeliveryLetterOutItem =>
            {
                var item = request.Items.Where(o => o.Id == subconDeliveryLetterOutItem.Identity).Single();

                subconDeliveryLetterOutItem.SetQuantity(item.Quantity);

                subconDeliveryLetterOutItem.Modify();

                await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDeliveryLetterOutItem);
            });

            subconDeliveryLetterOut.SetDate(request.DLDate.GetValueOrDefault());
            subconDeliveryLetterOut.SetEPOItemId(request.EPOItemId);
            subconDeliveryLetterOut.SetPONo(request.PONo);
            subconDeliveryLetterOut.SetRemark(request.Remark);
            

            subconDeliveryLetterOut.Modify();

            await _garmentSubconDeliveryLetterOutRepository.Update(subconDeliveryLetterOut);

            _storage.Save();

            return subconDeliveryLetterOut;
        }
    }
}
