using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.GarmentAvalProducts.ReadModels
{
    public class GarmentAvalProductReadModel : ReadModelBase
    {
        public GarmentAvalProductReadModel(Guid identity) : base(identity)
        {

        }

        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public DateTimeOffset AvalDate { get; internal set; }
    }
}