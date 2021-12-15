using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleRequest
{
    public class GarmentSampleRequestDto : BaseDto
    {
        public GarmentSampleRequestDto(GarmentSampleRequest garmentSampleRequest)
        {
            Id = garmentSampleRequest.Identity;
            SampleCategory = garmentSampleRequest.SampleCategory;
            SampleRequestNo = garmentSampleRequest.SampleRequestNo;
            RONoSample = garmentSampleRequest.RONoSample;
            RONoCC = garmentSampleRequest.RONoCC;
            Buyer = new Buyer(garmentSampleRequest.BuyerId.Value, garmentSampleRequest.BuyerCode, garmentSampleRequest.BuyerName);
            Comodity = new GarmentComodity(garmentSampleRequest.ComodityId.Value, garmentSampleRequest.ComodityCode, garmentSampleRequest.ComodityName);
            Date = garmentSampleRequest.Date;
            SampleType = garmentSampleRequest.SampleType;
            Packing = garmentSampleRequest.Packing;
            SentDate = garmentSampleRequest.SentDate;
            POBuyer = garmentSampleRequest.POBuyer;
            Attached = garmentSampleRequest.Attached;
            Remark = garmentSampleRequest.Remark;
            IsPosted = garmentSampleRequest.IsPosted;
            IsReceived = garmentSampleRequest.IsReceived;
            ReceivedDate = garmentSampleRequest.ReceivedDate;
            ReceivedBy = garmentSampleRequest.ReceivedBy;
            SampleProducts = new List<GarmentSampleRequestProductDto>();
            SampleSpecifications = new List<GarmentSampleRequestSpecificationDto>();
            Section = new SectionValueObject(garmentSampleRequest.SectionId.Value, garmentSampleRequest.SectionCode);
        }

        public Guid Id { get; set; }
        public string SampleCategory { get;  set; }
        public string SampleRequestNo { get;  set; }
        public string RONoSample { get; internal set; }
        public string RONoCC { get; internal set; }
        public DateTimeOffset Date { get;  set; }

        public Buyer Buyer { get;  set; }

        public GarmentComodity Comodity { get;  set; }

        public string SampleType { get;  set; }
        public string Packing { get;  set; }
        public DateTimeOffset SentDate { get;  set; }
        public string POBuyer { get;  set; }
        public string Attached { get;  set; }
        public string Remark { get;  set; }
        public bool IsPosted { get;  set; }
        public bool IsReceived { get;  set; }
        public DateTimeOffset? ReceivedDate { get; set; }
        public string ReceivedBy { get; set; }
        public SectionValueObject Section { get; set; }
        public List<GarmentSampleRequestProductDto> SampleProducts { get; set; }
        public List<GarmentSampleRequestSpecificationDto> SampleSpecifications { get; set; }
    }
}
