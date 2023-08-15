using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSample;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample
{
    public class GarmentServiceSampleCuttingDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSampleCuttingDto(new GarmentServiceSampleCutting(id, "ServiceSampleCuttingNo", "type", new UnitDepartmentId(1), "unitToCode", "unitToName",  DateTimeOffset.Now, false, new BuyerId(1), "buyerCode", "buyerName", new UomId(1), "uomUnit", 1, 1, 1, ""));

            Assert.NotNull(dto.Unit);

        }
    }
}
