using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Dtos.GarmentSample;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSampleCuttingItemDto(new GarmentServiceSampleCuttingItem(id,new Guid(), "roNo", "art", new Domain.Shared.ValueObjects.GarmentComodityId(1), "comoCode", "comoName"));

            Assert.NotNull(dto.Comodity);

        }
    }
}
