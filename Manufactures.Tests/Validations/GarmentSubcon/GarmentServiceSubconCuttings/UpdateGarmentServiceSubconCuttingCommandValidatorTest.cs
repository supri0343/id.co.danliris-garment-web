using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubcon.GarmentServiceSubconCuttings
{
    public class UpdateGarmentServiceSubconCuttingCommandValidatorTest
    {
        private UpdateGarmentServiceSubconCuttingCommandValidator GetValidationRules()
        {
            return new UpdateGarmentServiceSubconCuttingCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new UpdateGarmentServiceSubconCuttingCommand();

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
            var unitUnderTest = new UpdateGarmentServiceSubconCuttingCommand()
            {
                IsUsed = true,
                SubconDate = DateTimeOffset.Now,
                SubconNo = "CuttingOutNo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentServiceSubconCuttingItemValueObject>()
                {
                    new GarmentServiceSubconCuttingItemValueObject()
                    {
                        Id =id,
                        ServiceSubconCuttingId =id,
                        Article = "Article",
                        RONo = "RONo",
                        Comodity = new GarmentComodity()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Details= new List<GarmentServiceSubconCuttingDetailValueObject>()
                        {
                            new GarmentServiceSubconCuttingDetailValueObject
                            {
                                DesignColor ="DesignColor",
                                IsSave =true,
                                Product =new Product()
                                {
                                    Id = 1,
                                    Code = "Code",
                                    Name = "Name"
                                },
                                Quantity =1,
                                CuttingInDetailId =id,
                                CuttingInQuantity =1,
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
