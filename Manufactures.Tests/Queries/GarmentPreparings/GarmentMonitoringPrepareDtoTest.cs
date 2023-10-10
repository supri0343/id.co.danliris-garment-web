using Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeleted;
using Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentPreparings
{
    public class GarmentMonitoringPrepareDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentMonitoringPrepareDto prepareDto = new GarmentMonitoringPrepareDto();
            GarmentMonitoringPrepareDto dto = new GarmentMonitoringPrepareDto(prepareDto);
            Assert.NotNull(dto);

        }

        [Fact]
        public void ShouldSucces_Instantiate2()
        {
            GarmentMonPreHistoryDelDto prepareDto = new GarmentMonPreHistoryDelDto();
            GarmentMonPreHistoryDelDto dto = new GarmentMonPreHistoryDelDto(prepareDto);
            Assert.NotNull(dto);

        }
    }
    }
