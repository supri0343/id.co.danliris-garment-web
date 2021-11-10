using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels
{
    public class GarmentServiceSubconShrinkagePanel : AggregateRoot<GarmentServiceSubconShrinkagePanel, GarmentServiceSubconShrinkagePanelReadModel>
    {
        public string ServiceSubconShrinkagePanelNo { get; private set; }
        public DateTimeOffset ServiceSubconShrinkagePanelDate { get; private set; }
        public bool IsUsed { get; private set; }

        public GarmentServiceSubconShrinkagePanel(Guid identity, string serviceSubconShrinkagePanelNo, DateTimeOffset serviceSubconShrinkagePanelDate, bool isUsed) : base(identity)
        {
            Identity = identity;
            ServiceSubconShrinkagePanelNo = serviceSubconShrinkagePanelNo;
            ServiceSubconShrinkagePanelDate = serviceSubconShrinkagePanelDate;
            IsUsed = isUsed;

            ReadModel = new GarmentServiceSubconShrinkagePanelReadModel(Identity)
            {
                ServiceSubconShrinkagePanelNo = ServiceSubconShrinkagePanelNo,
                ServiceSubconShrinkagePanelDate = ServiceSubconShrinkagePanelDate,
                IsUsed = IsUsed
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconShrinkagePanelPlaced(Identity));
        }

        public GarmentServiceSubconShrinkagePanel(GarmentServiceSubconShrinkagePanelReadModel readModel) : base(readModel)
        {
            ServiceSubconShrinkagePanelNo = readModel.ServiceSubconShrinkagePanelNo;
            ServiceSubconShrinkagePanelDate = readModel.ServiceSubconShrinkagePanelDate;
            IsUsed = readModel.IsUsed;
        }

        public void SetServiceSubconShrinkagePanelDate(DateTimeOffset ServiceSubconShrinkagePanelDate)
        {
            if (this.ServiceSubconShrinkagePanelDate != ServiceSubconShrinkagePanelDate)
            {
                this.ServiceSubconShrinkagePanelDate = ServiceSubconShrinkagePanelDate;
                ReadModel.ServiceSubconShrinkagePanelDate = ServiceSubconShrinkagePanelDate;
            }
        }

        public void SetIsUsed(bool isUsed)
        {
            if (isUsed != IsUsed)
            {
                IsUsed = isUsed;
                ReadModel.IsUsed = isUsed;

                MarkModified();
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconShrinkagePanel GetEntity()
        {
            return this;
        }
    }
}
