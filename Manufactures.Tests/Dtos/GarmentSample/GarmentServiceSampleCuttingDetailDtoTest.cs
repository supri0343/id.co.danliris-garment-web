using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Dtos.GarmentSample;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingDetailDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSampleCuttingDetailDto(new GarmentServiceSampleCuttingDetail(id, Guid.NewGuid(), "ColorD", 1));

            Assert.NotNull(dto);

        }
    }
}
