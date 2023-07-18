using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.GarmentServiceSampleCuttings
{
    public class UpdateGarmentServiceSampleCuttingCommandValidatorTest
    {
        private UpdateGarmentServiceSampleCuttingCommandValidator GetValidationRules()
        {
            return new UpdateGarmentServiceSampleCuttingCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new UpdateGarmentServiceSampleCuttingCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentServiceSampleCuttingCommand()
            {
                IsUsed = true,
                SampleDate = DateTimeOffset.Now,
                SampleNo = "CuttingOutNo",
                NettWeight = 1, 
                GrossWeight = 1,
                Uom = new Uom()
                {
                    Id = 1,
                    Unit = "Unit"
                },
                QtyPacking = 1,
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Buyer = new Buyer
                {
                    Id = 1,
                    Code = "Buyercode",
                    Name = "BuyerName"
                },
                Items = new List<GarmentServiceSampleCuttingItemValueObject>()
                {
                    new GarmentServiceSampleCuttingItemValueObject()
                    {
                        Id =id,
                        ServiceSampleCuttingId =id,
                        Article = "Article",
                        RONo = "RONo",
                        Comodity = new GarmentComodity()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Details= new List<GarmentServiceSampleCuttingDetailValueObject>()
                        {
                            new GarmentServiceSampleCuttingDetailValueObject
                            {
                                DesignColor ="DesignColor",
                                IsSave =true,
                                CuttingInQuantity =1,
                                Sizes= new List<GarmentServiceSampleCuttingSizeValueObject>()
                                {
                                    new GarmentServiceSampleCuttingSizeValueObject
                                    {
                                        Size= new SizeValueObject()
                                        {
                                            Id=1,
                                            Size="size"
                                        },
                                        Color="RED",
                                        Quantity=1,
                                        Uom= new Uom
                                        {
                                            Id=1,
                                            Unit="uom"
                                        }
                                    }
                                }
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
