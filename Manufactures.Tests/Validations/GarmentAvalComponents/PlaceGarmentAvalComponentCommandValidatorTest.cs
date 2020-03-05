using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentAvalComponents
{
    public class PlaceGarmentAvalComponentCommandValidatorTest
    {
        private PlaceGarmentAvalComponentCommandValidator GetValidationRules()
        {
            return new PlaceGarmentAvalComponentCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentAvalComponentCommand();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            // validator.ShouldHaveValidationErrorFor(r => r.Unit, null as UnitDepartment);
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var a = new PlaceGarmentAvalComponentCommand();
            a.Unit = new UnitDepartment(1, "UnitCode", "UnitName");
            a.AvalComponentType = "AvalComponentType";
            a.RONo = "RONo";
            a.Price = 10;
            a.Comodity = new GarmentComodity(1, "Code", "Name");
            a.Date = DateTimeOffset.Now.AddDays(-1);
            a.Items = new List<PlaceGarmentAvalComponentItemValueObject>
            {
                new PlaceGarmentAvalComponentItemValueObject
                {
                    IsSave = true,
                    Product = new Product(1, "Code","Name"),
                    Quantity = 10,
                    SourceQuantity = 10
                }
            };

            // Action
            var result = validator.TestValidate(a);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}
