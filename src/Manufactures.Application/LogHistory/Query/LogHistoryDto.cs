using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.LogHistories.Query
{
    public class LogHistoryDto
    {
        public LogHistoryDto()
        {

        }

        public string Division { get; internal set; }
        public string Activity { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
        public string CreatedBy { get; internal set; }

        public LogHistoryDto(LogHistoryDto logHistoryDto)
        {
            Division = logHistoryDto.Division;
            Activity = logHistoryDto.Activity;
            CreatedDate = logHistoryDto.CreatedDate;
            CreatedBy = logHistoryDto.CreatedBy;
        }
    }
}
