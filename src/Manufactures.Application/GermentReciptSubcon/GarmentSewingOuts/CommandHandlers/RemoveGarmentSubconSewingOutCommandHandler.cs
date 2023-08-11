using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentSewingOuts.CommandHandlers
{
    public class RemoveGarmentSubconSewingOutCommandHandler : ICommandHandler<RemoveGarmentSubconSewingOutCommand, GarmentSubconSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSubconSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSubconSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSubconSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSubconSewingInItemRepository _garmentSewingInItemRepository;
        //private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public RemoveGarmentSubconSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSubconSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSubconSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSubconSewingOutDetailRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSubconSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSubconSewingInItemRepository>();
        }

        public async Task<GarmentSubconSewingOut> Handle(RemoveGarmentSubconSewingOutCommand request, CancellationToken cancellationToken)
        {
            var sewOut = _garmentSewingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconSewingOut(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
            {
                if (sewOut.IsDifferentSize)
                {
                    _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                    {
                        if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SewingInItemId))
                        {
                            sewInItemToBeUpdated[sewOutItem.SewingInItemId] += sewOutDetail.Quantity;
                        }
                        else
                        {
                            sewInItemToBeUpdated.Add(sewOutItem.SewingInItemId, sewOutDetail.Quantity);
                        }

                        sewOutDetail.Remove();
                        await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                    });
                }
                else
                {
                    if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SewingInItemId))
                    {
                        sewInItemToBeUpdated[sewOutItem.SewingInItemId] += sewOutItem.Quantity;
                    }
                    else
                    {
                        sewInItemToBeUpdated.Add(sewOutItem.SewingInItemId, sewOutItem.Quantity);
                    }
                }

                sewOutItem.Remove();
                await _garmentSewingOutItemRepository.Update(sewOutItem);
            });

            foreach (var sewingInItem in sewInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSubconSewingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + sewingInItem.Value);
                garmentSewInItem.Modify();
                await _garmentSewingInItemRepository.Update(garmentSewInItem);
            }

            sewOut.Remove();
            await _garmentSewingOutRepository.Update(sewOut);

            _storage.Save();

            return sewOut;
        }
    }
}
