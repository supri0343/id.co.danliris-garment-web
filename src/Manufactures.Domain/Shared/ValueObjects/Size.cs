using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class Size : ValueObject
    {
        public Size()
        {

        }
        public Size(int sizeId, string sizeName)
        {
            Id = sizeId;
            Name = sizeName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
