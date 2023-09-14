using Infrastructure.Domain.Commands;

using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPackingOut.Commands
{
    public class UpdateIsPackingListGarmentSubconPackingOutCommand : ICommand<int>
    {
        public UpdateIsPackingListGarmentSubconPackingOutCommand(List<string> nos, bool isReceived,string invoiceNo,int packingListId)
        {
            PackingOutNos = nos;
            IsReceived = isReceived;
            InvoiceNo = invoiceNo;
            PackingListId = packingListId;
        }

        public List<string> PackingOutNos { get; private set; }
        public bool IsReceived { get; private set; }
        public string InvoiceNo { get; private set; }
        public int PackingListId { get; private set; }
    }
}
