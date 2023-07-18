using System;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.GarmentServiceSampleSewings
{
    public class PlaceGarmentServiceSampleSewingCommandValidatorTest
    {
        private PlaceGarmentServiceSampleSewingCommandValidator GetValidationRules()
        {
            return new PlaceGarmentServiceSampleSewingCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new PlaceGarmentServiceSampleSewingCommand();

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
            var unitUnderTest = new PlaceGarmentServiceSampleSewingCommand();
            unitUnderTest.ServiceSampleSewingDate = DateTimeOffset.Now.AddDays(-7);

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
            var unitUnderTest = new PlaceGarmentServiceSampleSewingCommand()
            {
                ServiceSampleSewingDate = DateTimeOffset.Now,
                ServiceSampleSewingNo = "SewingOutNo",
                Buyer = new Buyer()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                NettWeight = 1,
                GrossWeight = 1,
                Items = new List<GarmentServiceSampleSewingItemValueObject>()
                {
                    new GarmentServiceSampleSewingItemValueObject()
                    {
                        Article = "Article",
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
                        RONo = "RONo",
                        Details= new List<GarmentServiceSampleSewingDetailValueObject>
                        {
                            new GarmentServiceSampleSewingDetailValueObject
                            {
                                DesignColor ="DesignColor",
                                Id =id,
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
                                ServiceSampleSewingId =id,
                                TotalQuantity =1,
                                Unit = new UnitDepartment()
                                {
                                    Id = 1,
                                    Code = "Code",
                                    Name = "Name"
                                },
                                Uom =new Uom()
                                {
                                    Id =1,
                                    Unit ="Unit"
                                },
                            }
                        }
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