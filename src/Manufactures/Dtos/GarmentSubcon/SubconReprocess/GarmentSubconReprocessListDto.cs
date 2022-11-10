using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.SubconReprocess
{
    public class GarmentSubconReprocessListDto: BaseDto
    {
        public GarmentSubconReprocessListDto(GarmentSubconReprocess garmentSubconReprocess)
        {
            Id = garmentSubconReprocess.Identity;
            ReprocessNo = garmentSubconReprocess.ReprocessNo;
            ReprocessType = garmentSubconReprocess.ReprocessType;
            Date = garmentSubconReprocess.Date;
            CreatedBy = garmentSubconReprocess.AuditTrail.CreatedBy;
        }

        public Guid Id { get; set; }
        public string ReprocessNo { get; set; }
        public string ReprocessType { get; set; }

        public DateTimeOffset Date { get; set; }
    
    }
}
