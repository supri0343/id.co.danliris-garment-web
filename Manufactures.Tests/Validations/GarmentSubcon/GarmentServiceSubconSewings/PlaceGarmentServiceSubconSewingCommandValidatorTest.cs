using System;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubcon.GarmentServiceSubconSewings
{
    public class PlaceGarmentServiceSubconSewingCommandValidatorTest
    {
        private PlaceGarmentServiceSubconSewingCommandValidator GetValidationRules()
        {
            return new PlaceGarmentServiceSubconSewingCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new PlaceGarmentServiceSubconSewingCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentServiceSubconSewingCommand();
            unitUnderTest.ServiceSubconSewingDate = DateTimeOffset.Now.AddDays(-7);

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentServiceSubconSewingCommand()
            {
                IsUsed = true,
                IsSave = true,
                Article = "Article",
                RONo = "RONo",
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Buyer = new Buyer()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                IsDifferentSize = true,
                ServiceSubconSewingDate = DateTimeOffset.Now,
                ServiceSubconSewingNo = "SewingOutNo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentServiceSubconSewingItemValueObject>()
                {
                    new GarmentServiceSubconSewingItemValueObject()
                    {
                        Color ="Color",
                        DesignColor ="DesignColor",
                        Id =id,
                        IsDifferentSize =true,
                        IsSave =true,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        SewingInId =id,
                        SewingInItemId =id,
                        SewingInQuantity =1,
                        ServiceSubconSewingId =id,
                        Size =new SizeValueObject()
                        {
                            Id =1,
                            Size ="Size"
                        },
                        TotalQuantity =1,
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit ="Unit"
                        },

                    }
                }
            };
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();

        }
    }
}