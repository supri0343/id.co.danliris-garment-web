using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLORawMaterialSewing
{
    public class GarmentSubconDLORawMaterialDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconDLORawMaterialReportDto realizationSubconReportDto = new GarmentSubconDLORawMaterialReportDto();
            GarmentSubconDLORawMaterialReportDto dto = new GarmentSubconDLORawMaterialReportDto(realizationSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
