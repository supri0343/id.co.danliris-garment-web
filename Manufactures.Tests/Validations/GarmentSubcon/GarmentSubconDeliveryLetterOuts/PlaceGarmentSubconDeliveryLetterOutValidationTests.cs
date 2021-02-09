using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Tests.Validations.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class PlaceGarmentSubconDeliveryLetterOutValidationTests
    {
        private PlaceGarmentSubconDeliveryLetterOutCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSubconDeliveryLetterOutCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand();

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
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractNo="test",
                ContractType= "test",
                DLDate=DateTimeOffset.Now,
                DLType= "test",
                EPOItemId=1,
                PONo= "test",
                Remark= "test",
                UENId=1,
                SubconContractId=new Guid(),
                TotalQty=1,
                UENNo= "test",
                UsedQty=1,
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        ProductRemark="test",
                        FabricType="test",
                        UENItemId=1,
                        Uom=new Uom()
                        {
                            Id=1,
                            Unit="test"
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

        [Fact]
        public void Place_HaveError_Qty()
        {
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractNo = "test",
                ContractType = "test",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                EPOItemId = 1,
                PONo = "test",
                Remark = "test",
                UENId = 1,
                SubconContractId = new Guid(),
                TotalQty = 9,
                UENNo = "test",
                UsedQty = 1,
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        ProductRemark="test",
                        FabricType="test",
                        UENItemId=1,
                        Uom=new Uom()
                        {
                            Id=1,
                            Unit="test"
                        }
                    }
                }
            };
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();

        }

        [Fact]
        public void Place_HaveError_Qty_Item()
        {
            // Arrange
            var validator = GetValidationRules();
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractNo = "test",
                ContractType = "test",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                EPOItemId = 1,
                PONo = "test",
                Remark = "test",
                UENId = 1,
                SubconContractId = new Guid(),
                TotalQty = 1,
                UENNo = "test",
                UsedQty = 1,
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =0,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        ProductRemark="test",
                        FabricType="test",
                        UENItemId=1,
                        Uom=new Uom()
                        {
                            Id=1,
                            Unit="test"
                        }
                    }
                }
            };

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }
    }
}
