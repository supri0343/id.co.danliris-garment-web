using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSubconCuttingDto(new GarmentServiceSubconCutting(id, "ServiceSubconCuttingNo", "type","roNo","art", new UnitDepartmentId(1), "unitToCode", "unitToName", new GarmentComodityId(1), "comodityCode", "ComodityName", DateTimeOffset.Now, false));

            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.Comodity);

        }
    }
}
