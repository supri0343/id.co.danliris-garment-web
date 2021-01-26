using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSubconCuttingItemDto(new GarmentServiceSubconCuttingItem(id,new Guid(),new Guid(), new Domain.Shared.ValueObjects.ProductId(1), "code","name","color",1));

            Assert.NotNull(dto.Product);

        }
    }
}
