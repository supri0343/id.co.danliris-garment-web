//using Infrastructure.Domain.Queries;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Manufactures.Application.GarmentSample.Queries.GarmentSampleContactReport
//{
//    public class GarmentSampleContactReportQuery : IQuery<GarmentSampleContactReportListViewModel>
//    {
//        public int page { get; private set; }
//        public int size { get; private set; }
//        public string order { get; private set; }
//        public int supplierNo { get; private set; }
//        public string contractType { get; private set; }
//        public DateTime dateFrom { get; private set; }
//        public DateTime dateTo { get; private set; }

//        public GarmentSampleContactReportQuery(int page, int size, string order, int supplierNo, string contractType, DateTime dateFrom, DateTime dateTo)
//        {
//            this.page = page;
//            this.size = size;
//            this.order = order;
//            this.supplierNo = supplierNo;
//            this.contractType = contractType;
//            this.dateFrom = dateFrom;
//            this.dateTo = dateTo;
//        }
//    }
//}
