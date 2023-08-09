using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Application.GarmentSample.Queries.GarmentSampleGarmentWashReport
{
    public class GarmentSampleGarmentWashReportListViewModel
    {
        public List<GarmentSampleGarmentWashReportDto> garmentSampleGarmentWashReportDto { get; set; }
        public int count { get; set; }       
    }
}
