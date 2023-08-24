using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentSewingIns.CommandHandlers
{
    public class RemoveGarmentSewingInCommandHandler : ICommandHandler<RemoveGarmentSubconSewingInCommand, GarmentSubconSewingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSubconSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentSubconLoadingOutItemRepository _garmentLoadingOutItemRepository;
        private readonly IGarmentSubconLoadingInItemRepository _garmentLoadingInItemRepository;
        //private readonly IGarmentSubconSewingOutItemRepository _garmentSewingOutItemRepository;
        //private readonly IGarmentSubconFinishingOutItemRepository _garmentFinishingOutItemRepository;

        public RemoveGarmentSewingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingInRepository = storage.GetRepository<IGarmentSubconSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSubconSewingInItemRepository>();
            _garmentLoadingOutItemRepository = storage.GetRepository<IGarmentSubconLoadingOutItemRepository>();
            _garmentLoadingInItemRepository = storage.GetRepository<IGarmentSubconLoadingInItemRepository>();
            //_garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            //_garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
        }

        public async Task<GarmentSubconSewingIn> Handle(RemoveGarmentSubconSewingInCommand request, CancellationToken cancellationToken)
        {
            var garmentSewingIn = _garmentSewingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconSewingIn(o)).Single();

            if (garmentSewingIn == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Identity));

            var garmentSewingInItems = _garmentSewingInItemRepository.Find(x => x.SewingInId == request.Identity);

            foreach (var item in garmentSewingInItems)
            {
                item.Remove();

                if (garmentSewingIn.SewingFrom == "CUTTING")
                {
                    var garmentLoadingItem = _garmentLoadingOutItemRepository.Query.Where(o => o.Identity == item.LoadingOutItemId).Select(s => new GarmentSubconLoadingOutItem(s)).Single();

                    double diffQty = garmentLoadingItem.Quantity - item.Quantity;
                    garmentLoadingItem.SetRealQtyOut(0);

                    garmentLoadingItem.Modify();
                    await _garmentLoadingOutItemRepository.Update(garmentLoadingItem);

                    if (diffQty > 0)
                    {
                        var garmentCuttingInItem = _garmentLoadingInItemRepository.Query.Where(x => x.Identity == garmentLoadingItem.LoadingInItemId).Select(s => new GarmentSubconLoadingInItem(s)).Single();

                        garmentCuttingInItem.SetRemainingQuantity(garmentCuttingInItem.RemainingQuantity - diffQty);

                        garmentCuttingInItem.Modify();

                        await _garmentLoadingInItemRepository.Update(garmentCuttingInItem);
                    }
                }
                //else if(garmentSewingIn.SewingFrom == "SEWING")
                //{
                //    var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(s => s.Identity == item.SewingOutItemId).Select(s => new GarmentSewingOutItem(s)).Single();

                //    garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity + item.Quantity);

                //    garmentSewingOutItem.Modify();
                //    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                //}
                //else if (garmentSewingIn.SewingFrom == "FINISHING")
                //{
                //    var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(s => s.Identity == item.FinishingOutItemId).Select(s => new GarmentFinishingOutItem(s)).Single();

                //    garmentFinishingOutItem.SetRemainingQuantity(garmentFinishingOutItem.RemainingQuantity + item.Quantity);

                //    garmentFinishingOutItem.Modify();
                //    await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                //}

                await _garmentSewingInItemRepository.Update(item);
            }

            garmentSewingIn.Remove();

            await _garmentSewingInRepository.Update(garmentSewingIn);

            _storage.Save();

            return garmentSewingIn;
        }
    }
}