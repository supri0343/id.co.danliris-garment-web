using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOCuttingSewingReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOSewing
{
    public class GarmentSubconDLOComponentDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconDLOCuttingSewingReportDto realizationSubconReportDto = new GarmentSubconDLOCuttingSewingReportDto();
            GarmentSubconDLOCuttingSewingReportDto dto = new GarmentSubconDLOCuttingSewingReportDto(realizationSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
