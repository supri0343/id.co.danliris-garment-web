using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.SubconReprocess.CommandHandlers
{
    public class RemoveGarmentSubconReprocessCommandHandler: ICommandHandler<RemoveGarmentSubconReprocessCommand, GarmentSubconReprocess>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconReprocessRepository _garmentSubconReprocessRepository;
        private readonly IGarmentSubconReprocessItemRepository _garmentSubconReprocessItemRepository;
        private readonly IGarmentSubconReprocessDetailRepository _garmentSubconReprocessDetailRepository;

        public RemoveGarmentSubconReprocessCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconReprocessRepository = storage.GetRepository<IGarmentSubconReprocessRepository>();
            _garmentSubconReprocessItemRepository = storage.GetRepository<IGarmentSubconReprocessItemRepository>();
            _garmentSubconReprocessDetailRepository = storage.GetRepository<IGarmentSubconReprocessDetailRepository>();
        }

        public async Task<GarmentSubconReprocess> Handle(RemoveGarmentSubconReprocessCommand request, CancellationToken cancellationToken)
        {
            var SubconReprocess = _garmentSubconReprocessRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconReprocess(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentSubconReprocessItemRepository.Find(o => o.ReprocessId == SubconReprocess.Identity).ForEach(async SubconReprocessItem =>
            {
                _garmentSubconReprocessDetailRepository.Find(i => i.ReprocessItemId == SubconReprocessItem.Identity).ForEach(async subconDetail =>
                {
                    subconDetail.Remove();
                    await _garmentSubconReprocessDetailRepository.Update(subconDetail);
                });
                SubconReprocessItem.Remove();
                await _garmentSubconReprocessItemRepository.Update(SubconReprocessItem);
            });

            SubconReprocess.Remove();
            await _garmentSubconReprocessRepository.Update(SubconReprocess);

            _storage.Save();

            return SubconReprocess;
        }
    }
}
