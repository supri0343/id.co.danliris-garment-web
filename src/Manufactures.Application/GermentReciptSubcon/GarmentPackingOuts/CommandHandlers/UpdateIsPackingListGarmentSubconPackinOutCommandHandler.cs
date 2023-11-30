using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.Commands;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GermentReciptSubcon.GarmentPackingIns.CommandHandlers
{
    public class UpdateIsPackingListGarmentSubconPackinOutCommandHandler : ICommandHandler<UpdateIsPackingListGarmentSubconPackingOutCommand, int>
    {
        private readonly IGarmentSubconPackingOutRepository _garmentPackingOutRepository;
        private readonly IStorage _storage;

        public UpdateIsPackingListGarmentSubconPackinOutCommandHandler(IStorage storage)
        {
            _garmentPackingOutRepository = storage.GetRepository<IGarmentSubconPackingOutRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateIsPackingListGarmentSubconPackingOutCommand request, CancellationToken cancellationToken)
        {

           
            foreach (var PackingOutNo in request.PackingOutNos)
            {
                var packingOut = _garmentPackingOutRepository.Query.Where(x => x.PackingOutNo == PackingOutNo).Select(o => new GarmentSubconPackingOut(o)).Single();
                if (request.IsReceived == true)
                {
                    packingOut.SetIsReceived(request.IsReceived);
                    packingOut.SetInvoice(request.InvoiceNo);
                    packingOut.SetPackingListId(request.PackingListId);
                }
                else
                {
                    packingOut.SetIsReceived(request.IsReceived);
                    packingOut.SetInvoice(null);
                    packingOut.SetPackingListId(0);
                }
                packingOut.Modify();
                await _garmentPackingOutRepository.Update(packingOut);
            }
            _storage.Save();

            return request.PackingOutNos.Count();
        }
    }
}
