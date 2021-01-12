using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers
{
    public class UpdateGarmentServiceSubconCuttingCommandHandler : ICommandHandler<UpdateGarmentServiceSubconCuttingCommand, GarmentServiceSubconCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconCuttingRepository _garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository _garmentServiceSubconCuttingItemRepository;

        public UpdateGarmentServiceSubconCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
        }

        public async Task<GarmentServiceSubconCutting> Handle(UpdateGarmentServiceSubconCuttingCommand request, CancellationToken cancellationToken)
        {
            var subconCutting = _garmentServiceSubconCuttingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconCutting(o)).Single();
            
            _garmentServiceSubconCuttingItemRepository.Find(o => o.ServiceSubconCuttingId == subconCutting.Identity).ForEach(async subconCuttingItem =>
            {
                var item = request.Items.Where(o => o.Id == subconCuttingItem.Identity).Single();

                if (!item.IsSave)
                {
                    item.Quantity = 0;
                    subconCuttingItem.Remove();

                }
                else
                {
                   
                    subconCuttingItem.SetQuantity(item.Quantity);
                    subconCuttingItem.Modify();
                }


                await _garmentServiceSubconCuttingItemRepository.Update(subconCuttingItem);
            });

           

            subconCutting.SetDate(request.SubconDate.GetValueOrDefault());
            subconCutting.Modify();
            await _garmentServiceSubconCuttingRepository.Update(subconCutting);

            _storage.Save();

            return subconCutting;
        }
    }
}