using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Dtos.GarmentSubcon;
using Manufactures.Helpers.PDFTemplates.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Helpers
{
    public class GarmentSubconDeliveryLetterOutPDFTemplateTest
    {
        [Fact]
        public void Generate_Return_Success()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSubconDeliveryLetterOutDto(new GarmentSubconDeliveryLetterOut(id, null, null, id, "", "", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "SUBCON SEWING"));

            var garmentSubconDLOutItem = new GarmentSubconDeliveryLetterOutItem(id, id, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "");
            var items = new List<GarmentSubconDeliveryLetterOutItemDto>()
            {
                new GarmentSubconDeliveryLetterOutItemDto(garmentSubconDLOutItem)
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);

            var result = GarmentSubconDeliveryLetterOutPDFTemplate.Generate(dto, "Supplier");

            Assert.NotNull(result);
        }
    }
}
