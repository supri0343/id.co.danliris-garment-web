using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.SubconReprocess
{
    public class GarmentSubconReprocessDto : BaseDto
    {
        public GarmentSubconReprocessDto(GarmentSubconReprocess garmentSubconReprocess)
        {
            Id = garmentSubconReprocess.Identity;
            ReprocessNo = garmentSubconReprocess.ReprocessNo;
            ReprocessType = garmentSubconReprocess.ReprocessType;
            Date = garmentSubconReprocess.Date;
            CreatedBy = garmentSubconReprocess.AuditTrail.CreatedBy;
            Items = new List<GarmentSubconReprocessItemDto>();
        }

        public Guid Id { get; set; }
        public string ReprocessNo { get; set; }
        public string ReprocessType { get; set; }

        public DateTimeOffset Date { get; set; }
        public List<GarmentSubconReprocessItemDto> Items { get; set; }
    }
}
