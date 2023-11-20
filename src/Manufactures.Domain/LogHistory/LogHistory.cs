using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.LogHistories.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.LogHistory
{
    public class LogHistory : AggregateRoot<LogHistory, LogHistoryReadModel>
    {
        public string Division { get; private set; }
        public string Activity { get; private set; }
        //public DateTime CreatedDate { get; private set; }
        //public string CreatedBy { get; private set; }

        public LogHistory(Guid id, string division, string activity, DateTime createdDate/*, string createdBy*/) : base(id)
        {
            Division = division;
            Activity = activity;
            //CreatedDate = createdDate;
            //CreatedBy = createdBy;

            ReadModel = new LogHistoryReadModel(Identity)
            {
                Division = Division,
                Activity = Activity,
                //CreatedDate = CreatedDate,
                //CreatedBy = CreatedBy,
            };

            ReadModel.AddDomainEvent(new OnGarmentPreparingPlaced(Identity));
        }

        public LogHistory(LogHistoryReadModel readModel) : base(readModel)
        {
            Division = readModel.Division;
            Activity = readModel.Activity;
            //CreatedDate = readModel.CreatedDate;
            //CreatedBy = readModel.CreatedBy;
           
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override LogHistory GetEntity()
        {
            return this;
        }
    }
}
