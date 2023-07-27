using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.GarmentServiceSampleCuttings
{
    public class PlaceGarmentServiceSampleCuttingCommandValidatorTest
    {
        private PlaceGarmentServiceSampleCuttingCommandValidator GetValidationRules()
        {
            return new PlaceGarmentServiceSampleCuttingCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new PlaceGarmentServiceSampleCuttingCommand();

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
            var unitUnderTest = new PlaceGarmentServiceSampleCuttingCommand()
            {
                IsUsed = true,
                SubconDate = DateTimeOffset.Now,
                SubconNo = "CuttingOutNo",
                QtyPacking = 1,
                NettWeight = 1,
                GrossWeight = 1,
                Uom = new Uom()
                {
                    Id = 1,
                    Unit = "ROLL"
                },
                SubconType = "test",
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
                                //Product =new Product()
                                //{
                                //    Id = 1,
                                //    Code = "Code",
                                //    Name = "Name"
                                //},
                                Quantity =0,
                                //CuttingInDetailId =id,
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

        [Fact]
        public void Place_HaveError_Qty()
        {
            // Arrange
            var validator = GetValidationRules();
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentServiceSampleCuttingCommand()
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
                                //Product =new Product()
                                //{
                                //    Id = 1,
                                //    Code = "Code",
                                //    Name = "Name"
                                //},
                                //Quantity =5,
                                //CuttingInDetailId =id,
                                CuttingInQuantity =1,
                            }
                        }
                    }
                }
            };

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Qty_MoreThan_CuttingInQty()
        {
            // Arrange
            var validator = GetValidationRules();
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentServiceSampleCuttingCommand()
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
                                //Product =new Product()
                                //{
                                //    Id = 1,
                                //    Code = "Code",
                                //    Name = "Name"
                                //},
                                //Quantity =5,
                                //CuttingInDetailId =id,
                                CuttingInQuantity =1,
                                Sizes= new List<GarmentServiceSampleCuttingSizeValueObject>()
                                {
                                    new GarmentServiceSampleCuttingSizeValueObject
                                    {
                                        Size= new SizeValueObject()
                                        {
                                            Id=1,
                                            Size="size"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldHaveError_Detail()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentServiceSampleCuttingCommand()
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
                                //Product =new Product()
                                //{
                                //    Id = 1,
                                //    Code = "Code",
                                //    Name = "Name"
                                //},
                                Quantity =10,
                                //CuttingInDetailId =id,
                                CuttingInQuantity =1,

                                Sizes= new List<GarmentServiceSampleCuttingSizeValueObject>()
                                {
                                    new GarmentServiceSampleCuttingSizeValueObject
                                    {
                                        
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
            result.ShouldHaveError();

        }
    }
}
