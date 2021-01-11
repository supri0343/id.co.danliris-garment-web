using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers
{
    public class UpdateDatesGarmentServiceSubconSewingCommandHandler : ICommandHandler<UpdateDatesGarmentServiceSubconSewingCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;

        public UpdateDatesGarmentServiceSubconSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconSewingRepository = storage.GetRepository<IGarmentServiceSubconSewingRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentServiceSubconSewingCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var SewOuts = _garmentServiceSubconSewingRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentServiceSubconSewing(a)).ToList();

            foreach (var model in SewOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _garmentServiceSubconSewingRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
