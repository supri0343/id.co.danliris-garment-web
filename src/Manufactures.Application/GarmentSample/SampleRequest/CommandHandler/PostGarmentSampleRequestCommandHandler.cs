using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleRequest.CommandHandler
{
    public class PostGarmentSampleRequestCommandHandler : ICommandHandler<PostGarmentSampleRequestCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;
        public PostGarmentSampleRequestCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
        }

        public async Task<int> Handle(PostGarmentSampleRequestCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Samples = _GarmentSampleRequestRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleRequest(a)).ToList();

            foreach (var model in Samples)
            {
                model.SetIsPosted(request.Posted);
                model.SetIsReceived(false);
                model.SetIsRejected(false);
                model.SetIsRevised(false);
                model.Modify();
                await _GarmentSampleRequestRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
