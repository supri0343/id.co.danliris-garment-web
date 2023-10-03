using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Manufactures.Domain.LogHistories.ReadModels
{
    public class LogHistoryReadModel : ReadModelBase
    {

        public LogHistoryReadModel(Guid identity) : base(identity)
        {
        }
        public string Division { get; internal set; }
        public string Activity { get; internal set; }
        //public DateTime CreatedDate { get; internal set; }
        //public string CreatedBy { get; internal set; }
    }
}
