using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleShrinkagePanels.ExcelTemplates
{
    public class XlsSampleShrinkageDto
    {
        public XlsSampleShrinkageDto()
        {
        }

        public string noBon { get; internal set; }
        public string code { get; internal set; }
        public string name { get; internal set; }
        public string design { get; internal set; }
        public decimal quantity { get; internal set; }
        public string satuan { get; internal set; }

        public XlsSampleShrinkageDto(XlsSampleShrinkageDto xlsgarmentserviceSampleshrinkagepanelsdto)
        {
            noBon = xlsgarmentserviceSampleshrinkagepanelsdto.noBon;
            code = xlsgarmentserviceSampleshrinkagepanelsdto.code;
            name = xlsgarmentserviceSampleshrinkagepanelsdto.name;
            design = xlsgarmentserviceSampleshrinkagepanelsdto.design;
            quantity = xlsgarmentserviceSampleshrinkagepanelsdto.quantity;
            satuan = xlsgarmentserviceSampleshrinkagepanelsdto.satuan;
        }
    }
}
