using System;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSubcon;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentServiceSubconSewingItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSubconSewingDto(new GarmentServiceSubconSewing(id, "serviceSubconSewingNo", new BuyerId(1), "BuyerCode", "BuyerName", new UnitDepartmentId(1), "unitCode", "unitName", "RoNo", "Article", new GarmentComodityId(1), "comodityCode", "ComodityName", DateTimeOffset.Now, false, false));

            Assert.NotNull(dto.Buyer);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.Comodity);

        }
    }
}
