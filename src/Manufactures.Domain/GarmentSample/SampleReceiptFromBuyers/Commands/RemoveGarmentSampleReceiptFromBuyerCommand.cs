using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleReceiptFromBuyers.Commands
{
	public class RemoveGarmentSampleReceiptFromBuyerCommand : ICommand<GarmentSampleReceiptFromBuyer>
	{
		public RemoveGarmentSampleReceiptFromBuyerCommand(Guid id)
		{
			Identity = id;
		}

		public Guid Identity { get; private set; }
	}
}
