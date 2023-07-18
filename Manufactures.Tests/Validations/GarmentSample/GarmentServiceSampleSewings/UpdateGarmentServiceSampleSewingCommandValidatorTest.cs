using System;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.GarmentServiceSampleSewings
{
    public class UpdateGarmentServiceSampleSewingCommandValidatorTest
    {
        private UpdateGarmentServiceSampleSewingCommandValidator GetValidationRules()
        {
            return new UpdateGarmentServiceSampleSewingCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new UpdateGarmentServiceSampleSewingCommand();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new UpdateGarmentServiceSampleSewingCommand();
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
            var unitUnderTest = new UpdateGarmentServiceSampleSewingCommand()
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
                                Uom =new Uom()
                                {
                                    Id =1,
                                    Unit ="Unit"
                                },
                                Unit = new UnitDepartment()
                                {
                                    Id = 1,
                                    Code = "Code",
                                    Name = "Name"
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
