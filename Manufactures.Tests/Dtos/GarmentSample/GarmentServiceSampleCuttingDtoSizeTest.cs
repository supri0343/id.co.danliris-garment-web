using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSample;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingDtoSizeTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSampleCuttingSizeDto(new GarmentServiceSampleCuttingSize(id, new SizeId(1), "", 1, new UomId(1), "", "ColorD", Guid.NewGuid(), Guid.Empty, Guid.Empty, new ProductId(1), "", ""));

            Assert.NotNull(dto.Size);
            Assert.NotNull(dto.Uom);
            Assert.NotNull(dto.Product);

        }
    }
}
