using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.Queries
{
    public class GarmentMonitoringServiceSampleCuttingDto
    {
        public GarmentMonitoringServiceSampleCuttingDto()
        { 
        }
        public string SampleNo { get; internal set; }
        public string SampleType { get; internal set; }
        public string unitName { get; internal set; }
        public string buyerName { get; internal set; }
        public DateTimeOffset? SampleDate { get; internal set; }
        public string roNo { get; internal set; }
        public string article { get; internal set; }
        public string comodity { get; internal set; }
        public string designColor { get; internal set; }
        //public double cuttingInQuantity { get; internal set; }
        public string sizeName { get; internal set; }
        public double quantity { get; internal set; }
        public string uomUnit { get; internal set; }
        public string color { get; internal set; }
        public string uomUnitPacking { get; internal set; }
        public int qtyPacking { get; internal set; }

        public GarmentMonitoringServiceSampleCuttingDto(GarmentMonitoringServiceSampleCuttingDto garmentMonitoring)
        {

            SampleNo = garmentMonitoring.SampleNo;
            SampleType = garmentMonitoring.SampleType;
            unitName = garmentMonitoring.unitName;
            buyerName = garmentMonitoring.buyerName;
            SampleDate = garmentMonitoring.SampleDate;
            roNo = garmentMonitoring.roNo;
            article = garmentMonitoring.article;
            comodity = garmentMonitoring.comodity;
            designColor = garmentMonitoring.designColor;
            //cuttingInQuantity = garmentMonitoring.cuttingInQuantity;
            sizeName = garmentMonitoring.sizeName;
            quantity = garmentMonitoring.quantity;
            uomUnit = garmentMonitoring.uomUnit;
            color = garmentMonitoring.color;
            uomUnitPacking = garmentMonitoring.uomUnitPacking;
            qtyPacking = garmentMonitoring.qtyPacking;
        }

    }
}
